namespace Sitecore.Feature.Navigation
{
    using Sitecore.Data;

    public struct Templates
    {
        public struct HasNavigationLabel
        {
            public static readonly ID ID = new ID("{8C9CF42A-988D-4B40-9827-6B2083B69588}");

            public struct Fields
            {
                public static readonly ID CategoriesLabel = new ID("{EFDABAEC-E463-4EA0-93EF-F5439397B208}");
            }
        }

        public struct QuickLink
        {
            public static readonly ID ID = new ID("{0909C439-6F31-4410-A83C-E8B830B1F16D}");

            public struct Fields
            {
                public static readonly ID Link = new ID("{B861B1ED-2820-4AD8-B169-8928AC381634}");
                public static readonly ID Text = new ID("{6CB9EB87-680B-4CB9-9946-9BAEB553F6B8}");
                public static readonly ID ShowWhenAuthenticated = new ID("{5723C6B6-8A8A-4960-AEC8-050200AE62D2}");
                public static readonly ID ShowAlways = new ID("{96914D76-288D-4E7A-90C8-E4C4594F9E12}");
                public static readonly ID GenerateSecureLink = new ID("{BA2DAACE-B30A-4489-9F25-DDD852C1B171}");
            }
        }

        public struct QuickLinks
        {
            public static readonly ID ID = new ID("{35A9F85D-4031-473B-9813-A5E0A67BFC34}");

            public struct Fields
            {
                public static readonly ID QuickLinks = new ID("{1250AE47-B620-4E3D-BD17-A36BC3595143}");
                public static readonly ID MyAccountAlt = new ID("{7E264FEB-61E2-4C45-A210-9E2FF8234E94}");
            }
        }

        public struct NavigationRoot
        {
            public static readonly ID ID = new ID("{F9F4FC05-98D0-4C62-860F-F08AE7F0EE25}");
        }

        public struct Navigable
        {
            public static readonly ID ID = new ID("{A1CBA309-D22B-46D5-80F8-2972C185363F}");

            public struct Fields
            {
                public static readonly ID ShowInNavigation = new ID("{5585A30D-B115-4753-93CE-422C3455DEB2}");
                public static readonly ID NavigationTitle = new ID("{1B483E91-D8C4-4D19-BA03-462074B55936}");
                public const string NavigationTitle_FieldName = "NavigationTitle";
            }
        }

        public struct Link
        {
            public static readonly ID ID = new ID("{A16B74E9-01B8-439C-B44E-42B3FB2EE14B}");

            public struct Fields
            {
                public static readonly ID Link = new ID("{FE71C30E-F07D-4052-8594-C3028CD76E1F}");
            }
        }

        public struct LinkMenuItem
        {
            public static readonly ID ID = new ID("{18BAF6B0-E0D6-4CCE-9184-A4849343E7E4}");

            public struct Fields
            {
                public static readonly ID Icon = new ID("{2C24649E-4460-4114-B026-886CFBE1A96D}");
                public static readonly ID DividerBefore = new ID("{4231CD60-47C1-42AD-B838-0A6F8F1C4CFB}");
            }
        }
    }
}