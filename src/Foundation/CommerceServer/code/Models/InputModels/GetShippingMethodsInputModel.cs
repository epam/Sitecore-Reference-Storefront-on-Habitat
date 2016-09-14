namespace Sitecore.Foundation.CommerceServer.Models.InputModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Defines the GetShippingMethodsInputModel class.
    /// </summary>
    public class GetShippingMethodsInputModel
    {
        /// <summary>
        /// Gets or sets the type of the shipping preference.
        /// </summary>
        /// <value>
        /// The type of the shipping preference.
        /// </value>
        [Required]
        public string ShippingPreferenceType { get; set; }

        /// <summary>
        /// Gets or sets the shipping address.
        /// </summary>
        /// <value>
        /// The shipping address.
        /// </value>
        [Required]
        public PartyInputModelItem ShippingAddress { get; set; }

        /// <summary>
        /// Gets or sets the lines.
        /// </summary>
        /// <value>
        /// The lines.
        /// </value>
        public List<CartLineInputModelItem> Lines { get; set; }
    }
}
