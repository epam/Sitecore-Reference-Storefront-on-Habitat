using Sitecore.Commerce.Connect.CommerceServer.Search.Models;
using Sitecore.ContentSearch;
using Sitecore.Data.Items;
using System.Collections.Generic;
using System.Linq;

namespace Sitecore.Foundation.CommerceServer.Interfaces
{
    /// <summary>
    /// Interface for catalog search finctionality. 
    /// </summary>
    public interface ICatalogSearchManager
    {
        IEnumerable<Search.Models.CommerceQueryFacet> GetFacetFieldsForItem(Item item);
        IEnumerable<Search.Models.CommerceQuerySort> GetSortFieldsForItem(Item item);
        int GetItemsPerPageForItem(Item item);
        ISearchIndex GetIndex();
        ISearchIndex GetIndex(string catalogName);
        IQueryable<T> AddSearchOptionsToQuery<T>(IQueryable<T> searchResults, CommerceSearchOptions connectSearchOptions)
            where T : ISearchResult;
        bool IsItemCatalog(Item item);
        bool IsItemVirtualCatalog(Item item);
    }
}