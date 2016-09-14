namespace Sitecore.Foundation.CommerceServer.Models
{
    /// <summary>
    /// Used to hold catalog route data
    /// </summary>
    public class CatalogRouteData
    {
        /// <summary>
        /// Gets or sets the catalog item id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the catalog name associated with the id
        /// </summary>
        public string Catalog { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the id is for a product or not
        /// </summary>
        public bool IsProduct { get; set; }
    }
}