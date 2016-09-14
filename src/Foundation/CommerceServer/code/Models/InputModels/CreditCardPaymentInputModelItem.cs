namespace Sitecore.Foundation.CommerceServer.Models.InputModels
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Defines the CreditCardPaymentInputModelItem class.
    /// </summary>
    public class CreditCardPaymentInputModelItem
    {
        /// <summary>
        /// Gets or sets the credit card number.
        /// </summary>
        /// <value>
        /// The credit card number.
        /// </value>
        [Required]
        public string CreditCardNumber { get; set; }

        /// <summary>
        /// Gets or sets the payment method identifier.
        /// </summary>
        /// <value>
        /// The payment method identifier.
        /// </value>
        [Required]
        public string PaymentMethodID { get; set; }

        /// <summary>
        /// Gets or sets the validation code.
        /// </summary>
        /// <value>
        /// The validation code.
        /// </value>
        [Required]
        public string ValidationCode { get; set; }

        /// <summary>
        /// Gets or sets the expiration month.
        /// </summary>
        /// <value>
        /// The expiration month.
        /// </value>
        [Required]
        public int ExpirationMonth { get; set; }

        /// <summary>
        /// Gets or sets the expiration year.
        /// </summary>
        /// <value>
        /// The expiration year.
        /// </value>
        [Required]
        public int ExpirationYear { get; set; }

        /// <summary>
        /// Gets or sets the customer name on payment.
        /// </summary>
        /// <value>
        /// The customer name on payment.
        /// </value>
        [Required]
        public string CustomerNameOnPayment { get; set; }

        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        /// <value>
        /// The amount.
        /// </value>
        [Required]
        public decimal Amount { get; set; }

        /// <summary>
        /// Gets or sets the party identifier.
        /// </summary>
        /// <value>
        /// The party identifier.
        /// </value>
        public string PartyID { get; set; }
    }
}