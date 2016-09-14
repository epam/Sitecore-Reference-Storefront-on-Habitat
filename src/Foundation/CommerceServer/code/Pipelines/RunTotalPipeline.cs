using CommerceServer.Core.Runtime.Orders;
using Sitecore.Commerce.Connect.CommerceServer.Orders.Pipelines;
using Sitecore.Diagnostics;

namespace Sitecore.Foundation.CommerceServer.Pipelines
{
    /// <summary>
    /// Defines the RunTotalPipeline class.
    /// </summary>
    public class RunTotalPipeline : Sitecore.Commerce.Connect.CommerceServer.Orders.Pipelines.RunTotalPipeline
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RunTotalPipeline"/> class.
        /// </summary>
        /// <param name="pipelineName">Name of the pipeline.</param>
        public RunTotalPipeline(string pipelineName)
            : base(pipelineName)
        {
        }

        /// <summary>
        /// Processes the specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public override void Process(Commerce.Pipelines.ServicePipelineArgs args)
        {
            base.Process(args);

            var cartContext = CartPipelineContext.Get(args.Request.RequestContext);
            Assert.IsNotNullOrEmpty(cartContext.UserId, "cartContext.UserId");
            Assert.IsNotNullOrEmpty(cartContext.ShopName, "cartContext.ShopName");

            if (cartContext.HasBasketErrors && !args.Result.Success)
            {
                args.Result.Success = true;
            }
        }

        /// <summary>
        /// Determines whether this instance [can run pipeline] the specified basket.
        /// </summary>
        /// <param name="basket">The basket.</param>
        /// <returns>True if the piepline can be executed.  Otherwise false.</returns>
        protected override bool CanRunPipeline(Basket basket)
        {
            bool canRunPipeline = true;

            foreach (OrderForm orderForm in basket.OrderForms)
            {
                foreach (LineItem lineItem in orderForm.LineItems)
                {
                    if (!this.IsValidShippingMethod(lineItem.ShippingMethodId))
                    {
                        return false;
                    }
                }
            }

            return canRunPipeline;
        }
    }
}
