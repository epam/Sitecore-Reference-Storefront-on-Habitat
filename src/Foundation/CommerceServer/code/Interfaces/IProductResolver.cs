using Sitecore.Data.Items;

namespace Sitecore.Foundation.CommerceServer.Interfaces
{
    public interface IProductResolver
    {
        Item ResolveCatalogItem(string itemId, string catalogName, bool isProduct);
    }
}