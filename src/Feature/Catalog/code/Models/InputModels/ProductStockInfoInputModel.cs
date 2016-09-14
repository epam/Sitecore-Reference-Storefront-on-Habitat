namespace Sitecore.Feature.Catalog.Models.InputModels
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Defines the ProductStockInfoInputModel class.
    /// </summary>
    public class ProductStockInfoInputModel
    {
        /// <summary>
        /// Gets or sets the product identifier.
        /// </summary>
        /// <value>
        /// The product identifier.
        /// </value>
        [Required]
        public string ProductId { get; set; }
    }
}