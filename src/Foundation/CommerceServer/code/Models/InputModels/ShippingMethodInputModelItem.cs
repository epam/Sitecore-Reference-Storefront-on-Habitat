namespace Sitecore.Foundation.CommerceServer.Models.InputModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Defines gthe ShippingMethodInputModelItem class.
    /// </summary>
    public class ShippingMethodInputModelItem
    {
        /// <summary>
        /// Gets or sets the shipping method identifier.
        /// </summary>
        /// <value>
        /// The shipping method identifier.
        /// </value>
        [Required]
        public string ShippingMethodID { get; set; }

        /// <summary>
        /// Gets or sets the name of the shipping method.
        /// </summary>
        /// <value>
        /// The name of the shipping method.
        /// </value>
        [Required]
        public string ShippingMethodName { get; set; }

        /// <summary>
        /// Gets or sets the type of the shipping preference.
        /// </summary>
        /// <value>
        /// The type of the shipping preference.
        /// </value>
        [Required]
        public string ShippingPreferenceType { get; set; }

        /// <summary>
        /// Gets or sets the party identifier.
        /// </summary>
        /// <value>
        /// The party identifier.
        /// </value>
        public string PartyID { get; set; }

        /// <summary>
        /// Gets or sets the electronic delivery email.
        /// </summary>
        /// <value>
        /// The electronic delivery email.
        /// </value>
        [EmailAddress]
        public string ElectronicDeliveryEmail { get; set; }

        /// <summary>
        /// Gets or sets the content of the electronic delivery email.
        /// </summary>
        /// <value>
        /// The content of the electronic delivery email.
        /// </value>
        public string ElectronicDeliveryEmailContent { get; set; }

        /// <summary>
        /// Gets or sets the line ids.
        /// </summary>
        /// <value>
        /// The line ids.
        /// </value>
        public List<string> LineIDs { get; set; }
    }
}