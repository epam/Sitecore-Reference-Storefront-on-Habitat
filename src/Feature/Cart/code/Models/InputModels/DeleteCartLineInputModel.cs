namespace Sitecore.Feature.Cart.Models.InputModels
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Defines the UpdateCartLineInputModel class.
    /// </summary>
    public class DeleteCartLineInputModel
    {
        /// <summary>
        /// Gets or sets the external cart line identifier.
        /// </summary>
        /// <value>
        /// The external cart line identifier.
        /// </value>
        [Required]
        public string ExternalCartLineId { get; set; }
    }
}