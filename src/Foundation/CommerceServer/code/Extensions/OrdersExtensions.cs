namespace Sitecore.Foundation.CommerceServer.Extensions
{
    using Sitecore.Commerce.Connect.CommerceServer.Orders.Pipelines;
    using Sitecore.Commerce.Services.Orders;

    /// <summary>
    /// Some utility methods for working with order requests
    /// </summary>
    public static class OrdersExtensions
    {
        /// <summary>
        /// Adds a value to a cart request indicating that the Commerce Server pipelines (.pcf) should/shouldn't be run on the cart
        /// </summary>
        /// <param name="request">The request to append to</param>
        /// <param name="refresh">if set to <c>true</c> run the Commerce Server pipelines.</param>
        public static void RefreshCart(this OrdersRequest request, bool refresh)
        {
            var info = CartRequestInformation.Get(request);

            if (info == null)
            {
                info = new CartRequestInformation(request, refresh);
            }
            else
            {
                info.Refresh = refresh;
            }
        }
    }
}