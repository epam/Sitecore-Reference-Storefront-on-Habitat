namespace Sitecore.Feature.Catalog.Models.JsonResults
{
    using Foundation.CommerceServer.Extensions;
    using Foundation.CommerceServer.Managers;
    using Sitecore.Commerce.Connect.CommerceServer.Inventory.Models;
    using Sitecore.Commerce.Entities.Inventory;
    using Sitecore.Commerce.Services;
    using Sitecore.Diagnostics;
    using Sitecore.Feature.Base.Models.JsonResults;

    /// <summary>
    /// The Json result of a request to retrieve product stock information.
    /// </summary>
    public class StockInfoBaseJsonResult : BaseJsonResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StockInfoBaseJsonResult"/> class.
        /// </summary>
        public StockInfoBaseJsonResult()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StockInfoBaseJsonResult"/> class.
        /// </summary>
        /// <param name="result">The service provider result.</param>
        public StockInfoBaseJsonResult(ServiceProviderResult result)
            : base(result)
        {
        }

        /// <summary>
        /// Gets or sets the product identifier.
        /// </summary>
        /// <value>
        /// The product identifier.
        /// </value>
        public string ProductId { get; set; }

        /// <summary>
        /// Gets or sets the variant identifier.
        /// </summary>
        /// <value>
        /// The variant identifier.
        /// </value>
        public string VariantId { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the availability data.
        /// </summary>
        /// <value>
        /// The availability data.
        /// </value>
        public string AvailabilityDate { get; set; }

        /// <summary>
        /// Gets or sets the count.
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        public double Count { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance can show sign up for notification.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance can show sign up for notification; otherwise, <c>false</c>.
        /// </value>
        public bool CanShowSignupForNotification { get; set; }

        /// <summary>
        /// Initializes the specified stock infos.
        /// </summary>
        /// <param name="stockInfo">The stock information.</param>
        public virtual void Initialize(StockInformation stockInfo)
        {
            Assert.ArgumentNotNull(stockInfo, "stockInfo");

            if (stockInfo == null || stockInfo.Status == null)
            {
                return;
            }

            this.ProductId = stockInfo.Product.ProductId;
            this.VariantId = string.IsNullOrEmpty(((CommerceInventoryProduct)stockInfo.Product).VariantId) ? string.Empty : ((CommerceInventoryProduct)stockInfo.Product).VariantId;
            this.Status = StorefrontManager.GetProductStockStatusName(stockInfo.Status);
            this.Count = stockInfo.Count < 0 ? 0 : stockInfo.Count;
            this.CanShowSignupForNotification = Sitecore.Context.User.IsAuthenticated;
            if (stockInfo.AvailabilityDate != null & stockInfo.AvailabilityDate.HasValue)
            {
                this.AvailabilityDate = stockInfo.AvailabilityDate.Value.ToDisplayedDate();
            }
        }
    }
}