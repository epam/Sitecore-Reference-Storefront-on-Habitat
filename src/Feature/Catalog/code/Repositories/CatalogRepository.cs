using Sitecore.Data.Fields;
using Sitecore.Data.Managers;
using Sitecore.Foundation.CommerceServer.Extensions;

namespace Sitecore.Feature.Catalog.Repositories
{
    using Sitecore.Commerce.Connect.CommerceServer.Inventory.Models;
    using Sitecore.Commerce.Entities.Inventory;
    using Sitecore.Data.Items;
    using Sitecore.Diagnostics;
    using Sitecore.Feature.Base.Models.JsonResults;
    using Sitecore.Feature.Base.Repositories;
    using Sitecore.Feature.Catalog.Models;
    using Sitecore.Feature.Catalog.Models.JsonResults;
    using Sitecore.Foundation.CommerceServer.Helpers;
    using Sitecore.Foundation.CommerceServer.Infrastructure.Constants;
    using Sitecore.Foundation.CommerceServer.Interfaces;
    using Sitecore.Foundation.CommerceServer.Managers;
    using Sitecore.Foundation.CommerceServer.Models;
    using Sitecore.Foundation.CommerceServer.Models.SitecoreItemModels;
    using Sitecore.Foundation.CommerceServer.Search.Models;
    using Sitecore.Mvc.Presentation;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using CommerceTemplates = Sitecore.Foundation.CommerceServer.Templates;

    public class CatalogRepository : BaseRepository, ICatalogRepository
    {
        private const string CurrentProductViewModelKeyName = "CurrentProductViewModel";
        private readonly ICartManager _cartManager;
        private readonly ICatalogManager _catalogManager;
        private readonly IInventoryManager _inventoryManager;
        private readonly IPricingManager _pricingManager;

        public CatalogRepository(
            [NotNull] IAccountManager accountManager,
            [NotNull] IContactFactory contactFactory,
            [NotNull] ICartManager cartManager,
            [NotNull] ICatalogManager catalogManager,
            [NotNull] IInventoryManager inventoryManager,
            [NotNull] IPricingManager pricingManager)
            : base(accountManager, contactFactory)
        {
            _cartManager = cartManager;
            _catalogManager = catalogManager;
            _inventoryManager = inventoryManager;
            _pricingManager = pricingManager;
        }

        public void ApplyFacet(string facetValue, bool? isApplied)
        {
            if (!string.IsNullOrWhiteSpace(facetValue) && isApplied.HasValue)
            {
                _catalogManager.FacetApplied(this.CurrentStorefront, facetValue, isApplied.Value);
            }
        }

        public void ApplySortOrder(string sortField, CommerceConstants.SortDirection? sortDirection)
        {
            if (!string.IsNullOrWhiteSpace(sortField))
            {
                _catalogManager.SortOrderApplied(this.CurrentStorefront, sortField, sortDirection);
            }
        }

        public CurrencyMenuViewModel GetCurrencyMenu(Rendering currentRendering)
        {
            var currencyMenuModel = new CurrencyMenuViewModel();
            var response = _pricingManager.GetSupportedCurrencies(this.CurrentStorefront, this.CurrentStorefront.DefaultCatalog.Name);

            if (response.ServiceProviderResult.Success)
            {
                currencyMenuModel.Initialize(currentRendering, response.ServiceProviderResult);
            }

            return currencyMenuModel;
        }

        public StockInfoListBaseJsonResult GetCurrentProductStockInfo(string productId)
        {
            var currentProductItem = SearchNavigation.GetProduct(productId, CurrentCatalog.Name);
            productId = currentProductItem.Name;
            var catalogName = currentProductItem[CommerceTemplates.CommerceGenerated.GeneralCategory.Fields.CatalogName];
            var products = new List<CommerceInventoryProduct>();

            if (currentProductItem.HasChildren)
            {
                foreach (Item item in currentProductItem.Children)
                {
                    products.Add(new CommerceInventoryProduct
                    {
                        ProductId = productId,
                        CatalogName = catalogName,
                        VariantId = item.Name
                    });
                }
            }
            else
            {
                products.Add(new CommerceInventoryProduct { ProductId = productId, CatalogName = catalogName });
            }

            var response = _inventoryManager.GetStockInformation(this.CurrentStorefront, products, StockDetailsLevel.All);
            var result = new StockInfoListBaseJsonResult(response.ServiceProviderResult);

            if (response.Result == null)
            {
                return result;
            }

            result.Initialize(response.Result);
            var stockInfo = response.Result.FirstOrDefault();

            if (stockInfo != null)
            {
                _inventoryManager.VisitedProductStockStatus(this.CurrentStorefront, stockInfo, string.Empty);
            }

            return result;
        }

        public ProductFacetsViewModel GetFacets(Rendering currentRendering, int? pageNumber, string facetValues, string sortField, int? pageSize, CommerceConstants.SortDirection? sortDirection)
        {
            var currentCategory = _catalogManager.GetCurrentCategoryByUrl();
            var productSearchOptions = new CommerceSearchOptions(pageSize.GetValueOrDefault(currentCategory.ItemsPerPage), pageNumber.GetValueOrDefault(0));

            SetSortParameters(currentCategory, ref sortField, ref sortDirection);
            UpdateOptionsWithFacets(currentCategory.RequiredFacets, facetValues, productSearchOptions);
            UpdateOptionsWithSorting(sortField, sortDirection, productSearchOptions);

            var productFacetsviewModel = GetProductFacetsViewModel(productSearchOptions, currentCategory.InnerItem, currentRendering);

            return productFacetsviewModel;
        }

        public MultipleProductSearchResults GetMultipleProductList(Item datasource, Rendering currentRendering, CommerceSearchOptions productSearchOptions)
        {
            var multipleProductSearchResults = GetMultipleProductSearchResults(datasource, productSearchOptions);

            if (multipleProductSearchResults != null)
            {
                multipleProductSearchResults.Initialize(currentRendering);
                multipleProductSearchResults.DisplayName = datasource.DisplayName;

                var products = multipleProductSearchResults.ProductSearchResults.SelectMany(productSearchResult => productSearchResult.Products).ToList();
                _catalogManager.GetProductBulkPrices(this.CurrentVisitorContext, products);
                _inventoryManager.GetProductsStockStatusForList(this.CurrentStorefront, products);

                foreach (var productViewModel in products)
                {
                    Item productItem = multipleProductSearchResults.SearchResults
                        .SelectMany(productSearchResult => productSearchResult.SearchResultItems)
                        .FirstOrDefault(item => item.Name == productViewModel.ProductId);
                    productViewModel.CustomerAverageRating = _catalogManager.GetProductRating(productItem);
                }
            }

            return multipleProductSearchResults;
        }

        public RelatedCatalogItemsViewModel GetRelatedCatalogItems(Item item, Rendering currentRendering)
        {
            if (item.Name == "*")
            {
                var productViewModel = GetWildCardProductViewModel(item, currentRendering);
                var relatedCatalogItemsModel = _catalogManager.GetRelationshipsFromItem(this.CurrentStorefront, this.CurrentVisitorContext, productViewModel.Item, currentRendering);

                return relatedCatalogItemsModel;
            }
            else
            {
                var relatedCatalogItemsModel = _catalogManager.GetRelationshipsFromItem(this.CurrentStorefront, this.CurrentVisitorContext, item, currentRendering);

                return relatedCatalogItemsModel;
            }
        }

        public PaginationViewModel GetPagination(Rendering currentRendering, int? pageNumber, int? pageSize, string facetValues)
        {
            var currentCategory = _catalogManager.GetCurrentCategoryByUrl();
            var productSearchOptions = new CommerceSearchOptions(pageSize.GetValueOrDefault(currentCategory.ItemsPerPage),
                pageNumber.GetValueOrDefault(0));

            UpdateOptionsWithFacets(currentCategory.RequiredFacets, facetValues, productSearchOptions);

            var paginationViewModel = GetPaginationViewModel(productSearchOptions, currentCategory.InnerItem, currentRendering);

            return paginationViewModel;
        }

        public CategoryViewModel GetProductList(Rendering currentRendering, int? pageNumber, string facetValues, string sortField, int? pageSize, CommerceConstants.SortDirection? sortDirection)
        {
            var currentCategory = _catalogManager.GetCurrentCategoryByUrl();
            var productSearchOptions = new CommerceSearchOptions(pageSize.GetValueOrDefault(currentCategory.ItemsPerPage),
                pageNumber.GetValueOrDefault(0));

            UpdateOptionsWithFacets(currentCategory.RequiredFacets, facetValues, productSearchOptions);
            UpdateOptionsWithSorting(sortField, sortDirection, productSearchOptions);

            var categoryViewModel = GetCategoryViewModel(productSearchOptions, currentCategory.SortFields,
                currentCategory.InnerItem, currentRendering, currentCategory.InnerItem.DisplayName);

            return categoryViewModel;
        }

        public ProductListHeaderViewModel GetProductListHeader(Rendering currentRendering, int? pageNumber, string facetValues, string sortField, int? pageSize, CommerceConstants.SortDirection? sortDirection)
        {
            var currentCategory = _catalogManager.GetCurrentCategoryByUrl();
            var productSearchOptions = new CommerceSearchOptions(pageSize.GetValueOrDefault(currentCategory.ItemsPerPage),
                pageNumber.GetValueOrDefault(0));

            SetSortParameters(currentCategory, ref sortField, ref sortDirection);
            UpdateOptionsWithFacets(currentCategory.RequiredFacets, facetValues, productSearchOptions);
            UpdateOptionsWithSorting(sortField, sortDirection, productSearchOptions);

            var productListHeaderViewModel = GetProductListHeaderViewModel(productSearchOptions, currentCategory.SortFields, currentCategory.InnerItem, currentRendering);

            if (sortField != null)
            {
                productListHeaderViewModel.SelecterSortField = sortField;
                productListHeaderViewModel.SelectedSortDirection = sortDirection;
            }

            return productListHeaderViewModel;
        }

        public ProductViewModel GetWildCardProductViewModel(Item datasource, Rendering currentRendering)
        {
            ProductViewModel productViewModel;
            var productId = CatalogUrlManager.ExtractItemIdFromCurrentUrl().Replace(" ", "-");
            var virtualProductCacheKey = string.Format(CultureInfo.InvariantCulture, "VirtualProduct_{0}", productId);

            if (this.CurrentSiteContext.Items.Contains(virtualProductCacheKey))
            {
                productViewModel = this.CurrentSiteContext.Items[virtualProductCacheKey] as ProductViewModel;
            }
            else
            {
                if (string.IsNullOrEmpty(productId))
                {
                    //No ProductId passed in on the URL
                    //Use to Storefront DefaultProductId
                    productId = StorefrontManager.CurrentStorefront.DefaultProductId;
                }

                var productItem = SearchNavigation.GetProduct(productId, this.CurrentCatalog.Name);

                if (productItem == null)
                {
                    var message = string.Format(CultureInfo.InvariantCulture, "The requested product '{0}' does not exist in the catalog '{1}' or cannot be displayed in the language '{2}'", productId, this.CurrentCatalog.Name, Context.Language);
                    Log.Error(message, this);
                    throw new InvalidOperationException(message);
                }

                datasource = productItem;
                productViewModel = this.GetProductViewModel(datasource, currentRendering);
                this.CurrentSiteContext.Items.Add(virtualProductCacheKey, productViewModel);
            }

            return productViewModel;
        }

        public BaseJsonResult SwitchCurrency(string currency)
        {
            if (!string.IsNullOrWhiteSpace(currency))
            {
                if (this.CurrentStorefront.IsSupportedCurrency(currency))
                {
                    StorefrontManager.SetCustomerCurrency(currency);
                    _pricingManager.CurrencyChosenPageEvent(this.CurrentStorefront, currency);
                    _cartManager.UpdateCartCurrency(this.CurrentStorefront, this.CurrentVisitorContext, currency);
                }
                else
                {
                    var json = new BaseJsonResult { Success = false };
                    json.Errors.Add(StorefrontManager.GetSystemMessage(StorefrontConstants.SystemMessages.InvalidCurrencyError));
                    return json;
                }
            }

            return new BaseJsonResult();
        }

        private CategoryViewModel GetCategoryViewModel(CommerceSearchOptions productSearchOptions, IEnumerable<CommerceQuerySort> sortFields, Item categoryItem, Rendering rendering, string cacheName)
        {
            var cacheKey = string.Format(CultureInfo.InvariantCulture, "Category/{0}", cacheName);

            if (this.CurrentSiteContext.Items[cacheKey] != null)
            {
                return (CategoryViewModel)this.CurrentSiteContext.Items[cacheKey];
            }

            var categoryViewModel = new CategoryViewModel(categoryItem);
            var childProducts = this.GetChildProducts(productSearchOptions, categoryItem);
            categoryViewModel.Initialize(rendering, childProducts, sortFields, productSearchOptions);

            if (childProducts != null && childProducts.SearchResultItems.Count > 0)
            {
                _catalogManager.GetProductBulkPrices(this.CurrentVisitorContext, categoryViewModel.ChildProducts);
                _inventoryManager.GetProductsStockStatusForList(this.CurrentStorefront, categoryViewModel.ChildProducts);

                foreach (var productViewModel in categoryViewModel.ChildProducts)
                {
                    Item productItem = childProducts.SearchResultItems.Single(item => item.Name == productViewModel.ProductId);
                    productViewModel.CustomerAverageRating = _catalogManager.GetProductRating(productItem);
                }
            }

            this.CurrentSiteContext.Items[cacheKey] = categoryViewModel;

            return categoryViewModel;
        }

        private SearchResults GetChildProducts(CommerceSearchOptions searchOptions, Item categoryItem)
        {
            var childProductsCacheKey = string.Format(CultureInfo.InvariantCulture, "ChildProductSearch_{0}", categoryItem.ID.ToString());

            if (this.CurrentSiteContext.Items.Contains(childProductsCacheKey))
            {
                return (SearchResults)this.CurrentSiteContext.Items[childProductsCacheKey];
            }

            var products = _catalogManager.GetChildProducts(searchOptions, categoryItem);
            this.CurrentSiteContext.Items[childProductsCacheKey] = products;

            return products;
        }

        private MultipleProductSearchResults GetMultipleProductSearchResults(BaseItem dataSource,
            CommerceSearchOptions productSearchOptions)
        {
            Assert.ArgumentNotNull(productSearchOptions, "productSearchOptions");

            MultilistField searchesField = dataSource.Fields[Templates.ProductSearch.Fields.NamedSearches.ToString()];
            var searches = searchesField.GetItems();
            var productsSearchResults = new List<SearchResults>();

            foreach (var search in searches)
            {
                if (TemplateManager.GetTemplate(search).GetBaseTemplates().FirstOrDefault(t => t.ID == Templates.NamedSearch.ID) != null)
                {
                    var productsSearchResult = _catalogManager.GetProductSearchResults(search, productSearchOptions);

                    if (productsSearchResult != null)
                    {
                        productsSearchResult.NamedSearchItem = search;
                        productsSearchResult.DisplayName = search[Templates.NamedSearch.Fields.Title.ToString()];
                        productsSearchResults.Add(productsSearchResult);
                    }
                }
                else if (TemplateManager.GetTemplate(search).GetBaseTemplates().FirstOrDefault(t => t.ID == Templates.SelectedProducts.ID) != null)
                {
                    var itemCount = 0;
                    var staticSearchList = new SearchResults
                    {
                        DisplayName = search[Templates.SelectedProducts.Fields.Title.ToString()],
                        NamedSearchItem = search
                    };

                    MultilistField productListField = search.Fields[Templates.SelectedProducts.Fields.ProductList.ToString()];
                    var productList = productListField.GetItems();

                    foreach (var productItem in productList)
                    {
                        var catalogItemtype = productItem.ItemType();

                        if (catalogItemtype == StorefrontConstants.ItemTypes.Category || catalogItemtype == StorefrontConstants.ItemTypes.Product)
                        {
                            staticSearchList.SearchResultItems.Add(productItem);
                            itemCount++;
                        }
                    }

                    staticSearchList.TotalItemCount = itemCount;
                    staticSearchList.TotalPageCount = itemCount;
                    productsSearchResults.Add(staticSearchList);
                }
            }

            return new MultipleProductSearchResults(productsSearchResults);
        }

        private PaginationViewModel GetPaginationViewModel(CommerceSearchOptions productSearchOptions, Item categoryItem, Rendering rendering)
        {
            var viewModel = new PaginationViewModel();
            SearchResults childProducts = null;

            if (productSearchOptions != null)
            {
                childProducts = GetChildProducts(productSearchOptions, categoryItem);
            }

            viewModel.Initialize(rendering, childProducts, productSearchOptions);

            return viewModel;
        }

        private ProductViewModel GetProductViewModel(Item productItem, Rendering rendering)
        {
            if (this.CurrentSiteContext.Items[CurrentProductViewModelKeyName] != null)
            {
                return (ProductViewModel)this.CurrentSiteContext.Items[CurrentProductViewModelKeyName];
            }

            var variants = new List<VariantViewModel>();

            if (productItem != null && productItem.HasChildren)
            {
                foreach (Item item in productItem.Children)
                {
                    var v = new VariantViewModel(item);
                    variants.Add(v);
                }
            }

            var productViewModel = new ProductViewModel(productItem);
            productViewModel.Initialize(rendering, variants);
            productViewModel.ProductName = productViewModel.DisplayName;

            if (this.CurrentSiteContext.UrlContainsCategory)
            {
                productViewModel.ParentCategoryId = CatalogUrlManager.ExtractCategoryNameFromCurrentUrl();
                var category = _catalogManager.GetCategory(productViewModel.ParentCategoryId);

                if (category != null)
                {
                    productViewModel.ParentCategoryName = category.DisplayName;
                }
            }

            _catalogManager.GetProductPrice(this.CurrentVisitorContext, productViewModel);
            productViewModel.CustomerAverageRating = _catalogManager.GetProductRating(productItem);
            this.CurrentSiteContext.Items[CurrentProductViewModelKeyName] = productViewModel;

            return productViewModel;
        }

        private ProductFacetsViewModel GetProductFacetsViewModel(CommerceSearchOptions productSearchOptions, Item categoryItem, Rendering rendering)
        {
            var viewModel = new ProductFacetsViewModel();
            SearchResults childProducts = null;

            if (productSearchOptions != null)
            {
                childProducts = GetChildProducts(productSearchOptions, categoryItem);
            }

            viewModel.Initialize(rendering, childProducts, productSearchOptions);

            return viewModel;
        }

        private ProductListHeaderViewModel GetProductListHeaderViewModel(CommerceSearchOptions productSearchOptions, IEnumerable<CommerceQuerySort> sortFields, Item categoryItem, Rendering rendering)
        {
            var viewModel = new ProductListHeaderViewModel();
            SearchResults childProducts = null;

            if (productSearchOptions != null)
            {
                childProducts = GetChildProducts(productSearchOptions, categoryItem);
            }

            viewModel.Initialize(rendering, childProducts, sortFields, productSearchOptions);

            return viewModel;
        }

        private void SetSortParameters(Category category, ref string sortField, ref CommerceConstants.SortDirection? sortOrder)
        {
            if (string.IsNullOrWhiteSpace(sortField))
            {
                var sortfieldList = category.SortFields;

                if (sortfieldList != null && sortfieldList.Any())
                {
                    sortField = sortfieldList.First().Name;
                    sortOrder = CommerceConstants.SortDirection.Asc;
                }
            }
        }

        private void UpdateOptionsWithFacets(IEnumerable<CommerceQueryFacet> facets, string valueQueryString, CommerceSearchOptions productSearchOptions)
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