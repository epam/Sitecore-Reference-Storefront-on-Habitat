using CommerceServer.Core;
using CommerceServer.Core.Catalog;
using Sitecore.Foundation.CommerceServer.Requests;

namespace Sitecore.Feature.Catalog.Pipelines.Prices
{
    using System;
    using System.Linq;
    using Sitecore.Commerce.Connect.CommerceServer.Catalog;
    using Sitecore.Commerce.Connect.CommerceServer.Catalog.Pipelines;
    using Sitecore.Commerce.Entities;
    using Sitecore.Commerce.Services;
    using Sitecore.Diagnostics;
    using Sitecore.Foundation.CommerceServer.Models;
    using Sitecore.Foundation.CommerceServer.Utils;

    /// <summary>
    /// Pipeline is used to get product prices
    /// </summary>
    public class GetProductPrices : PricePipelineProcessor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetProductPrices"/> class.
        /// </summary>
        /// <param name="entityFactory">The entity factory.</param>
        public GetProductPrices([NotNull] IEntityFactory entityFactory)
        {
            Assert.ArgumentNotNull(entityFactory, "entityFactory");

            this.EntityFactory = entityFactory;
        }

        /// <summary>
        /// Gets or sets the entity factory.
        /// </summary>
        /// <value>
        /// The entity factory.
        /// </value>
        public IEntityFactory EntityFactory { get; set; }

        /// <summary>
        /// Processes the specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public override void Process(Commerce.Pipelines.ServicePipelineArgs args)
        {
            Assert.ArgumentNotNull(args, "args");
            Assert.ArgumentNotNull(args.Request, "args.request");
            Assert.ArgumentCondition(args.Request is GetProductPricesRequest, "args.Request", "args.Request is GetProductPricesRequest");
            Assert.ArgumentCondition(args.Result is Sitecore.Commerce.Services.Prices.GetProductPricesResult, "args.Result", "args.Result is GetProductPricesResult");

            GetProductPricesRequest request = (GetProductPricesRequest)args.Request;
            Sitecore.Commerce.Services.Prices.GetProductPricesResult result = (Sitecore.Commerce.Services.Prices.GetProductPricesResult)args.Result;

            Assert.ArgumentNotNull(request.ProductCatalogName, "request.ProductCatalogName");
            Assert.ArgumentNotNull(request.ProductId, "request.ProductId");
            Assert.ArgumentNotNull(request.PriceTypeIds, "request.PriceTypeIds");

            ICatalogRepository catalogRepository = ContextTypeLoader.CreateInstance<ICatalogRepository>();
            bool isList = request.PriceTypeIds.FirstOrDefault(x => x.Equals(PriceTypes.List, StringComparison.OrdinalIgnoreCase)) != null;
            bool isAdjusted = request.PriceTypeIds.FirstOrDefault(x => x.Equals(PriceTypes.Adjusted, StringComparison.OrdinalIgnoreCase)) != null;

            try
            {
                var product = catalogRepository.GetProductReadOnly(request.ProductCatalogName, request.ProductId);

                ExtendedCommercePrice extendedPrice = this.EntityFactory.Create<ExtendedCommercePrice>("Price");

                // BasePrice is a List price and ListPrice is Adjusted price 
                if (isList)
                {
                    if (product.HasProperty("BasePrice") && product["BasePrice"] != null)
                    {
                        extendedPrice.Amount = (product["BasePrice"] as decimal?).Value;
                    }
                    else
                    {
                        // No base price is defined, the List price is set to the actual ListPrice define in the catalog
                        extendedPrice.Amount = product.ListPrice;
                    }
                }

                if (isAdjusted && !product.IsListPriceNull())
                {
                    extendedPrice.ListPrice = product.ListPrice;
                }

                result.Prices.Add(request.ProductId, extendedPrice);

                if (request.IncludeVariantPrices && product is ProductFamily)
                {
                    foreach (Variant variant in ((ProductFamily)product).Variants)
                    {
                        ExtendedCommercePrice variantExtendedPrice = this.EntityFactory.Create<ExtendedCommercePrice>("Price");

                        bool hasBasePrice = product.HasProperty("BasePrice");

                        if (hasBasePrice && variant["BasePriceVariant"] != null)
                        {
                            variantExtendedPrice.Amount = (variant["BasePriceVariant"] as decimal?).Value;
                        }

                        if (!variant.IsListPriceNull())
                        {
                            variantExtendedPrice.ListPrice = variant.ListPrice;

                            if (!hasBasePrice)
                            {
                                variantExtendedPrice.Amount = variant.ListPrice;
                            }
                        }

                        result.Prices.Add(variant.VariantId.Trim(), variantExtendedPrice);
                    }
                }
            }
            catch (EntityDoesNotExistException e)
            {
                result.Success = false;
                result.SystemMessages.Add(new SystemMessage { Message = e.Message });
            }
        }
    }
}