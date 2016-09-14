namespace Sitecore.Foundation.CommerceServer.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// The model to display related catalog items.
    /// </summary>
    public class RelatedCatalogItemsViewModel : Sitecore.Mvc.Presentation.RenderingModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RelatedCatalogItemsViewModel" /> class.
        /// </summary>
        public RelatedCatalogItemsViewModel()
        {
            this.RelatedProducts = new List<CategoryViewModel>();
            this.RelatedCategories = new List<CategoryViewModel>();
        }

        /// <summary>
        /// Gets the list of related products.
        /// </summary>
        public List<CategoryViewModel> RelatedProducts { get; private set; }

        /// <summary>
        /// Gets the list of related categories.
        /// </summary>
        public List<CategoryViewModel> RelatedCategories { get; private set; }
    }
}