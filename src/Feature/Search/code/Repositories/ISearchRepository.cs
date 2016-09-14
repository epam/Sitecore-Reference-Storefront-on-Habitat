using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Feature.Search.Models;
using Sitecore.Foundation.CommerceServer.Infrastructure.Constants;
using Sitecore.Foundation.CommerceServer.Models;
using Sitecore.Foundation.CommerceServer.Search.Models;
using Sitecore.Mvc.Presentation;

namespace Sitecore.Feature.Search.Repositories
{
    public interface ISearchRepository
    {
        PaginationViewModel GetPaginationViewModel(
            CommerceSearchOptions productSearchOptions,
            string searchKeyword,
            string catalogName,
            Item item,
            Rendering rendering);

        ProductFacetsViewModel GetProductFacetsViewModel(
            CommerceSearchOptions productSearchOptions,
            string searchKeyword,
            string catalogName,
            Item item,
            Rendering rendering);

        CategoryViewModel GetProductListViewModel(
            CommerceSearchOptions productSearchOptions,
            IEnumerable<CommerceQuerySort> sortFields,
            string searchKeyword,
            string catalogName,
            Item item,
            Rendering rendering);

        ProductListHeaderViewModel GetProductListHeaderViewModel(
            CommerceSearchOptions productSearchOptions,
            IEnumerable<CommerceQuerySort> sortFields,
            string searchKeyword,
            string catalogName,
            Item item,
            Rendering rendering);

        SearchInfo GetSearchInfo(
            Item datasource,
            string searchKeyword,
            int? pageNumber,
            string facetValues,
            string sortField,
            int? pageSize,
            CommerceConstants.SortDirection? sortDirection);
    }
}