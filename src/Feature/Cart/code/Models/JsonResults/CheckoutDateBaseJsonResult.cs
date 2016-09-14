namespace Sitecore.Feature.Cart.Models.JsonResults
{
    using Sitecore.Commerce.Entities.Payments;
    using Sitecore.Commerce.Entities.Shipping;
    using Sitecore.Commerce.Services;
    using Sitecore.Feature.Base.Models.JsonResults;
    using System.Collections.Generic;

    /// <summary>
    /// The Json result of a request to retrieve the available states..
    /// </summary>
    public class CheckoutDataBaseJsonResult : BaseJsonResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CheckoutDataBaseJsonResult"/> class.
        /// </summary>
        public CheckoutDataBaseJsonResult()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckoutDataBaseJsonResult"/> class.
        /// </summary>
        /// <param name="result">The service provider result.</param>
        public CheckoutDataBaseJsonResult(ServiceProviderResult result)
            : base(result)
        {
        }

        /// <summary>
        /// Gets or sets the order shipping options.
        /// </summary>
        public IEnumerable<ShippingOption> OrderShippingOptions { get; set; }

        /// <summary>
        /// Gets or sets the line item shipping options.
        /// </summary>
        public IEnumerable<LineShippingOption> LineShippingOptions { get; set; }

        /// <summary>
        /// Gets or sets the ID of the 'email' delivery method.
        /// </summary>
        /// <value>
        /// The email delivery method.
        /// </value>
        public ShippingMethod EmailDeliveryMethod { get; set; }

        /// <summary>
        /// Gets or sets the ID of the 'ship to store' delivery method.
        /// </summary>
        /// <value>
        /// The ship to store delivery method.
        /// </value>
        public ShippingMethod ShipToStoreDeliveryMethod { get; set; }

        /// <summary>
        /// Gets or sets the countries that items can be shipped to.
        /// </summary>
        public IDictionary<string, string> Countries { get; set; }

        /// <summary>
        /// Gets or sets the available payment options.
        /// </summary>
        public IEnumerable<PaymentOption> PaymentOptions { get; set; }

        /// <summary>
        /// Gets or sets the available payment methods.
        /// </summary>
        public IEnumerable<PaymentMethod> PaymentMethods { get; set; }

        /// <summary>
        /// Gets or sets the available shipping methods.
        /// </summary>
        public IEnumerable<ShippingMethod> ShippingMethods { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user is authenticated.
        /// </summary>
        public bool IsUserAuthenticated { get; set; }

        /// <summary>
        /// Gets or sets the addresses saved in the user's profile.
        /// </summary>
        public AddressListItemBaseJsonResult UserAddresses { get; set; }

        /// <summary>
        /// Gets or sets the user's email address.
        /// </summary>
        public string UserEmail { get; set; }

        /// <summary>
        /// Gets or sets the ID of the loyalty card associated with the cart.
        /// </summary>
        public string CartLoyaltyCardNumber { get; set; }

        /// <summary>
        /// Gets or sets the currency code.
        /// </summary>
        /// <value>The currency code.</value>
        public string CurrencyCode { get; set; }

        /// <summary>
        /// Gets or sets the cart.
        /// </summary>
        /// <value>The cart.</value>
        public CartBaseJsonResult Cart { get; set; }
    }
}