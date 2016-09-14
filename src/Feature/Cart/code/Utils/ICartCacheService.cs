using Sitecore.Commerce.Connect.CommerceServer.Orders.Models;

namespace Sitecore.Feature.Cart.Utils
{
    public interface ICartCacheService
    {
        CommerceCart GetCart(string id);

        void AddToCart(CommerceCart cartLine);

        void Invalidate(string customerId);
    }
}