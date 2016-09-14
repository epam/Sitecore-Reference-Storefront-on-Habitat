namespace Sitecore.Feature.Cart.Models.JsonResults
{
    using Sitecore.Commerce.Entities.Carts;
    using Sitecore.Diagnostics;
    using Sitecore.Feature.Base.Models.JsonResults;

    /// <summary>
    /// Emits the Json result of a cart adjustment request.
    /// </summary>
    public class CartAdjustmentBaseJsonResult : BaseJsonResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CartAdjustmentBaseJsonResult"/> class.
        /// </summary>
        /// <param name="adjustment">The cart adjustment.</param>
        public CartAdjustmentBaseJsonResult(CartAdjustment adjustment)
        {
            Assert.ArgumentNotNull(adjustment, "adjustment");
            this.Amount = adjustment.Amount.ToString("C", Sitecore.Context.Language.CultureInfo);
            this.Description = adjustment.Description;
            this.IsCharge = adjustment.IsCharge;
            this.LineNumber = adjustment.LineNumber;
            this.Percentage = adjustment.Percentage;
        }

        /// <summary>
        /// Gets or sets the adjustment amount.
        /// </summary>
        public string Amount { get; set; }

        /// <summary>
        /// Gets or sets the adjustment desccription.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the adjustment is a charge.
        /// </summary>
        public bool IsCharge { get; set; }

        /// <summary>
        /// Gets or sets the adjustment line number.
        /// </summary>
        public uint LineNumber { get; set; }

        /// <summary>
        /// Gets or sets the adjustment percentage.
        /// </summary>
        public float Percentage { get; set; }
    }
}