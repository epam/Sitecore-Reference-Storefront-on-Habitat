using System.Collections.Generic;
using System.Globalization;
using Sitecore.Commerce.Connect.CommerceServer.Pipelines;
using Sitecore.Commerce.Entities;
using Sitecore.Foundation.CommerceServer.Factories;
using Sitecore.Foundation.CommerceServer.Infrastructure.Constants;
using Sitecore.Foundation.CommerceServer.Requests;
using OrderAddress = CommerceServer.Core.Runtime.Orders.OrderAddress;
using OrderAddressCollection = CommerceServer.Core.Runtime.Orders.OrderAddressCollection;

namespace Sitecore.Feature.Cart.Pipelines.Carts
{
    using ConnectOrdersPipelines = Commerce.Connect.CommerceServer.Orders.Pipelines;

    /// <summary>
    /// Defines the TranslateOrderGroupToEntity class.
    /// </summary>
    public class TranslateOrderGroupToEntity : ConnectOrdersPipelines.TranslateOrderGroupToEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TranslateOrderGroupToEntity"/> class.
        /// </summary>
        /// <param name="entityFactory">The entity factory.</param>
        public TranslateOrderGroupToEntity([NotNull]IEntityFactory entityFactory)
            : base(entityFactory)
        {
        }

        public TranslateOrderGroupToEntity() : base(WindsorConfig.Container.Kernel.Resolve<IEntityFactory>())
        {

        }

        /// <summary>
        /// Translates the addresses.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="cart">The cart.</param>
        protected override void TranslateAddresses(OrderAddressCollection collection, Commerce.Entities.Carts.Cart cart)
        {
            List<Party> partyList = new List<Party>();

            foreach (OrderAddress commerceAddress in collection)
            {
                int partyType = (commerceAddress[CommerceServerStorefrontConstants.KnowWeaklyTypesProperties.PartyType] == null) ? 1 : System.Convert.ToInt32(commerceAddress[CommerceServerStorefrontConstants.KnowWeaklyTypesProperties.PartyType], CultureInfo.InvariantCulture);

                Party party = null;

                switch (partyType)
                {
                    default:
                    case 1:
                        party = this.EntityFactory.Create<Commerce.Connect.CommerceServer.Orders.Models.CommerceParty>("Party");
                        this.TranslateAddress(commerceAddress, party as Sitecore.Commerce.Connect.CommerceServer.Orders.Models.CommerceParty);
                        break;

                    case 2:
                        party = this.EntityFactory.Create<EmailParty>("EmailParty");
                        this.TranslateAddress(commerceAddress, party as EmailParty);
                        break;
                }

                partyList.Add(party);
            }

            cart.Parties = partyList.AsReadOnly();
        }

        /// <summary>
        /// Translates the address.
        /// </summary>
        /// <param name="sourceAddress">The source address.</param>
        /// <param name="destinationParty">The destination party.</param>
        protected override void TranslateAddress(OrderAddress sourceAddress, Commerce.Connect.CommerceServer.Orders.Models.CommerceParty destinationParty)
        {
            TranslateOrderAddressToEntityRequest request = new TranslateOrderAddressToEntityRequest(sourceAddress, destinationParty);
            PipelineUtility.RunCommerceConnectPipeline<TranslateOrderAddressToEntityRequest, CommerceResult>(PipelineNames.TranslateOrderAddressToEntity, request);
        }

        /// <summary>
        /// Translates the address.
        /// </summary>
        /// <param name="sourceAddress">The source address.</param>
        /// <param name="destinationParty">The destination party.</param>
        protected virtual void TranslateAddress(OrderAddress sourceAddress, EmailParty destinationParty)
        {
            TranslateOrderAddressToEntityRequest request = new TranslateOrderAddressToEntityRequest(sourceAddress, destinationParty);
            PipelineUtility.RunCommerceConnectPipeline<TranslateOrderAddressToEntityRequest, CommerceResult>(PipelineNames.TranslateOrderAddressToEntity, request);
        }
    }
}