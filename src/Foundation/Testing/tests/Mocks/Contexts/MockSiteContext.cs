namespace Sitecore.Foundation.Testing.Mocks.Contexts
{
    using System.Collections;
    using System.Web;
    using Sitecore.Data.Items;
    using Sitecore.Foundation.CommerceServer.Interfaces;

    public class MockSiteContext : ISiteContext
    {
        public HttpContext CurrentContext { get; }
        public IDictionary Items { get; }
        public Item CurrentCatalogItem { get; set; }
        public bool IsCategory { get; }
        public bool IsProduct { get; }
        public bool UrlContainsCategory { get; set; }
    }
}
