namespace Sitecore.Feature.Cart.Models.JsonResults
{
    using Sitecore.Commerce.Connect.CommerceServer.Orders.Models;
    using Sitecore.Commerce.Entities;
    using Sitecore.Commerce.Services;

    /// <summary>
    /// Json result for party operations.
    /// </summary>
    public class AddressItemJsonResult : AddressItemBaseJsonResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddressItemJsonResult"/> class.
        /// </summary>
        public AddressItemJsonResult()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AddressItemJsonResult"/> class.
        /// </summary>
        /// <param name="result">The service provider result.</param>
        public AddressItemJsonResult(ServiceProviderResult result)
            : base(result)
        {
        }

        /// <summary>
        /// Initializes the specified address.
        /// </summary>
        /// <param name="address">The address.</param>
        public override void Initialize(Party address)
        {
            base.Initialize(address);
            this.Name = ((CommerceParty)address).Name;
            this.IsPrimary = ((CommerceParty)address).IsPrimary;
        }
    }
}