using Sitecore.Commerce.Connect.CommerceServer.Search.Models;
using Sitecore.ContentSearch;
using Sitecore.Data.Items;
using Sitecore.Foundation.CommerceServer.Utils;
using System.Collections.Generic;
using System.Linq;
using Sitecore.Commerce.Connect.CommerceServer.Search;

namespace Sitecore.Foundation.CommerceServer.Managers
{
    public class CatalogSearchManager : Interfaces.ICatalogSearchManager
    {
        private readonly ICommerceSearchManager _connectSearchManager;

        public CatalogSearchManager()
        {
            _connectSearchManager = ContextTypeLoader.CreateInstance<ICommerceSearchManager>();
        }

        public IEnumerable<Search.Models.CommerceQueryFacet> GetFacetFieldsForItem(Item item)
        {
            return _connectSearchManager.GetFacetFieldsForItem(item).Select(Search.Models.CommerceQueryFacet.Wrap);
        }

        public IEnumerable<Search.Models.CommerceQuerySort> GetSortFieldsForItem(Item item)
        {
            return _connectSearchManager.GetSortFieldsForItem(item).Select(q => new Search.Models.CommerceQuerySort(q));
        }

        public int GetItemsPerPageForItem(Item item)
        {
            return _connectSearchManager.GetItemsPerPageForItem(item);
        }

        public ISearchIndex GetIndex()
        {
            return _connectSearchManager.GetIndex();
        }

        public ISearchIndex GetIndex(string catalogName)
        {
            return _connectSearchManager.GetIndex(catalogName);
        }

        public IQueryable<T> AddSearchOptionsToQuery<T>(IQueryable<T> searchResults, CommerceSearchOptions connectSearchOptions)
            where T : ISearchResult
        {
            return _connectSearchManager.AddSearchOptionsToQuery(searchResults, connectSearchOptions);
        }

        public bool IsItemCatalog(Item item)
        {
            return _connectSearchManager.IsItemCatalog(item);
        }

        public bool IsItemVirtualCatalog(Item item)
        {
            return _connectSearchManager.IsItemVirtualCatalog(item);
        }
    }
}