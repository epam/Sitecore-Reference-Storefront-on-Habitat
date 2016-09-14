namespace Sitecore.Foundation.CommerceServer.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Commerce.Connect.CommerceServer.Inventory;
    using Commerce.Connect.CommerceServer.Inventory.Models;
    using Commerce.Entities.Inventory;
    using Commerce.Multishop;
    using Commerce.Services.Inventory;
    using Configuration;
    using Diagnostics;
    using Helpers;
    using Interfaces;
    using Models;
    using Models.InputModels;

    /// <summary>
    /// Defines the InventoryManager class.
    /// </summary>
    public class InventoryManager : IInventoryManager
    {
        private readonly CommerceContextBase _obecContext;
        private readonly InventoryServiceProvider _inventoryServiceProvider;
        private readonly IContactFactory _contactFactory;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="InventoryManager" /> class.
        /// </summary>
        /// <param name="inventoryServiceProvider">The inventory service provider.</param>
        /// <param name="contactFactory">The contact factory.</param>
        public InventoryManager(
            [NotNull] InventoryServiceProvider inventoryServiceProvider,
            [NotNull] IContactFactory contactFactory)
        {
            Assert.ArgumentNotNull(inventoryServiceProvider, "inventoryServiceProvider");
            Assert.ArgumentNotNull(contactFactory, "contactFactory");

            this._inventoryServiceProvider = inventoryServiceProvider;
            this._contactFactory = contactFactory;
            this._obecContext = (CommerceContextBase)Factory.CreateObject("commerceContext", true);
        }

        #endregion

        #region Methods (public, virtual)

        /// <summary>
        /// Gets the products stock status where lists of products are displayed on the site.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="productViewModels">The product view models.</param>
        public virtual void GetProductsStockStatusForList([NotNull] CommerceStorefront storefront, List<ProductViewModel> productViewModels)
        {
            if (!StorefrontManager.CurrentStorefront.UseIndexFileForProductStatusInLists)
            {
                this.GetProductsStockStatus(storefront, productViewModels);
            }
            else
            {
                SearchNavigation.GetProductStockStatusFromIndex(productViewModels);
            }
        }

        /// <summary>
        /// Gets the product stock status.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="productViewModels">The product view models.</param>
        public virtual void GetProductsStockStatus([NotNull] CommerceStorefront storefront, List<ProductViewModel> productViewModels)
        {
            if (productViewModels == null || !productViewModels.Any())
            {
                return;
            }

            var products = new List<CommerceInventoryProduct>();
            foreach (var viewModel in productViewModels)
            {
                if (viewModel.Variants != null && viewModel.Variants.Any())
                {
                    foreach (var variant in viewModel.Variants)
                    {
                        products.Add(new CommerceInventoryProduct
                        {
                            ProductId = viewModel.ProductId,
                            CatalogName = viewModel.CatalogName,
                            VariantId = variant.VariantId
                        });
                    }
                }
                else
                {
                    products.Add(new CommerceInventoryProduct { ProductId = viewModel.ProductId, CatalogName = viewModel.CatalogName });
                }
            }

            if (products.Any())
            {
                var response = this.GetStockInformation(storefront, products, StockDetailsLevel.All);
                if (response.Result != null)
                {
                    var stockInfoList = response.Result.ToList();

                    foreach (var viewModel in productViewModels)
                    {
                        StockInformation foundItem = null;
                        if (viewModel.Variants != null && viewModel.Variants.Any())
                        {
                            foreach (var variant in viewModel.Variants)
                            {
                                foundItem = stockInfoList.Find(p => p.Product.ProductId == viewModel.ProductId && ((CommerceInventoryProduct)p.Product).VariantId == variant.VariantId);
                            }
                        }
                        else
                        {
                            foundItem = stockInfoList.Find(p => p.Product.ProductId == viewModel.ProductId);
                        }

                        if (foundItem != null)
                        {
                            viewModel.StockStatus = foundItem.Status;
                            viewModel.StockStatusName = StorefrontManager.GetProductStockStatusName(foundItem.Status);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets the stock information.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="products">The products.</param>
        /// <param name="detailsLevel">The details level.</param>
        /// <returns>
        /// The manager response which returns an enumerable collection of StockInformation in the Result.
        /// </returns>
        public virtual ManagerResponse<GetStockInformationResult, IEnumerable<StockInformation>> GetStockInformation([NotNull] CommerceStorefront storefront, IEnumerable<InventoryProduct> products, StockDetailsLevel detailsLevel)
        {
            Assert.ArgumentNotNull(storefront, "storefront");
            Assert.ArgumentNotNull(products, "products");

            var request = new GetStockInformationRequest(storefront.ShopName, products, detailsLevel) { Location = this._obecContext.InventoryLocation, VisitorId = this._contactFactory.GetContact() };
            var result = this._inventoryServiceProvider.GetStockInformation(request);

            // Currently, both Categories and Products are passed in and are waiting for a fix to filter the categories out.  Until then, this code is commented
            // out as it generates an unecessary Error event indicating the product cannot be found.
            // Helpers.LogSystemMessages(result.SystemMessages, result);
            return new ManagerResponse<GetStockInformationResult, IEnumerable<StockInformation>>(result, result.StockInformation ?? new List<StockInformation>());
        }

        /// <summary>
        /// Gets the pre orderable information.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="products">The products.</param>
        /// <returns>The manager response which returns an enumerable collection of OrderableInformation in the Result.</returns>
        public virtual ManagerResponse<GetPreOrderableInformationResult, IEnumerable<OrderableInformation>> GetPreOrderableInformation([NotNull] CommerceStorefront storefront, IEnumerable<InventoryProduct> products)
        {
            Assert.ArgumentNotNull(storefront, "storefront");
            Assert.ArgumentNotNull(products, "products");

            var request = new GetPreOrderableInformationRequest(storefront.ShopName, products);
            var result = this._inventoryServiceProvider.GetPreOrderableInformation(request);

            // Helpers.LogSystemMessages(result.SystemMessages, result);
            return new ManagerResponse<GetPreOrderableInformationResult, IEnumerable<OrderableInformation>>(result, !result.Success || result.OrderableInformation == null ? new List<OrderableInformation>() : result.OrderableInformation);
        }

        /// <summary>
        /// Gets the back orderable information.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="products">The products.</param>
        /// <returns>The manager response which returns an enumerable collection of OrderableInformation in the Result.</returns>
        public virtual ManagerResponse<GetBackOrderableInformationResult, IEnumerable<OrderableInformation>> GetBackOrderableInformation([NotNull] CommerceStorefront storefront, IEnumerable<InventoryProduct> products)
        {
            Assert.ArgumentNotNull(storefront, "storefront");
            Assert.ArgumentNotNull(products, "products");

            var request = new GetBackOrderableInformationRequest(storefront.ShopName, products);
            var result = this._inventoryServiceProvider.GetBackOrderableInformation(request);

            // Helpers.LogSystemMessages(result.SystemMessages, result);
            return new ManagerResponse<GetBackOrderableInformationResult, IEnumerable<OrderableInformation>>(result, !result.Success || result.OrderableInformation == null ? new List<OrderableInformation>() : result.OrderableInformation);
        }

        /// <summary>
        /// Visiteds the product stock status.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="stockInformation">The stock information.</param>
        /// <param name="location">The location.</param>
        /// <returns>The manager response which returns success flag in the Result.</returns>
        public virtual ManagerResponse<VisitedProductStockStatusResult, bool> VisitedProductStockStatus([NotNull] CommerceStorefront storefront, StockInformation stockInformation, string location)
        {
            Assert.ArgumentNotNull(storefront, "storefront");
            Assert.ArgumentNotNull(stockInformation, "stockInformation");

            var request = new VisitedProductStockStatusRequest(storefront.ShopName, stockInformation) { Location = location };
            var result = this._inventoryServiceProvider.VisitedProductStockStatus(request);

            // Helpers.LogSystemMessages(result.SystemMessages, result);
            return new ManagerResponse<VisitedProductStockStatusResult, bool>(result, result.Success);
        }

        /// <summary>
        /// Visitors the sign up for stock notification.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="model">The model.</param>
        /// <param name="location">The location.</param>
        /// <returns>
        /// The manager response which returns success flag in the Result.
        /// </returns>
        public virtual ManagerResponse<VisitorSignUpForStockNotificationResult, bool> VisitorSignupForStockNotification([NotNull] CommerceStorefront storefront, SignUpForNotificationInputModel model, string location)
        {
            Assert.ArgumentNotNull(storefront, "storefront");
            Assert.ArgumentNotNull(model, "model");
            Assert.ArgumentNotNullOrEmpty(model.ProductId, "model.ProductId");
            Assert.ArgumentNotNullOrEmpty(model.Email, "model.Email");

            var visitorId = this._contactFactory.GetContact();
            var builder = new CommerceInventoryProductBuilder();
            CommerceInventoryProduct inventoryProduct = (CommerceInventoryProduct)builder.CreateInventoryProduct(model.ProductId);
            if (string.IsNullOrEmpty(model.VariantId))
            {
                (inventoryProduct).VariantId = model.VariantId;
            }

            if (string.IsNullOrEmpty(inventoryProduct.CatalogName))
            {
                (inventoryProduct).CatalogName = model.CatalogName;
            }

            DateTime interestDate;
            var isDate = DateTime.TryParse(model.InterestDate, out interestDate);
            var request = new VisitorSignUpForStockNotificationRequest(storefront.ShopName, visitorId, model.Email, inventoryProduct) { Location = location };
            if (isDate)
            {
                request.InterestDate = interestDate;
            }

            var result = this._inventoryServiceProvider.VisitorSignUpForStockNotification(request);

            // Helpers.LogSystemMessages(result.SystemMessages, result);
            return new ManagerResponse<VisitorSignUpForStockNotificationResult, bool>(result, result.Success);
        }

        #endregion
    }
}
