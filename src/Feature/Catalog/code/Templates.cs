namespace Sitecore.Feature.Catalog
{
    using Sitecore.Data;

    public struct Templates
    {
        public struct Pagination
        {
            public static readonly ID ID = new ID("{13A3571A-BC9D-4CB6-8AF6-A0CD8BED3C3F}");

            public struct Fields
            {
                public static readonly ID PagesOnEachSideOfCurrent = new ID("{DE3B606F-C73D-4B1F-9A7C-847EFEBBA18C}");
            }
        }

        public struct ProductSearch
        {
            public static readonly ID ID = new ID("{B09F123A-126F-439B-92C5-16FAC701756B}");

            public struct Fields
            {
                public static readonly ID NamedSearches = new ID("{BB8B5123-FD0C-48DF-B908-B59C6D67D9CF}");
            }
        }

        public struct NamedSearch
        {
            public static readonly ID ID = new ID("{591C5D76-32B6-4DD5-8410-C48D706C5425}");

            public struct Fields
            {
                public static readonly ID Title = new ID("{439CDD2A-66A9-4D67-913A-5B696327C867}");
            }
        }

        public struct SelectedProducts
        {
            public static readonly ID ID = new ID("{A45D0030-79F2-4DBF-9A74-226A33C58249}");

            public struct Fields
            {
                public static readonly ID ProductList = new ID("{75DDF936-9B45-4EBA-88BB-783C9D95486E}");
                public static readonly ID Title = new ID("{357469EE-6674-432F-9B43-EF3534763773}");
            }
        }

        public struct ProductInformation
        {
            public static readonly ID ID = new ID("{917BAFA7-4CD6-44B5-9DAC-59FC063A9743}");

            public struct Fields
            {
                public static readonly ID Category = new ID("{BC3FB1F0-651E-44CC-9820-BCD5B20AA04B}");
                public static readonly ID ItemNumber = new ID("{6B8C24DA-F3DD-40A8-86AD-2979736786AD}");
                public static readonly ID Description = new ID("{246545E8-3349-4188-9B81-2AA923F28BBF}");
                public static readonly ID Color = new ID("{07CAA1DA-E5EC-43BD-B2D8-38716A98A082}");
                public static readonly ID Size = new ID("{1C4B65A9-7BA7-46A5-A832-CC587BF4BF37}");
                public static readonly ID ItemCount = new ID("{56852681-A8AE-4637-B14F-3E6C7A009B20}");
                public static readonly ID ItemsCount = new ID("{5AF849AE-19C4-4E84-85E8-8351E7AAC8A0}");
                public static readonly ID SignUpForNotificationLabel = new ID("{835D9052-694C-4CBF-A376-C7693AE67BA2}");
                public static readonly ID SignUpForNotificationTooltip = new ID("{7FAE2425-7256-40EA-9EBB-7BD7C7A345A7}");
                public static readonly ID CloseLabel = new ID("{5B793011-27F5-4BD6-B088-D566CBE39ADB}");
                public static readonly ID NameLabel = new ID("{FD795459-1B65-4D95-9199-B1E1888C3D34}");
                public static readonly ID EmailLabel = new ID("{098D3FF9-C4A9-43E1-8F9A-F15F5C82C764}");
                public static readonly ID ConfirmEmailLabel = new ID("{BD93B2D4-4E5B-40D4-8DE7-B8CDFED69104}");
                public static readonly ID SignUpLabel = new ID("{E02D3CB5-39FD-4D67-B8B7-7D9BC6929311}");
                public static readonly ID SigningUpLabel = new ID("{570CA6EA-E9C8-4EAB-86DE-25E4ACC6610F}");

                public static readonly ID SavePercentLead = new ID("{06A6A591-CB8B-480B-87E5-D87AEAE9CB20}");
                public static readonly ID InvalidVariant = new ID("{57CA5CC4-949A-4264-9EA0-6FA7D0D1D362}");
            }
        }

        public struct CatalogHeader
        {
            public static readonly ID ID = new ID("{E02B7BFE-E88A-4DF8-87D2-B82882F1A91C}");

            public struct Fields
            {
                public static readonly ID SortBy = new ID("{63E6284B-2F67-4F0C-84B3-17A0C6F0A80D}");
                public static readonly ID Showing = new ID("{05C11D25-1E1D-4768-9513-A9102C382CF3}");
                public static readonly ID ResultsPerPage = new ID("{367AD7F0-816C-46E4-8A66-21A368230057}");
                public static readonly ID Ascending = new ID("{A0186324-66D4-4B05-81C2-811DC35BD05E}");
                public static readonly ID Descending = new ID("{EC21D0D2-25EF-4DB2-97CB-DCA5CF0C5C09}");
                public static readonly ID Of = new ID("{8E44D8A6-6B06-4F72-A46A-349206C5BC8E}");
                public static readonly ID Header = new ID("{8D82CAD4-6C30-4733-9D54-A7EFB847602B}");

                public static readonly ID ResultsPerPageValues = new ID("{B06F5434-7BE9-444A-A0BE-2BD6FA01E4F5}");
            }
        }

    }
}
