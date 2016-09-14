using CommerceServer.Core;
using CommerceServer.Core.Catalog;
using Sitecore.Foundation.CommerceServer.Requests;

namespace Sitecore.Feature.Catalog.Pipelines.Prices
{
    using System;
    using System.Globalization;
    using System.Linq;
    using Sitecore.Commerce.Connect.CommerceServer.Catalog;
    using Sitecore.Commerce.Connect.CommerceServer.Catalog.Pipelines;
    using Sitecore.Commerce.Entities;
    using Sitecore.Commerce.Services;
    using Sitecore.Diagnostics;
    using Sitecore.Foundation.CommerceServer.Models;
    using Sitecore.Foundation.CommerceServer.Utils;


    /// <summary>
    /// Pipeline is used to get product bulk prices
    /// </summary>
    public class GetProductBulkPrices : PricePipelineProcessor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetProductBulkPrices"/> class.
        /// </summary>
        /// <param name="entityFactory">The entity factory.</param>
        public GetProductBulkPrices([NotNull] IEntityFactory entityFactory)
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
        /// Gets or sets a value indicating whether to use the product base price as variant lowest price.
        /// </summary>
        /// <value>
        /// <c>true</c> if use product base price as variant lowest price; otherwise, <c>false</c>.
        /// </value>
        public bool UseProductBasePriceAsVariantPrice { get; set; }

        /// <summary>
        /// Processes the specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public override void Process(Commerce.Pipelines.ServicePipelineArgs args)
        {
            Assert.ArgumentNotNull(args, "args");
            Assert.ArgumentNotNull(args.Request, "args.request");
            Assert.ArgumentCondition(args.Request is GetProductBulkPricesRequest, "args.Request", "args.Request is GetProductBulkPricesRequest");
            Assert.ArgumentCondition(args.Result is Sitecore.Commerce.Services.Prices.GetProductBulkPricesResult, "args.Result", "args.Result is GetProductBulkPricesResult");

            GetProductBulkPricesRequest request = (GetProductBulkPricesRequest)args.Request;
            Sitecore.Commerce.Services.Prices.GetProductBulkPricesResult result = (Sitecore.Commerce.Services.Prices.GetProductBulkPricesResult)args.Result;

            Assert.ArgumentNotNull(request.ProductCatalogName, "request.ProductCatalogName");
            Assert.ArgumentNotNull(request.ProductIds, "request.ProductIds");
            Assert.ArgumentNotNull(request.PriceType, "request.PriceType");

            ICatalogRepository catalogRepository = ContextTypeLoader.CreateInstance<ICatalogRepository>();

            bool isList = request.PriceTypeIds.FirstOrDefault(x => x.Equals(PriceTypes.List, StringComparison.OrdinalIgnoreCase)) != null;
            bool isAdjusted = request.PriceTypeIds.FirstOrDefault(x => x.Equals(PriceTypes.Adjusted, StringComparison.OrdinalIgnoreCase)) != null;
            bool isLowestPriceVariantSpecified = request.PriceTypeIds.FirstOrDefault(x => x.Equals(PriceTypes.LowestPricedVariant, StringComparison.OrdinalIgnoreCase)) != null;
            bool isLowestPriceVariantListPriceSpecified = request.PriceTypeIds.FirstOrDefault(x => x.Equals(PriceTypes.LowestPricedVariantListPrice, StringComparison.OrdinalIgnoreCase)) != null;
            bool isHighestPriceVariantSpecified = request.PriceTypeIds.FirstOrDefault(x => x.Equals(PriceTypes.HighestPricedVariant, StringComparison.OrdinalIgnoreCase)) != null;

            var uniqueIds = request.ProductIds.ToList().Distinct();
            foreach (var requestedProductId in uniqueIds)
            {
                try
                {
                    var product = catalogRepository.GetProductReadOnly(request.ProductCatalogName, requestedProductId);

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

                    if ((isLowestPriceVariantSpecified || isLowestPriceVariantListPriceSpecified || isHighestPriceVariantSpecified) && product is ProductFamily)
                    {
                        this.SetVariantPrices(product as ProductFamily, extendedPrice, isLowestPriceVariantSpecified, isLowestPriceVariantListPriceSpecified, isHighestPriceVariantSpecified);
                    }

                    result.Prices.Add(requestedProductId, extendedPrice);
                }
                catch (EntityDoesNotExistException e)
                {
                    result.Success = false;
                    result.SystemMessages.Add(new SystemMessage { Message = e.Message });
                    continue;
                }
            }
        }

        /// <summary>
        /// Sets the variant prices.
        /// </summary>
        /// <param name="productFamily">The product family.</param>
        /// <param name="extendedPrice">The extended price.</param>
        /// <param name="isLowestPriceVariantSpecified">if set to <c>true</c> the lowest priced variant adjusted price is returned.</param>
        /// <param name="isLowestPriceVariantListPriceSpecified">if set to <c>true</c> the lowest priced variant list price is returned.</param>
        /// <param name="isHighestPriceVariantSpecified">if set to <c>true</c> the highest priced variant adjusted price.</param>
        protected virtual void SetVariantPrices(ProductFamily productFamily, ExtendedCommercePrice extendedPrice, bool isLowestPriceVariantSpecified, bool isLowestPriceVariantListPriceSpecified, bool isHighestPriceVariantSpecified)
        {
            if (this.UseProductBasePriceAsVariantPrice)
            {
                this.SetVariantPricesFromBaseProduct(productFamily, extendedPrice, isLowestPriceVariantSpecified, isLowestPriceVariantListPriceSpecified, isHighestPriceVariantSpecified);
            }
            else
            {
                this.SetVariantPricesFromProductVariants(productFamily, extendedPrice, isLowestPriceVariantSpecified, isLowestPriceVariantListPriceSpecified, isHighestPriceVariantSpecified);
            }
        }

        /// <summary>
        /// Sets the variant prices from base product.
        /// </summary>
        /// <param name="productFamily">The product family.</param>
        /// <param name="extendedPrice">The extended price.</param>
        /// <param name="isLowestPriceVariantSpecified">if set to <c>true</c> the lowest priced variant adjusted price is returned.</param>
        /// <param name="isLowestPriceVariantListPriceSpecified">if set to <c>true</c> the lowest priced variant list price is returned.</param>
        /// <param name="isHighestPriceVariantSpecified">if set to <c>true</c> the highest priced variant adjusted price.</param>
        protected virtual void SetVariantPricesFromBaseProduct(ProductFamily productFamily, ExtendedCommercePrice extendedPrice, bool isLowestPriceVariantSpecified, bool isLowestPriceVariantListPriceSpecified, bool isHighestPriceVariantSpecified)
        {
            decimal lowestPricedVariantAdjustedPrice;
            decimal lowestPricedVariantListPrice;

            if (productFamily.HasProperty("BasePrice") && productFamily["BasePrice"] != null)
            {
                lowestPricedVariantAdjustedPrice = productFamily.ListPrice;
                lowestPricedVariantListPrice = (productFamily["BasePrice"] as decimal?).Value;
            }
            else
            {
                // No base price is defined, the List price is set to the actual ListPrice define in the catalog
                lowestPricedVariantAdjustedPrice = productFamily.ListPrice;
                lowestPricedVariantListPrice = productFamily.ListPrice;
            }

            if (isLowestPriceVariantSpecified)
            {
                extendedPrice.LowestPricedVariant = lowestPricedVariantAdjustedPrice;
            }

            if (isLowestPriceVariantListPriceSpecified)
            {
                extendedPrice.LowestPricedVariantListPrice = lowestPricedVariantListPrice;
            }
        }

        /// <summary>
        /// Sets the variant prices by looping through all of the variants.
        /// </summary>
        /// <param name="productFamily">The product family.</param>
        /// <param name="extendedPrice">The extended price.</param>
        /// <param name="isLowestPriceVariantSpecified">if set to <c>true</c> the lowest priced variant adjusted price is returned.</param>
        /// <param name="isLowestPriceVariantListPriceSpecified">if set to <c>true</c> the lowest priced variant list price is returned.</param>
        /// <param name="isHighestPriceVariantSpecified">if set to <c>true</c> the highest priced variant adjusted price.</param>
        protected virtual void SetVariantPricesFromProductVariants(ProductFamily productFamily, ExtendedCommercePrice extendedPrice, bool isLowestPriceVariantSpecified, bool isLowestPriceVariantListPriceSpecified, bool isHighestPriceVariantSpecified)
        {
            if (productFamily.Variants != null && productFamily.Variants.Count > 0)
            {
                decimal highestPrice = 0.0M;
                decimal lowestPrice = 0.0M;
                decimal basePrice = 0.0M;
                bool processingFirstItem = true;

                foreach (Variant variant in productFamily.Variants)
                {
                    if (!variant.IsListPriceNull())
                    {
                        if (processingFirstItem || variant.ListPrice < lowestPrice)
                        {
                            lowestPrice = variant.ListPrice;
                            basePrice = variant.DataRow.Table.Columns.Contains("BasePriceVariant") && variant.DataRow["BasePriceVariant"] != DBNull.Value ? Convert.ToDecimal(variant.DataRow["BasePriceVariant"], CultureInfo.InvariantCulture) : 0.0M;
                        }

                        if (processingFirstItem || variant.ListPrice > highestPrice)
                        {
                            highestPrice = variant.ListPrice;
                        }

                        processingFirstItem = false;
                    }
                }

                if (isLowestPriceVariantSpecified)
                {
                    extendedPrice.LowestPricedVariant = lowestPrice;
                }

                if (isLowestPriceVariantListPriceSpecified)
                {
                    extendedPrice.LowestPricedVariantListPrice = basePrice;
                }

                if (isHighestPriceVariantSpecified)
                {
                    extendedPrice.HighestPricedVariant = highestPrice;
                }
            }
        }
    }
}