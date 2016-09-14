using System.Linq;
using Sitecore.Foundation.CommerceServer.Search.Models;

namespace Sitecore.Foundation.CommerceServer.Extensions
{
    using Sitecore.ContentSearch.Linq;

    /// <summary>
    /// Extension methods to help with some search functionality
    /// </summary>
    public static class SearchExtensions
    {
        /// <summary>
        /// Removes invalid facet values from a facet
        /// </summary>
        /// <param name="facet">The facet to clean</param>
        public static void Clean(this CommerceQueryFacet facet)
        {
            if (facet.FoundValues != null)
            {
                var items = facet.FoundValues.Where(v => string.IsNullOrEmpty(v.Name) || v.AggregateCount == 0);
                items.ToList().ForEach(v => facet.FoundValues.Remove(v));
            }
        }

        /// <summary>
        /// Checks to make sure a facet is valid for use
        /// </summary>
        /// <param name="facet">The facet to check</param>
        /// <returns>Returns True if valid and False otherwise</returns>
        public static bool IsValid(this CommerceQueryFacet facet)
        {
            facet.Clean();

            if (facet.FoundValues != null && facet.FoundValues.Count > 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks to make sure a facet value is valid for use
        /// </summary>
        /// <param name="value">The facet value to check</param>
        /// <returns>Returns True if valid and False otherwise</returns>
        public static bool IsValid(this FacetValue value)
        {
            if (value.AggregateCount > 0)
            {
                return true;
            }

            return false;
        }
    }
}