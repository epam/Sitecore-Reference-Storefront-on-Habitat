using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Foundation.CommerceServer.Models;
using Sitecore.Foundation.CommerceServer.Search.Models;

namespace Sitecore.Feature.Search.Models
{
    public class SearchInfo
    {
        public string SearchKeyword { get; set; }

        public IEnumerable<CommerceQueryFacet> RequiredFacets { get; set; }

        public IEnumerable<CommerceQuerySort> SortFields { get; set; }

        public int ItemsPerPage { get; set; }

        public Foundation.CommerceServer.Models.Catalog Catalog { get; set; }

        public CommerceSearchOptions SearchOptions { get; set; }
    }
}
