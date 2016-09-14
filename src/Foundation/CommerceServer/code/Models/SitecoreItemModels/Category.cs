namespace Sitecore.Foundation.CommerceServer.Models.SitecoreItemModels
{
    using Infrastructure.Constants;
    using Sitecore.Data.Items;
    using Sitecore.Foundation.CommerceServer.Search.Models;
    using System.Collections.Generic;

    /// <summary>
    /// Category class.
    /// </summary>
    public class Category : SitecoreItemBase
    {
        private int _itemsPerPage;

        /// <summary>
        /// Initializes a new instance of the <see cref="Category"/> class.
        /// </summary>
        /// <param name="item">The item.</param>
        public Category(Item item)
        {
            this.InnerItem = item;
        }

        /// <summary>
        /// Gets the Name of the Item
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name
        {
            get { return this.InnerItem.Name; }
        }

        /// <summary>
        /// Gets the DisplayName of the Item
        /// </summary>
        /// <value>
        /// The display name.
        /// </value>
        public string DisplayName
        {
            get
            {
                return this.InnerItem.DisplayName;
            }
        }

        /// <summary>
        /// Gets or sets the Required Facets
        /// </summary>
        public IEnumerable<CommerceQueryFacet> RequiredFacets { get; set; }

        /// <summary>
        /// Gets or sets the Sort Fields
        /// </summary>
        public IEnumerable<CommerceQuerySort> SortFields { get; set; }

        /// <summary>
        /// Gets or sets the Items per page
        /// </summary>
        public int ItemsPerPage
        {
            get
            {
                return _itemsPerPage == 0 ? StorefrontConstants.Settings.DefaultItemsPerPage : _itemsPerPage;
            }

            set
            {
                _itemsPerPage = value;
            }
        }

        /// <summary>
        /// Label for the Category field
        /// </summary>
        /// <returns>The name title.</returns>
        public string NameTitle()
        {
            return this.InnerItem["Name Title"];
        }

        /// <summary>
        /// The Title of the Category Page
        /// </summary>
        /// <returns>The title.</returns>
        public string Title()
        {
            return this.InnerItem[StorefrontConstants.ItemFields.Title];
        }
    }
}