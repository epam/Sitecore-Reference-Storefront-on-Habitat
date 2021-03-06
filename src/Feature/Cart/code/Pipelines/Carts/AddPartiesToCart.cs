﻿using System;
using System.Collections.Generic;
using System.Globalization;
using CommerceServer.Core.Runtime.Orders;
using CommerceServer.Core.Runtime.Profiles;
using Sitecore.Commerce.Connect.CommerceServer.Orders.Pipelines;
using Sitecore.Commerce.Connect.CommerceServer.Pipelines;
using Sitecore.Commerce.Connect.CommerceServer.Profiles;
using Sitecore.Commerce.Entities;
using Sitecore.Commerce.Services.Carts;
using Sitecore.Diagnostics;
using Sitecore.Foundation.CommerceServer.Utils;
using TranslateEntityToOrderAddressRequest = Sitecore.Foundation.CommerceServer.Requests.TranslateEntityToOrderAddressRequest;
using TranslateOrderAddressToEntityRequest = Sitecore.Foundation.CommerceServer.Requests.TranslateOrderAddressToEntityRequest;

namespace Sitecore.Feature.Cart.Pipelines.Carts
{
    using ConnectOrderModels = Commerce.Connect.CommerceServer.Orders.Models;

    /// <summary>
    /// Defines the AddPartiesToCart class.
    /// </summary>
    public class AddPartiesToCart : CommerceCartPipelineProcessor
    {
        private ICommerceProfileRepository _profileRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddPartiesToCart"/> class.
        /// </summary>
        /// <param name="entityFactory">The entity factory.</param>
        public AddPartiesToCart([NotNull] IEntityFactory entityFactory)
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

            AddPartiesRequest request = (AddPartiesRequest)args.Request;
            AddPartiesResult result = (AddPartiesResult)args.Result;

            var cartContext = CartPipelineContext.Get(request.RequestContext);
            Assert.IsNotNull(cartContext, "cartContext");

            List<Party> partiesAdded = new List<Party>();

            if (cartContext.Basket != null)
            {
                foreach (Party party in request.Parties)
                {
                    Party newParty;

                    if (party == null)
                    {
                        continue;
                    }

                    newParty = this.ProcessParty(party, cartContext);

                    partiesAdded.Add(newParty);
                }
            }

            result.Parties = partiesAdded;

            // Needed by the RunSaveCart CommerceConnect pipeline.
            var translateCartRequest = new TranslateOrderGroupToEntityRequest(cartContext.UserId, cartContext.ShopName, cartContext.Basket);
            var translateCartResult = PipelineUtility.RunCommerceConnectPipeline<TranslateOrderGroupToEntityRequest, TranslateOrderGroupToEntityResult>(PipelineNames.TranslateOrderGroupToEntity, translateCartRequest);

            result.Cart = translateCartResult.Cart;
        }

        /// <summary>
        /// Processes the party.
        /// </summary>
        /// <param name="party">The party.</param>
        /// <param name="cartContext">The cart context.</param>
        /// <returns>A party instance.</returns>
        protected virtual Party ProcessParty(Party party, CartPipelineContext cartContext)
        {
            Party newParty;

            if (party is ConnectOrderModels.CommerceParty && ((ConnectOrderModels.CommerceParty)party).UserProfileAddressId != Guid.Empty)
            {
                newParty = this.AddUserProfileAddress(party as ConnectOrderModels.CommerceParty, cartContext);
            }
            else
            {
                newParty = this.AddParty(party, "Party", cartContext);
            }

            return newParty;
        }

        /// <summary>
        /// Adds the user profile address.
        /// </summary>
        /// <param name="party">The party.</param>
        /// <param name="cartContext">The cart context.</param>
        /// <returns>The commerce party that was added.</returns>
        protected virtual ConnectOrderModels.CommerceParty AddUserProfileAddress(ConnectOrderModels.CommerceParty party, CartPipelineContext cartContext)
        {
            Assert.IsTrue(party.UserProfileAddressId != Guid.Empty, "party.UserProfileAddressId != Guid.Empty");
            Assert.IsNotNullOrEmpty(party.Name, "party.Name");

            var repository = this.GetProfileRepository();

            Profile addressProfile = repository.GetProfile("Address", party.UserProfileAddressId.ToString("B"));
            Assert.IsNotNull(addressProfile, string.Format(CultureInfo.InvariantCulture, "An invalid address profile was provided: {0}", party.UserProfileAddressId.ToString("B")));

            OrderAddress newOrderAddress = ContextTypeLoader.CreateInstance<OrderAddress>(party.Name, addressProfile);

            cartContext.Basket.Addresses.Add(newOrderAddress);

            ConnectOrderModels.CommerceParty translatedParty = this.EntityFactory.Create<ConnectOrderModels.CommerceParty>("Party");
            Assert.ArgumentNotNull(translatedParty, "translatedParty");

            var translateOrderAddressRequest = new TranslateOrderAddressToEntityRequest(newOrderAddress, translatedParty);
            PipelineUtility.RunCommerceConnectPipeline<TranslateOrderAddressToEntityRequest, CommerceResult>(PipelineNames.TranslateOrderAddressToEntity, translateOrderAddressRequest);

            return translatedParty;
        }

        /// <summary>
        /// Adds the party.
        /// </summary>
        /// <param name="party">The party.</param>
        /// <param name="entityName">Name of the entity.</param>
        /// <param name="cartContext">The cart context.</param>
        /// <returns>
        /// The commerce party that was added.
        /// </returns>
        protected virtual ConnectOrderModels.CommerceParty AddParty(Party party, string entityName, CartPipelineContext cartContext)
        {
            OrderAddress destinationAddress = ContextTypeLoader.CreateInstance<OrderAddress>(this.GetPartyName(party), party.ExternalId);

            TranslateEntityToOrderAddressRequest translateRequest = new TranslateEntityToOrderAddressRequest(party, destinationAddress);

            var translateResult = PipelineUtility.RunCommerceConnectPipeline<TranslateEntityToOrderAddressRequest, TranslateEntityToOrderAddressResult>(PipelineNames.TranslateEntityToOrderAddress, translateRequest);

            OrderAddress newOrderAddress = translateResult.Address;

            cartContext.Basket.Addresses.Add(newOrderAddress);

            ConnectOrderModels.CommerceParty translatedParty = this.EntityFactory.Create<ConnectOrderModels.CommerceParty>(entityName);
            Assert.ArgumentNotNull(translatedParty, "translatedParty");

            TranslateOrderAddressToEntityRequest translateOrderAddressRequest = new TranslateOrderAddressToEntityRequest(newOrderAddress, translatedParty);
            PipelineUtility.RunCommerceConnectPipeline<TranslateOrderAddressToEntityRequest, CommerceResult>(PipelineNames.TranslateOrderAddressToEntity, translateOrderAddressRequest);

            return translatedParty;
        }

        /// <summary>
        /// Gets the profile repository.
        /// </summary>
        /// <returns>An instance of the profile respository.</returns>
        protected virtual ICommerceProfileRepository GetProfileRepository()
        {
            if (this._profileRepository == null)
            {
                this._profileRepository = ContextTypeLoader.CreateInstance<ICommerceProfileRepository>();
            }

            return this._profileRepository;
        }

        /// <summary>
        /// Gets the name of the party.
        /// </summary>
        /// <param name="party">The party.</param>
        /// <returns>The name of the party.</returns>
        protected virtual string GetPartyName(Party party)
        {
            if (party is ConnectOrderModels.CommerceParty)
            {
                return ((ConnectOrderModels.CommerceParty)party).Name;
            }
            else if (party is EmailParty)
            {
                return ((EmailParty)party).Name;
            }
            else
            {
                return "Undefined";
            }
        }
    }
}