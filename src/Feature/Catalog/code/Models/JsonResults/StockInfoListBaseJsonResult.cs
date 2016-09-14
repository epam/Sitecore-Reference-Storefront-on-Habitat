namespace Sitecore.Feature.Catalog.Models.JsonResults
{
    using Sitecore.Commerce.Entities.Inventory;
    using Sitecore.Commerce.Services;
    using Sitecore.Diagnostics;
    using Sitecore.Feature.Base.Models.JsonResults;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The Json result of a request to retrieve product stock information.
    /// </summary>
    public class StockInfoListBaseJsonResult : BaseJsonResult
    {
        private readonly List<StockInfoBaseJsonResult> _stockInformations = new List<StockInfoBaseJsonResult>();

        /// <summary>
        /// Initializes a new instance of the <see cref="StockInfoListBaseJsonResult"/> class.
        /// </summary>
        public StockInfoListBaseJsonResult()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StockInfoListBaseJsonResult"/> class.
        /// </summary>
        /// <param name="result">The service provider result.</param>
        public StockInfoListBaseJsonResult(ServiceProviderResult result)
            : base(result)
        {
        }

        /// <summary>
        /// Gets the stock informations.
        /// </summary>
        /// <value>
        /// The stock informations.
        /// </value>
        public List<StockInfoBaseJsonResult> StockInformations
        {
            get { return this._stockInformations; }
        }

        /// <summary>
        /// Initializes the specified stock infos.
        /// </summary>
        /// <param name="stockInformations">The stock informations.</param>
        public virtual void Initialize(IEnumerable<StockInformation> stockInformations)
        {
            Assert.ArgumentNotNull(stockInformations, "stockInformations");

            var stockInfos = stockInformations as IList<StockInformation> ?? stockInformations.ToList();
            if (!stockInfos.Any())
            {
                return;
            }

            foreach (var info in stockInfos)
            {
                var stockInfo = new StockInfoBaseJsonResult();
                stockInfo.Initialize(info);
                this._stockInformations.Add(stockInfo);
            }
        }
    }
}