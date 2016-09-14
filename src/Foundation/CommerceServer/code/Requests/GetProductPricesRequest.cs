using Sitecore.Diagnostics;

namespace Sitecore.Foundation.CommerceServer.Requests
{
    /// <summary>
    /// The get product prices request.
    /// </summary>
    public class GetProductPricesRequest : Sitecore.Commerce.Services.Prices.GetProductPricesRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetProductPricesRequest"/> class.
        /// </summary>
        /// <param name="catalogName">Name of the catalog.</param>
        /// <param name="productId">The product identifier.</param>
        /// <param name="priceTypeIds">The price type ids.</param>
        public GetProductPricesRequest(string catalogName, string productId, params string[] priceTypeIds)
            : base(productId, priceTypeIds)
        {
            Assert.ArgumentNotNull(catalogName, "catalogName");
            this.ProductCatalogName = catalogName;
            this.IncludeVariantPrices = false;
        }

        /// <summary>
        /// Gets or sets the name of the product catalog.
        /// </summary>       
        public string ProductCatalogName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [include variant prices].
        /// </summary>        
        public bool IncludeVariantPrices { get; set; }
    }
}