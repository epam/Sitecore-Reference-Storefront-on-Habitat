namespace Sitecore.Feature.Catalog.Models
{
    using Foundation.CommerceServer.Models;
    using Sitecore.Foundation.CommerceServer.Search.Models;
    using Sitecore.Mvc.Presentation;
    using System.Collections.Generic;

    /// <summary>
    /// Used to represent a product facet list.
    /// </summary>
    public class ProductFacetsViewModel : RenderingModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductFacetsViewModel"/> class.
        /// </summary>
        public ProductFacetsViewModel()
        {
            this.ChildProductFacets = new List<CommerceQueryFacet>();
            this.ActiveFacets = new List<CommerceQueryFacet>();
        }

        /// <summary>
        /// Gets or sets the list of product facets.
        /// </summary>
        public IEnumerable<CommerceQueryFacet> ChildProductFacets
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the list of active facets.
        /// </summary>
        public IEnumerable<CommerceQueryFacet> ActiveFacets
        {
            get;
            protected set;
        }

        /// <summary>
        /// Initializes the view model.
        /// </summary>
        /// <param name="rendering">The rendering.</param>
        /// <param name="products">The list of child products.</param>
        /// <param name="searchOptions">Any search options used to find products in this category.</param>
        public void Initialize(Rendering rendering, SearchResults products, CommerceSearchOptions searchOptions)
        {
            base.Initialize(rendering);

            if (products != null)
            {
                this.ChildProductFacets = products.Facets;
            }

            this.ActiveFacets = searchOptions.FacetFields;
        }
    }
}