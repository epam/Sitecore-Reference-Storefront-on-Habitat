namespace Sitecore.Foundation.CommerceServer.Models
{
    using System.Web;
    using System.Linq;
    using Sitecore.Data.Items;

    /// <summary>
    /// Base class for Sitecore Item wrappers
    /// </summary>
    public class SitecoreItemBase
    {
        /// <summary>
        /// The _item
        /// </summary>
        private Item _item;

        /// <summary>
        /// Gets or sets the inner item.
        /// </summary>
        /// <value>
        /// The inner item.
        /// </value>
        public Item InnerItem
        {
            get
            {
                return this._item;
            }

            set
            {
                this._item = value;
            }
        }

        /// <summary>
        /// Gets the Id for this Item
        /// </summary>
        public string Id
        {
            get { return this._item.ID.ToShortID().ToString(); }
        }

        /// <summary>
        /// Gets the field with default.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The field value or the defaultValue if the item is null.</returns>
        public string GetFieldWithDefault(string fieldName, string defaultValue)
        {
            return this._item == null ? defaultValue : this._item[fieldName];
        }
    }
}