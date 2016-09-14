namespace Sitecore.Foundation.CommerceServer.Models.InputModels
{
    /// <summary>
    /// Defines the CartLineInputModelItem class.
    /// </summary>
    public class CartLineInputModelItem
    {
        /// <summary>
        /// Gets or sets the external cart line identifier.
        /// </summary>
        /// <value>
        /// The external cart line identifier.
        /// </value>
        public string ExternalCartLineId { get; set; }

        /// <summary>
        /// Gets or sets the type of the shipping preference.
        /// </summary>
        /// <value>
        /// The type of the shipping preference.
        /// </value>
        public string ShippingPreferenceType { get; set; }
    }
}