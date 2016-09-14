namespace Sitecore.Foundation.CommerceServer.Managers
{
    using Sitecore.Data.Items;
    using Sitecore.Foundation.CommerceServer.Interfaces;
    using Sitecore.Foundation.CommerceServer.Utils;
    using System;
    using System.Globalization;
    using System.Web;

    /// <summary>
    /// The MetadataManager class definition.
    /// </summary>
    public static class MetadataManager
    {
        /// <summary>
        /// Gets the tags.
        /// </summary>
        /// <returns>The Html metadata tags if necessary.</returns>
        public static HtmlString GetTags()
        {
            var siteContext = ContextTypeLoader.CreateInstance<ISiteContext>();

            if (siteContext.IsCategory)
            {
                return new HtmlString(GetCategoryTags(siteContext.CurrentCatalogItem));
            }
            else if (siteContext.IsProduct)
            {
                return new HtmlString(GetProductTags(siteContext.CurrentCatalogItem));
            }

            return new HtmlString(string.Empty);
        }

        /// <summary>
        /// Gets the category tags.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>Returns the metadata tags for the given category.</returns>
        private static string GetCategoryTags(Item item)
        {
            return String.Format(CultureInfo.InvariantCulture, "<link rel='canonical' href='{0}'/>", GetServerUrl() + "/category/" + item.Name);
        }

        /// <summary>
        /// Gets the product tags.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>The metadata tags for the given product item.</returns>
        private static string GetProductTags(Item item)
        {
            return String.Format(CultureInfo.InvariantCulture, "<link rel='canonical' href='{0}'/>", GetServerUrl() + "/product/" + item.Name);
        }

        /// <summary>
        /// Gets the server URL.
        /// </summary>
        /// <returns>Returns the url as an Http link even if we are browsing as Https.</returns>
        private static string GetServerUrl()
        {
            string serverUrl = Web.WebUtil.GetServerUrl();
            if (serverUrl.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                serverUrl = serverUrl.Replace(serverUrl.Substring(0, 8), "http://");
            }

            return serverUrl;
        }
    }
}
