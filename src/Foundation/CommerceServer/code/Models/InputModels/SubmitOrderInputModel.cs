namespace Sitecore.Foundation.CommerceServer.Models.InputModels
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Defines the SubmitOrderInputModel class.
    /// </summary>
    public class SubmitOrderInputModel
    {
        /// <summary>
        /// Gets or sets the user email.
        /// </summary>
        /// <value>
        /// The user email.
        /// </value>
        [Required]
        [EmailAddress]
        public string UserEmail { get; set; }

        /// <summary>
        /// Gets or sets the credit card payment.
        /// </summary>
        /// <value>
        /// The credit card payment.
        /// </value>
        public CreditCardPaymentInputModelItem CreditCardPayment { get; set; }

        /// <summary>
        /// Gets or sets the gift card payment.
        /// </summary>
        /// <value>
        /// The gift card payment.
        /// </value>
        public GiftCardPaymentInputModelItem GiftCardPayment { get; set; }

        /// <summary>
        /// Gets or sets the loyalty card payment.
        /// </summary>
        /// <value>
        /// The loyalty card payment.
        /// </value>
        public LoyaltyCardPaymentInputModelItem LoyaltyCardPayment { get; set; }

        /// <summary>
        /// Gets or sets the billing address.
        /// </summary>
        /// <value>
        /// The billing address.
        /// </value>
        public PartyInputModelItem BillingAddress { get; set; }
    }
}
