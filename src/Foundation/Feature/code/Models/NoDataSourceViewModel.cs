namespace Sitecore.Foundation.Feature.Models
{
    using Sitecore.Data.Items;
    using Sitecore.Diagnostics;

    /// <summary>
    /// Used to represent a no datasource view
    /// </summary>
    public class NoDataSourceViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NoDataSourceViewModel" /> class
        /// </summary>
        /// <param name="noDataSourceItem">The no data source item.</param>
        public NoDataSourceViewModel(Item noDataSourceItem)
        {
            Assert.ArgumentNotNull(noDataSourceItem, "noDatasourceItem");
            this.Message = noDataSourceItem["Text"];
        }

        /// <summary>
        /// Gets the error message to display when there is no datasource
        /// </summary>
        public string Message { get; private set; }
    }
}