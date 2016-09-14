namespace Sitecore.Foundation.CommerceServer.Pipelines
{
    using Sitecore.Data.Items;
    using Sitecore.Diagnostics;
    using Sitecore.Foundation.CommerceServer.Extensions;
    using Sitecore.Foundation.CommerceServer.Infrastructure.Constants;
    using Sitecore.Foundation.CommerceServer.Managers;
    using Sitecore.Links;

    /// <summary>
    /// Class to help build dynamic urls
    /// </summary>
    public class CatalogLinkProvider : LinkProvider
    {
        /// <summary>
        /// The attribute name of the includeCatalogs setting.
        /// </summary>
        public const string IncludeCatalogsAttribute = "includeCatalog";

        /// <summary>
        /// The attribute name of the useShopLinks setting.
        /// </summary>
        public const string UseShopLinksAttribute = "useShopLinks";

        /// <summary>
        /// The attribute name of the includeFriendlyName setting.
        /// </summary>
        public const string IncludeFriendlyNameAttribute = "includeFriendlyName";

        /// <summary>
        /// The default value for the includeCatalogs setting.
        /// </summary>
        public const bool IncludeCatalogsDefault = false;

        /// <summary>
        /// The default value for the useShopLinks setting.
        /// </summary>
        public const bool UseShopLinksDefault = true;

        /// <summary>
        /// The default value for the includeFriendlyName setting.
        /// </summary>
        public const bool IncludeFriendlyNameDefault = true;

        /// <summary>
        /// Gets or sets a value indicating whether or not to include catalog names in item urls.
        /// </summary>
        public bool IncludeCatalog { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not shop links should be generated for item urls.
        /// </summary>
        public bool UseShopLinks { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether friendly names should be included in item urls.
        /// </summary>
        public bool IncludeFriendlyName { get; set; }

        /// <summary>
        /// Initializes the provider.
        /// </summary>
        /// <param name="name">The friendly name of the provider.</param>
        /// <param name="config">A collection of the name/value pairs representing the provider-specific attributes specified in the configuration for this provider.</param>
        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            base.Initialize(name, config);

            this.IncludeCatalog = MainUtil.GetBool(config[IncludeCatalogsAttribute], IncludeCatalogsDefault);
            this.UseShopLinks = MainUtil.GetBool(config[UseShopLinksAttribute], UseShopLinksDefault);
            this.IncludeFriendlyName = MainUtil.GetBool(config[IncludeFriendlyNameAttribute], IncludeFriendlyNameDefault);
        }

        /// <summary>
        /// This method returns the dynamicly generated URL based on the item type.
        /// </summary>
        /// <param name="item">The item</param>
        /// <param name="options">The options</param>
        /// <returns>The dynamically built URL</returns>
        public override string GetDynamicUrl(Item item, LinkUrlOptions options)
        {
            Assert.ArgumentNotNull(item, "item");
            Assert.ArgumentNotNull(options, "options");

            var url = string.Empty;
            var itemType = item.ItemType();

            bool productCatalogLinkRequired = Sitecore.Web.WebUtil.GetRawUrl().IndexOf(ProductItemResolver.NavigationItemName, System.StringComparison.OrdinalIgnoreCase) >= 0;
            if (productCatalogLinkRequired)
            {
                url = CatalogUrlManager.BuildProductCatalogLink(item);
            }
            else if (this.UseShopLinks)
            {
                if (itemType == StorefrontConstants.ItemTypes.Product)
                {
                    url = CatalogUrlManager.BuildProductShopLink(item, this.IncludeCatalog, this.IncludeFriendlyName, true);
                }
                else if (itemType == StorefrontConstants.ItemTypes.Category)
                {
                    url = CatalogUrlManager.BuildCategoryShopLink(item, this.IncludeCatalog, this.IncludeFriendlyName);
                }
                else if (itemType == StorefrontConstants.ItemTypes.Variant)
                {
                    url = CatalogUrlManager.BuildVariantShopLink(item, this.IncludeCatalog, this.IncludeFriendlyName, true);
                }
            }
            else
            {
                if (itemType == StorefrontConstants.ItemTypes.Product)
                {
                    url = CatalogUrlManager.BuildProductLink(item, this.IncludeCatalog, this.IncludeFriendlyName);
                }
                else if (itemType == StorefrontConstants.ItemTypes.Category)
                {
                    url = CatalogUrlManager.BuildCategoryLink(item, this.IncludeCatalog, this.IncludeFriendlyName);
                }
            }

            if (string.IsNullOrEmpty(url))
            {
                url = base.GetDynamicUrl(item, options);
            }

            return url;
        }
    }
}