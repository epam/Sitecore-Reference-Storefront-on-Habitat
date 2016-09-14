namespace Sitecore.Foundation.CommerceServer.Models
{
    using Infrastructure.Constants;
    using Sitecore.Data.Items;

    /// <summary>
    /// CommercePromotion class
    /// </summary>
    public class Catalog : SitecoreItemBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Catalog"/> class.
        /// </summary>
        /// <param name="item">The item.</param>
        public Catalog(Item item)
        {
            this.InnerItem = item;
        }

        /// <summary>
        /// Gets the Name of the item
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name
        {
            get { return this.InnerItem.Name; }
        }

        /// <summary>
        /// The Title of the Create Wish List Page
        /// </summary>
        /// <returns>The title.</returns>
        public string Title()
        {
            return this.InnerItem[StorefrontConstants.ItemFields.Title];
        }

        /// <summary>
        /// Label for the Wish List Name field
        /// </summary>
        /// <returns>The name title.</returns>
        public string NameTitle()
        {
            return this.InnerItem["Name Title"];
        }
    }
}