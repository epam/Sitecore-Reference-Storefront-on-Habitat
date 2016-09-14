using Sitecore.Mvc.Presentation;

namespace Sitecore.Feature.GiftCard.Repositories
{
    using Sitecore.Data.Items;
    using Sitecore.Foundation.CommerceServer.Models;
    
    public interface IGiftCardRepository
    {
        ProductViewModel GetGiftCardViewModel(Item productItem, Rendering currentRendering);
    }
}