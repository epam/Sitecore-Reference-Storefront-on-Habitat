namespace Sitecore.Foundation.CommerceServer.Managers
{
    using System.Collections.Generic;
    using Models;
    using Sitecore.Commerce.Entities.Prices;
    using Sitecore.Commerce.Services;
    using Sitecore.Commerce.Services.Prices;
    using Sitecore.Diagnostics;
    using Sitecore.Foundation.CommerceServer.Interfaces;

    /// <summary>
    /// Defines the PricingManager class.
    /// </summary>
    public class PricingManager : IPricingManager
    {
        private static readonly string[] DefaultPriceTypeIds = { PriceTypes.List, PriceTypes.Adjusted, PriceTypes.LowestPricedVariant, PriceTypes.LowestPricedVariantListPrice, PriceTypes.HighestPricedVariant };

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PricingManager"/> class.
        /// </summary>
        /// <param name="pricingServiceProvider">The pricing service provider.</param>
        public PricingManager([NotNull] PricingServiceProvider pricingServiceProvider)
        {
            Assert.ArgumentNotNull(pricingServiceProvider, "pricingServiceProvider");

            this._pricingServiceProvider = pricingServiceProvider;
        }

        #endregion

        #region Members

        /// <summary>
        /// Gets or sets the pricing service provider.
        /// </summary>
        /// <value>
        /// The pricing service provider.
        /// </value>
        private readonly PricingServiceProvider _pricingServiceProvider;

        #endregion

        #region Methods (public, virtual)

        /// <summary>
        /// Gets the product prices.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="visitorContext">The visitor context.</param>
        /// <param name="catalogName">Name of the catalog.</param>
        /// <param name="productId">The product identifier.</param>
        /// <param name="includeVariants">if set to <c>true</c> [include variants].</param>
        /// <param name="priceTypeIds">The price type ids.</param>
        /// <returns>
        /// The manager response with the list of prices in the Result.
        /// </returns>
        public virtual ManagerResponse<Sitecore.Commerce.Services.Prices.GetProductPricesResult, IDictionary<string, Price>> GetProductPrices([NotNull] CommerceStorefront storefront, [NotNull] IVisitorContext visitorContext, string catalogName, string productId, bool includeVariants, params string[] priceTypeIds)
        {
            Assert.ArgumentNotNull(storefront, "storefront");

            if (priceTypeIds == null)
            {
                priceTypeIds = DefaultPriceTypeIds;
            }

            var request = new Requests.GetProductPricesRequest(catalogName, productId, priceTypeIds);

            if (Sitecore.Context.User.IsAuthenticated)
            {
                request.UserId = visitorContext.GetCustomerId();
            }

            request.IncludeVariantPrices = includeVariants;
            request.CurrencyCode = StorefrontManager.GetCustomerCurrency();
            var result = this._pricingServiceProvider.GetProductPrices(request);

            // Helpers.LogSystemMessages(result.SystemMessages, result);
            return new ManagerResponse<Sitecore.Commerce.Services.Prices.GetProductPricesResult, IDictionary<string, Price>>(result, result.Prices == null ? new Dictionary<string, Price>() : result.Prices);
        }

        /// <summary>
        /// Gets the product bulk prices.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="visitorContext">The visitor context.</param>
        /// <param name="catalogName">Name of the catalog.</param>
        /// <param name="productIds">The product ids.</param>
        /// <param name="priceTypeIds">The price type ids.</param>
        /// <returns>
        /// The manager response with the list of prices in the Result.
        /// </returns>
        public virtual ManagerResponse<Sitecore.Commerce.Services.Prices.GetProductBulkPricesResult, IDictionary<string, Price>> GetProductBulkPrices([NotNull] CommerceStorefront storefront, [NotNull] IVisitorContext visitorContext, string catalogName, IEnumerable<string> productIds, params string[] priceTypeIds)
        {
            Assert.ArgumentNotNull(storefront, "storefront");

            if (priceTypeIds == null)
            {
                priceTypeIds = DefaultPriceTypeIds;
            }

            var request = new Requests.GetProductBulkPricesRequest(catalogName, productIds, priceTypeIds);
            request.CurrencyCode = StorefrontManager.GetCustomerCurrency();

            var result = this._pricingServiceProvider.GetProductBulkPrices(request);

            // Currently, both Categories and Products are passed in and are waiting for a fix to filter the categories out.  Until then, this code is commented
            // out as it generates an unecessary Error event indicating the product cannot be found.
            // Helpers.LogSystemMessages(result.SystemMessages, result);
            return new ManagerResponse<Sitecore.Commerce.Services.Prices.GetProductBulkPricesResult, IDictionary<string, Price>>(result, result.Prices == null ? new Dictionary<string, Price>() : result.Prices);
        }

        /// <summary>
        /// Gets the supported currencies.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="catalogName">Name of the catalog.</param>
        /// <returns>The manager response.</returns>
        public virtual ManagerResponse<Sitecore.Commerce.Services.Prices.GetSupportedCurrenciesResult, IReadOnlyCollection<string>> GetSupportedCurrencies(CommerceStorefront storefront, string catalogName)
        {
            Assert.ArgumentNotNull(storefront, "storefront");

            var request = new Requests.GetSupportedCurrenciesRequest(storefront.ShopName, catalogName);
            var result = _pricingServiceProvider.GetSupportedCurrencies(request);

            return new ManagerResponse<GetSupportedCurrenciesResult, IReadOnlyCollection<string>>(result, result.Currencies);
        }

        /// <summary>
        /// Generates the currency chosen page event.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="currency">The currency.</param>
        /// <returns>The manager response.</returns>
        public virtual ManagerResponse<ServiceProviderResult, bool> CurrencyChosenPageEvent(CommerceStorefront storefront, string currency)
        {
            Assert.ArgumentNotNull(storefront, "storefront");
            Assert.ArgumentNotNullOrEmpty(currency, "currency");

            var request = new CurrencyChosenRequest(storefront.ShopName, currency);
            var result = this._pricingServiceProvider.CurrencyChosen(request);

            return new ManagerResponse<ServiceProviderResult, bool>(result, result.Success);
        }

        #endregion
    }
}