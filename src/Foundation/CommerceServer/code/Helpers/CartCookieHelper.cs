namespace Sitecore.Foundation.CommerceServer.Helpers
{
    using System;
    using System.Web;

    /// <summary>
    /// Cart Cookie Helper
    /// </summary>
    /// <remarks>
    /// As constantly getting a basket from Commerce server can be expensive, this cache can be used as a way to mitigate getting a cart.
    /// When a cart for a customer is created, it will be placed into the cache.  When operations are performed on the cart, the cahce is invalidated for that
    /// customer cart, if it exists in the cahce.
    /// </remarks>
    public static class CartCookieHelper
    {
        /// <summary>
        /// mini cart cookie name
        /// </summary>
        public const string CookieName = "_minicart";

        /// <summary>
        /// visitorId for the cart cookie
        /// </summary>
        public const string VisitorIdKey = "VisitorId";

        /// <summary>
        /// cookie expiration time in days
        /// </summary>
        private const int CookieExpirationInDays = 365;

        /// <summary>
        /// Does the cart cookie exist for the given customer
        /// </summary>
        /// <param name="customerId">the customer id</param>
        /// <returns>true if the cookie exists</returns>
        public static bool DoesCookieExistForCustomer(string customerId)
        {
            var cartCookie = HttpContext.Current.Request.Cookies[CookieName];

            return cartCookie != null && cartCookie.Values[VisitorIdKey] == customerId;
        }

        /// <summary>
        /// Creates the cart cookie for the customer
        /// </summary>
        /// <param name="customerId">the customer id</param>
        public static void CreateCartCookieForCustomer(string customerId)
        {
            var cartCookie = HttpContext.Current.Request.Cookies[CookieName] ?? new HttpCookie(CookieName);
            cartCookie.Values[VisitorIdKey] = customerId;
            cartCookie.Expires = DateTime.Now.AddDays(CookieExpirationInDays);
            HttpContext.Current.Response.Cookies.Add(cartCookie);
        }

        /// <summary>
        /// Deletes the cart cookie for the customer
        /// </summary>
        /// <param name="customerId">the customer id</param>
        /// <returns>true if the cookie was deleted</returns>
        public static bool DeleteCartCookieForCustomer(string customerId)
        {
            var cartCookie = HttpContext.Current.Request.Cookies[CookieName];
            if (cartCookie == null)
            {
                return false;
            }

            // invalidate the cookie
            HttpContext.Current.Response.Cookies.Remove(CookieName);
            cartCookie.Expires = DateTime.Now.AddDays(-10);
            cartCookie.Values[VisitorIdKey] = null;
            cartCookie.Value = null;
            HttpContext.Current.Response.SetCookie(cartCookie);

            return true;
        }
    }
}