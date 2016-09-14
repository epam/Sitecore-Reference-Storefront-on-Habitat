namespace Sitecore.Foundation.CommerceServer.Models
{
    using Infrastructure.Constants;
    using Sitecore.Data.Items;
    
    /// <summary>
    /// Defines the CurrencyInformationModel class.
    /// </summary>
    public class CurrencyInformationModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CurrencyInformationModel"/> class.
        /// </summary>
        /// <param name="item">The item.</param>
        public CurrencyInformationModel(Item item)
        {
            this.Name = item.Name;
            this.Description = item[StorefrontConstants.KnownFieldNames.CurrencyDescription];
            this.Symbol = item[StorefrontConstants.KnownFieldNames.CurrencySymbol];
            this.SymbolPosition = MainUtil.GetInt(item[StorefrontConstants.KnownFieldNames.CurrencySymbolPosition], 3);
            this.CurrencyNumberFormatCulture = item[StorefrontConstants.KnownFieldNames.CurrencyNumberFormatCulture];
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the currency description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the currency symbol.
        /// </summary>
        /// <value>
        /// The symbol.
        /// </value>
        public string Symbol { get; set; }

        /// <summary>
        /// Gets or sets the symbol position.
        /// </summary>
        /// <value>
        /// The symbol position.
        /// </value>
        public int SymbolPosition { get; set; }

        /// <summary>
        /// Gets or sets the currency number format culture.
        /// </summary>
        /// <value>
        /// The currency number format culture.
        /// </value>
        public string CurrencyNumberFormatCulture { get; set; }
    }
}