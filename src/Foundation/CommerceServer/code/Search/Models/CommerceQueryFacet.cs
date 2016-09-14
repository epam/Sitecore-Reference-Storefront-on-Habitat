using Sitecore.ContentSearch.Linq;
using System.Collections.Generic;
using ConnectQueryFacet = Sitecore.Commerce.Connect.CommerceServer.Search.Models.CommerceQueryFacet;

namespace Sitecore.Foundation.CommerceServer.Search.Models
{
    /// <summary>
    /// Represents a querying facet. 
    /// </summary>
    public class CommerceQueryFacet
    {
        private readonly ConnectQueryFacet _connectQueryFacet;

        internal CommerceQueryFacet(ConnectQueryFacet connectQueryFacet)
        {
            _connectQueryFacet = connectQueryFacet;
        }

        /// <summary>
        /// Gets or sets the name of the facet
        /// </summary>
        public string Name
        {
            get
            {
                return _connectQueryFacet.Name;
            }
            set
            {
                _connectQueryFacet.Name = value;
            }
        }

        /// <summary>
        /// Gets or sets the display name of the facet.
        /// </summary>
        public string DisplayName
        {
            get
            {
                return _connectQueryFacet.DisplayName;
            }
            set
            {
                _connectQueryFacet.DisplayName = value;
            }
        }

        /// <summary>
        /// Gets or sets any values requried during a query.
        /// </summary>
        public List<object> Values
        {
            get
            {
                return _connectQueryFacet.Values;
            }
            set
            {
                _connectQueryFacet.Values = value;
            }
        }

        /// <summary>
        /// Gets or sets any values found as a result of a query. 
        /// </summary>
        public List<FacetValue> FoundValues
        {
            get
            {
                return _connectQueryFacet.FoundValues;
            }
            set
            {
                _connectQueryFacet.FoundValues = value;
            }
        }

        /// <summary>
        /// Wraps specified instance of <see cref="Sitecore.Commerce.Connect.CommerceServer.Search.Models.CommerceQueryFacet"/> to <see cref="CommerceQueryFacet"/>.
        /// </summary>
        /// <param name="facet">The <see cref="Sitecore.Commerce.Connect.CommerceServer.Search.Models.CommerceQueryFacet"/> to wrap.</param>
        /// <returns>Return new instance of <see cref="CommerceQueryFacet"/>.</returns>
        public static CommerceQueryFacet Wrap(ConnectQueryFacet facet)
        {
            return new CommerceQueryFacet(facet);
        }

        /// <summary>
        /// Unwraps specified instance of <see cref="CommerceQueryFacet"/> to <see cref="Sitecore.Commerce.Connect.CommerceServer.Search.Models.CommerceQueryFacet"/>.
        /// </summary>
        /// <param name="facet">The <see cref="CommerceQueryFacet"/> to unwrap.</param>
        /// <returns>Return new instance of <see cref="Sitecore.Commerce.Connect.CommerceServer.Search.Models.CommerceQueryFacet"/>.</returns>
        internal static ConnectQueryFacet Unwrap(CommerceQueryFacet facet)
        {
            return new ConnectQueryFacet
            {
                Name = facet.Name,
                DisplayName = facet.DisplayName,
                Values = facet.Values,
                FoundValues = facet.FoundValues
            };
        }
    }
}