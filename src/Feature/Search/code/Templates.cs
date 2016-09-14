namespace Sitecore.Feature.Search
{
    using Sitecore.Data;

    public class Templates
    {
        public struct SearchBar
        {
            public static readonly ID ID = new ID("{A6ACD2B2-E8A2-40A4-8831-AB44699C4C44}");

            public struct Fields
            {
                public static readonly ID PagesOnEachSideOfCurrent = new ID("{EE8CD542-F88E-49C7-8BB4-9190EDAD1272}");
            }
        }

        public struct SearchHeader
        {
            public static readonly ID ID = new ID("{2E078EC1-782D-476D-B297-09C3415E5DAB}");

            public struct Fields
            {
                public static readonly ID SortBy = new ID("{BC0C9BD0-4419-4053-8637-99069ABFBEE2}");
                public static readonly ID Showing = new ID("{5C153654-1C2B-41AC-890C-D525CA8D734A}");
                public static readonly ID ResultsPerPage = new ID("{DDF4B28A-506E-47CB-8387-41993FAB7B6E}");
                public static readonly ID Ascending = new ID("{6DDCBBCE-CAF3-4903-894A-2E770B960D9B}");
                public static readonly ID Descending = new ID("{0D7E3EC4-8467-461D-BC0E-6A5A7D6BEC62}");
                public static readonly ID Of = new ID("{19CA9897-00B8-4C16-B73F-19C50C87C384}");
                public static readonly ID Header = new ID("{63A50BDA-38D2-4BB8-81ED-079B5F396D3D}");

                public static readonly ID ResultsPerPageValues = new ID("{586A5893-04C1-4AB8-A9E8-B536526ECF75}");
            }
        }

        public struct SearchPagination
        {
            public static readonly ID ID = new ID("{609D8F1A-78A5-4DF7-8EAA-860FE160EE40}");

            public struct Fields
            {
                public static readonly ID PagesOnEachSideOfCurrent = new ID("{249D1014-A951-4F93-9AEB-F7F773E12598}");
            }
        }

        public struct SearchPanel
        {
            public static readonly ID ID = new ID("{8BE1206D-CB18-4871-AE89-B5A51A44FB65}");

            public struct Fields
            {
                public static readonly ID Category = new ID("{7DAE4441-1832-472A-AD46-BA1B088FAF83}");
                public static readonly ID ItemNumber = new ID("{663B9F9E-32F1-40A7-8B54-32117ADB794B}");
                public static readonly ID Description = new ID("{E162C24F-D680-404C-B227-183DEA23B5DC}");
                public static readonly ID Color = new ID("{4FD9927A-7E31-492F-96EB-E19A3B2AD0E1}");
                public static readonly ID Size = new ID("{BF3756C4-A5D7-4DD3-B969-46F1B5FF7E97}");
                public static readonly ID ItemCount = new ID("{E4945B55-2510-4F55-AC88-2A1EA2ABE6D5}");
                public static readonly ID ItemsCount = new ID("{6B305AE7-B9B1-415D-B075-D5FA69081F50}");
                public static readonly ID SignUpForNotificationLabel = new ID("{41AEB698-08A0-4104-AB5A-9238FDA1087B}");
                public static readonly ID SignUpForNotificationTooltip = new ID("{FD7A7765-5B89-4E50-9E43-68DE52F6DAE8}");
                public static readonly ID CloseLabel = new ID("{235C1B7A-A533-4FB4-8ECB-E27119A2816F}");
                public static readonly ID NameLabel = new ID("{F689A9E0-CA90-4389-9861-65E2AE7A403C}");
                public static readonly ID EmailLabel = new ID("{2716F9FE-B6E6-4035-9A9E-C6D6C3DF0478}");
                public static readonly ID ConfirmEmailLabel = new ID("{6FFB4271-E7BD-4A06-A540-88E41570DF4A}");
                public static readonly ID SignUpLabel = new ID("{BEBD0C79-F322-45A0-9E4C-26236E26199B}");
                public static readonly ID SigningUpLabel = new ID("{983E9818-BD15-4794-B634-EDB4B5766434}");

                public static readonly ID SavePercentLead = new ID("{D1A1C215-DFC4-4568-9FE3-C047FED16777}");
                public static readonly ID InvalidVariant = new ID("{249D1014-A951-4F93-9AEB-F7F773E12598}");
            }
        }

        public struct SearchResults
        {
            public static ID ID = new ID("{14E452CA-064D-48A8-9FF2-2744D10437A1}");

            public struct Fields
            {
                public static readonly ID SearchBoxTitle = new ID("{80E30DD8-8021-45F5-9FE1-23D2702CC206}");
                public static readonly ID Root = new ID("{CD904125-3AE5-4709-9E6D-71473C5D5007}");
            }
        }

        public struct PagedSearchResultsParameters
        {
            public static ID ID = new ID("{D1D3E60F-E571-48D2-84CF-B053EE660C13}");

            public struct Fields
            {
                public static readonly ID ResultsOnPage = new ID("{FCC7E3B4-46AB-4A51-975F-A6B259B3D214}");
                public static readonly ID PagesToShow = new ID("{FCC7E3B4-46AB-4A51-975F-A6B259B3D214}");
            }
        }
    }
}