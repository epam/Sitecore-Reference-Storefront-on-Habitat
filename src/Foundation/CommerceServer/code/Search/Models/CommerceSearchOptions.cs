using Sitecore.Foundation.CommerceServer.Infrastructure.Constants;
using System.Collections.Generic;
using System.Linq;
using ConnectSearchOptions = Sitecore.Commerce.Connect.CommerceServer.Search.Models.CommerceSearchOptions;
using ConnectQueryFacet = Sitecore.Commerce.Connect.CommerceServer.Search.Models.CommerceQueryFacet;

namespace Sitecore.Foundation.CommerceServer.Search.Models
{
    /// <summary>
    /// Class representing the different paging options for search queries.
    /// Wraps <see cref="Sitecore.Commerce.Connect.CommerceServer.Search.Models.CommerceQueryFacet"/>.
    /// </summary>
    public class CommerceSearchOptions
    {
        private readonly ConnectSearchOptions _connectSearchOptions;

        public int NumberOfItemsToReturn
        {
            get
            {
                return _connectSearchOptions.NumberOfItemsToReturn;
            }
            set
            {
                _connectSearchOptions.NumberOfItemsToReturn = value;
            }
        }

        public int StartPageIndex
        {
            get
            {
                return _connectSearchOptions.StartPageIndex;
            }
            set
            {
                _connectSearchOptions.StartPageIndex = value;
            }
        }

        public string SortField
        {
            get
            {
                return _connectSearchOptions.SortField;
            }
            set
            {
                _connectSearchOptions.SortField = value;
            }
        }

        public bool HasFacets
        {
            get
            {
                return _connectSearchOptions.HasFacets;
            }
        }

        public CommerceConstants.SortDirection SortDirection
        {
            get
            {
                return (CommerceConstants.SortDirection)_connectSearchOptions.SortDirection;
            }
            set
            {
                _connectSearchOptions.SortDirection = (Commerce.Connect.CommerceServer.CommerceConstants.SortDirection)value;
            }
        }

        public IEnumerable<CommerceQueryFacet> FacetFields
        {
            get
            {
                return _connectSearchOptions.FacetFields.Select(CommerceQueryFacet.Wrap);
            }
            set
            {
                _connectSearchOptions.FacetFields =
                    value?.Select(CommerceQueryFacet.Unwrap) ?? Enumerable.Empty<ConnectQueryFacet>();
            }
        }

        internal ConnectSearchOptions ConnectSearchOptions
        {
            get
            {
                return _connectSearchOptions;
            }
        }

        public CommerceSearchOptions()
            : this(-1, -1, Enumerable.Empty<CommerceQueryFacet>())
        {
        }

        public CommerceSearchOptions(int numberOfItemsToReturn, int startPageIndex)
            : this(numberOfItemsToReturn, startPageIndex, Enumerable.Empty<CommerceQueryFacet>())
        {
        }

        public CommerceSearchOptions(int numberOfItemsToReturn, int startPageIndex, IEnumerable<CommerceQueryFacet> facetFields)
            : this(new ConnectSearchOptions(numberOfItemsToReturn, startPageIndex, facetFields.Select(CommerceQueryFacet.Unwrap)))
        {
        }

        internal CommerceSearchOptions(ConnectSearchOptions connectSearchOptions)
        {
            _connectSearchOptions = connectSearchOptions;
        }
    }
}