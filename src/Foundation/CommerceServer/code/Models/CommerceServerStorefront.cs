namespace Sitecore.Foundation.CommerceServer.Models
{
    using Infrastructure.Constants;
    using Sitecore.Data.Items;

    /// <summary>
    /// The CommerceServerStorefront class.
    /// </summary>
    public class CommerceServerStorefront : CommerceStorefront
    {
        private Item _countryAndRegionItem;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommerceServerStorefront"/> class.
        /// </summary>
        public CommerceServerStorefront()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommerceServerStorefront"/> class.
        /// </summary>
        /// <param name="item">The item.</param>
        public CommerceServerStorefront(Item item)
            : base(item)
        {
        }

        /// <summary>
        /// Gets the country and region item.
        /// </summary>
        /// <value>
        /// The country and region item.
        /// </value>
        public Item CountryAndRegionItem
        {
            get
            {
                if (this._countryAndRegionItem == null)
                {
                    this._countryAndRegionItem = this.HomeItem.Database.GetItem(this.HomeItem[CommerceServerStorefrontConstants.KnownFieldNames.CountryLocationPath]);
                }

                return this._countryAndRegionItem;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the storefront supports wishlists.
        /// </summary>
        /// <value>
        /// <c>true</c> if the storefront supports wishlists; otherwise, <c>false</c>.
        /// </value>
        public override bool SupportsWishLists
        {
            get
            {
                return MainUtil.GetBool(this.HomeItem[CommerceServerStorefrontConstants.KnownFieldNames.SupportsWishLists], false);
            }
        }

        /// <summary>
        /// Gets a value indicating whether storefront supports loyalty programs.
        /// </summary>
        /// <value>
        /// <c>true</c> if the storefront supports loyalty programs; otherwise, <c>false</c>.
        /// </value>
        public override bool SupportsLoyaltyPrograms
        {
            get
            {
                return MainUtil.GetBool(this.HomeItem[CommerceServerStorefrontConstants.KnownFieldNames.SupportsLoyaltyProgram], false);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the storefront supports gift card payment.
        /// </summary>
        /// <value>
        /// <c>true</c> if the storefront supports gift card payment; otherwise, <c>false</c>.
        /// </value>
        public override bool SupportsGiftCardPayment
        {
            get
            {
                return MainUtil.GetBool(this.HomeItem[CommerceServerStorefrontConstants.KnownFieldNames.SupportsGirstCardPayment], false);
            }
        }
    }
}