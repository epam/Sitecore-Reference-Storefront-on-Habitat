namespace Sitecore.Foundation.CommerceServer.Models.InputModels
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// View model for a Basket LineItem.
    /// </summary>
    public class AddCartLineInputModel
    {
        /// <summary>
        /// Gets or sets the Product Id of the current LineItem.
        /// </summary>
        [Required]
        public string ProductId { get; set; }

        /// <summary>
        /// Gets or sets the VariantId of the current LineItem.
        /// </summary>
        public string VariantId { get; set; }

        /// <summary>
        /// Gets or sets the CatalogName of the current LineItem.
        /// </summary>
        [Required]
        public string CatalogName { get; set; }

        /// <summary>
        /// Gets or sets the Quantity of the current LineItem.
        /// </summary>
        [Required]
        public decimal? Quantity { get; set; }

        /// <summary>
        /// Gets or sets the image URL.
        /// </summary>       
        public string ImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the product URL.
        /// </summary>       
        public string ProductUrl { get; set; }

        /// <summary>
        /// Gets or sets the gift card amount.
        /// </summary>       
        public decimal? GiftCardAmount { get; set; }
    }
}
