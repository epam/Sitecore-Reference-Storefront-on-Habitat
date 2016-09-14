using Sitecore.Data;

namespace Sitecore.Feature.Navigation.Models
{
    using Foundation.CommerceServer.Infrastructure.Constants;
    using Foundation.CommerceServer.Models;
    using Sitecore.Data.Items;
    using Sitecore.Mvc.Presentation;
    using System.Collections.Generic;
    
    /// <summary>
    /// Used to represent the navigation
    /// </summary>
    public class NavigationViewModel : Sitecore.Mvc.Presentation.RenderingModel
    {
        /// <summary>
        /// Id of the current category.
        /// </summary>
        public ID CategoryId { get; private set; } 

        /// <summary>
        /// Gets the list of child categories
        /// </summary>
        public List<Item> ChildCategories
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the title of the store
        /// </summary>
        public string StoreTitle
        {
            get
            {
                var home = Context.Database.GetItem(Context.Site.RootPath + Context.Site.StartItem);
                return home[StorefrontConstants.ItemFields.Title];
            }
        }

        /// <summary>
        /// Initializes the view model
        /// </summary>
        /// <param name="rendering">The rendering</param>
        /// <param name="childCategories">The list of child categories</param>
        /// <param name="categoryId">Id of the category.</param>
        public void Initialize(Rendering rendering, CategorySearchResults childCategories, ID categoryId)
        {
            base.Initialize(rendering);

            CategoryId = categoryId;

            if (childCategories != null)
            {
                ChildCategories = childCategories.CategoryItems;
            }
        }
    }
}