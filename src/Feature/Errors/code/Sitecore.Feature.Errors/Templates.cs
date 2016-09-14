namespace Sitecore.Feature.Errors
{
    using Sitecore.Data;

    public struct Templates
    {
        public struct HasErrorImage
        {
            public static readonly ID ID = new ID("{6786C458-127B-406D-978A-0CBA1E698A96}");

            public struct Fields
            {
                public static readonly ID Image = new ID("{0070E965-050D-409B-B66C-AC238D33BA7A}");
                public static readonly ID Link = new ID("{F47ABA5C-98DD-4E03-8DE7-E90D3A1286DE}");
            }
        }

        public struct GlobalError
        {
            public static readonly ID ID = new ID("{EB13D2CB-A7B8-49AB-A8E1-9962E5B03835}");

            public struct Fields
            {
                public static readonly ID PageText = new ID("{57A207A1-AAEC-4085-B18B-9B092A42C7EF}");
            }
        }
    }
}