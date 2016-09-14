namespace Sitecore.Foundation.CommerceServer.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using Sitecore.Mvc.Presentation;

    /// <summary>
    /// Used to represent a product search result item
    /// </summary>
    public class MultipleProductSearchResults : Sitecore.Mvc.Presentation.RenderingModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MultipleProductSearchResults"/> class.
        /// </summary>
        /// <param name="productSearchResults">The productSearchResults to init the item with</param>
        public MultipleProductSearchResults(List<SearchResults> productSearchResults)
        {
            SearchResults = productSearchResults;
        }

        /// <summary>
        /// Gets or sets the Displayname to show
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets the ProductSearchResults items for the results
        /// </summary>
        public List<SearchResults> SearchResults { get; private set; }

        /// <summary>
        /// Gets or sets the product results.
        /// </summary>
        /// <value>
        /// The product results.
        /// </value>
        public List<ProductSearchResultViewModel> ProductSearchResults { get; set; }

        /// <summary>
        /// Initializes the specified rendering.
        /// </summary>
        /// <param name="rendering">The rendering.</param>
        public override void Initialize(Rendering rendering)
        {
            base.Initialize(rendering);

            this.ProductSearchResults = new List<ProductSearchResultViewModel>();
            if (this.SearchResults == null || !this.SearchResults.Any())
            {
                return;
            }

            foreach (var searchResult in this.SearchResults)
            {
                var productSearchResultModel = new ProductSearchResultViewModel();
                productSearchResultModel.Initialize(this.Rendering, searchResult);
                this.ProductSearchResults.Add(productSearchResultModel);
            }
        }
    }
}