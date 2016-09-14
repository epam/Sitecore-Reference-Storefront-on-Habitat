namespace Sitecore.Feature.Catalog.Models
{
    using Foundation.CommerceServer.Infrastructure.Constants;
    using Foundation.CommerceServer.Models;
    using Sitecore.Foundation.CommerceServer.Search.Models;
    using Sitecore.Mvc.Presentation;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Used to represent a product list header
    /// </summary>
    public class ProductListHeaderViewModel : RenderingModel
    {
        public CommerceConstants.SortDirection? SelectedSortDirection { get; set; }

        public string SelecterSortField { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductListHeaderViewModel"/> class.
        /// </summary>
        public ProductListHeaderViewModel()
        {
            this.PageSizeClass = StorefrontConstants.StyleClasses.ChangePageSize;
        }

        /// <summary>
        /// Gets or sets the list of sortable fields
        /// </summary>
        public IEnumerable<CommerceQuerySort> SortFields
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the pagination details for this category
        /// </summary>
        public PaginationModel Pagination { get; set; }

        /// <summary>
        /// Gets or sets the name of the page size CSS class.
        /// </summary>
        public string PageSizeClass { get; set; }

        /// <summary>
        /// Initializes the view model
        /// </summary>
        /// <param name="rendering">The rendering</param>
        /// <param name="products">The list of child products</param>
        /// <param name="sortFields">The fields to allow sorting on</param>
        /// <param name="searchOptions">Any search options used to find products in this category</param>
        public void Initialize(Rendering rendering, SearchResults products, IEnumerable<CommerceQuerySort> sortFields, CommerceSearchOptions searchOptions)
        {
            base.Initialize(rendering);

            int itemsPerPage = searchOptions?.NumberOfItemsToReturn ?? 0;

            if (products != null)
            {
                var alreadyShown = products.CurrentPageNumber * searchOptions.NumberOfItemsToReturn;
                Pagination = new PaginationModel
                {
                    PageNumber = products.CurrentPageNumber,
                    TotalResultCount = products.TotalItemCount,
                    NumberOfPages = products.TotalPageCount,
                    PageResultCount = itemsPerPage,
                    StartResultIndex = alreadyShown + 1,
                    EndResultIndex = System.Math.Min(products.TotalItemCount, alreadyShown + itemsPerPage)
                };
            }

            SortFields = sortFields ?? Enumerable.Empty<CommerceQuerySort>();
        }
    }
}