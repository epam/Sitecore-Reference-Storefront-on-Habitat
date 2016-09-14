namespace Sitecore.Foundation.CommerceServer.Search.ComputedFields
{
    /// <summary>
    /// Defines the VariantIndexInfo class.
    /// </summary>
    public class VariantIndexInfo
    {
        /// <summary>
        /// Gets or sets the variant identifier.
        /// </summary>
        /// <value>
        /// The variant identifier.
        /// </value>
        public string VariantId { get; set; }

        /// <summary>
        /// Gets or sets the list price.
        /// </summary>
        /// <value>
        /// The list price.
        /// </value>
        public decimal ListPrice { get; set; }

        /// <summary>
        /// Gets or sets the base price.
        /// </summary>
        /// <value>
        /// The base price.
        /// </value>
        public decimal BasePrice { get; set; }
    }
}
