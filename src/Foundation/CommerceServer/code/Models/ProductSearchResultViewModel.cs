namespace Sitecore.Foundation.CommerceServer.Models
{
    using System.Collections.Generic;
    using Sitecore.Mvc;
    using Sitecore.Mvc.Presentation;
    using Sitecore.Data.Items;
    using System.Web;
    using Infrastructure.Constants;
    /// <summary>
    /// Used to represent a product search result item
    /// </summary>
    public class ProductSearchResultViewModel : RenderingModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductSearchResultViewModel" /> class.
        /// </summary>
        public ProductSearchResultViewModel()
        {
            this.Products = new List<ProductViewModel>();
            this.DisplayName = string.Empty;
        }

        /// <summary>
        /// Gets or sets the Displayname to show
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
        /// Gets the named search item display name html render string.
        /// </summary>
        /// <value>
        /// The named search item display name render.
        /// </value>
        public HtmlString NamedSearchItemDisplayNameRender
        {
            get
            {
                return PageContext.Current.HtmlHelper.Sitecore().Field(StorefrontConstants.KnownFieldNames.Title, this.NamedSearchItem);
            }
        }

        /// <summary>
        /// Gets or sets the products.
        /// </summary>
        /// <value>
        /// The products.
        /// </value>
        public List<ProductViewModel> Products { get; set; }

        /// <summary>
        /// Initializes the specified rendering.
        /// </summary>
        /// <param name="rendering">The rendering.</param>
        /// <param name="searchResult">The search result.</param>
        public virtual void Initialize(Rendering rendering, SearchResults searchResult)
        {
            base.Initialize(rendering);

            if (searchResult == null)
            {
                return;
            }

            this.DisplayName = searchResult.DisplayName;
            this.NamedSearchItem = searchResult.NamedSearchItem;
            this.Products = new List<ProductViewModel>();
            foreach (var child in searchResult.SearchResultItems)
            {
                var productModel = new ProductViewModel(child);
                productModel.Initialize(this.Rendering);
                this.Products.Add(productModel);
            }
        }
    }
}