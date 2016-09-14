namespace Sitecore.Feature.Cart.Models.JsonResults
{
    using System.Collections.Generic;
    using Sitecore.Commerce.Entities;
    using Sitecore.Commerce.Services;
    using Sitecore.Diagnostics;
    using Sitecore.Feature.Base.Models.JsonResults;
    using Sitecore.Mvc.Extensions;

    /// <summary>
    /// Json result for list of parties operations.
    /// </summary>
    public class AddressListItemBaseJsonResult : BaseJsonResult
    {
        private readonly List<AddressItemBaseJsonResult> _addresses = new List<AddressItemBaseJsonResult>();
        private readonly Dictionary<string, string> _countries = new Dictionary<string, string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Sitecore.Feature.Cart.Models.JsonResults.AddressListItemBaseJsonResult"/> class.
        /// </summary>
        public AddressListItemBaseJsonResult()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Sitecore.Feature.Cart.Models.JsonResults.AddressListItemBaseJsonResult"/> class.
        /// </summary>
        /// <param name="result">The service provider result.</param>
        public AddressListItemBaseJsonResult(ServiceProviderResult result)
            : base(result)
        {
        }

        /// <summary>
        /// Gets the list of addresses.
        /// </summary>
        public List<AddressItemBaseJsonResult> Addresses
        {
            get
            {
                return this._addresses;
            }
        }

        /// <summary>
        /// Gets the available countries.
        /// </summary>
        public Dictionary<string, string> Countries
        {
            get
            {
                return this._countries;
            }
        }

        /// <summary>
        /// Initializes the specified addresses.
        /// </summary>
        /// <param name="addresses">The addresses.</param>
        /// <param name="countries">The countries.</param>
        public virtual void Initialize(IEnumerable<Party> addresses, Dictionary<string, string> countries)
        {
            Assert.ArgumentNotNull(addresses, "addresses");

            if (countries != null && countries.Count > 0)
            {
                this.Countries.AddRange(countries);
            }
        }
    }
}