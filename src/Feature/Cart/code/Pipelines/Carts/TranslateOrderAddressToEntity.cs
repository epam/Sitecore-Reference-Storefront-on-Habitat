using CommerceServer.Core.Runtime.Orders;
using Sitecore.Commerce.Connect.CommerceServer.Pipelines;
using Sitecore.Commerce.Entities;
using Sitecore.Diagnostics;
using Sitecore.Foundation.CommerceServer.Infrastructure.Constants;
using Sitecore.Foundation.CommerceServer.Requests;

namespace Sitecore.Feature.Cart.Pipelines.Carts
{
    using ConnectOrderModels = Commerce.Connect.CommerceServer.Orders.Models;

    /// <summary>
    /// Defines the TranslateOrderAddressToEntity class.
    /// </summary>
    public class TranslateOrderAddressToEntity : CommerceTranslateProcessor
    {
        /// <summary>
        /// Processes the specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public override void Process(Sitecore.Commerce.Pipelines.ServicePipelineArgs args)
        {
            Assert.ArgumentNotNull(args, "args");
            Assert.ArgumentNotNull(args.Request, "args.request");
            Assert.ArgumentNotNull(args.Result, "args.result");

            TranslateOrderAddressToEntityRequest request = (TranslateOrderAddressToEntityRequest)args.Request;
            Assert.ArgumentNotNull(request.SourceAddress, "request.SourceAddress");
            Assert.ArgumentNotNull(request.DestinationParty, "request.DestinationParty");

            if (request.DestinationParty is ConnectOrderModels.CommerceParty)
            {
                this.TranslateToCommerceParty(request.SourceAddress, request.DestinationParty as ConnectOrderModels.CommerceParty);
            }
            else if (request.DestinationParty is EmailParty)
            {
                this.TranslateToEmailParty(request.SourceAddress, request.DestinationParty as EmailParty);
            }
            else
            {
                this.TranslateToCustomParty(request.SourceAddress, request.DestinationParty);
            }
        }

        /// <summary>
        /// Translates to commerce party.
        /// </summary>
        /// <param name="sourceAddress">The source address.</param>
        /// <param name="destinationParty">The destination party.</param>
        protected virtual void TranslateToCommerceParty(OrderAddress sourceAddress, ConnectOrderModels.CommerceParty destinationParty)
        {
            destinationParty.ExternalId = sourceAddress.OrderAddressId;
            destinationParty.City = sourceAddress.City;
            destinationParty.Country = sourceAddress.CountryName;
            destinationParty.CountryCode = sourceAddress.CountryCode;
            destinationParty.PhoneNumber = sourceAddress.DaytimePhoneNumber;
            destinationParty.Email = sourceAddress.Email;
            destinationParty.FirstName = sourceAddress.FirstName;
            destinationParty.LastName = sourceAddress.LastName;
            destinationParty.Address1 = sourceAddress.Line1;
            destinationParty.Address2 = sourceAddress.Line2;
            destinationParty.ZipPostalCode = sourceAddress.PostalCode;
            destinationParty.State = sourceAddress.State;
            destinationParty.EveningPhoneNumber = sourceAddress.EveningPhoneNumber;
            destinationParty.FaxNumber = sourceAddress.FaxNumber;
            destinationParty.Name = sourceAddress.Name;
            destinationParty.Company = sourceAddress.Organization;
            destinationParty.RegionCode = sourceAddress.RegionCode;
            destinationParty.RegionName = sourceAddress.RegionName;

            this.TranslateToCustomAddressProperties(sourceAddress, destinationParty);
        }

        /// <summary>
        /// Translates to custom address properties.
        /// </summary>
        /// <param name="sourceAddress">The source address.</param>
        /// <param name="destinationParty">The destination party.</param>
        protected virtual void TranslateToCustomAddressProperties([NotNull] OrderAddress sourceAddress, [NotNull] ConnectOrderModels.CommerceParty destinationParty)
        {
        }

        /// <summary>
        /// Translates to email party.
        /// </summary>
        /// <param name="sourceAddress">The source address.</param>
        /// <param name="destinationParty">The destination party.</param>
        protected virtual void TranslateToEmailParty(OrderAddress sourceAddress, EmailParty destinationParty)
        {
            destinationParty.ExternalId = sourceAddress.OrderAddressId;
            destinationParty.Name = sourceAddress.Name;
            destinationParty.Email = sourceAddress.Email;
            destinationParty.FirstName = sourceAddress.FirstName;
            destinationParty.LastName = sourceAddress.LastName;
            destinationParty.Company = sourceAddress.Organization;
            destinationParty.Text = sourceAddress[CommerceServerStorefrontConstants.KnowWeaklyTypesProperties.EmailText] as string;

            this.TranslateToCustomAddressProperties(sourceAddress, destinationParty);
        }

        /// <summary>
        /// Translates to custom address properties.
        /// </summary>
        /// <param name="sourceAddress">The source address.</param>
        /// <param name="destinationParty">The destination party.</param>
        protected virtual void TranslateToCustomAddressProperties([NotNull] OrderAddress sourceAddress, [NotNull] EmailParty destinationParty)
        {
        }

        /// <summary>
        /// Translates to custom party.
        /// </summary>
        /// <param name="sourceAddress">The source address.</param>
        /// <param name="destinationParty">The destination party.</param>
        protected virtual void TranslateToCustomParty(OrderAddress sourceAddress, Party destinationParty)
        {
        }
    }
}