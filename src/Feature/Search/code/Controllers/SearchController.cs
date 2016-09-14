using Sitecore.Feature.Search.Repositories;

namespace Sitecore.Feature.Search.Controllers
{
    using System.Web.Mvc;
    using Sitecore.Feature.Base.Controllers;
    using Sitecore.Feature.Search.Models;
    using Sitecore.Foundation.CommerceServer.Infrastructure.Constants;
    using Sitecore.Foundation.CommerceServer.Interfaces;
    using CommerceConstants = Sitecore.Foundation.CommerceServer.Infrastructure.Constants.CommerceConstants;

    public class SearchController : CSBaseController
    {
        private readonly ISearchRepository _searchRepository;

        public SearchController(
            IAccountManager accountManager,
            IContactFactory contactFactory,
            ISearchRepository searchRepository)
            : base(accountManager, contactFactory)
        {
            _searchRepository = searchRepository;
        }

        public ActionResult SearchBar(
            [Bind(Prefix = StorefrontConstants.QueryStrings.SearchKeyword)] string searchKeyword)
        {
            var model = new SearchBarModel { SearchKeyword = searchKeyword };

            return View("SearchBar", model);
        }

        /// <summary>
        /// The action for rendering the search results facets view
        /// </summary>
        /// <param name="searchKeyword">The search keyword.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="facetValues">The facet values.</param>
        /// <param name="sortField">The sort field.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="sortDirection">The sort direction.</param>
        /// <returns>The product search facets.</returns>
        public ActionResult ProductSearchResultsFacets(
            [Bind(Prefix = StorefrontConstants.QueryStrings.SearchKeyword)] string searchKeyword,
            [Bind(Prefix = StorefrontConstants.QueryStrings.Paging)] int? pageNumber,
            [Bind(Prefix = StorefrontConstants.QueryStrings.PageSize)] int? pageSize,
            [Bind(Prefix = StorefrontConstants.QueryStrings.Facets)] string facetValues,
            [Bind(Prefix = StorefrontConstants.QueryStrings.Sort)] string sortField,
            [Bind(Prefix = StorefrontConstants.QueryStrings.SortDirection)] CommerceConstants.SortDirection? sortDirection)
        {
            if (searchKeyword == null)
            {
                searchKeyword = string.Empty;
            }

            var searchInfo = _searchRepository.GetSearchInfo(Item, searchKeyword, pageNumber, facetValues, sortField, pageSize, sortDirection);
            var viewModel = _searchRepository.GetProductFacetsViewModel(searchInfo.SearchOptions, searchKeyword, searchInfo.Catalog.Name, this.Item, this.CurrentRendering);

            return View("ProductFacets", viewModel);
        }

        /// <summary>
        /// The action for rendering the search results list header view
        /// </summary>
        /// <param name="searchKeyword">The search keyword.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="facetValues">The facet values.</param>
        /// <param name="sortField">The sort field.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="sortDirection">The sort direction.</param>
        /// <returns>The product search result list header view.</returns>
        public ActionResult ProductSearchResultsListHeader(
            [Bind(Prefix = StorefrontConstants.QueryStrings.SearchKeyword)] string searchKeyword,
            [Bind(Prefix = StorefrontConstants.QueryStrings.Paging)] int? pageNumber,
            [Bind(Prefix = StorefrontConstants.QueryStrings.Facets)] string facetValues,
            [Bind(Prefix = StorefrontConstants.QueryStrings.Sort)] string sortField,
            [Bind(Prefix = StorefrontConstants.QueryStrings.PageSize)] int? pageSize,
            [Bind(Prefix = StorefrontConstants.QueryStrings.SortDirection)] CommerceConstants.SortDirection? sortDirection)
        {
            var searchInfo = _searchRepository.GetSearchInfo(Item, searchKeyword, pageNumber, facetValues, sortField, pageSize, sortDirection);
            var viewModel = _searchRepository.GetProductListHeaderViewModel(searchInfo.SearchOptions, searchInfo.SortFields, searchInfo.SearchKeyword, searchInfo.Catalog.Name, this.Item, this.CurrentRendering);

            return View("ProductListHeader", viewModel);
        }

        /// <summary>
        /// An action to manage data for the search results list
        /// </summary>
        /// <param name="searchKeyword">The search keyword.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="facetValues">The facet values.</param>
        /// <param name="sortField">The sort field.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="sortDirection">The sort direction.</param>
        /// <returns>
        /// The view that represents the search results list
        /// </returns>
        public ActionResult ProductSearchResultsList(
            [Bind(Prefix = StorefrontConstants.QueryStrings.SearchKeyword)] string searchKeyword,
            [Bind(Prefix = StorefrontConstants.QueryStrings.Paging)] int? pageNumber,
            [Bind(Prefix = StorefrontConstants.QueryStrings.Facets)] string facetValues,
            [Bind(Prefix = StorefrontConstants.QueryStrings.Sort)] string sortField,
            [Bind(Prefix = StorefrontConstants.QueryStrings.PageSize)] int? pageSize,
            [Bind(Prefix = StorefrontConstants.QueryStrings.SortDirection)] CommerceConstants.SortDirection? sortDirection)
        {
            var searchInfo = _searchRepository.GetSearchInfo(Item, searchKeyword, pageNumber, facetValues, sortField, pageSize, sortDirection);
            var viewModel = _searchRepository.GetProductListViewModel(searchInfo.SearchOptions, searchInfo.SortFields, searchInfo.SearchKeyword, searchInfo.Catalog.Name, this.Item, this.CurrentRendering);

            return View("ProductList", viewModel);
        }

        /// <summary>
        /// The action for rendering the pagination view
        /// </summary>
        /// <param name="searchKeyword">The search keyword.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="facetValues">The facet values.</param>
        /// <param name="sortField">The sort field.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="sortDirection">The sort direction.</param>
        /// <returns>The pagination view.</returns>
        public ActionResult ProductSearchResultsPagination(
            [Bind(Prefix = StorefrontConstants.QueryStrings.SearchKeyword)] string searchKeyword,
            [Bind(Prefix = StorefrontConstants.QueryStrings.Paging)] int? pageNumber,
            [Bind(Prefix = StorefrontConstants.QueryStrings.Facets)] string facetValues,
            [Bind(Prefix = StorefrontConstants.QueryStrings.Sort)] string sortField,
            [Bind(Prefix = StorefrontConstants.QueryStrings.PageSize)] int? pageSize,
            [Bind(Prefix = StorefrontConstants.QueryStrings.SortDirection)] CommerceConstants.SortDirection? sortDirection)
        {
            var searchInfo = _searchRepository.GetSearchInfo(Item, searchKeyword, pageNumber, facetValues, sortField, pageSize, sortDirection);
            var viewModel = _searchRepository.GetPaginationViewModel(searchInfo.SearchOptions, searchInfo.SearchKeyword, searchInfo.Catalog.Name, this.Item, this.CurrentRendering);

            return View("Pagination", viewModel);
        }
    }
}