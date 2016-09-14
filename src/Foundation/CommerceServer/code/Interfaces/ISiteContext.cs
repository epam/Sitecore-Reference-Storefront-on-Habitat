namespace Sitecore.Foundation.CommerceServer.Interfaces
{
    using Sitecore.Data.Items;
    using System;

    /// <summary>
    /// Interface that represents the current site context.
    /// </summary>
    public interface ISiteContext
    {
        /// <summary>
        /// Gets the current HTTP context.
        /// </summary>
        System.Web.HttpContext CurrentContext { get; }

        /// <summary>
        /// Gets the current HTTP context items collection.
        /// </summary>
        System.Collections.IDictionary Items { get; }

        /// <summary>
        /// Gets or sets the current catalog item.
        /// </summary>
        /// <value>
        /// The current catalog item.
        /// </value>
        Item CurrentCatalogItem { get; set; }

        /// <summary>
        /// Gets a value indicating whether the instance of the CurrentCatalogItem is a category.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is category; otherwise, <c>false</c>.
        /// </value>
        bool IsCategory { get; }

        /// <summary>
        /// Gets a value indicating whether the instance of the CurrentCatalogItem is product.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is product; otherwise, <c>false</c>.
        /// </value>
        bool IsProduct { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the current url contains the category.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the url contains the category; otherwise, <c>false</c>.
        /// </value>
        bool UrlContainsCategory { get; set; }
    }
}
