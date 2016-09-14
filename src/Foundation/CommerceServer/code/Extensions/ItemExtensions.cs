namespace Sitecore.Foundation.CommerceServer.Extensions
{
    using Sitecore.Data.Items;
    using Sitecore.Data.Managers;
    using Sitecore.Foundation.CommerceServer.Infrastructure.Constants;
    using System.Linq;

    /// <summary>
    /// Defines the ItemExtensions class.
    /// </summary>
    public static class ItemExtensions
    {
        /// <summary>
        /// Items the type.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>The item type of Unknown if the item is null or unknown.></returns>
        public static StorefrontConstants.ItemTypes ItemType(this Item item)
        {
            if (item != null)
            {
                var templateList = TemplateManager.GetTemplate(item).GetBaseTemplates();

                if (templateList.FirstOrDefault(t => t.ID == CommerceConstants.KnownTemplateIds.CommerceProductTemplate) != null)
                {
                    return StorefrontConstants.ItemTypes.Product;
                }

                if (templateList.FirstOrDefault(t => t.ID == CommerceConstants.KnownTemplateIds.CommerceCategoryTemplate) != null)
                {
                    return StorefrontConstants.ItemTypes.Category;
                }

                if (templateList.FirstOrDefault(t => t.ID == CommerceConstants.KnownTemplateIds.CommerceProductVariantTemplate) != null)
                {
                    return StorefrontConstants.ItemTypes.Variant;
                }

                if (templateList.FirstOrDefault(t => t.ID == StorefrontConstants.KnownTemplateItemIds.SecuredPage) != null)
                {
                    return StorefrontConstants.ItemTypes.SecuredPage;
                }

                if (templateList.FirstOrDefault(t => t.ID == StorefrontConstants.KnownTemplateItemIds.StandardPage) != null)
                {
                    return StorefrontConstants.ItemTypes.StandardPage;
                }
            }

            return StorefrontConstants.ItemTypes.Unknown;
        }
    }
}
