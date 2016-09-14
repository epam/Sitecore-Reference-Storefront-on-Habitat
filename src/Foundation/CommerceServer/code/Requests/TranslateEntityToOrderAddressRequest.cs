using Sitecore.Commerce.Connect.CommerceServer.Pipelines;
using Sitecore.Commerce.Entities;
using Sitecore.Diagnostics;
using OrderAddress = CommerceServer.Core.Runtime.Orders.OrderAddress;

namespace Sitecore.Foundation.CommerceServer.Requests
{
    /// <summary>
    /// Defines the TranslateEntityToOrderAddressRequest class.
    /// </summary>
    public class TranslateEntityToOrderAddressRequest : CommerceRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TranslateEntityToOrderAddressRequest"/> class.
        /// </summary>
        /// <param name="sourceParty">The source party.</param>
        /// <param name="destinationAddress">The destination address.</param>
        public TranslateEntityToOrderAddressRequest([NotNull] Party sourceParty, [NotNull] OrderAddress destinationAddress)
        {
            Assert.ArgumentNotNull(sourceParty, "sourceParty");
            Assert.ArgumentNotNull(destinationAddress, "destinationAddress");

            this.SourceParty = sourceParty;
            this.DestinationAddress = destinationAddress;
        }

        /// <summary>
        /// Gets or sets the source party.
        /// </summary>
        /// <value>
        /// The source party.
        /// </value>
        public Party SourceParty { get; set; }

        /// <summary>
        /// Gets or sets the destination address.
        /// </summary>
        /// <value>
        /// The destination address.
        /// </value>
        public OrderAddress DestinationAddress { get; set; }
    }
}