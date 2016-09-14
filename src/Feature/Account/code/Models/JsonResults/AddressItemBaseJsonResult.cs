namespace Sitecore.Feature.Account.Models.JsonResults
{
    using Sitecore.Commerce.Entities;
    using Sitecore.Commerce.Services;
    using Sitecore.Diagnostics;
    using Sitecore.Feature.Base.Models.JsonResults;
    using Sitecore.Foundation.CommerceServer.Managers;

    /// <summary>
    /// Json result for party operations.
    /// </summary>
    public class AddressItemBaseJsonResult : BaseJsonResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddressItemBaseJsonResult"/> class.
        /// </summary>
        public AddressItemBaseJsonResult()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AddressItemBaseJsonResult"/> class.
        /// </summary>
        /// <param name="result">The service provider result.</param>
        public AddressItemBaseJsonResult(ServiceProviderResult result)
            : base(result)
        {
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the external identifier.
        /// </summary>
        /// <value>
        /// The external identifier.
        /// </value>
        public string ExternalId { get; set; }

        /// <summary>
        /// Gets or sets the address1.
        /// </summary>
        /// <value>
        /// The address1.
        /// </value>
        public string Address1 { get; set; }

        /// <summary>
        /// Gets or sets the postal code.
        /// </summary>
        /// <value>
        /// The postal code.
        /// </value>
        public string ZipPostalCode { get; set; }

        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        /// <value>
        /// The city.
        /// </value>
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>
        /// The state.
        /// </value>
        public string State { get; set; }

        /// <summary>
        /// Gets or sets the country.
        /// </summary>
        /// <value>
        /// The country.
        /// </value>
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets the full address.
        /// </summary>
        /// <value>
        /// The full address.
        /// </value>
        public string FullAddress { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is primary.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is primary; otherwise, <c>false</c>.
        /// </value>
        public bool IsPrimary { get; set; }

        /// <summary>
        /// Gets or sets the details URL.
        /// </summary>
        /// <value>
        /// The details URL.
        /// </value>
        public string DetailsUrl { get; set; }

        /// <summary>
        /// Initializes the specified address.
        /// </summary>
        /// <param name="address">The address.</param>
        public virtual void Initialize(Party address)
        {
            Assert.ArgumentNotNull(address, "address");

            this.ExternalId = address.ExternalId;
            this.Address1 = address.Address1;
            this.City = address.City;
            this.State = address.State;
            this.ZipPostalCode = address.ZipPostalCode;
            this.Country = address.Country;
            this.FullAddress = string.Concat(address.Address1, ", ", address.City, ", ", address.ZipPostalCode);
            this.DetailsUrl = string.Concat(StorefrontManager.StorefrontUri("/accountmanagement/addressbook"), "?id=", address.ExternalId);
        }
    }
}