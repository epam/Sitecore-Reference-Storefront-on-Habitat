using Sitecore.Commerce.Connect.CommerceServer.Orders.Models;
using Sitecore.Foundation.CommerceServer.Helpers;
using Sitecore.Foundation.CommerceServer.Utils;

namespace Sitecore.Feature.Cart.Utils
{
    public class CartCacheService : ICartCacheService
    {
        private readonly CartCacheHelper _cartCacheHelper;

        public CartCacheService()
        {
            _cartCacheHelper = ContextTypeLoader.CreateInstance<CartCacheHelper>();
        }

        public CommerceCart GetCart(string id)
        {
            return _cartCacheHelper.GetCart(id);
        }

        public void AddToCart(CommerceCart cartLine)
        {
            _cartCacheHelper.AddCartToCache(cartLine);
        }

        public void Invalidate(string customerId)
        {
            _cartCacheHelper.InvalidateCartCache(customerId);
        }
    }
}
