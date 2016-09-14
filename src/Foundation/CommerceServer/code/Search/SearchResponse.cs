using Sitecore.ContentSearch.Linq;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data.Items;
using Sitecore.Foundation.CommerceServer.Search.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using ConnectFacetValue = Sitecore.ContentSearch.Linq.FacetValue;
using ConnectQueryFacet = Sitecore.Commerce.Connect.CommerceServer.Search.Models.CommerceQueryFacet;
using ConnectSearchResponse = Sitecore.Commerce.Connect.CommerceServer.Search.SearchResponse;
//using FacetValue = Sitecore.Foundation.CommerceServer.Search.Models.FacetValue;

namespace Sitecore.Foundation.CommerceServer.Search
{
    /// <summary>
    /// Represents a search response.
    /// Wraps <see cref="Sitecore.Commerce.Connect.CommerceServer.Search.SearchResponse"/>.
    /// </summary>
    public class SearchResponse
    {
        private readonly ConnectSearchResponse _searchResponse;

        /// <summary>
        /// Creates new instance of <see cref="SearchResponse"/>.
        /// </summary>
        public SearchResponse()
        {
            _searchResponse = new ConnectSearchResponse();
        }

        /// <summary>
        /// Creates new instance of <see cref="SearchResponse"/>.
        /// </summary>
        internal SearchResponse(ConnectSearchResponse searchResponse)
        {
            _searchResponse = searchResponse;
        }

        /// <summary>
        /// Gets the search result items.
        /// </summary>
        public List<Item> ResponseItems
        {
            get
            {
                return _searchResponse.ResponseItems;
            }
        }

        /// <summary>
        /// Gets the paging options.
        /// </summary>
        public CommerceSearchOptions SearchOptions
        {
            get
            {
                return new CommerceSearchOptions(_searchResponse.SearchOptions);
            }
        }

        /// <summary>
        /// Gets the total item count. 
        /// </summary>
        public int TotalItemCount
        {
            get
            {
                return _searchResponse.TotalItemCount;
            }
        }

        /// <summary>
        /// Gets the total page count. 
        /// </summary>
        public int TotalPageCount
        {
            get
            {
                return _searchResponse.TotalPageCount;
            }
        }

        /// <summary>
        /// Gets the list of facets. 
        /// </summary>
        public IEnumerable<CommerceQueryFacet> Facets
        {
            get; private set;
            //get
            //{
            //    return _searchResponse.Facets.Select(CommerceQueryFacet.Wrap);
            //}
            //private set
            //{
            //    _searchResponse.Facets = value == null
            //        ? Enumerable.Empty<CommerceQueryFacet>()
            //        : value.Select(CommerceQueryFacet.Unwrap);
            //}
        }

        public static SearchResponse CreateFromSearchResultsItems<T>(
            CommerceSearchOptions searchOptions,
            SearchResults<T> sitecoreSearchResults)
            where T : SearchResultItem
        {
            return Create(searchOptions, sitecoreSearchResults, i => i.GetItem());
        }

        public static SearchResponse CreateFromUISearchResultsItems(
                CommerceSearchOptions searchOptions,
                SearchResults<SitecoreUISearchResultItem> sitecoreSearchResults)
        {
            return Create(searchOptions, sitecoreSearchResults, i => i.GetItem());
        }

        public static SearchResponse Create<T>(
            CommerceSearchOptions searchOptions,
            SearchResults<T> searchResults,
            Func<T, Item> getItemDelegate)
        {
            var connectResponse = ConnectSearchResponse.Create(searchOptions.ConnectSearchOptions, searchResults, getItemDelegate);
            var responseFacets = connectResponse.Facets.Select(CommerceQueryFacet.Wrap).ToList();

            if (searchResults.Facets != null)
            {
                var searchFacets = new List<ConnectQueryFacet>();

                foreach (var foundCategory in searchResults.Facets.Categories)
                {
                    foreach (var responseFacet in responseFacets)
                    {
                        if (responseFacet.Name.Equals(foundCategory.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            responseFacet.FoundValues = foundCategory.Values;
                        }
                    }

                    foreach (var searchFacet in searchOptions.ConnectSearchOptions.FacetFields)
                    {
                        if (searchFacet.Name.Equals(foundCategory.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            searchFacet.FoundValues = new List<ConnectFacetValue>(foundCategory.Values);
                        }

                        searchFacets.Add(searchFacet);
                    }
                }

                searchOptions.ConnectSearchOptions.FacetFields = searchFacets;
            }

            var response = new SearchResponse(connectResponse) { Facets = responseFacets };

            return response;
        }
    }
}