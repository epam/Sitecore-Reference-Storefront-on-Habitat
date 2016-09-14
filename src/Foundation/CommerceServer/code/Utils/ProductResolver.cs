using Sitecore.Data.Items;
using Sitecore.Foundation.CommerceServer.Interfaces;
using Sitecore.Foundation.CommerceServer.Pipelines;

namespace Sitecore.Foundation.CommerceServer.Utils
{
    public class ProductResolver : IProductResolver
    {
        public Item ResolveCatalogItem(string itemId, string catalogName, bool isProduct)
        {
            return ProductItemResolver.ResolveCatalogItem(itemId, catalogName, isProduct);
        }

    }
}
