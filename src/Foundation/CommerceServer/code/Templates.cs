namespace Sitecore.Foundation.CommerceServer
{
    using Sitecore.Data;

    public struct Templates
    {
        public struct CommerceNavigationItem
        {
            public static readonly ID ID = new ID("{E55834FB-7C93-44A2-87C0-62BEBA282CED}");

            public struct Fields
            {
                public static readonly ID CategoryDatasource = new ID("{2882072B-E310-406B-8DD9-B22C9EA4A0F3}");
            }
        }

        public struct CommerceStorefront
        {
            public static readonly ID ID = new ID("{57507DA9-3EEA-46AA-87E8-E64CDE2F73DC}");

            public struct Fields
            {
                public static readonly ID DefaultCurrency = new ID("{7315350F-F3B5-4740-A227-186F7A2BA9F7}");
            }
        }

        /// <summary>
        /// This templates generated via Commerce.update
        /// </summary>
        public struct CommerceGenerated
        {
            public struct GeneralCategory
            {

                public struct Fields
                {
                    public static readonly string Image = "Image";
                    public static readonly string Brand = "Brand";
                    public static readonly string CatalogName = "CatalogName";
                }
            }
        }
    }
}
