namespace Sitecore.Feature.Catalog.Models.JsonResults
{
    using Foundation.CommerceServer.Extensions;
    using Foundation.CommerceServer.Managers;
    using Sitecore.Commerce.Entities.GiftCards;
    using Sitecore.Commerce.Services;
    using Sitecore.Diagnostics;
    using Sitecore.Feature.Base.Models.JsonResults;

    /// <summary>
    /// The Json result of a request to retrieve gift card information.
    /// </summary>
    public class GiftCardBaseJsonResult : BaseJsonResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GiftCardBaseJsonResult"/> class.
        /// </summary>
        public GiftCardBaseJsonResult()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GiftCardBaseJsonResult"/> class.
        /// </summary>
        /// <param name="result">The service provider result.</param>
        public GiftCardBaseJsonResult(ServiceProviderResult result)
            : base(result)
        {
        }

        /// <summary>
        /// Gets or sets the external identifier.
        /// </summary>
        /// <value>
        /// The external identifier.
        /// </value>
        public string ExternalId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the customer identifier.
        /// </summary>
        /// <value>
        /// The customer identifier.
        /// </value>
        public string CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the name of the shop.
        /// </summary>
        /// <value>
        /// The name of the shop.
        /// </value>
        public string ShopName { get; set; }

        /// <summary>
        /// Gets or sets the currency code.
        /// </summary>
        /// <value>
        /// The currency code.
        /// </value>
        public string CurrencyCode { get; set; }

        /// <summary>
        /// Gets or sets the balance.
        /// </summary>
        /// <value>
        /// The balance.
        /// </value>
        public decimal Balance { get; set; }

        /// <summary>
        /// Gets or sets the formatted balance.
        /// </summary>
        /// <value>
        /// The formatted balance.
        /// </value>
        public string FormattedBalance { get; set; }

        /// <summary>
        /// Gets or sets the original amount.
        /// </summary>
        /// <value>
        /// The original amount.
        /// </value>
        public string OriginalAmount { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Initializes the specified gift card.
        /// </summary>
        /// <param name="giftCard">The gift card.</param>
        public virtual void Initialize(GiftCard giftCard)
        {
            Assert.ArgumentNotNull(giftCard, "giftCard");

            var currencyCode = StorefrontManager.GetCustomerCurrency();

            this.ExternalId = giftCard.ExternalId;
            this.Name = giftCard.Name;
            this.CustomerId = giftCard.CustomerId;
            this.ShopName = giftCard.ShopName;
            this.CurrencyCode = giftCard.CurrencyCode;
            this.Balance = giftCard.Balance;
            this.FormattedBalance = giftCard.Balance.ToCurrency(currencyCode);
            this.OriginalAmount = giftCard.OriginalAmount.ToCurrency(currencyCode);
            this.Description = giftCard.Description;
        }
    }
}