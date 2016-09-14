namespace Sitecore.Feature.Account.Tests
{
    using Sitecore.Data;

    public struct Templates
    {
        public struct SiteName
        {
            public static string Name => "fake";

            public static ID ID => new ID("{A7EBF38A-5F66-4579-92D1-568A8BA50293}");
        }

        public struct Global
        {
            public static string Name => "Global";

            public static ID ID => new ID("{9d5f2178-3ece-4eab-b42a-aff85d1dce32}");
        }
    }
}