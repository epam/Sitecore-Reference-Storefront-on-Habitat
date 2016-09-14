using System.Collections.Generic;
using Sitecore.Diagnostics;

namespace Sitecore.Foundation.CommerceServer.Requests
{
    /// <summary>
    /// The get product bulk prices request.
    /// </summary>
    public class GetProductBulkPricesRequest : Sitecore.Commerce.Services.Prices.GetProductBulkPricesRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetProductBulkPricesRequest"/> class.
        /// </summary>
        /// <param name="catalogName">Name of the catalog.</param>
        /// <param name="productIds">The product ids.</param>
        /// <param name="priceTypeIds">The price type ids.</param>
        public GetProductBulkPricesRequest(string catalogName, IEnumerable<string> productIds, params string[] priceTypeIds)
            : base(productIds)
        {
            Assert.ArgumentNotNull(catalogName, "catalogName");
            this.ProductCatalogName = catalogName;
            this.PriceTypeIds = priceTypeIds;
        }

        /// <summary>
        /// Gets or sets the name of the product catalog.
        /// </summary>       
        public string ProductCatalogName { get; set; }

        /// <summary>
        /// Gets or sets the price type ids.
        /// </summary>        
        public IEnumerable<string> PriceTypeIds { get; protected set; }
    }
}