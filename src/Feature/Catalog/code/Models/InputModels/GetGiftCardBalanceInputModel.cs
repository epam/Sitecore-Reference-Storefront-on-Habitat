namespace Sitecore.Feature.Catalog.Models.InputModels
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// View model for a checking gift card balance.
    /// </summary>
    public class GetGiftCardBalanceInputModel
    {
        /// <summary>
        /// Gets or sets the GiftCard Id.
        /// </summary>
        [Required]
        public string GiftCardId { get; set; }
    }
}
