namespace Sitecore.Foundation.CommerceServer.Models.InputModels
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Defines the SignUpForNotificationInputModel class.
    /// </summary>
    public class SignUpForNotificationInputModel
    {
        /// <summary>
        /// Gets or sets the full name.
        /// </summary>
        /// <value>
        /// The full name.
        /// </value>
        [Required]
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets the product identifier.
        /// </summary>
        /// <value>
        /// The product identifier.
        /// </value>
        [Required]
        public string ProductId { get; set; }

        /// <summary>
        /// Gets or sets the name of the catalog.
        /// </summary>
        /// <value>
        /// The name of the catalog.
        /// </value>
        [Required]
        public string CatalogName { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the variant identifier.
        /// </summary>
        /// <value>
        /// The variant identifier.
        /// </value>
        public string VariantId { get; set; }

        /// <summary>
        /// Gets or sets the interest date.
        /// </summary>
        /// <value>
        /// The interest date.
        /// </value>
        public string InterestDate { get; set; }
    }
}