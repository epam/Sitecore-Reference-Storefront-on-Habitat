using System.Collections.Generic;

namespace Sitecore.Foundation.CommerceServer.Infrastructure.Constants
{
    using Sitecore.Commerce.Connect.CommerceServer.Utilities;
    using Sitecore.Data;
    using ConnectConstants = Sitecore.Commerce.Connect.CommerceServer.CommerceConstants;

    /// <summary>
    /// Mediator class used to translate all of the constants used by Sitecore.Commerce.Connect.CommerceServer.
    /// </summary>
    public static class CommerceConstants
    {
        /// <summary>
        /// The list of roles defined by the Commerce system.
        /// </summary>
        public static readonly IEnumerable<string> CommerceRolesList = ConnectConstants.CommerceRolesList;

        /// <summary>
        /// The list of roles security for the /sitecore/templates/System/Templates/Sections/Appearance/Appearance/__Display name item.
        /// </summary>
        public static readonly IEnumerable<RoleSecurityAccess> DisplayNameTemplateSecurityAccesses =
            ConnectConstants.DisplayNameTemplateSecurityAccesses;
        
        /// <summary>
        /// Sort direction types.
        /// </summary>
        public enum SortDirection
        {
            Asc = ConnectConstants.SortDirection.Asc,
            Desc = ConnectConstants.SortDirection.Desc
        }

        /// <summary>
        /// Known list of template ids.
        /// </summary>
        public static class KnownTemplateIds
        {
            /// <summary>
            /// Template for a category item
            /// </summary>
            public static readonly ID CommerceCategoryTemplate = ConnectConstants.KnownTemplateIds.CommerceCategoryTemplate;
            
            /// <summary>
            /// The base template for product items
            /// </summary>
            public static readonly ID CommerceProductTemplate = ConnectConstants.KnownTemplateIds.CommerceProductTemplate;
            
            /// <summary>
            /// The template for a product variant
            /// </summary>
            public static readonly ID CommerceProductVariantTemplate = ConnectConstants.KnownTemplateIds.CommerceProductVariantTemplate;
            
            /// <summary>
            /// The template for the dynamic navigation item
            /// </summary>
            public static readonly ID CommerceDynamicCategoryTemplate = ConnectConstants.KnownTemplateIds.CommerceDynamicCategoryTemplate;
        }

        /// <summary>
        /// A list of known field IDs.
        /// </summary>
        public static class KnownFieldIds
        {
            /// <summary>
            /// The field that holds the catalog name
            /// </summary>
            public static readonly ID CatalogName = ConnectConstants.KnownFieldIds.CatalogName;
            
            /// <summary>
            /// The field for relationships
            /// </summary>
            public static readonly ID RelationshipList = ConnectConstants.KnownFieldIds.RelationshipList;
        }

        /// <summary>
        /// Known list of item ids.
        /// </summary>
        public static class KnownItemIds
        {
            /// <summary>
            /// An item to indicate what catalog to use as the default for the site
            /// </summary>
            public static readonly ID DefaultCatalog = ConnectConstants.KnownItemIds.DefaultCatalog;
        }
        
        /// <summary>
        /// Used to hold some of the default settings for cart
        /// </summary>
        public static class CartSettings
        {
            /// <summary>
            /// The default cart name
            /// </summary>
            public const string DefaultCartName = ConnectConstants.CartSettings.DefaultCartName;
        }
        
        /// <summary>
        /// Known list of commerce pipelines.
        /// </summary>
        public static class PipelineNames
        {
            /// <summary>
            /// The name of the DeleteProfile pipeline
            /// </summary>
            public const string DeleteProfile = ConnectConstants.PipelineNames.DeleteProfile;

            /// <summary>
            /// The name of the CreateProfile pipeline
            /// </summary>
            public const string CreateProfile = ConnectConstants.PipelineNames.CreateProfile;

            /// <summary>
            /// The name of the GetProfile pipeline
            /// </summary>
            public const string GetProfile = ConnectConstants.PipelineNames.GetProfile;
        }
        
        /// <summary>
        /// Known items we use in Sitecore.
        /// </summary>
        public static class KnownItemPaths
        {
            /// <summary>
            /// The path to the buttons for the images edit frame
            /// </summary>
            public const string EditFrameImages = ConnectConstants.KnownItemPaths.EditFrameImages;
        }
        
        /// <summary>
        /// Known list of cache names we use.
        /// </summary>
        public static class KnownCacheNames
        {
            /// <summary>
            /// The name of the cache that stores commerce catalog associations
            /// </summary>
            public const string CommerceAssociationsCache = ConnectConstants.KnownCacheNames.CommerceAssociationsCache;

            /// <summary>
            /// The name of the cache that stores catalog friendly urls
            /// </summary>
            public const string FriendlyUrlsCache = ConnectConstants.KnownCacheNames.FriendlyUrlsCache;

            /// <summary>
            /// The name of the cache that stores catalog friendly urls
            /// </summary>
            public const string CommerceCartCache = ConnectConstants.KnownCacheNames.CommerceCartCache;
        }

        /// <summary>
        /// Known list of cache prefixes we use to prefix the cache name.
        /// </summary>
        public static class KnownCachePrefixes
        {
            /// <summary>
            /// The Sitecore cache prefix
            /// </summary>
            public const string Sitecore = ConnectConstants.KnownCachePrefixes.Sitecore;
        }
        
        /// <summary>
        /// Know set of fieldnames we use in Sitecore.
        /// </summary>
        public static class KnownSitecoreFieldNames
        {
            /// <summary>
            /// The field used on a bucket item to store the filter
            /// </summary>
            public const string PersistentBucketFilter = ConnectConstants.KnownSitecoreFieldNames.PersistentBucketFilter;

            /// <summary>
            /// The field used on a bucket item to store the default query
            /// </summary>
            public const string DefaultBucketQuery = ConnectConstants.KnownSitecoreFieldNames.DefaultBucketQuery;
        }
        
        /// <summary>
        /// Contains a list of profile constants
        /// </summary>
        public static class ProfilesStrings
        {
            /// <summary>
            /// Domain name to use for commerce users
            /// </summary>
            public const string CommerceUsersDomainName = ConnectConstants.ProfilesStrings.CommerceUsersDomainName;
        }
    }
}