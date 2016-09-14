namespace Sitecore.Foundation.CommerceServer.Managers
{
    using Helpers;
    using Infrastructure.Constants;
    using Models;
    using Models.SitecoreItemModels;
    using Sitecore.Commerce.Connect.CommerceServer.Catalog;
    using Sitecore.Commerce.Connect.CommerceServer.Catalog.Fields;
    using Sitecore.Commerce.Entities.Prices;
    using Sitecore.Commerce.Services.Catalog;
    using Sitecore.Data;
    using Sitecore.Data.Items;
    using Sitecore.Diagnostics;
    using Sitecore.Foundation.CommerceServer.Interfaces;
    using Sitecore.Foundation.CommerceServer.Search;
    using Sitecore.Foundation.CommerceServer.Search.Models;
    using Sitecore.Foundation.CommerceServer.Utils;
    using Sitecore.Mvc.Presentation;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// CatalogManager class.
    /// </summary>  
    public class CatalogManager : ICatalogManager
    {
        private Catalog _currentCatalog;
        private readonly ILogger _logger;
        private ICatalogSearchManager _catalogSearchManager;
        private ISiteContext _siteContext;
        private readonly CatalogServiceProvider _catalogServiceProvider;
        private readonly IPricingManager _pricingManager;

        /// <summary>
        /// Gets the current catalog being accessed
        /// </summary>
        private Catalog CurrentCatalog
        {
            get
            {
                if (_currentCatalog == null)
                {
                    _currentCatalog = StorefrontManager.CurrentStorefront.DefaultCatalog;
                    if (_currentCatalog == null)
                    {
                        //There was no catalog associated with the storefront or we are not using multi-storefront
                        //So we use the default catalog
                        _currentCatalog = new Catalog(Context.Database.GetItem(CommerceConstants.KnownItemIds.DefaultCatalog));
                    }
                }

                return _currentCatalog;
            }
        }

        /// <summary>
        /// Gets the currently loaded Search Manager
        /// Instantiates a new one if one is not loaded
        /// </summary>
        private ICatalogSearchManager CatalogSearchManager
        {
            get
            {
                return this._catalogSearchManager ??
                       (this._catalogSearchManager = ContextTypeLoader.CreateInstance<ICatalogSearchManager>());
            }
        }

        /// <summary>
        /// Gets the current sitecontext
        /// </summary>
        private ISiteContext CurrentSiteContext
        {
            get
            {
                return this._siteContext ??
                        (this._siteContext = ContextTypeLoader.CreateInstance<ISiteContext>());
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CatalogManager" /> class.
        /// </summary>
        /// <param name="catalogServiceProvider">The catalog service provider.</param>
        /// <param name="pricingManager">The pricing manager.</param>
        /// <param name="inventoryManager">The inventory manager.</param>
        /// <param name="logger">The logger.</param>
        public CatalogManager(
            [NotNull] CatalogServiceProvider catalogServiceProvider,
            [NotNull] IPricingManager pricingManager,
            [NotNull] IInventoryManager inventoryManager,
            [NotNull] ILogger logger)
        {
            Assert.ArgumentNotNull(catalogServiceProvider, "catalogServiceProvider");
            Assert.ArgumentNotNull(pricingManager, "pricingManager");
            Assert.ArgumentNotNull(inventoryManager, "inventoryManager");

            this._catalogServiceProvider = catalogServiceProvider;
            this._pricingManager = pricingManager;
            this.InventoryManager = inventoryManager;
            _logger = logger;
        }

        /// <summary>
        /// Gets or sets the inventory manager.
        /// </summary>
        /// <value>
        /// The inventory manager.
        /// </value>
        public IInventoryManager InventoryManager { get; }
        
        /// <summary>
        /// This method returns child products for this category
        /// </summary>
        /// <param name="searchOptions">The options to perform the search with</param>
        /// <param name="categoryItem">The category item whose children to retrieve</param>
        /// <returns>A list of child products</returns>        
        public SearchResults GetChildProducts(CommerceSearchOptions searchOptions, Item categoryItem)
        {
            IEnumerable<CommerceQueryFacet> facets = null;
            var returnList = new List<Item>();
            var totalPageCount = 0;
            var totalProductCount = 0;
            SearchResponse searchResponse = null;

            if (CatalogUtility.IsItemDerivedFromCommerceTemplate(categoryItem, CommerceConstants.KnownTemplateIds.CommerceDynamicCategoryTemplate) || categoryItem.TemplateName == "Commerce Named Search")
            {
                try
                {
                    var defaultBucketQuery = categoryItem[CommerceConstants.KnownSitecoreFieldNames.DefaultBucketQuery];
                    var persistendBucketFilter = categoryItem[CommerceConstants.KnownSitecoreFieldNames.PersistentBucketFilter];
                    persistendBucketFilter = CleanLanguageFromFilter(persistendBucketFilter);
                    searchResponse = SearchNavigation.SearchCatalogItems(defaultBucketQuery, persistendBucketFilter, searchOptions);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message, this);
                }
            }
            else
            {
                searchResponse = SearchNavigation.GetCategoryProducts(categoryItem.ID, searchOptions);
            }

            if (searchResponse != null)
            {
                returnList.AddRange(searchResponse.ResponseItems);

                totalProductCount = searchResponse.TotalItemCount;
                totalPageCount = searchResponse.TotalPageCount;
                facets = searchResponse.Facets;
            }

            return new SearchResults(returnList, totalProductCount, totalPageCount, searchOptions.StartPageIndex, facets);
        }

        /// <summary>
        /// The <see cref="RelatedCatalogItemsViewModel" /> representing the related catalog items.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="visitorContext">The visitor context.</param>
        /// <param name="catalogItem">The catalog item.</param>
        /// <param name="rendering">The target renering.</param>
        /// <returns>
        /// The related catalog item view model.
        /// </returns>
        public RelatedCatalogItemsViewModel GetRelationshipsFromItem([NotNull] CommerceStorefront storefront, [NotNull] IVisitorContext visitorContext, Item catalogItem, Rendering rendering)
        {
            Assert.ArgumentNotNull(storefront, "storefront");

            if (catalogItem != null &&
                catalogItem.Fields.Contains(CommerceConstants.KnownFieldIds.RelationshipList) &&
                !string.IsNullOrEmpty(catalogItem[CommerceConstants.KnownFieldIds.RelationshipList]))
            {
                var field = new RelationshipField(catalogItem.Fields[CommerceConstants.KnownFieldIds.RelationshipList]);
                if (rendering != null &&
                    !string.IsNullOrWhiteSpace(rendering.RenderingItem.InnerItem["RelationshipsToDisplay"]))
                {
                    var relationshipsToDisplay = rendering.RenderingItem.InnerItem["RelationshipsToDisplay"].Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                    return this.GetRelationshipsFromField(storefront, visitorContext, field, rendering, relationshipsToDisplay);
                }
                else
                {
                    return this.GetRelationshipsFromField(storefront, visitorContext, field, rendering);
                }
            }

            return null;
        }

        /// <summary>
        /// Gets a lists of target items from a relationship field
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="visitorContext">The visitor context.</param>
        /// <param name="field">the relationship field</param>
        /// <param name="rendering">The target renering.</param>
        /// <returns>
        /// a list of relationship targets or null if no items found
        /// </returns>
        public RelatedCatalogItemsViewModel GetRelationshipsFromField([NotNull] CommerceStorefront storefront, [NotNull] IVisitorContext visitorContext, RelationshipField field, Rendering rendering)
        {
            Assert.ArgumentNotNull(storefront, "storefront");

            return GetRelationshipsFromField(storefront, visitorContext, field, rendering, null);
        }

        /// <summary>
        /// Gets a lists of target items from a relationship field
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="visitorContext">The visitor context.</param>
        /// <param name="field">the relationship field</param>
        /// <param name="rendering">The target renering.</param>
        /// <param name="relationshipNames">the names of the relationships, to retrieve (for example upsell).</param>
        /// <returns>
        /// a list of relationship targets or null if no items found
        /// </returns>
        public RelatedCatalogItemsViewModel GetRelationshipsFromField([NotNull] CommerceStorefront storefront, [NotNull] IVisitorContext visitorContext, RelationshipField field, Rendering rendering, IEnumerable<string> relationshipNames)
        {
            Assert.ArgumentNotNull(storefront, "storefront");

            relationshipNames = relationshipNames ?? Enumerable.Empty<string>();
            relationshipNames = relationshipNames.Select(s => s.Trim());
            var model = new RelatedCatalogItemsViewModel();

            if (field != null)
            {
                var productRelationshipInfoList = field.GetRelationships();
                productRelationshipInfoList = productRelationshipInfoList.OrderBy(x => x.Rank);
                var productModelList = this.GroupRelationshipsByDescription(storefront, visitorContext, field, relationshipNames, productRelationshipInfoList, rendering);
                model.RelatedProducts.AddRange(productModelList);
            }

            model.Initialize(rendering);

            return model;
        }

        /// <summary>
        /// Registers an event specifying that the category page has been visited.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="categoryId">The category identifier.</param>
        /// <param name="categoryName">The category name.</param>
        /// <returns>
        /// A <see cref="CatalogResult" /> specifying the result of the service request.
        /// </returns>
        public CatalogResult VisitedCategoryPage([NotNull] CommerceStorefront storefront, [NotNull] string categoryId, string categoryName)
        {
            Assert.ArgumentNotNull(storefront, "storefront");

            var request = new VisitedCategoryPageRequest(storefront.ShopName, categoryId, categoryName);
            return this._catalogServiceProvider.VisitedCategoryPage(request);
        }

        /// <summary>
        /// Registers an event specifying that the product details page has been visited.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="productId">the product id.</param>
        /// <param name="productName">Name of the product.</param>
        /// <param name="parentCategoryId">The parent category identifier.</param>
        /// <param name="parentCategoryName">the parent category name.</param>
        /// <returns>
        /// A <see cref="CatalogResult" /> specifying the result of the service request.
        /// </returns>
        public CatalogResult VisitedProductDetailsPage([NotNull] CommerceStorefront storefront, [NotNull] string productId, string productName, string parentCategoryId, string parentCategoryName)
        {
            Assert.ArgumentNotNull(storefront, "storefront");

            var request = new VisitedProductDetailsPageRequest(storefront.ShopName, productId, productName, parentCategoryId, parentCategoryName);
            return this._catalogServiceProvider.VisitedProductDetailsPage(request);
        }

        /// <summary>
        /// This method returns the ProductSearchResults for a datasource
        /// </summary>
        /// <param name="dataSource">The datasource to perform the search with</param>
        /// <param name="productSearchOptions">The search options.</param>
        /// <returns>A ProductSearchResults</returns>     
        public SearchResults GetProductSearchResults(Item dataSource, CommerceSearchOptions productSearchOptions)
        {
            Assert.ArgumentNotNull(productSearchOptions, "productSearchOptions");

            if (dataSource != null)
            {
                int totalProductCount = 0;
                int totalPageCount = 0;
                string error = string.Empty;

                if (dataSource.TemplateName == StorefrontConstants.KnownTemplateNames.CommerceNamedSearch || dataSource.TemplateName == StorefrontConstants.KnownTemplateNames.NamedSearch)
                {
                    var returnList = new List<Item>();
                    var facets = Enumerable.Empty<CommerceQueryFacet>();
                    var searchOptions = new CommerceSearchOptions(-1, 0);
                    var defaultBucketQuery = dataSource[CommerceConstants.KnownSitecoreFieldNames.DefaultBucketQuery];
                    var persistendBucketFilter = CleanLanguageFromFilter(dataSource[CommerceConstants.KnownSitecoreFieldNames.PersistentBucketFilter]);

                    try
                    {
                        var searchResponse = SearchNavigation.SearchCatalogItems(defaultBucketQuery, persistendBucketFilter, searchOptions);

                        if (searchResponse != null)
                        {
                            returnList.AddRange(searchResponse.ResponseItems);

                            totalProductCount = searchResponse.TotalItemCount;
                            totalPageCount = searchResponse.TotalPageCount;
                            facets = searchResponse.Facets;
                        }
                    }
                    catch (Exception ex)
                    {
                        error = ex.Message;
                    }

                    return new SearchResults(returnList, totalProductCount, totalPageCount, searchOptions.StartPageIndex, facets);
                }

                var childProducts = GetChildProducts(productSearchOptions, dataSource).SearchResultItems;
                return new SearchResults(childProducts, totalProductCount, totalPageCount, productSearchOptions.StartPageIndex, new List<CommerceQueryFacet>());
            }

            return null;
        }

        /// <summary>
        /// This method returns the current category by URL
        /// </summary>
        /// <returns>The category.</returns>
        public Category GetCurrentCategoryByUrl()
        {
            Category currentCategory;

            var categoryId = CatalogUrlManager.ExtractItemIdFromCurrentUrl();

            string virtualCategoryCacheKey = string.Format(CultureInfo.InvariantCulture, "VirtualCategory_{0}", categoryId);

            if (CurrentSiteContext.Items.Contains(virtualCategoryCacheKey))
            {
                currentCategory = CurrentSiteContext.Items[virtualCategoryCacheKey] as Category;
            }
            else
            {
                currentCategory = GetCategory(categoryId);
                CurrentSiteContext.Items.Add(virtualCategoryCacheKey, currentCategory);
            }

            return currentCategory;
        }

        /// <summary>
        /// Get category by id
        /// </summary>
        /// <param name="categoryId">The category identifier.</param>
        /// <returns>The category.</returns>
        public Category GetCategory(string categoryId)
        {
            var categoryItem = SearchNavigation.GetCategory(categoryId, CurrentCatalog.Name);
            return GetCategory(categoryItem);
        }

        /// <summary>
        /// Get category by item
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>The catgory.</returns>
        public Category GetCategory(Item item)
        {
            var category = new Category(item)
            {
                RequiredFacets = CatalogSearchManager.GetFacetFieldsForItem(item),
                SortFields = CatalogSearchManager.GetSortFieldsForItem(item),
                ItemsPerPage = CatalogSearchManager.GetItemsPerPageForItem(item)
            };

            return category;
        }

        /// <summary>
        /// Gets the product price.
        /// </summary>
        /// <param name="visitorContext">The visitor context.</param>
        /// <param name="productViewModel">The product view model.</param>
        public virtual void GetProductPrice([NotNull] IVisitorContext visitorContext, ProductViewModel productViewModel)
        {
            if (productViewModel == null)
            {
                return;
            }

            bool includeVariants = productViewModel.Variants != null && productViewModel.Variants.Count > 0;
            var pricesResponse = this._pricingManager.GetProductPrices(StorefrontManager.CurrentStorefront, visitorContext, productViewModel.CatalogName, productViewModel.ProductId, includeVariants, null);
            if (pricesResponse != null && pricesResponse.ServiceProviderResult.Success && pricesResponse.Result != null)
            {
                Price price;
                if (pricesResponse.Result.TryGetValue(productViewModel.ProductId, out price))
                {
                    ExtendedCommercePrice extendedPrice = (ExtendedCommercePrice)price;
                    productViewModel.ListPrice = price.Amount;
                    productViewModel.AdjustedPrice = extendedPrice.ListPrice;
                }

                if (includeVariants)
                {
                    foreach (var variant in productViewModel.Variants)
                    {
                        if (pricesResponse.Result.TryGetValue(variant.VariantId, out price))
                        {
                            ExtendedCommercePrice extendedPrice = (ExtendedCommercePrice)price;

                            variant.ListPrice = extendedPrice.Amount;
                            variant.AdjustedPrice = extendedPrice.ListPrice;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets the product price.
        /// </summary>
        /// <param name="visitorContext">The visitor context.</param>
        /// <param name="productViewModels">The product view models.</param>
        public virtual void GetProductBulkPrices([NotNull] IVisitorContext visitorContext, List<ProductViewModel> productViewModels)
        {
            if (productViewModels == null || !productViewModels.Any())
            {
                return;
            }

            var catalogName = productViewModels.Select(p => p.CatalogName).First().ToString();
            var productIds = productViewModels.Select(p => p.ProductId).ToList();

            var pricesResponse = this._pricingManager.GetProductBulkPrices(StorefrontManager.CurrentStorefront, visitorContext, catalogName, productIds, null);
            var prices = pricesResponse != null && pricesResponse.Result != null ? pricesResponse.Result : new Dictionary<string, Price>();

            foreach (var productViewModel in productViewModels)
            {
                Price price;
                if (prices.Any() && prices.TryGetValue(productViewModel.ProductId, out price))
                {
                    ExtendedCommercePrice extendedPrice = (ExtendedCommercePrice)price;

                    productViewModel.ListPrice = extendedPrice.Amount;
                    productViewModel.AdjustedPrice = extendedPrice.ListPrice;

                    productViewModel.LowestPricedVariantAdjustedPrice = extendedPrice.LowestPricedVariant;
                    productViewModel.LowestPricedVariantListPrice = extendedPrice.LowestPricedVariantListPrice;
                    productViewModel.HighestPricedVariantAdjustedPrice = extendedPrice.HighestPricedVariant;
                }
            }
        }

        /// <summary>
        /// Gets the product rating.
        /// </summary>
        /// <param name="productItem">The product item.</param>
        /// <returns>The product rating</returns>
        public virtual decimal GetProductRating(Item productItem)
        {
            var ratingString = productItem["Rating"];
            decimal rating;
            if (decimal.TryParse(ratingString, out rating))
            {
                return rating;
            }

            return 0;
        }

        /// <summary>
        /// Visiteds the product details page.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <returns>
        /// The manager response
        /// </returns>
        public virtual ManagerResponse<CatalogResult, bool> VisitedProductDetailsPage([NotNull] CommerceStorefront storefront)
        {
            Assert.ArgumentNotNull(storefront, "storefront");

            string productId = CatalogUrlManager.ExtractItemIdFromCurrentUrl();
            string parentCategoryName = CatalogUrlManager.ExtractCategoryNameFromCurrentUrl();
            var request = new VisitedProductDetailsPageRequest(storefront.ShopName, productId, productId, parentCategoryName, parentCategoryName);

            var result = this._catalogServiceProvider.VisitedProductDetailsPage(request);

            if (!result.Success)
            {
                _logger.LogInfo(string.Join(Environment.NewLine, result.SystemMessages.Select(m => m.Message)), this);
            }

            return new ManagerResponse<CatalogResult, bool>(result, result.Success);
        }

        /// <summary>
        /// Facets the applied.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="facet">The facet.</param>
        /// <param name="isApplied">if set to <c>true</c> [is applied].</param>
        /// <returns>The manager response.</returns>
        public virtual ManagerResponse<CatalogResult, bool> FacetApplied([NotNull] CommerceStorefront storefront, string facet, bool isApplied)
        {
            Assert.ArgumentNotNull(storefront, "storefront");

            var request = new FacetAppliedRequest(storefront.ShopName, facet, isApplied);
            var result = this._catalogServiceProvider.FacetApplied(request);

            if (!result.Success)
            {
                _logger.LogInfo(string.Join(Environment.NewLine, result.SystemMessages.Select(m => m.Message)), this);
            }

            return new ManagerResponse<CatalogResult, bool>(result, result.Success);
        }

        /// <summary>
        /// Sorts the order applied.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="sortKey">The sort key.</param>
        /// <param name="sortDirection">The sort direction.</param>
        /// <returns>The manager response.</returns>
        public virtual ManagerResponse<CatalogResult, bool> SortOrderApplied([NotNull] CommerceStorefront storefront, string sortKey, CommerceConstants.SortDirection? sortDirection)
        {
            Assert.ArgumentNotNull(storefront, "storefront");

            Commerce.Entities.Catalog.SortDirection connectSortDirection = Commerce.Entities.Catalog.SortDirection.Ascending;
            if (sortDirection.HasValue)
            {
                switch (sortDirection.Value)
                {
                    case CommerceConstants.SortDirection.Asc:
                        connectSortDirection = Commerce.Entities.Catalog.SortDirection.Ascending;
                        break;

                    default:
                        connectSortDirection = Commerce.Entities.Catalog.SortDirection.Descending;
                        break;
                }
            }

            var request = new ProductSortingRequest(storefront.ShopName, sortKey, connectSortDirection);
            var result = this._catalogServiceProvider.ProductSorting(request);

            if (!result.Success)
            {
                _logger.LogInfo(string.Join(Environment.NewLine, result.SystemMessages.Select(m => m.Message)), this);
            }

            return new ManagerResponse<CatalogResult, bool>(result, result.Success);
        }

        /// <summary>
        /// Registers the search event.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="searchKeyword">The search keyword.</param>
        /// <param name="numberOfHits">The number of hits.</param>
        /// <returns>
        /// The manager response
        /// </returns>
        public virtual ManagerResponse<CatalogResult, bool> RegisterSearchEvent([NotNull] CommerceStorefront storefront, string searchKeyword, int numberOfHits)
        {
            Assert.ArgumentNotNull(storefront, "storefront");
            Assert.ArgumentNotNullOrEmpty(searchKeyword, "searchKeyword");

            var request = new SearchInitiatedRequest(storefront.ShopName, searchKeyword, numberOfHits);
            var result = this._catalogServiceProvider.SearchInitiated(request);

            if (!result.Success)
            {
                _logger.LogInfo(string.Join(Environment.NewLine, result.SystemMessages.Select(m => m.Message)), this);
            }

            return new ManagerResponse<CatalogResult, bool>(result, result.Success);
        }

        #region Protected helper methods

        /// <summary>
        /// Cleans the language from filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>The updated filter.</returns>
        protected string CleanLanguageFromFilter(string filter)
        {
            if (filter.IndexOf("language:", StringComparison.OrdinalIgnoreCase) < 0)
            {
                return filter;
            }

            var newFilter = new StringBuilder();

            var statementList = filter.Split(';');
            foreach (var statement in statementList)
            {
                if (statement.IndexOf("language", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    continue;
                }

                if (newFilter.Length > 0)
                {
                    newFilter.Append(';');
                }

                newFilter.Append(statement);
            }

            return newFilter.ToString();
        }

        /// <summary>
        /// Groups the provided relationships by name, and converts them into a list of <see cref="CategoryViewModel" /> objects.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="visitorContext">The visitor context.</param>
        /// <param name="field">The relationship field.</param>
        /// <param name="relationshipNames">The names of the relationships to retrieve.</param>
        /// <param name="productRelationshipInfoList">The list of rerlationships to group and convert.</param>
        /// <param name="rendering">The rendering.</param>
        /// <returns>The grouped relationships converted into a list of <see cref="CategoryViewModel" /> objects.</returns>
        protected IEnumerable<CategoryViewModel> GroupRelationshipsByDescription([NotNull] CommerceStorefront storefront, [NotNull] IVisitorContext visitorContext, RelationshipField field, IEnumerable<string> relationshipNames, IEnumerable<CatalogRelationshipInformation> productRelationshipInfoList, Rendering rendering)
        {
            var relationshipGroups = new Dictionary<string, CategoryViewModel>(StringComparer.OrdinalIgnoreCase);

            if (field != null && productRelationshipInfoList != null)
            {
                foreach (var relationshipInfo in productRelationshipInfoList)
                {
                    if (!relationshipNames.Any() || relationshipNames.Contains(relationshipInfo.RelationshipName, StringComparer.OrdinalIgnoreCase))
                    {
                        Item lookupItem = null;
                        bool usingRelationshipName = string.IsNullOrWhiteSpace(relationshipInfo.RelationshipDescription);
                        var relationshipDescription = string.IsNullOrWhiteSpace(relationshipInfo.RelationshipDescription) ? StorefrontManager.GetRelationshipName(relationshipInfo.RelationshipName, out lookupItem) : relationshipInfo.RelationshipDescription;
                        CategoryViewModel categoryModel;
                        if (!relationshipGroups.TryGetValue(relationshipDescription, out categoryModel))
                        {
                            categoryModel = new CategoryViewModel
                            {
                                ChildProducts = new List<ProductViewModel>(),
                                RelationshipName = relationshipInfo.RelationshipName,
                                RelationshipDescription = relationshipDescription,
                                LookupRelationshipItem = (usingRelationshipName) ? lookupItem : null
                            };

                            relationshipGroups[relationshipDescription] = categoryModel;
                        }

                        var targetItemId = ID.Parse(relationshipInfo.ToItemExternalId);
                        var targetItem = field.InnerField.Database.GetItem(targetItemId);
                        var productModel = new ProductViewModel(targetItem);
                        productModel.Initialize(rendering);

                        this.GetProductRating(targetItem);

                        categoryModel.ChildProducts.Add(productModel);
                    }
                }
            }

            if (relationshipGroups.Count > 0)
            {
                List<ProductViewModel> productViewModelList = new List<ProductViewModel>();

                foreach (string key in relationshipGroups.Keys)
                {
                    CategoryViewModel viewModel = relationshipGroups[key];
                    var childProducts = viewModel.ChildProducts;
                    if (childProducts != null && childProducts.Count > 0)
                    {
                        productViewModelList.AddRange(childProducts);
                    }
                }

                if (productViewModelList.Count > 0)
                {
                    this.GetProductBulkPrices(visitorContext, productViewModelList);
                    this.InventoryManager.GetProductsStockStatusForList(storefront, productViewModelList);
                }
            }

            return relationshipGroups.Values;
        }

        #endregion
    }
}