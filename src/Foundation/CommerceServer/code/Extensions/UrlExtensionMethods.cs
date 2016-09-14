namespace Sitecore.Foundation.CommerceServer.Extensions
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Sitecore.Foundation.CommerceServer.Collections;
    using Sitecore.Foundation.CommerceServer.Helpers;
    using Sitecore.Foundation.CommerceServer.Infrastructure.Constants;

    /// <summary>
    /// Extension methods for the MVC UrlHelper class
    /// </summary>
    public static class UrlExtensionMethods
    {
        /// <summary>
        /// Creates a url for a facet
        /// </summary>
        /// <param name="helper">The helper class we are extending from</param>
        /// <param name="facetName">The name of the facet</param>
        /// <param name="facetValue">The value of the facet</param>
        /// <returns>The new facet url</returns>
        //TODO:remove unused
        public static string AddToFacets(this UrlHelper helper, string facetName, string facetValue)
        {
            var currentUrl = UrlBuilder.CurrentUrl;
            var facetQuery = currentUrl.QueryList[StorefrontConstants.QueryStrings.Facets];
            var facetQueryString = GetFacetQueryString(facetQuery, facetName, facetValue);

            if (!string.IsNullOrEmpty(facetQueryString))
            {
                currentUrl.QueryList.AddOrSet(StorefrontConstants.QueryStrings.Facets, HttpUtility.UrlDecode(facetQueryString).Remove(0, 1));
            }
            else
            {
                currentUrl.QueryList.Remove(StorefrontConstants.QueryStrings.Facets);
            }

            currentUrl.QueryList.Remove(StorefrontConstants.QueryStrings.Paging);

            return currentUrl.ToString(true);
        }

        /// <summary>
        /// Creates a url for a page number.
        /// </summary>
        /// <param name="helper">The helper class we are extending from</param>
        /// <param name="page">The number of the new page to point to</param>
        /// <param name="queryStringToken">The query string token.</param>
        /// <returns>
        /// The new page number url
        /// </returns>
        /// TODO:remove unused
        public static string AddPageNumber(this UrlHelper helper, int page, string queryStringToken)
        {
            var current = UrlBuilder.CurrentUrl;
            current.QueryList.AddOrSet(queryStringToken, page.ToString(CultureInfo.InvariantCulture));

            CleanNestedCollections(current);

            var url = current.ToString(true);

            return url;
        }

        /// <summary>
        /// Creates an query string for a facet
        /// </summary>
        /// <param name="facetQuery">The facet query.</param>
        /// <param name="facetName">Name of the facet.</param>
        /// <param name="facetValue">The facet value.</param>
        /// <returns>The query string for the facet.</returns>
        private static string GetFacetQueryString(string facetQuery, string facetName, string facetValue)
        {
            var facetCollection = new QueryStringCollection();

            if (!string.IsNullOrEmpty(facetQuery))
            {
                facetCollection.Parse(HttpUtility.UrlDecode(facetQuery));
            }

            if (facetCollection.Contains(facetName))
            {
                var facetQueryValues = facetCollection[facetName];

                if (facetQueryValues.Contains(facetValue))
                {
                    var newFacetQueryValues = string.Empty;
                    var facetValues = facetQueryValues.Split('|').Where(p => !string.Equals(p, facetValue, StringComparison.OrdinalIgnoreCase)).ToList();

                    if (facetValues.Count() > 0)
                    {
                        facetCollection.Set(facetName, string.Join("|", facetValues));
                    }
                    else
                    {
                        facetCollection.Remove(facetName);
                    }
                }
                else
                {
                    facetCollection.Set(facetName, facetQueryValues + StorefrontConstants.QueryStrings.FacetsSeparator + facetValue);
                }
            }
            else
            {
                facetCollection.Add(facetName, facetValue);
            }

            return facetCollection.ToString();
        }

        private static void CleanNestedCollections(UrlBuilder current)
        {
            var facetQuery = current.QueryList[StorefrontConstants.QueryStrings.Facets];

            if (facetQuery != null)
            {
                var decodedFacets = HttpUtility.UrlDecode(facetQuery);
                current.QueryList.AddOrSet(StorefrontConstants.QueryStrings.Facets, decodedFacets);
            }
        }
    }
}