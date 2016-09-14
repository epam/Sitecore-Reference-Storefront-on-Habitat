namespace Sitecore.Foundation.CommerceServer.Models.InputModels
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Defines the UpdateCartLineInputModel class.
    /// </summary>
    public class UpdateCartLineInputModel
    {
        /// <summary>
        /// Gets or sets the external cart line identifier.
        /// </summary>
        /// <value>
        /// The external cart line identifier.
        /// </value>
        [Required]
        public string ExternalCartLineId { get; set; }

        /// <summary>
        /// Gets or sets the quantity.
        /// </summary>
        /// <value>
        /// The quantity.
        /// </value>
        [Required]
        public uint Quantity { get; set; }
    }
}