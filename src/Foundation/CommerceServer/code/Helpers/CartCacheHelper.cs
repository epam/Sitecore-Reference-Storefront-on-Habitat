namespace Sitecore.Foundation.CommerceServer.Helpers
{
    using System;
    using System.Globalization;
    using Sitecore;
    using Sitecore.Commerce.Connect.CommerceServer;
    using Sitecore.Commerce.Connect.CommerceServer.Caching;
    using Sitecore.Commerce.Connect.CommerceServer.Orders.Models;
    using Sitecore.Diagnostics;
    using Sitecore.Foundation.CommerceServer.Utils;

    // TODO: The duplicate of Sitecore.Feature.Cart.Helpers.CartCacheHelper. Delete when refactor.
    /// <summary>
    /// Cart Cache Helper
    /// </summary>
    /// <remarks>
    /// As constantly getting a basket from Commerce server can be expensive, this cache can be used as a way to mitigate getting a cart.
    /// When a cart for a customer is created, it will be placed into the cache.  When operations are performed on the cart, the cahce is invalidated for that
    /// customer cart, if it exists in the cahce.
    /// </remarks>
    public class CartCacheHelper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CartCacheHelper"/> class.
        /// </summary>
        /// TODO:remove unused
        public CartCacheHelper()
        {
        }

        /// <summary>
        /// Invalidates the cart cache.
        /// </summary>
        /// <param name="customerId">the customer id</param>
        public virtual void InvalidateCartCache([NotNull]string customerId)
        {
            var cacheProvider = GetCacheProvider();
            var id = this.GetCustomerId(customerId);

            if (!cacheProvider.Contains(Infrastructure.Constants.CommerceConstants.KnownCachePrefixes.Sitecore, Infrastructure.Constants.CommerceConstants.KnownCacheNames.CommerceCartCache, id))
            {
                var msg = string.Format(CultureInfo.InvariantCulture, "CartCacheHelper::InvalidateCartCache - Cart for customer id {0} is not in the cache!", id);
                CommerceTrace.Current.Write(msg);
            }

            cacheProvider.RemoveData(Infrastructure.Constants.CommerceConstants.KnownCachePrefixes.Sitecore, Infrastructure.Constants.CommerceConstants.KnownCacheNames.CommerceCartCache, id);

            CartCookieHelper.DeleteCartCookieForCustomer(id);
        }

        /// <summary>
        /// Adds the cart to cache.
        /// </summary>
        /// <param name="cart">The cart.</param>
        public virtual void AddCartToCache(CommerceCart cart)
        {
            var cacheProvider = GetCacheProvider();
            var id = this.GetCustomerId(cart.CustomerId);

            if (cacheProvider.Contains(Infrastructure.Constants.CommerceConstants.KnownCachePrefixes.Sitecore, Infrastructure.Constants.CommerceConstants.KnownCacheNames.CommerceCartCache, id))
            {
                var msg = string.Format(CultureInfo.InvariantCulture, "CartCacheHelper::AddCartToCache - Cart for customer id {0} is already in the cache!", id);
                CommerceTrace.Current.Write(msg);
            }

            cacheProvider.AddData(Infrastructure.Constants.CommerceConstants.KnownCachePrefixes.Sitecore, Infrastructure.Constants.CommerceConstants.KnownCacheNames.CommerceCartCache, id, cart);
            CartCookieHelper.CreateCartCookieForCustomer(id);
        }

        /// <summary>
        /// Gets a cart from the cache.
        /// </summary>
        /// <param name="customerId">the customer id</param>
        /// <returns>A Cart. Returns null if no cart is in the cache</returns>
        public virtual CommerceCart GetCart([NotNull]string customerId)
        {
            var cacheProvider = GetCacheProvider();

            string id = this.GetCustomerId(customerId);

            var cart = cacheProvider.GetData<CommerceCart>(Infrastructure.Constants.CommerceConstants.KnownCachePrefixes.Sitecore, Infrastructure.Constants.CommerceConstants.KnownCacheNames.CommerceCartCache, id);

            if (cart == null)
            {
                var msg = string.Format(CultureInfo.InvariantCulture, "CartCacheHelper::GetCart - Cart for customerId {0} does not exist in the cache!", id);
                CommerceTrace.Current.Write(msg);
            }

            return cart;
        }

        /// <summary>
        /// Gets the customer identifier.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <returns>The customer id string.</returns>
        protected virtual string GetCustomerId([NotNull] string customerId)
        {
            Guid csCustomerId;
            string id;
            if (Guid.TryParse(customerId, out csCustomerId))
            {
                id = Guid.Parse(customerId).ToString("D");
            }
            else
            {
                id = customerId;
            }

            return id;
        }

        /// <summary>
        /// Gets the cache provider
        /// </summary>
        /// <returns>the cahce provider</returns>
        private static ICacheProvider GetCacheProvider()
        {
            var cacheProvider = ContextTypeLoader.GetCacheProvider(Infrastructure.Constants.CommerceConstants.KnownCacheNames.CommerceCartCache);
            Assert.IsNotNull(cacheProvider, "cacheProvider");

            return cacheProvider;
        }
    }
}