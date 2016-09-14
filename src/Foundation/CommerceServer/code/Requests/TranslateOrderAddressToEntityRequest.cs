using CommerceServer.Core.Runtime.Orders;
using Sitecore.Commerce.Connect.CommerceServer.Pipelines;
using Sitecore.Commerce.Entities;
using Sitecore.Diagnostics;

namespace Sitecore.Foundation.CommerceServer.Requests
{
    /// <summary>
    /// Defines the TranslateOrderAddressToEntityRequest class.
    /// </summary>
    public class TranslateOrderAddressToEntityRequest : CommerceRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TranslateOrderAddressToEntityRequest"/> class.
        /// </summary>
        /// <param name="sourceAddress">The source address.</param>
        /// <param name="destinationParty">The destination party.</param>
        public TranslateOrderAddressToEntityRequest([NotNull] OrderAddress sourceAddress, [NotNull] Party destinationParty)
        {
            Assert.ArgumentNotNull(sourceAddress, "sourceAddress");
            Assert.ArgumentNotNull(destinationParty, "destinationParty");

            this.SourceAddress = sourceAddress;
            this.DestinationParty = destinationParty;
        }

        /// <summary>
        /// Gets or sets the destination party.
        /// </summary>
        /// <value>
        /// The destination party.
        /// </value>
        public Party DestinationParty { get; set; }

        /// <summary>
        /// Gets or sets the source address.
        /// </summary>
        /// <value>
        /// The source address.
        /// </value>
        public OrderAddress SourceAddress { get; set; }
    }
}
