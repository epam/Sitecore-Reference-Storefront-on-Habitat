namespace Sitecore.Foundation.CommerceServer.Extensions
{
    using Sitecore.Commerce.Connect.CommerceServer.Orders.Pipelines;
    using Sitecore.Commerce.Entities.Carts;
    using Sitecore.Commerce.Services.Carts;

    /// <summary>
    /// Some utility methods for working with cart requests
    /// </summary>
    public static class CartExtensions
    {
        /// <summary>
        /// Adds a value to a cart request indicating that the Commerce Server pipelines (.pcf) should/shouldn't be run on the cart
        /// </summary>
        /// <param name="request">The request to append to</param>
        /// <param name="refresh">if set to <c>true</c> run the Commerce Server pipelines.</param>
        public static void RefreshCart(this CartRequest request, bool refresh)
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

        /// <summary>
        /// Determines whether [has basket errors] [the specified cart].
        /// </summary>
        /// <param name="cart">The cart.</param>
        /// <returns>True if basket errors have been detected; Otherwise false.</returns>
        public static bool HasBasketErrors(this CartBase cart)
        {
            return cart.Properties.ContainsProperty("_Basket_Errors");
        }
    }
}