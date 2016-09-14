namespace Sitecore.Feature.PageContent
{
    using Sitecore.Data;

    public struct Templates
    {
        public struct HasPageContent
        {
            public static ID ID = new ID("{AF74A00B-8CA7-4C9A-A5C1-156A68590EE2}");

            public struct Fields
            {
                public static readonly ID Title = new ID("{3F405520-D894-47BC-AF8B-089F7914CAD8}");
                public static readonly string Title_FieldName = "Title";
                public static readonly ID Text = new ID("{AC3FD4DB-8266-476D-9635-67814D91E901}");
                public static readonly string Text_FieldName = "Text";
                public static readonly ID SummaryText = new ID("{D74F396D-5C5E-4916-BD0A-BFD58B6B1967}");
                public static readonly ID DisplayInSearchResults = new ID("{9492E0BB-9DF9-46E7-8188-EC795C4ADE44}");
                public static readonly ID Image = new ID("{BEE4C2B5-638F-406A-98EA-FD968F9BBAAA}");
            }
        }
    }
}