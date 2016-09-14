using Sitecore.Mvc.Presentation;

namespace Sitecore.Feature.Catalog.Repositories
{
    using Sitecore.Data.Items;
    using Sitecore.Feature.Base.Models.JsonResults;
    using Sitecore.Feature.Catalog.Models;
    using Sitecore.Feature.Catalog.Models.JsonResults;
    using Sitecore.Foundation.CommerceServer.Infrastructure.Constants;
    using Sitecore.Foundation.CommerceServer.Models;
    using Sitecore.Foundation.CommerceServer.Search.Models;

    public interface ICatalogRepository
    {
        void ApplyFacet(string facetValue, bool? isApplied);

        void ApplySortOrder(string sortField, CommerceConstants.SortDirection? sortDirection);

        CurrencyMenuViewModel GetCurrencyMenu(Rendering currentRendering);

        StockInfoListBaseJsonResult GetCurrentProductStockInfo(string productId);

        ProductFacetsViewModel GetFacets(Rendering currentRendering, int? pageNumber, string facetValues, string sortField, int? pageSize, CommerceConstants.SortDirection? sortDirection);

        MultipleProductSearchResults GetMultipleProductList(Item datasource, Rendering currentRendering, CommerceSearchOptions productSearchOptions);

        RelatedCatalogItemsViewModel GetRelatedCatalogItems(Item item, Rendering currentRendering);

        PaginationViewModel GetPagination(Rendering currentRendering, int? pageNumber, int? pageSize, string facetValues);

        CategoryViewModel GetProductList(Rendering currentRendering, int? pageNumber, string facetValues, string sortField, int? pageSize, CommerceConstants.SortDirection? sortDirection);

        ProductListHeaderViewModel GetProductListHeader(Rendering currentRendering, int? pageNumber, string facetValues, string sortField, int? pageSize, CommerceConstants.SortDirection? sortDirection);

        ProductViewModel GetWildCardProductViewModel(Item datasource, Rendering currentRendering);

        BaseJsonResult SwitchCurrency(string currency);
    }
}