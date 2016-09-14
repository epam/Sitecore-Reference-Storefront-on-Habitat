namespace Sitecore.Feature.Cart.Models.InputModels
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Defines the GetNearbyStoresInputModel class.
    /// </summary>
    public class GetNearbyStoresInputModel
    {
        /// <summary>
        /// Gets or sets the longitude.
        /// </summary>
        /// <value>
        /// The longitude.
        /// </value>
        [Required]
        public string Longitude { get; set; }

        /// <summary>
        /// Gets or sets the latitude.
        /// </summary>
        /// <value>
        /// The latitude.
        /// </value>
        [Required]
        public string Latitude { get; set; }
    }
}