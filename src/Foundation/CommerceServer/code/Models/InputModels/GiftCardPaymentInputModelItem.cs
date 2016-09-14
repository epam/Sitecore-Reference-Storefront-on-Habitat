namespace Sitecore.Foundation.CommerceServer.Models.InputModels
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Defines the GiftCardPaymentInputModelItem class.
    /// </summary>
    public class GiftCardPaymentInputModelItem
    {
        /// <summary>
        /// Gets or sets the payment method identifier.
        /// </summary>
        /// <value>
        /// The payment method identifier.
        /// </value>
        [Required]
        public string PaymentMethodID { get; set; }

        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        /// <value>
        /// The amount.
        /// </value>
        [Required]
        public decimal Amount { get; set; }
    }
}