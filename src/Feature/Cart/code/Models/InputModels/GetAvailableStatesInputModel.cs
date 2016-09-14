namespace Sitecore.Feature.Cart.Models.InputModels
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Defines the GetAvailableStatesInputModel class.
    /// </summary>
    public class GetAvailableStatesInputModel
    {
        /// <summary>
        /// Gets or sets the country code.
        /// </summary>
        /// <value>
        /// The country code.
        /// </value>
        [Required]
        public string CountryCode { get; set; }
    }
}
