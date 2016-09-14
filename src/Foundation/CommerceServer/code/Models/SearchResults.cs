namespace Sitecore.Foundation.CommerceServer.Models
{
    using Sitecore.Data.Items;
    using Sitecore.Foundation.CommerceServer.Search.Models;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Used to represent a search result items
    /// </summary>
    public class SearchResults
    {
        private List<Item> _searchResultItems;
        private IEnumerable<CommerceQueryFacet> _facets;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchResults" /> class
        /// </summary>
        public SearchResults()
            : this(null, 0, 0, 0, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchResults" /> class
        /// </summary>
        /// <param name="searchResultItems">The search result items.</param>
        /// <param name="totalItemCount">The total number of search result items.</param>
        /// <param name="totalPageCount">The total number of pages.</param>
        /// <param name="currentPageNumber">The current page number.</param>
        /// <param name="facets">The facets for the collection of search result items.</param>
        public SearchResults(List<Item> searchResultItems, int totalItemCount, int totalPageCount, int currentPageNumber, IEnumerable<CommerceQueryFacet> facets)
        {
            this.SearchResultItems = searchResultItems ?? new List<Item>();
            this.TotalPageCount = totalPageCount;
            this.TotalItemCount = totalItemCount;
            this.Facets = facets ?? Enumerable.Empty<CommerceQueryFacet>();
            this.CurrentPageNumber = currentPageNumber;
        }

        /// <summary>
        /// Gets or sets the display name to show
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the named search item.
        /// </summary>
        /// <value>
        /// The named search item.
        /// </value>
        public Item NamedSearchItem { get; set; }

        /// <summary>
        /// Gets or sets the items for the results
        /// </summary>
        public List<Item> SearchResultItems
        {
            get
            {
                return this._searchResultItems;
            }

            set
            {
                Sitecore.Diagnostics.Assert.ArgumentNotNull(value, "value");
                this._searchResultItems = value;
            }
        }

        /// <summary>
        /// Gets or sets the total item count
        /// </summary>
        public int TotalItemCount { get; set; }

        /// <summary>
        /// Gets or sets the total page count
        /// </summary>
        public int TotalPageCount { get; set; }

        /// <summary>
        /// Gets or sets the collection of facets for the collection of search results
        /// </summary>
        public IEnumerable<CommerceQueryFacet> Facets
        {
            get
            {
                return this._facets;
            }

            set
            {
                Sitecore.Diagnostics.Assert.ArgumentNotNull(value, "value");
                this._facets = value;
            }
        }

        /// <summary>
        /// Gets or sets the current page number
        /// </summary>
        public int CurrentPageNumber { get; set; }
    }
}
