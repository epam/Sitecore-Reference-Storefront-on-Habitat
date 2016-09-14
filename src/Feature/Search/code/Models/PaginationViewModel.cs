namespace Sitecore.Feature.Search.Models
{
    using Foundation.CommerceServer.Infrastructure.Constants;
    using Foundation.CommerceServer.Models;
    using Sitecore.Foundation.CommerceServer.Search.Models;
    using Sitecore.Mvc.Presentation;

    /// <summary>
    /// Used to represent a pagination
    /// </summary>
    public class PaginationViewModel : Sitecore.Mvc.Presentation.RenderingModel
    {
        /// <summary>
        /// Gets or sets the pagination details for this category
        /// </summary>
        public PaginationModel Pagination { get; set; }

        /// <summary>
        /// Gets or sets the paging querystring token.
        /// </summary>
        public string QueryStringToken { get; set; }

        /// <summary>
        /// Initializes the view model
        /// </summary>
        /// <param name="rendering">The rendering</param>
        /// <param name="products">The list of child products</param>
        /// <param name="searchOptions">Any search options used to find products in this category</param>
        public void Initialize(Rendering rendering, SearchResults products, CommerceSearchOptions searchOptions)
        {
            base.Initialize(rendering);
            this.QueryStringToken = StorefrontConstants.QueryStrings.Paging;

            int itemsPerPage = (searchOptions != null) ? searchOptions.NumberOfItemsToReturn : 20;

            if (products != null)
            {
                var alreadyShown = products.CurrentPageNumber * itemsPerPage;
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
        }
    }
}