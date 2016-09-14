using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Sitecore.Commerce.Connect.CommerceServer.Search.Models;
using Sitecore.Commerce.Entities.Inventory;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Linq;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.ContentSearch.Security;
using Sitecore.ContentSearch.Utilities;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Foundation.CommerceServer.Infrastructure.Constants;
using Sitecore.Foundation.CommerceServer.Interfaces;
using Sitecore.Foundation.CommerceServer.Items;
using Sitecore.Foundation.CommerceServer.Managers;
using Sitecore.Foundation.CommerceServer.Models;
using Sitecore.Foundation.CommerceServer.Utils;

namespace Sitecore.Foundation.CommerceServer.Helpers
{
    using Sitecore.Commerce.Connect.CommerceServer.Search;

    /// <summary>
    /// Static helper class to aid with search for navigation.
    /// </summary>
    public static class SearchNavigation
    {
        /// <summary>
        /// Gets the current language based off of the sitecore context.
        /// </summary>
        public static string CurrentLanguageName
        {
            get
            {
                return Sitecore.Context.Language.Name;
            }
        }

        /// <summary>
        /// Returns the navigation categories based on a root navigation ID identified by a Data Source string.
        /// </summary>
        /// <param name="navigationDataSource">A Sitecore Item ID or query that identifies the root navigation ID.</param>
        /// <param name="searchOptions">The paging options for this query.</param>
        /// <returns>A list of category items.</returns>
        /// TODO:remove unused
        public static Search.SearchResponse GetNavigationCategories(string navigationDataSource, Search.Models.CommerceSearchOptions searchOptions)
        {
            ID navigationId;
            var searchManager = ContextTypeLoader.CreateInstance<ICatalogSearchManager>();
            var searchIndex = searchManager.GetIndex();

            if (navigationDataSource.IsGuid())
            {
                navigationId = ID.Parse(navigationDataSource);
            }
            else
            {
                using (var context = searchIndex.CreateSearchContext())
                {
                    var query = LinqHelper.CreateQuery<SitecoreUISearchResultItem>(context, SearchStringModel.ParseDatasourceString(navigationDataSource))
                        .Select(result => result.GetItem().ID);

                    if (query.Any())
                    {
                        navigationId = query.First();
                    }
                    else
                    {
                        return new Search.SearchResponse();
                    }
                }
            }

            using (var context = searchIndex.CreateSearchContext())
            {
                var searchResults = context.GetQueryable<CommerceBaseCatalogSearchResultItem>()
                   .Where(item => item.CommerceSearchItemType == CommerceSearchResultItemType.Category)
                   .Where(item => item.Language == CurrentLanguageName)
                   .Where(item => item.CommerceAncestorIds.Contains(navigationId))
                   .Select(p => new CommerceBaseCatalogSearchResultItem()
                   {
                       ItemId = p.ItemId,
                       Uri = p.Uri
                   });

                searchResults = searchManager.AddSearchOptionsToQuery(searchResults, searchOptions.ConnectSearchOptions);

                var results = searchResults.GetResults();
                var response = Search.SearchResponse.CreateFromSearchResultsItems(searchOptions, results);

                return response;
            }
        }

        /// <summary>
        /// Gets a category based on its name.
        /// </summary>
        /// <param name="categoryName">The name of the category.</param>
        /// <param name="catalogName">The name of the catalog containing the category.</param>        
        /// <returns>The found category item, or null if not found.</returns>
        public static Item GetCategory(string categoryName, string catalogName)
        {
            Sitecore.Diagnostics.Assert.ArgumentNotNullOrEmpty(catalogName, "catalogName");

            Item result = null;
            var searchManager = ContextTypeLoader.CreateInstance<ICatalogSearchManager>();
            var searchIndex = searchManager.GetIndex(catalogName);

            using (var context = searchIndex.CreateSearchContext())
            {
                var categoryQuery = context.GetQueryable<CommerceBaseCatalogSearchResultItem>()
                    .Where(item => item.CommerceSearchItemType == CommerceSearchResultItemType.Category)
                    .Where(item => item.Language == CurrentLanguageName)
                    .Where(item => (item.Name == categoryName && item.CatalogName == catalogName) || (item.Name == categoryName))
                    .Select(p => new CommerceBaseCatalogSearchResultItem()
                    {
                        ItemId = p.ItemId,
                        Uri = p.Uri
                    })
                    .Take(1);

                var foundSearchItem = categoryQuery.FirstOrDefault();
                if (foundSearchItem != null)
                {
                    result = foundSearchItem.GetItem();
                }
            }

            return result;
        }

        /// <summary>
        /// Gets a product based on its product id.
        /// </summary>
        /// <param name="productId">The product's id.</param> 
        /// <param name="catalogName">The name of the catalog containing the product.</param>		       
        /// <returns>The found product item, or null if not found.</returns>
        public static Item GetProduct(string productId, string catalogName)
        {
            Sitecore.Diagnostics.Assert.ArgumentNotNullOrEmpty(catalogName, "catalogName");

            Item result = null;
            var searchManager = ContextTypeLoader.CreateInstance<ICatalogSearchManager>();
            var searchIndex = searchManager.GetIndex(catalogName);

            using (var context = searchIndex.CreateSearchContext())
            {
                var productQuery = context.GetQueryable<CommerceProductSearchResultItem>()
                    .Where(item => item.CommerceSearchItemType == CommerceSearchResultItemType.Product)
                    .Where(item => item.CatalogName == catalogName)
                    .Where(item => item.Language == CurrentLanguageName)
                    .Where(item => item.CatalogItemId == productId.ToLowerInvariant())
                    .Select(p => new CommerceProductSearchResultItem()
                    {
                        ItemId = p.ItemId,
                        Uri = p.Uri
                    })
                    .Take(1);

                var foundSearchItem = productQuery.FirstOrDefault();
                if (foundSearchItem != null)
                {
                    result = foundSearchItem.GetItem();
                }
            }

            return result;
        }

        /// <summary>
        /// Executes a search to retrieve catalog items.
        /// </summary>
        /// <param name="defaultBucketQuery">The search default bucket query value.</param>
        /// <param name="persistentBucketFilter">The search persistent bucket filter value.</param>
        /// <param name="searchOptions">The search options.</param>
        /// <returns>A list of catalog items.</returns>
        public static Search.SearchResponse SearchCatalogItems(string defaultBucketQuery, string persistentBucketFilter, Search.Models.CommerceSearchOptions searchOptions)
        {
            var searchManager = ContextTypeLoader.CreateInstance<ICatalogSearchManager>();
            var searchIndex = searchManager.GetIndex();

            var defaultQuery = defaultBucketQuery.Replace("&", ";");
            var persistentQuery = persistentBucketFilter.Replace("&", ";");
            var combinedQuery = CombineQueries(persistentQuery, defaultQuery);
            var searchStringModel = SearchStringModel.ParseDatasourceString(combinedQuery);

            using (var context = searchIndex.CreateSearchContext(SearchSecurityOptions.EnableSecurityCheck))
            {
                var query = LinqHelper.CreateQuery<SitecoreUISearchResultItem>(context, searchStringModel)
                    .Where(item => item.Language == SearchNavigation.CurrentLanguageName);

                query = searchManager.AddSearchOptionsToQuery(query, searchOptions.ConnectSearchOptions);

                var results = query.GetResults();
                var response = Search.SearchResponse.CreateFromUISearchResultsItems(searchOptions, results);

                return response;
            }
        }

        /// <summary>
        /// Executes a search in a bucket to retrieve catalog items.
        /// </summary>
        /// <param name="defaultBucketQuery">The search default bucket query value.</param>
        /// <param name="persistentBucketFilter">The search persistent bucket filter value.</param>
        /// <returns>A list of catalog items.</returns>
        /// TODO:remove unused
        public static Search.SearchResponse SearchBucketForCatalogItems(string defaultBucketQuery, string persistentBucketFilter)
        {
            var searchManager = ContextTypeLoader.CreateInstance<ICatalogSearchManager>();
            var searchIndex = searchManager.GetIndex();

            var defaultQuery = defaultBucketQuery.Replace("&", ";");
            var persistentQuery = persistentBucketFilter.Replace("&", ";");
            var combinedQuery = CombineQueries(persistentQuery, defaultQuery);
            var searchStringModel = SearchStringModel.ParseDatasourceString(combinedQuery);

            using (var context = searchIndex.CreateSearchContext(SearchSecurityOptions.EnableSecurityCheck))
            {
                var query = LinqHelper.CreateQuery<SitecoreUISearchResultItem>(context, searchStringModel)
                    .Where(item => item.Language == SearchNavigation.CurrentLanguageName);

                var results = query.GetResults();
                var response = Search.SearchResponse.CreateFromUISearchResultsItems(new Search.Models.CommerceSearchOptions(), results);

                return response;
            }
        }

        /// <summary>
        /// Gets the products with the passed in productid.
        /// </summary>
        /// <param name="productId">The category name.</param>
        /// <param name="searchOptions">The paging options for this query.</param>
        /// <returns>A list of child products.</returns>
        /// TODO:remove unused
        public static Search.SearchResponse GetProductByProductId(string productId, Search.Models.CommerceSearchOptions searchOptions)
        {
            var searchManager = ContextTypeLoader.CreateInstance<ICatalogSearchManager>();
            var searchIndex = searchManager.GetIndex();

            using (var context = searchIndex.CreateSearchContext())
            {
                var searchResults = context.GetQueryable<CommerceProductSearchResultItem>()
                                    .Where(item => item.CommerceSearchItemType == CommerceSearchResultItemType.Product)
                                    .Where(item => item.Language == CurrentLanguageName)
                                    .Where(item => item.Name == productId)
                                    .Select(p => new CommerceProductSearchResultItem()
                                    {
                                        ItemId = p.ItemId,
                                        Uri = p.Uri
                                    });

                searchResults = searchManager.AddSearchOptionsToQuery(searchResults, searchOptions.ConnectSearchOptions);

                var results = searchResults.GetResults();
                var response = Search.SearchResponse.CreateFromSearchResultsItems(searchOptions, results);

                return response;
            }
        }

        /// <summary>
        /// Gets all the products under a specific category.
        /// </summary>
        /// <param name="categoryId">The category name.</param>
        /// <param name="searchOptions">The paging options for this query.</param>
        /// <returns>A list of child products.</returns>
        public static Search.SearchResponse GetCategoryProducts(ID categoryId, Search.Models.CommerceSearchOptions searchOptions)
        {
            var searchManager = ContextTypeLoader.CreateInstance<ICatalogSearchManager>();
            var searchIndex = searchManager.GetIndex();

            using (var context = searchIndex.CreateSearchContext())
            {
                var searchResults = context.GetQueryable<CommerceProductSearchResultItem>()
                                    .Where(item => item.CommerceSearchItemType == CommerceSearchResultItemType.Product)
                                    .Where(item => item.Language == CurrentLanguageName)
                                    .Where(item => item.CommerceAncestorIds.Contains(categoryId))
                                    .Select(p => new CommerceProductSearchResultItem()
                                    {
                                        ItemId = p.ItemId,
                                        Uri = p.Uri
                                    });

                searchResults = searchManager.AddSearchOptionsToQuery(searchResults, searchOptions.ConnectSearchOptions);

                var results = searchResults.GetResults();
                var response = Search.SearchResponse.CreateFromSearchResultsItems(searchOptions, results);

                return response;
            }
        }

        /// <summary>
        /// Searches for catalog items based on keyword.
        /// </summary>
        /// <param name="keyword">The keyword to search for.</param>
        /// <param name="catalogName">The name of the catalog containing the keyword.</param>		
        /// <param name="searchOptions">The paging options for this query.</param>
        /// <returns>A list of child products.</returns>
        public static Search.SearchResponse SearchCatalogItemsByKeyword(string keyword, string catalogName, Search.Models.CommerceSearchOptions searchOptions)
        {
            Sitecore.Diagnostics.Assert.ArgumentNotNullOrEmpty(catalogName, "catalogName");
            var searchManager = ContextTypeLoader.CreateInstance<ICatalogSearchManager>();
            var searchIndex = searchManager.GetIndex(catalogName);

            using (var context = searchIndex.CreateSearchContext())
            {
                var searchResults = context.GetQueryable<CommerceProductSearchResultItem>()
                    .Where(item => item.Name.Equals(keyword) || item["_displayname"].Equals(keyword) || item.Content.Contains(keyword))
                    .Where(item => item.CommerceSearchItemType == CommerceSearchResultItemType.Product || item.CommerceSearchItemType == CommerceSearchResultItemType.Category)
                    .Where(item => item.CatalogName == catalogName)
                    .Where(item => item.Language == CurrentLanguageName)
                    .Select(p => new CommerceProductSearchResultItem()
                    {
                        ItemId = p.ItemId,
                        Uri = p.Uri
                    });

                searchResults = searchManager.AddSearchOptionsToQuery(searchResults, searchOptions.ConnectSearchOptions);

                var results = searchResults.GetResults();
                var response = Search.SearchResponse.CreateFromSearchResultsItems(searchOptions, results);

                return response;
            }
        }

        /// <summary>
        /// Searches for site content items based on keyword.
        /// </summary>
        /// <param name="keyword">The keyword to search for.</param>
        /// <param name="searchOptions">The paging options for this query.</param>
        /// <returns>A list of child products. </returns>
        /// TODO:remove unused
        public static Search.SearchResponse SearchSiteByKeyword(string keyword, Search.Models.CommerceSearchOptions searchOptions)
        {
            const string IndexNameFormat = "sitecore_{0}_index";
            string indexName = string.Format(
                System.Globalization.CultureInfo.InvariantCulture,
                IndexNameFormat,
                Sitecore.Context.Database.Name);

            var searchIndex = ContentSearchManager.GetIndex(indexName);
            using (var context = searchIndex.CreateSearchContext())
            {
                var searchResults = context.GetQueryable<SearchResultItem>();
                searchResults = searchResults.Where(item => item.Path.StartsWith(Sitecore.Context.Site.ContentStartPath));
                searchResults = searchResults.Where(item => item.Language == CurrentLanguageName);
                searchResults = searchResults.Where(GetContentExpression(keyword));
                searchResults = searchResults.Page(searchOptions.StartPageIndex, searchOptions.NumberOfItemsToReturn);

                var results = searchResults.GetResults();
                var response = Search.SearchResponse.CreateFromSearchResultsItems(searchOptions, results);

                return response;
            }
        }

        /// <summary>
        /// Gets all the products under a specific category
        /// </summary>
        /// <param name="categoryId">The parent category id</param>
        /// <param name="searchOptions">The paging options for this query</param>
        /// <returns>A list of child products</returns>
        public static CategorySearchResults GetCategoryChildCategories(ID categoryId, Search.Models.CommerceSearchOptions searchOptions)
        {
            var childCategoryList = new List<Item>();
            var searchManager = ContextTypeLoader.CreateInstance<ICatalogSearchManager>();
            var searchIndex = searchManager.GetIndex();

            using (var context = searchIndex.CreateSearchContext())
            {
                var searchResults = context.GetQueryable<CommerceBaseCatalogSearchResultItem>()
                    .Where(item => item.CommerceSearchItemType == CommerceSearchResultItemType.Category)
                    .Where(item => item.Language == CurrentLanguageName)
                    .Where(item => item.ItemId == categoryId)
                    .Select(p => p);
                var list = searchResults.ToList();

                if (list.Count > 0)
                {
                    if (list[0].Fields.ContainsKey(StorefrontConstants.KnownIndexFields.ChildCategoriesSequence))
                    {
                        var childCategoryDelimitedString = list[0][StorefrontConstants.KnownIndexFields.ChildCategoriesSequence];
                        string[] categoryIdArray = childCategoryDelimitedString.Split('|');

                        foreach (var childCategoryId in categoryIdArray)
                        {
                            var childCategoryItem = Sitecore.Context.Database.GetItem(ID.Parse(childCategoryId));

                            if (childCategoryItem != null)
                            {
                                childCategoryList.Add(childCategoryItem);
                            }
                        }
                    }
                }
            }

            return new CategorySearchResults(childCategoryList, childCategoryList.Count, 1, 1, new List<FacetCategory>());
        }

        /// <summary>
        /// Gets the index of the product stock status from.
        /// </summary>
        /// <param name="viewModelList">The view model list.</param>
        public static void GetProductStockStatusFromIndex(List<ProductViewModel> viewModelList)
        {
            if (viewModelList == null || viewModelList.Count == 0)
            {
                return;
            }

            var searchManager = ContextTypeLoader.CreateInstance<ICatalogSearchManager>();
            var searchIndex = searchManager.GetIndex();

            using (var context = searchIndex.CreateSearchContext())
            {
                var predicate = PredicateBuilder.Create<InventorySearchResultItem>(item => item[StorefrontConstants.KnownIndexFields.InStockLocations].Contains("Default"));
                predicate = predicate.Or(item => item[StorefrontConstants.KnownIndexFields.OutOfStockLocations].Contains("Default"));
                predicate = predicate.Or(item => item[StorefrontConstants.KnownIndexFields.OrderableLocations].Contains("Default"));
                predicate = predicate.Or(item => item[StorefrontConstants.KnownIndexFields.PreOrderable].Contains("0"));

                var searchResults = context.GetQueryable<InventorySearchResultItem>()
                    .Where(item => item.CommerceSearchItemType == CommerceSearchResultItemType.Product)
                    .Where(item => item.Language == SearchNavigation.CurrentLanguageName)
                    .Where(BuildProductIdListPredicate(viewModelList))
                    .Where(predicate)
                    .Select(x => new { x.OutOfStockLocations, x.OrderableLocations, x.PreOrderable, x.InStockLocations, x.Fields, x.Name });

                var results = searchResults.GetResults();

                if (results.TotalSearchResults == 0)
                {
                    return;
                }

                foreach (var result in results)
                {
                    var resultDocument = result.Document;

                    if (resultDocument == null)
                    {
                        continue;
                    }

                    StockStatus status;
                    var isInStock = resultDocument.Fields.ContainsKey(StorefrontConstants.KnownIndexFields.InStockLocations)
                                    && resultDocument.Fields[StorefrontConstants.KnownIndexFields.InStockLocations] != null;
                    if (isInStock)
                    {
                        status = StockStatus.InStock;
                    }
                    else
                    {
                        var isPreOrderable = resultDocument.Fields.ContainsKey(StorefrontConstants.KnownIndexFields.PreOrderable)
                                             && result.Document.PreOrderable != null
                                             && (result.Document.PreOrderable.Equals("1", StringComparison.OrdinalIgnoreCase)
                                                || result.Document.PreOrderable.Equals("true", StringComparison.OrdinalIgnoreCase));
                        if (isPreOrderable)
                        {
                            status = StockStatus.PreOrderable;
                        }
                        else
                        {
                            var isOutOfStock = resultDocument.Fields.ContainsKey(StorefrontConstants.KnownIndexFields.OutOfStockLocations)
                                               && result.Document.OutOfStockLocations != null;
                            var isBackOrderable = resultDocument.Fields.ContainsKey(StorefrontConstants.KnownIndexFields.OrderableLocations)
                                                  && result.Document.OrderableLocations != null;

                            if (isOutOfStock && isBackOrderable)
                            {
                                status = StockStatus.BackOrderable;
                            }
                            else
                            {
                                status = isOutOfStock ? StockStatus.OutOfStock : null;
                            }
                        }
                    }

                    var foundModel = viewModelList.Find(x => x.ProductId == result.Document.Name);

                    if (foundModel != null)
                    {
                        foundModel.StockStatus = status;
                        foundModel.StockStatusName = StorefrontManager.GetProductStockStatusName(foundModel.StockStatus);
                    }
                }
            }
        }

        /// <summary>
        /// Builds the product identifier list predicate.
        /// </summary>
        /// <param name="viewModelList">The view model list.</param>
        /// <returns>The search predicate ORing the product ids.</returns>
        private static Expression<Func<InventorySearchResultItem, bool>> BuildProductIdListPredicate(List<ProductViewModel> viewModelList)
        {
            Expression<Func<InventorySearchResultItem, bool>> predicate = null;

            bool isFirst = true;
            foreach (var viewModel in viewModelList)
            {
                if (isFirst)
                {
                    predicate = PredicateBuilder.Create<InventorySearchResultItem>(p => p.CatalogItemId == viewModel.ProductId.ToLowerInvariant());
                }
                else
                {
                    predicate = predicate.Or(p => p.CatalogItemId == viewModel.ProductId.ToLowerInvariant());
                }

                isFirst = false;
            }

            return predicate;
        }

        /// <summary>
        /// Combines multiple string queries
        /// </summary>
        /// <param name="query1">The first query</param>
        /// <param name="query2">The second query</param>
        /// <returns>Both queries combined</returns>
        private static string CombineQueries(string query1, string query2)
        {
            if (!string.IsNullOrWhiteSpace(query1) && !string.IsNullOrWhiteSpace(query2))
            {
                return string.Concat(query1, ";", query2);
            }

            if (!string.IsNullOrWhiteSpace(query1))
            {
                return query1;
            }

            return query2;
        }

        /// <summary>
        /// Gets the site content search expression that will be used to select results based on a user input search phrase.
        /// </summary>
        /// <param name="searchPhrase">The search phrase entered by the user.</param>
        /// <returns>An expression that represents the search phrase entered by the user.</returns>
        private static Expression<Func<SearchResultItem, bool>> GetContentExpression(string searchPhrase)
        {
            if (!string.IsNullOrWhiteSpace(searchPhrase))
            {
                Expression<Func<SearchResultItem, bool>> predicate = null;
                var termList = searchPhrase.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var term in termList)
                {
                    if (predicate == null)
                    {
                        predicate = PredicateBuilder.Create<SearchResultItem>(item => item.Content.Contains(term));
                    }
                    else
                    {
                        predicate = predicate.And(item => item.Content.Contains(term));
                    }
                }

                return predicate;
            }

            return PredicateBuilder.False<SearchResultItem>();
        }
    }
}
