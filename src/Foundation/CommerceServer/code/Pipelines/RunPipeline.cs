using Sitecore.Commerce.Connect.CommerceServer.Orders.Pipelines;
using Sitecore.Diagnostics;

namespace Sitecore.Foundation.CommerceServer.Pipelines
{
    /// <summary>
    /// Defines the RunPipeline class.
    /// </summary>
    public class RunPipeline : Sitecore.Commerce.Connect.CommerceServer.Orders.Pipelines.RunPipeline
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RunPipeline"/> class.
        /// </summary>
        /// <param name="pipelineName">Name of the pipeline.</param>
        public RunPipeline(string pipelineName) : base(pipelineName)
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
    }
}