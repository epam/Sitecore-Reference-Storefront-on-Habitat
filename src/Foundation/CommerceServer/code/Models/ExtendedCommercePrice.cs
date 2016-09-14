namespace Sitecore.Foundation.CommerceServer.Models
{
    using Sitecore.Commerce.Connect.CommerceServer.Orders.Models;    

    /// <summary>
    /// Defines the ExtendedCommercePrice class.
    /// </summary>
    public class ExtendedCommercePrice : CommercePrice
    {
        /// <summary>
        /// Gets or sets the lowest priced variant.
        /// </summary>
        /// <value>
        /// The lowest priced variant.
        /// </value>
        public decimal? LowestPricedVariant { get; set; }

        /// <summary>
        /// Gets or sets the lowest priced variant list price.
        /// </summary>
        /// <value>
        /// The lowest priced variant list price.
        /// </value>
        public decimal? LowestPricedVariantListPrice { get; set; }

        /// <summary>
        /// Gets or sets the highest priced variant.
        /// </summary>
        /// <value>
        /// The highest priced variant.
        /// </value>
        public decimal? HighestPricedVariant { get; set; }
    }
}
