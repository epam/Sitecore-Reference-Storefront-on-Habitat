using System.Collections.Generic;
using Sitecore.Commerce.Entities.Prices;
using Sitecore.Commerce.Services;
using Sitecore.Foundation.CommerceServer.Models;

namespace Sitecore.Foundation.CommerceServer.Interfaces
{
    public interface IPricingManager
    {
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
        ManagerResponse<Sitecore.Commerce.Services.Prices.GetProductPricesResult, IDictionary<string, Price>> GetProductPrices([NotNull] CommerceStorefront storefront, [NotNull] IVisitorContext visitorContext, string catalogName, string productId, bool includeVariants, params string[] priceTypeIds);

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
        ManagerResponse<Sitecore.Commerce.Services.Prices.GetProductBulkPricesResult, IDictionary<string, Price>> GetProductBulkPrices([NotNull] CommerceStorefront storefront, [NotNull] IVisitorContext visitorContext, string catalogName, IEnumerable<string> productIds, params string[] priceTypeIds);

        /// <summary>
        /// Gets the supported currencies.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="catalogName">Name of the catalog.</param>
        /// <returns>The manager response.</returns>
        ManagerResponse<Sitecore.Commerce.Services.Prices.GetSupportedCurrenciesResult, IReadOnlyCollection<string>> GetSupportedCurrencies(CommerceStorefront storefront, string catalogName);

        /// <summary>
        /// Generates the currency chosen page event.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="currency">The currency.</param>
        /// <returns>The manager response.</returns>
        ManagerResponse<ServiceProviderResult, bool> CurrencyChosenPageEvent(CommerceStorefront storefront, string currency);
    }
}