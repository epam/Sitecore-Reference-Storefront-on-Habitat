using Sitecore.Diagnostics;

namespace Sitecore.Foundation.CommerceServer.Requests
{
    /// <summary>
    /// Defines the GetSupportedCurrenciesRequest class.
    /// </summary>
    public class GetSupportedCurrenciesRequest : Sitecore.Commerce.Services.Prices.GetSupportedCurrenciesRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetSupportedCurrenciesRequest"/> class.
        /// </summary>
        /// <param name="shopName">Name of the shop.</param>
        /// <param name="catalogName">Name of the catalog.</param>
        public GetSupportedCurrenciesRequest(string shopName, string catalogName) : base(shopName)
        {
            Assert.ArgumentNotNullOrEmpty(catalogName, "catalogName");

            this.CatalogName = catalogName;
        }

        /// <summary>
        /// Gets or sets the name of the catalog.
        /// </summary>
        /// <value>
        /// The name of the catalog.
        /// </value>
        public string CatalogName { get; set; }
    }
}
