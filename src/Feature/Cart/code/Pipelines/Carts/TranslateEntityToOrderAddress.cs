using CommerceServer.Core.Runtime.Orders;
using Sitecore.Commerce.Connect.CommerceServer.Orders.Pipelines;
using Sitecore.Commerce.Connect.CommerceServer.Pipelines;
using Sitecore.Commerce.Entities;
using Sitecore.Diagnostics;
using Sitecore.Foundation.CommerceServer.Infrastructure.Constants;
using TranslateEntityToOrderAddressRequest = Sitecore.Foundation.CommerceServer.Requests.TranslateEntityToOrderAddressRequest;

namespace Sitecore.Feature.Cart.Pipelines.Carts
{
    using ConnectOrderModels = Commerce.Connect.CommerceServer.Orders.Models;

    /// <summary>
    /// Defines the TranslateEntityToOrderAddress class.
    /// </summary>
    public class TranslateEntityToOrderAddress : CommerceTranslateProcessor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TranslateEntityToOrderAddress"/> class.
        /// </summary>
        /// <param name="entityFactory">The entity factory.</param>
        public TranslateEntityToOrderAddress([NotNull] IEntityFactory entityFactory)
        {
            Assert.ArgumentNotNull(entityFactory, "entityFactory");

            this.EntityFactory = entityFactory;
        }

        /// <summary>
        /// Gets or sets the entity factory.
        /// </summary>
        /// <value>
        /// The entity factory.
        /// </value>
        public IEntityFactory EntityFactory { get; set; }

        /// <summary>
        /// Processes the specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public override void Process(Sitecore.Commerce.Pipelines.ServicePipelineArgs args)
        {
            Assert.ArgumentNotNull(args, "args");
            Assert.ArgumentNotNull(args.Request, "args.request");
            Assert.ArgumentNotNull(args.Result, "args.result");

            Party requestSourceParty;
            OrderAddress requestDestinationAddress;
            if (args.Request is TranslateEntityToOrderAddressRequest)
            {
                var refStorefrontRequest = (TranslateEntityToOrderAddressRequest)args.Request;
                Assert.ArgumentNotNull(refStorefrontRequest.SourceParty, "request.SourceParty");
                Assert.ArgumentNotNull(refStorefrontRequest.DestinationAddress, "request.DestinationAddress");

                requestSourceParty = refStorefrontRequest.SourceParty;
                requestDestinationAddress = refStorefrontRequest.DestinationAddress;
            }
            else
            {
                var csConnectRequest = (Sitecore.Commerce.Connect.CommerceServer.Orders.Pipelines.TranslateEntityToOrderAddressRequest)args.Request;
                Assert.ArgumentNotNull(csConnectRequest.SourceParty, "request.SourceParty");
                Assert.ArgumentNotNull(csConnectRequest.DestinationAddress, "request.DestinationAddress");

                requestSourceParty = csConnectRequest.SourceParty;
                requestDestinationAddress = csConnectRequest.DestinationAddress;
            }

            if (requestSourceParty is ConnectOrderModels.CommerceParty)
            {
                this.TranslateCommerceParty(requestSourceParty as ConnectOrderModels.CommerceParty, requestDestinationAddress);
            }
            else if (requestSourceParty is EmailParty)
            {
                this.TranslateEmailParty(requestSourceParty as EmailParty, requestDestinationAddress);
            }
            else
            {
                this.TranslateCustomParty(requestSourceParty, requestDestinationAddress);
            }

            TranslateEntityToOrderAddressResult result = (TranslateEntityToOrderAddressResult)args.Result;

            result.Address = requestDestinationAddress;
        }

        /// <summary>
        /// Translates the specified source party.
        /// </summary>
        /// <param name="sourceParty">The source party.</param>
        /// <param name="destinationAddress">The destination address.</param>
        protected virtual void TranslateCommerceParty([NotNull] ConnectOrderModels.CommerceParty sourceParty, [NotNull] OrderAddress destinationAddress)
        {
            Assert.ArgumentNotNullOrEmpty(sourceParty.Name, "sourceParty.Name");

            destinationAddress.City = sourceParty.City;
            destinationAddress.CountryName = sourceParty.Country;
            destinationAddress.DaytimePhoneNumber = sourceParty.PhoneNumber;
            destinationAddress.Email = sourceParty.Email;
            destinationAddress.FirstName = sourceParty.FirstName;
            destinationAddress.LastName = sourceParty.LastName;
            destinationAddress.Line1 = sourceParty.Address1;
            destinationAddress.Line2 = sourceParty.Address2;
            destinationAddress.PostalCode = sourceParty.ZipPostalCode;
            destinationAddress.State = sourceParty.State;
            destinationAddress.CountryCode = sourceParty.CountryCode;
            destinationAddress.EveningPhoneNumber = sourceParty.EveningPhoneNumber;
            destinationAddress.FaxNumber = sourceParty.FaxNumber;
            destinationAddress.Name = sourceParty.Name;
            destinationAddress.Organization = sourceParty.Company;
            destinationAddress.RegionCode = sourceParty.RegionCode;
            destinationAddress.RegionName = sourceParty.RegionName;

            this.TranslateCommercePartyCustomProperties(sourceParty, destinationAddress);
        }

        /// <summary>
        /// Translates the custom properties.
        /// </summary>
        /// <param name="sourceParty">The source party.</param>
        /// <param name="destinationAddress">The destination address.</param>
        protected virtual void TranslateCommercePartyCustomProperties([NotNull] ConnectOrderModels.CommerceParty sourceParty, [NotNull] OrderAddress destinationAddress)
        {
            destinationAddress[CommerceServerStorefrontConstants.KnowWeaklyTypesProperties.PartyType] = 1;
        }

        /// <summary>
        /// Translates the email party.
        /// </summary>
        /// <param name="sourceParty">The source party.</param>
        /// <param name="destinationAddress">The destination address.</param>
        protected virtual void TranslateEmailParty([NotNull] EmailParty sourceParty, [NotNull] OrderAddress destinationAddress)
        {
            Assert.ArgumentNotNullOrEmpty(sourceParty.Name, "sourceParty.Name");

            destinationAddress.Name = sourceParty.Name;
            destinationAddress.Email = sourceParty.Email;
            destinationAddress.FirstName = sourceParty.FirstName;
            destinationAddress.LastName = sourceParty.LastName;
            destinationAddress.Organization = sourceParty.Company;
            destinationAddress[CommerceServerStorefrontConstants.KnowWeaklyTypesProperties.EmailText] = sourceParty.Text;

            this.TranslateEmailPartyCustomProperties(sourceParty, destinationAddress);
        }

        /// <summary>
        /// Translates the custom properties.
        /// </summary>
        /// <param name="sourceParty">The source party.</param>
        /// <param name="destinationAddress">The destination address.</param>
        protected virtual void TranslateEmailPartyCustomProperties([NotNull] EmailParty sourceParty, [NotNull] OrderAddress destinationAddress)
        {
            destinationAddress[CommerceServerStorefrontConstants.KnowWeaklyTypesProperties.PartyType] = 2;
        }

        /// <summary>
        /// Translates the custom party.
        /// </summary>
        /// <param name="sourceParty">The source party.</param>
        /// <param name="destinationAddress">The destination address.</param>
        protected virtual void TranslateCustomParty([NotNull] Party sourceParty, [NotNull] OrderAddress destinationAddress)
        {
        }
    }
}