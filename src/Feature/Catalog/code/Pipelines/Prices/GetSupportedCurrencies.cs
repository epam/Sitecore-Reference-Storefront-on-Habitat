namespace Sitecore.Feature.Catalog.Pipelines.Prices
{
    using System.Collections.Generic;
    using System.Linq;
    using Sitecore.Commerce.Connect.CommerceServer.Catalog;
    using Sitecore.Commerce.Connect.CommerceServer.Catalog.Pipelines;
    using Sitecore.Commerce.Services.Prices;
    using Sitecore.Diagnostics;
    using Sitecore.Foundation.CommerceServer.Utils;

    /// <summary>
    /// Defines the GetSupportedCurrencies class.
    /// </summary>
    public class GetSupportedCurrencies : PricePipelineProcessor
    {
        private string _currencyToInjextString;
        private List<string> _currenciesToInject = new List<string>();

        /// <summary>
        /// Gets or sets the currencies to inject in the list returned. Use primarily as a debuging tool.
        /// </summary>
        /// <value>
        /// The inject currencies.
        /// </value>
        public string InjectCurrencies
        {
            get
            {
                return this._currencyToInjextString;
            }

            set
            {
                value.Split(',').ToList().ForEach(x => this._currenciesToInject.Add(x.Trim()));
                this._currencyToInjextString = value;
            }
        }

        /// <summary>
        /// Processes the specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public override void Process(Commerce.Pipelines.ServicePipelineArgs args)
        {
            Assert.ArgumentNotNull(args, "args");
            Assert.ArgumentNotNull(args.Request, "args.request");
            Assert.ArgumentCondition(args.Request is Foundation.CommerceServer.Requests.GetSupportedCurrenciesRequest, "args.Request", "args.Request is RefSFArgs.GetSupportedCurrenciesRequest");
            Assert.ArgumentCondition(args.Result is GetSupportedCurrenciesResult, "args.Result", "args.Result is GetSupportedCurrenciesResult");

            var request = (Foundation.CommerceServer.Requests.GetSupportedCurrenciesRequest)args.Request;
            var result = (GetSupportedCurrenciesResult)args.Result;

            Assert.ArgumentNotNullOrEmpty(request.CatalogName, "request.CatalogName");

            ICatalogRepository catalogRepository = ContextTypeLoader.CreateInstance<ICatalogRepository>();

            var catalog = catalogRepository.GetCatalogReadOnly(request.CatalogName);

            List<string> currencyList = new List<string>();

            currencyList.Add(catalog["Currency"].ToString());

            if (this._currenciesToInject.Count > 0)
            {
                currencyList.AddRange(this._currenciesToInject);
            }

            result.Currencies = new System.Collections.ObjectModel.ReadOnlyCollection<string>(currencyList);
        }
    }
}