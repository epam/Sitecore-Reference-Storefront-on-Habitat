namespace Sitecore.Foundation.CommerceServer.Interfaces
{
    using System.Collections.Generic;
    using Sitecore.Commerce.Connect.CommerceServer.Orders.Models;
    using Sitecore.Commerce.Entities.Orders;
    using Sitecore.Commerce.Services.Orders;
    using Sitecore.Foundation.CommerceServer.Models;
    using Sitecore.Foundation.CommerceServer.Models.InputModels;

    public interface IOrderManager
    {
        /// <summary>
        /// Submits the visitor order.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="visitorContext">The visitor context.</param>
        /// <param name="inputModel">The input model.</param>
        /// <returns>
        /// The manager response where the new CommerceOrder is returned in the Result.
        /// </returns>
        ManagerResponse<SubmitVisitorOrderResult, CommerceOrder> SubmitVisitorOrder([NotNull] CommerceStorefront storefront, [NotNull] IVisitorContext visitorContext, [NotNull] SubmitOrderInputModel inputModel);

        /// <summary>
        /// Gets the available states.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="visitorContext">The visitor context.</param>
        /// <param name="countryCode">The country code.</param>
        /// <returns>
        /// The manager response where the available states are returned in the Result.
        /// </returns>
        ManagerResponse<GetAvailableRegionsResult, Dictionary<string, string>> GetAvailableRegions([NotNull] CommerceStorefront storefront, [NotNull] IVisitorContext visitorContext, [NotNull] string countryCode);

        /// <summary>
        /// Gets the orders.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <param name="shopName">Name of the shop.</param>
        /// <returns>The manager response where list of order headers are returned in the Result.</returns>
        ManagerResponse<GetVisitorOrdersResult, IEnumerable<OrderHeader>> GetOrders(string customerId, string shopName);

        /// <summary>
        /// Gets the order details.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="visitorContext">The visitor context.</param>
        /// <param name="orderId">The order identifier.</param>
        /// <returns>
        /// The manager response where the order detail returned in the Result.
        /// </returns>
        ManagerResponse<GetVisitorOrderResult, CommerceOrder> GetOrderDetails([NotNull] CommerceStorefront storefront, [NotNull] IVisitorContext visitorContext, [NotNull] string orderId);

        /// <summary>
        /// Gets the available countries.
        /// </summary>
        /// <returns>The manager response where the available country dictionary is returned in the Result.</returns>
        ManagerResponse<GetAvailableCountriesResult, Dictionary<string, string>> GetAvailableCountries();
    }
}