namespace Sitecore.Feature.Catalog.Models
{
    using Foundation.CommerceServer.Managers;
    using Foundation.CommerceServer.Models;
    using Sitecore.Commerce.Services.Prices;
    using Sitecore.Mvc.Presentation;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Defines the CurrencyMenuViewModel class.
    /// </summary>
    public class CurrencyMenuViewModel : RenderingModel
    {
        private List<CurrencyInformationModel> _currencyList = new List<CurrencyInformationModel>();

        /// <summary>
        /// Gets the currency list.
        /// </summary>
        /// <value>
        /// The currency list.
        /// </value>
        public List<CurrencyInformationModel> CurrencyList
        {
            get
            {
                return _currencyList;
            }
        }

        /// <summary>
        /// Initializes the specified rendering.
        /// </summary>
        /// <param name="rendering">The rendering.</param>
        /// <param name="result">The result.</param>
        public void Initialize(Rendering rendering, GetSupportedCurrenciesResult result)
        {
            if (!result.Success || result.Currencies == null)
            {
                return;
            }

            List<string> supportedCurrencies = StorefrontManager.CurrentStorefront.SupportedCurrencies;

            var currencies = supportedCurrencies.Intersect(result.Currencies);

            foreach (string currency in currencies)
            {
                var currencyInfoModel = StorefrontManager.GetCurrencyInformation(currency);
                if (currencyInfoModel != null)
                {
                    this._currencyList.Add(currencyInfoModel);
                }
            }
        }
    }
}