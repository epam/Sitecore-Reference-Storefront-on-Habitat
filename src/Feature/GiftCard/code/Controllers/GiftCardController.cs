using System.Web.Mvc;
using Sitecore.Foundation.CommerceServer.Helpers;
using Sitecore.Foundation.CommerceServer.Managers;

namespace Sitecore.Feature.GiftCard.Controllers
{
    using Sitecore.Feature.Base.Controllers;
    using Sitecore.Feature.GiftCard.Repositories;
    using Sitecore.Foundation.CommerceServer.Interfaces;

    public class GiftCardController : CSBaseController
    {
        private readonly IGiftCardRepository _giftCardRepository;

        public GiftCardController(
            [NotNull] IAccountManager accountManager,
            [NotNull] IContactFactory contactFactory,
            [NotNull] IGiftCardRepository giftCardRepository)
            : base(accountManager, contactFactory)
        {
            _giftCardRepository = giftCardRepository;
        }

        public ActionResult GiftCardInformation()
        {
            this.Item = SearchNavigation.GetProduct(StorefrontManager.CurrentStorefront.GiftCardProductId, CurrentCatalog.Name);
            var productViewModel = _giftCardRepository.GetGiftCardViewModel(this.Item, this.CurrentRendering);

            return View("GiftCardInformation", productViewModel);
        }

        public ActionResult GiftCardImages()
        {
            this.Item = SearchNavigation.GetProduct(StorefrontManager.CurrentStorefront.GiftCardProductId, CurrentCatalog.Name);
            var productViewModel = _giftCardRepository.GetGiftCardViewModel(this.Item, this.CurrentRendering);

            return View("GiftCardImages", productViewModel);
        }

        public ActionResult GiftCardRating()
        {
            this.Item = SearchNavigation.GetProduct(StorefrontManager.CurrentStorefront.GiftCardProductId, CurrentCatalog.Name);
            var productViewModel = _giftCardRepository.GetGiftCardViewModel(this.Item, this.CurrentRendering);

            return View("GiftCardRating", productViewModel);
        }
    }
}