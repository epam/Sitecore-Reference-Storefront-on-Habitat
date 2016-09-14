namespace Sitecore.Feature.Cart.Models.JsonResults
{
    using Sitecore.Commerce.Connect.CommerceServer.Orders.Models;
    using Sitecore.Commerce.Entities;
    using Sitecore.Commerce.Services;
    using System.Collections.Generic;

    /// <summary>
    /// Json result for list of parties operations.
    /// </summary>
    public class AddressListItemJsonResult : AddressListItemBaseJsonResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddressListItemJsonResult"/> class.
        /// </summary>
        public AddressListItemJsonResult()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AddressListItemJsonResult"/> class.
        /// </summary>
        /// <param name="result">The service provider result.</param>
        public AddressListItemJsonResult(ServiceProviderResult result)
            : base(result)
        {
        }

        /// <summary>
        /// Initializes the specified addresses.
        /// </summary>
        /// <param name="addresses">The addresses.</param>
        /// <param name="countries">The countries.</param>
        public override void Initialize(IEnumerable<Party> addresses, Dictionary<string, string> countries)
        {
            base.Initialize(addresses, countries);

            foreach (var address in addresses)
            {
                var result = new AddressItemJsonResult();
                result.Initialize(address as CommerceParty);
                this.Addresses.Add(result);
            }
        }
    }
}