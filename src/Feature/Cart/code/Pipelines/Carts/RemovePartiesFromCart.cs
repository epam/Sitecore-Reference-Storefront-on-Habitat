using System.Collections.Generic;
using CommerceServer.Core.Runtime.Orders;
using Sitecore.Commerce.Connect.CommerceServer.Orders.Pipelines;
using Sitecore.Commerce.Connect.CommerceServer.Pipelines;
using Sitecore.Commerce.Entities;
using Sitecore.Commerce.Services.Carts;
using Sitecore.Diagnostics;

namespace Sitecore.Feature.Cart.Pipelines.Carts
{
    /// <summary>
    /// Defines the RemovePartiesFromCart class.
    /// </summary>
    public class RemovePartiesFromCart : CommerceCartPipelineProcessor
    {
        /// <summary>
        /// Processes the specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public override void Process(Sitecore.Commerce.Pipelines.ServicePipelineArgs args)
        {
            Assert.ArgumentNotNull(args, "args");
            Assert.ArgumentNotNull(args.Request, "args.request");

            RemovePartiesRequest request = (RemovePartiesRequest)args.Request;
            RemovePartiesResult result = (RemovePartiesResult)args.Result;

            var cartContext = CartPipelineContext.Get(request.RequestContext);
            Assert.IsNotNull(cartContext, "cartContext");

            List<Party> partiesRemoved = new List<Party>();

            if (cartContext.Basket != null)
            {
                foreach (Party party in request.Parties)
                {
                    if (party != null)
                    {
                        Assert.ArgumentNotNullOrEmpty(party.ExternalId, "party.ExternalId");

                        OrderAddress orderAddress = cartContext.Basket.Addresses[party.ExternalId];
                        if (orderAddress != null)
                        {
                            cartContext.Basket.Addresses.Remove(orderAddress);

                            partiesRemoved.Add(party);
                        }
                    }
                }
            }

            result.Parties = partiesRemoved;

            // Needed by the RunSaveCart CommerceConnect pipeline.
            var translateCartRequest = new TranslateOrderGroupToEntityRequest(cartContext.UserId, cartContext.ShopName, cartContext.Basket);
            var translateCartResult = PipelineUtility.RunCommerceConnectPipeline<TranslateOrderGroupToEntityRequest, TranslateOrderGroupToEntityResult>(PipelineNames.TranslateOrderGroupToEntity, translateCartRequest);

            result.Cart = translateCartResult.Cart;
        }
    }
}