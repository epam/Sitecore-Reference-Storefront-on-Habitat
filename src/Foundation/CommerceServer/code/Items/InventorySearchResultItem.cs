namespace Sitecore.Foundation.CommerceServer.Items
{
    using Sitecore.Commerce.Connect.CommerceServer.Search.Models;
    using Sitecore.ContentSearch;

    /// <summary>
    /// Defines the InventorySearchResultItem class.
    /// </summary>
    public class InventorySearchResultItem : CommerceProductSearchResultItem
    {
        /// <summary>
        /// Gets or sets the out of stock locations.
        /// </summary>
        /// <value>
        /// The out of stock locations.
        /// </value>
        [IndexField("outofstocklocations")]
        public string OutOfStockLocations { get; set; }

        /// <summary>
        /// Gets or sets the orderable locations.
        /// </summary>
        /// <value>
        /// The orderable locations.
        /// </value>
        [IndexField("orderablelocations")]
        public string OrderableLocations { get; set; }

        /// <summary>
        /// Gets or sets the pre orderable.
        /// </summary>
        /// <value>
        /// The pre orderable.
        /// </value>
        [IndexField("preorderable")]
        public string PreOrderable { get; set; }

        /// <summary>
        /// Gets or sets the in stock locations.
        /// </summary>
        /// <value>
        /// The in stock locations.
        /// </value>
        [IndexField("instocklocations")]
        public string InStockLocations { get; set; }
    }
}
