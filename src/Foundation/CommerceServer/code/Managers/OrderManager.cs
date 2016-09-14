namespace Sitecore.Foundation.CommerceServer.Managers
{
    using System.Collections.Generic;
    using System.Linq;
    using Interfaces;
    using Sitecore.Commerce.Connect.CommerceServer.Orders;
    using Sitecore.Commerce.Connect.CommerceServer.Orders.Models;
    using Sitecore.Commerce.Entities.Orders;
    using Sitecore.Commerce.Services;
    using Sitecore.Commerce.Services.Orders;
    using Sitecore.Diagnostics;
    using Sitecore.Foundation.CommerceServer.Extensions;
    using Sitecore.Foundation.CommerceServer.Helpers;
    using Sitecore.Foundation.CommerceServer.Infrastructure.Constants;
    using Sitecore.Foundation.CommerceServer.Models;
    using Sitecore.Foundation.CommerceServer.Models.InputModels;
    using Sitecore.Foundation.CommerceServer.Utils;

    public class OrderManager : IOrderManager
    {
        private readonly OrderServiceProvider _orderServiceProvider;
        private readonly ICartManager _cartManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderManager" /> class.
        /// </summary>
        /// <param name="orderServiceProvider">The order service provider.</param>
        /// <param name="cartManager">The cart manager.</param>
        public OrderManager(
             CommerceOrderServiceProvider orderServiceProvider,
             ICartManager cartManager)
        {
            Assert.ArgumentNotNull(orderServiceProvider, "orderServiceProvider");
            Assert.ArgumentNotNull(cartManager, "cartManager");

            this._orderServiceProvider = orderServiceProvider;
            this._cartManager = cartManager;
        }

        /// <summary>
        /// Submits the visitor order.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="visitorContext">The visitor context.</param>
        /// <param name="inputModel">The input model.</param>
        /// <returns>
        /// The manager response where the new CommerceOrder is returned in the Result.
        /// </returns>
        public ManagerResponse<SubmitVisitorOrderResult, CommerceOrder> SubmitVisitorOrder([NotNull] CommerceStorefront storefront, [NotNull] IVisitorContext visitorContext, [NotNull] SubmitOrderInputModel inputModel)
        {
            Assert.ArgumentNotNull(storefront, "storefront");
            Assert.ArgumentNotNull(visitorContext, "visitorContext");
            Assert.ArgumentNotNull(inputModel, "inputModel");

            SubmitVisitorOrderResult errorResult = new SubmitVisitorOrderResult { Success = false };

            var response = this._cartManager.GetCurrentCart(storefront, visitorContext, true);
            if (!response.ServiceProviderResult.Success || response.Result == null)
            {
                response.ServiceProviderResult.SystemMessages.ToList().ForEach(m => errorResult.SystemMessages.Add(m));
                return new ManagerResponse<SubmitVisitorOrderResult, CommerceOrder>(errorResult, null);
            }

            var cart = (CommerceCart)response.ServiceProviderResult.Cart;

            if (cart.Lines.Count == 0)
            {
                errorResult.SystemMessages.Add(new SystemMessage { Message = StorefrontManager.GetSystemMessage(StorefrontConstants.SystemMessages.SubmitOrderHasEmptyCart) });
                return new ManagerResponse<SubmitVisitorOrderResult, CommerceOrder>(errorResult, null);
            }

            var cartChanges = new CommerceCart();

            cartChanges.Properties["Email"] = inputModel.UserEmail;

            var updateCartResult = this._cartManager.UpdateCart(storefront, visitorContext, cart, cartChanges);
            if (!updateCartResult.ServiceProviderResult.Success)
            {
                response.ServiceProviderResult.SystemMessages.ToList().ForEach(m => errorResult.SystemMessages.Add(m));
                return new ManagerResponse<SubmitVisitorOrderResult, CommerceOrder>(errorResult, null);
            }

            var request = new SubmitVisitorOrderRequest(cart);
            request.RefreshCart(true);
            errorResult = this._orderServiceProvider.SubmitVisitorOrder(request);
            if (errorResult.Success && errorResult.Order != null && errorResult.CartWithErrors == null)
            {
                var cartCache = ContextTypeLoader.CreateInstance<CartCacheHelper>();
                cartCache.InvalidateCartCache(visitorContext.GetCustomerId());
            }

            //Helpers.LogSystemMessages(errorResult.SystemMessages, errorResult);
            return new ManagerResponse<SubmitVisitorOrderResult, CommerceOrder>(errorResult, errorResult.Order as CommerceOrder);
        }

        /// <summary>
        /// Gets the available states.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="visitorContext">The visitor context.</param>
        /// <param name="countryCode">The country code.</param>
        /// <returns>
        /// The manager response where the available states are returned in the Result.
        /// </returns>
        public ManagerResponse<GetAvailableRegionsResult, Dictionary<string, string>> GetAvailableRegions([NotNull] CommerceStorefront storefront, [NotNull] IVisitorContext visitorContext, [NotNull] string countryCode)
        {
            Assert.ArgumentNotNull(storefront, "storefront");
            Assert.ArgumentNotNull(visitorContext, "visitorContext");
            Assert.ArgumentNotNullOrEmpty(countryCode, "countryCode");

            var request = new GetAvailableRegionsRequest(countryCode);
            var result = ((CommerceOrderServiceProvider)this._orderServiceProvider).GetAvailableRegions(request);

            //Helpers.LogSystemMessages(result.SystemMessages, result);
            return new ManagerResponse<GetAvailableRegionsResult, Dictionary<string, string>>(result, new Dictionary<string, string>(result.AvailableRegions));
        }

        /// <summary>
        /// Gets the orders.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <param name="shopName">Name of the shop.</param>
        /// <returns>The manager response where list of order headers are returned in the Result.</returns>
        public ManagerResponse<GetVisitorOrdersResult, IEnumerable<OrderHeader>> GetOrders(string customerId, string shopName)
        {
            Assert.ArgumentNotNullOrEmpty(customerId, "customerId");
            Assert.ArgumentNotNullOrEmpty(shopName, "shopName");

            var request = new GetVisitorOrdersRequest(customerId, shopName);
            var result = this._orderServiceProvider.GetVisitorOrders(request);
            if (result.Success && result.OrderHeaders != null && result.OrderHeaders.Count > 0)
            {
                return new ManagerResponse<GetVisitorOrdersResult, IEnumerable<OrderHeader>>(result, result.OrderHeaders.ToList());
            }

            //Helpers.LogSystemMessages(result.SystemMessages, result);
            return new ManagerResponse<GetVisitorOrdersResult, IEnumerable<OrderHeader>>(result, new List<OrderHeader>());
        }

        /// <summary>
        /// Gets the order details.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="visitorContext">The visitor context.</param>
        /// <param name="orderId">The order identifier.</param>
        /// <returns>
        /// The manager response where the order detail returned in the Result.
        /// </returns>
        public ManagerResponse<GetVisitorOrderResult, CommerceOrder> GetOrderDetails([NotNull] CommerceStorefront storefront, [NotNull] IVisitorContext visitorContext, [NotNull] string orderId)
        {
            Assert.ArgumentNotNull(storefront, "storefront");
            Assert.ArgumentNotNull(visitorContext, "visitorContext");
            Assert.ArgumentNotNullOrEmpty(orderId, "orderId");

            var customerId = visitorContext.GetCustomerId();
            var request = new GetVisitorOrderRequest(orderId, customerId, storefront.ShopName);
            var result = this._orderServiceProvider.GetVisitorOrder(request);

            //Helpers.LogSystemMessages(result.SystemMessages, result);
            return new ManagerResponse<GetVisitorOrderResult, CommerceOrder>(result, result.Order as CommerceOrder);
        }

        /// <summary>
        /// Gets the available countries.
        /// </summary>
        /// <returns>The manager response where the available country dictionary is returned in the Result.</returns>
        public ManagerResponse<GetAvailableCountriesResult, Dictionary<string, string>> GetAvailableCountries()
        {
            var request = new GetAvailableCountriesRequest();
            var result = ((CommerceOrderServiceProvider)this._orderServiceProvider).GetAvailableCountries(request);

            //Helpers.LogSystemMessages(result.SystemMessages, result);
            return new ManagerResponse<GetAvailableCountriesResult, Dictionary<string, string>>(result, new Dictionary<string, string>(result.AvailableCountries));
        }
    }
}
