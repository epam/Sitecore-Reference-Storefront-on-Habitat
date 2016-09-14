using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Feature.Base.Repositories;
using Sitecore.Feature.Search.Models;
using Sitecore.Foundation.CommerceServer.Helpers;
using Sitecore.Foundation.CommerceServer.Infrastructure.Constants;
using Sitecore.Foundation.CommerceServer.Interfaces;
using Sitecore.Foundation.CommerceServer.Managers;
using Sitecore.Foundation.CommerceServer.Models;
using Sitecore.Foundation.CommerceServer.Search.Models;
using Sitecore.Foundation.CommerceServer.Utils;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sitecore.Feature.Search.Repositories
{
    public class SearchRepository : BaseRepository, ISearchRepository
    {
        private const string CurrentCategoryViewModelKeyName = "CurrentCategoryViewModel";
        private const string CurrentSearchProductResultsKeyName = "CurrentSearchProductResults";
        private const string CurrentSearchInfoKeyName = "CurrentSearchInfo";
        private readonly ICatalogManager _catalogManager;

        public SearchRepository(
            IAccountManager accountManager,
            IContactFactory contactFactory,
            ICatalogManager catalogManager)
            : base(accountManager, contactFactory)
        {
            this._catalogManager = catalogManager;
        }

        public PaginationViewModel GetPaginationViewModel(CommerceSearchOptions productSearchOptions, string searchKeyword,
            string catalogName, Item item, Rendering rendering)
        {
            var viewModel = new PaginationViewModel();
            SearchResults childProducts = null;

            if (productSearchOptions != null)
            {
                childProducts = this.GetChildProducts(item, productSearchOptions, searchKeyword, catalogName);
            }

            viewModel.Initialize(rendering, childProducts, productSearchOptions);

            return viewModel;
        }

        public ProductFacetsViewModel GetProductFacetsViewModel(
            CommerceSearchOptions productSearchOptions,
            string searchKeyword,
            string catalogName,
            Item item,
            Rendering rendering)
        {
            var viewModel = new ProductFacetsViewModel();
            SearchResults childProducts = null;

            if (productSearchOptions != null)
            {
                childProducts = this.GetChildProducts(item, productSearchOptions, searchKeyword, catalogName);
            }

            viewModel.Initialize(rendering, childProducts, productSearchOptions);

            return viewModel;
        }

        public CategoryViewModel GetProductListViewModel(CommerceSearchOptions productSearchOptions, IEnumerable<CommerceQuerySort> sortFields,
            string searchKeyword, string catalogName, Item item, Rendering rendering)
        {
            if (this.CurrentSiteContext.Items[CurrentCategoryViewModelKeyName] == null)
            {
                var categoryViewModel = new CategoryViewModel();
                var childProducts = GetChildProducts(item, productSearchOptions, searchKeyword, catalogName);
                categoryViewModel.Initialize(rendering, childProducts, sortFields, productSearchOptions);

                if (childProducts != null && childProducts.SearchResultItems.Count > 0)
                {
                    this._catalogManager.GetProductBulkPrices(this.CurrentVisitorContext, categoryViewModel.ChildProducts);
                    this._catalogManager.InventoryManager.GetProductsStockStatusForList(this.CurrentStorefront, categoryViewModel.ChildProducts);
                    foreach (var productViewModel in categoryViewModel.ChildProducts)
                    {
                        var productItem = childProducts.SearchResultItems.Single(i => i.Name == productViewModel.ProductId);
                        productViewModel.CustomerAverageRating = this._catalogManager.GetProductRating(productItem);
                    }
                }

                this.CurrentSiteContext.Items[CurrentCategoryViewModelKeyName] = categoryViewModel;
            }

            var viewModel = (CategoryViewModel)this.CurrentSiteContext.Items[CurrentCategoryViewModelKeyName];

            return viewModel;
        }

        public ProductListHeaderViewModel GetProductListHeaderViewModel(CommerceSearchOptions productSearchOptions,
            IEnumerable<CommerceQuerySort> sortFields, string searchKeyword, string catalogName, Item item, Rendering rendering)
        {
            var viewModel = new ProductListHeaderViewModel();
            SearchResults childProducts = null;

            if (productSearchOptions != null)
            {
                childProducts = this.GetChildProducts(item, productSearchOptions, searchKeyword, catalogName);
            }

            viewModel.Initialize(rendering, childProducts, sortFields, productSearchOptions);

            return viewModel;
        }

        protected SearchResults GetChildProducts(Item item, CommerceSearchOptions searchOptions, string searchKeyword, string catalogName)
        {
            if (this.CurrentSiteContext.Items[CurrentSearchProductResultsKeyName] != null)
            {
                return (SearchResults)this.CurrentSiteContext.Items[CurrentSearchProductResultsKeyName];
            }

            Assert.ArgumentNotNull(searchKeyword, "searchOptions");
            Assert.ArgumentNotNull(searchKeyword, "searchKeyword");
            Assert.ArgumentNotNull(searchKeyword, "catalogName");

            var returnList = new List<Item>();
            var totalPageCount = 0;
            var totalProductCount = 0;
            var facets = Enumerable.Empty<CommerceQueryFacet>();

            if (item != null && !string.IsNullOrEmpty(searchKeyword.Trim()))
            {
                var searchResponse = SearchNavigation.SearchCatalogItemsByKeyword(searchKeyword, catalogName, searchOptions);

                if (searchResponse != null)
                {
                    returnList.AddRange(searchResponse.ResponseItems);
                    totalProductCount = searchResponse.TotalItemCount;
                    totalPageCount = searchResponse.TotalPageCount;
                    facets = searchResponse.Facets;
                }
            }

            var results = new SearchResults(returnList, totalProductCount, totalPageCount, searchOptions.StartPageIndex, facets);
            this.CurrentSiteContext.Items[CurrentSearchProductResultsKeyName] = results;

            return results;
        }

        public SearchInfo GetSearchInfo(
            Item datasource,
            string searchKeyword,
            int? pageNumber,
            string facetValues,
            string sortField,
            int? pageSize,
            CommerceConstants.SortDirection? sortDirection)
        {
            if (this.CurrentSiteContext.Items[CurrentSearchInfoKeyName] != null)
            {
                return (SearchInfo)this.CurrentSiteContext.Items[CurrentSearchInfoKeyName];
            }

            var searchManager = ContextTypeLoader.CreateInstance<ICatalogSearchManager>();

            var searchInfo = new SearchInfo
            {
                SearchKeyword = searchKeyword ?? string.Empty,
                RequiredFacets = searchManager.GetFacetFieldsForItem(datasource),
                SortFields = searchManager.GetSortFieldsForItem(datasource),
                Catalog = StorefrontManager.CurrentStorefront.DefaultCatalog,
                ItemsPerPage = pageSize ?? searchManager.GetItemsPerPageForItem(datasource)
            };

            if (searchInfo.ItemsPerPage == 0)
            {
                searchInfo.ItemsPerPage = StorefrontConstants.Settings.DefaultItemsPerPage;
            }

            var productSearchOptions = new CommerceSearchOptions(searchInfo.ItemsPerPage, pageNumber.GetValueOrDefault(0));
            UpdateOptionsWithFacets(searchInfo.RequiredFacets, facetValues, productSearchOptions);
            UpdateOptionsWithSorting(sortField, sortDirection, productSearchOptions);
            searchInfo.SearchOptions = productSearchOptions;

            this.CurrentSiteContext.Items[CurrentSearchInfoKeyName] = searchInfo;

            return searchInfo;
        }

        private void UpdateOptionsWithFacets(
            IEnumerable<CommerceQueryFacet> facets,
            string valueQueryString,
            CommerceSearchOptions productSearchOptions)
        {
            if (facets != null && facets.Any())
            {
                if (!string.IsNullOrEmpty(valueQueryString))
                {
                    var facetValuesCombos = valueQueryString.Split('&');

                    foreach (var facetValuesCombo in facetValuesCombos)
                    {
                        var facetValues = facetValuesCombo.Split('=');

                        var name = facetValues[0];

                        var existingFacet = facets.FirstOrDefault(item => item.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

                        if (existingFacet != null)
                        {
                            var values = facetValues[1].Split(StorefrontConstants.QueryStrings.FacetsSeparator);

                            foreach (var value in values)
                            {
                                existingFacet.Values.Add(value);
                            }
                        }
                    }
                }

                productSearchOptions.FacetFields = facets;
            }
        }

        private void UpdateOptionsWithSorting(string sortField, CommerceConstants.SortDirection? sortDirection, CommerceSearchOptions productSearchOptions)
        {
            if (!string.IsNullOrEmpty(sortField))
            {
                productSearchOptions.SortField = sortField;

                if (sortDirection.HasValue)
                {
                    productSearchOptions.SortDirection = sortDirection.Value;
                }
            }
        }

    }
}