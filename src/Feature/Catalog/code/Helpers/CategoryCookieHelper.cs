namespace Sitecore.Feature.Catalog.Helpers
{
    using System;
    using System.Web;

    /// <summary>
    /// Defines the CategoryCookieHelper class.
    /// </summary>
    public static class CategoryCookieHelper
    {
        private const string CookieName = "_lastVisitedCategory";
        private const string VisitorIdKey = "VisitorId";
        private const string CategoryIdKey = "CategoryId";

        /// <summary>
        /// Gets the last visited category.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <returns>The last visited category id or empty sting.</returns>
        public static string GetLastVisitedCategory(string customerId)
        {
            var categoryCookie = HttpContext.Current.Request.Cookies[CookieName];

            return categoryCookie != null && categoryCookie[VisitorIdKey] != null && categoryCookie[VisitorIdKey].Equals(customerId, StringComparison.OrdinalIgnoreCase) ? categoryCookie[CategoryIdKey] : string.Empty;
        }

        /// <summary>
        /// Sets the last visited category.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <param name="categoryId">The category identifier.</param>
        public static void SetLastVisitedCategory(string customerId, string categoryId)
        {
            // The cookie does not defined an expiry date therefore the browser will not persist it.
            var categoryCookie = HttpContext.Current.Request.Cookies[CookieName] ?? new HttpCookie(CookieName);
            categoryCookie.Values[VisitorIdKey] = customerId;
            categoryCookie.Values[CategoryIdKey] = categoryId;
            HttpContext.Current.Response.Cookies.Add(categoryCookie);
        }
    }
}