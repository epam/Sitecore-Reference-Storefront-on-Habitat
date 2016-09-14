namespace Sitecore.Foundation.CommerceServer.Models.InputModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Defines the SetShippingMethodsInputModel class.
    /// </summary>
    public class SetShippingMethodsInputModel
    {
        /// <summary>
        /// Gets or sets the type of the order shipping preference.
        /// </summary>
        /// <value>
        /// The type of the order shipping preference.
        /// </value>
        [Required]
        public string OrderShippingPreferenceType { get; set; }

        /// <summary>
        /// Gets or sets the shipping methods.
        /// </summary>
        /// <value>
        /// The shipping methods.
        /// </value>
        [Required]
        public List<ShippingMethodInputModelItem> ShippingMethods { get; set; }

        /// <summary>
        /// Gets or sets the shipping addresses.
        /// </summary>
        /// <value>
        /// The shipping addresses.
        /// </value>
        public List<PartyInputModelItem> ShippingAddresses { get; set; }
    }
}