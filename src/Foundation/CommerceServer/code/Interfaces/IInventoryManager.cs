using System.Collections.Generic;
using Sitecore.Commerce.Entities.Inventory;
using Sitecore.Commerce.Services.Inventory;
using Sitecore.Foundation.CommerceServer.Managers;
using Sitecore.Foundation.CommerceServer.Models;
using Sitecore.Foundation.CommerceServer.Models.InputModels;

namespace Sitecore.Foundation.CommerceServer.Interfaces
{
    public interface IInventoryManager
    {
        /// <summary>
        /// Gets the products stock status where lists of products are displayed on the site.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="productViewModels">The product view models.</param>
        void GetProductsStockStatusForList([NotNull] CommerceStorefront storefront, List<ProductViewModel> productViewModels);

        /// <summary>
        /// Gets the product stock status.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="productViewModels">The product view models.</param>
        void GetProductsStockStatus([NotNull] CommerceStorefront storefront, List<ProductViewModel> productViewModels);

        /// <summary>
        /// Gets the stock information.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="products">The products.</param>
        /// <param name="detailsLevel">The details level.</param>
        /// <returns>
        /// The manager response which returns an enumerable collection of StockInformation in the Result.
        /// </returns>
        ManagerResponse<GetStockInformationResult, IEnumerable<StockInformation>> GetStockInformation([NotNull] CommerceStorefront storefront, IEnumerable<InventoryProduct> products, StockDetailsLevel detailsLevel);

        /// <summary>
        /// Gets the pre orderable information.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="products">The products.</param>
        /// <returns>The manager response which returns an enumerable collection of OrderableInformation in the Result.</returns>
        ManagerResponse<GetPreOrderableInformationResult, IEnumerable<OrderableInformation>> GetPreOrderableInformation([NotNull] CommerceStorefront storefront, IEnumerable<InventoryProduct> products);

        /// <summary>
        /// Gets the back orderable information.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="products">The products.</param>
        /// <returns>The manager response which returns an enumerable collection of OrderableInformation in the Result.</returns>
        ManagerResponse<GetBackOrderableInformationResult, IEnumerable<OrderableInformation>> GetBackOrderableInformation([NotNull] CommerceStorefront storefront, IEnumerable<InventoryProduct> products);

        /// <summary>
        /// Visiteds the product stock status.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="stockInformation">The stock information.</param>
        /// <param name="location">The location.</param>
        /// <returns>The manager response which returns success flag in the Result.</returns>
        ManagerResponse<VisitedProductStockStatusResult, bool> VisitedProductStockStatus([NotNull] CommerceStorefront storefront, StockInformation stockInformation, string location);

        /// <summary>
        /// Visitors the sign up for stock notification.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="model">The model.</param>
        /// <param name="location">The location.</param>
        /// <returns>
        /// The manager response which returns success flag in the Result.
        /// </returns>
        ManagerResponse<VisitorSignUpForStockNotificationResult, bool> VisitorSignupForStockNotification([NotNull] CommerceStorefront storefront, SignUpForNotificationInputModel model, string location);
    }
}