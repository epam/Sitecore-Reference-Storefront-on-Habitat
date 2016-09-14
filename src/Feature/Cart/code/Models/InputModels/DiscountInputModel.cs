namespace Sitecore.Feature.Cart.Models.InputModels
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Defines the DiscountInputModel class.
    /// </summary>
    public class DiscountInputModel
    {
        /// <summary>
        /// Gets or sets the promo code.
        /// </summary>
        /// <value>
        /// The promo code.
        /// </value>
        [Required]
        public string PromoCode { get; set; }
    }
}
