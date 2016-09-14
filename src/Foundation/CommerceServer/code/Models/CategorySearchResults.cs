namespace Sitecore.Foundation.CommerceServer.Models
{
    using System.Collections.Generic;

    using Sitecore.ContentSearch.Linq;
    using Sitecore.Data.Items;

    /// <summary>
    /// Used to represent a category search result item
    /// </summary>
    public class CategorySearchResults
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CategorySearchResults" /> class
        /// </summary>
        /// <param name="categoryItems">The products to init the item with</param>
        /// <param name="totalCategoryCount">The total number of categories</param>
        /// <param name="totalPageCount">The total number of pages</param>
        /// <param name="currentPageNumber">The current page number</param>
        /// <param name="facets">The facets for the collection of categories</param>
        public CategorySearchResults(List<Item> categoryItems, int totalCategoryCount, int totalPageCount, int currentPageNumber, List<FacetCategory> facets)
        {
            this.CategoryItems = categoryItems;
            this.TotalPageCount = totalPageCount;
            this.TotalCategoryCount = totalCategoryCount;
            this.Facets = facets;
            this.CurrentPageNumber = currentPageNumber;
        }

        /// <summary>
        /// Gets the list of items the results are based on
        /// </summary>
        public List<Item> CategoryItems { get; private set; }

        /// <summary>
        /// Gets the total number of categories
        /// </summary>
        public int TotalCategoryCount { get; private set; }

        /// <summary>
        /// Gets the total page count
        /// </summary>
        public int TotalPageCount { get; private set; }

        /// <summary>
        /// Gets the collection of facets for the collection of categories
        /// </summary>        
        public List<FacetCategory> Facets { get; private set; }

        /// <summary>
        /// Gets the current page number
        /// </summary>
        public int CurrentPageNumber { get; private set; }
    }
}