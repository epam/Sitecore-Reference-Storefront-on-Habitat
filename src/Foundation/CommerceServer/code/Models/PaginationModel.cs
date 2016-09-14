namespace Sitecore.Foundation.CommerceServer.Models
{
    /// <summary>
    /// Used to represent pagination details
    /// </summary>
    public class PaginationModel
    {
        /// <summary>
        /// Gets or sets the current page number
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// Gets or sets the number of pages in the result set
        /// </summary>
        public int NumberOfPages { get; set; }

        /// <summary>
        /// Gets or sets the number of items on this page
        /// </summary>
        public int PageResultCount { get; set; }

        /// <summary>
        /// Gets or sets the index of the first item in this result set
        /// </summary>
        public int StartResultIndex { get; set; }

        /// <summary>
        /// Gets or sets the index ofthe last item in this result set
        /// </summary>
        public int EndResultIndex { get; set; }

        /// <summary>
        /// Gets or sets the overall total count of items
        /// </summary>
        public int TotalResultCount { get; set; }
    }
}