using Sitecore.Feature.Cart.Utils;

namespace Sitecore.Feature.Cart.Repositories
{
    using System.Collections.Generic;
    using Sitecore.Commerce.Connect.CommerceServer.Orders.Models;
    using Sitecore.Feature.Base.Models.JsonResults;
    using Sitecore.Feature.Base.Repositories;
    using Sitecore.Feature.Cart.Models.InputModels;
    using Sitecore.Feature.Cart.Models.JsonResults;
    using Sitecore.Foundation.CommerceServer.Extensions;
    using Sitecore.Foundation.CommerceServer.Helpers;
    using Sitecore.Foundation.CommerceServer.Interfaces;
    using Sitecore.Foundation.CommerceServer.Models.InputModels;

    public class CartRepository : BaseRepository, ICartRepository
    {
        private readonly ICartManager _cartManager;
        private readonly ICartCacheService _cartCacheService;
        private readonly IProductResolver _productResolver;

        public CartRepository(
            IAccountManager accountManager,
            IContactFactory contactFactory,
            ICartManager cartManager,
            ICartCacheService cartCacheService,
            IProductResolver productResolver)
            : base(accountManager, contactFactory)
        {
            _cartManager = cartManager;
            _cartCacheService = cartCacheService;
            _productResolver = productResolver;
        }

        public BaseJsonResult AddCartLine(AddCartLineInputModel inputModel)
        {
            var response = _cartManager.AddLineItemsToCart(
                CurrentStorefront,
                CurrentVisitorContext,
                new List<AddCartLineInputModel> { inputModel });
            var result = new BaseJsonResult(response.ServiceProviderResult);

            return result;
        }

        public BaseJsonResult AddCartLines(IEnumerable<AddCartLineInputModel> inputModels)
        {
            var response = _cartManager.AddLineItemsToCart(CurrentStorefront, CurrentVisitorContext, inputModels);
            var result = new BaseJsonResult(response.ServiceProviderResult);

            return result;
        }

        public CSCartBaseJsonResult ApplyDiscount(DiscountInputModel model)
        {
            var response = _cartManager.AddPromoCodeToCart(CurrentStorefront, CurrentVisitorContext, model.PromoCode);
            var result = new CSCartBaseJsonResult(response.ServiceProviderResult);
            if (response.ServiceProviderResult.Success && response.Result != null)
            {
                result.Initialize(response.Result, _productResolver);
            }

            return result;
        }

        public CSCartBaseJsonResult DeleteLineItem(DeleteCartLineInputModel model)
        {
            var response = _cartManager.RemoveLineItemFromCart(CurrentStorefront, CurrentVisitorContext, model.ExternalCartLineId);
            var result = new CSCartBaseJsonResult(response.ServiceProviderResult);
            if (response.ServiceProviderResult.Success && response.Result != null)
            {
                result.Initialize(response.Result, _productResolver);
            }

            return result;
        }

        public CSCartBaseJsonResult GetCurrentCart()
        {
            var id = this.CurrentVisitorContext.GetCustomerId();
            var cart = _cartCacheService.GetCart(id);

            CSCartBaseJsonResult cartResult;

            // The following condition stops the creation of empty carts on startup.
            if (cart == null && CartCookieHelper.DoesCookieExistForCustomer(id))
            {
                var response = _cartManager.GetCurrentCart(CurrentStorefront, CurrentVisitorContext, true);
                cartResult = new CSCartBaseJsonResult(response.ServiceProviderResult);
                if (response.ServiceProviderResult.Success && response.Result != null)
                {
                    cartResult.Initialize(response.ServiceProviderResult.Cart, _productResolver);
                    if (response.ServiceProviderResult.Cart != null)
                    {
                        _cartCacheService.AddToCart(response.ServiceProviderResult.Cart as CommerceCart);
                    }
                }
            }
            else
            {
                cartResult = new CSCartBaseJsonResult();
                cartResult.Initialize(cart, _productResolver);
            }

            return cartResult;
        }

        public CSCartBaseJsonResult RemoveDiscount(DiscountInputModel model)
        {
            var response = _cartManager.RemovePromoCodeFromCart(CurrentStorefront, CurrentVisitorContext, model.PromoCode);
            var result = new CSCartBaseJsonResult(response.ServiceProviderResult);
            if (response.ServiceProviderResult.Success && response.Result != null)
            {
                result.Initialize(response.Result, _productResolver);
            }

            return result;
        }

        public MiniCartBaseJsonResult UpdateMiniCart(bool updateCart)
        {
            var response = _cartManager.GetCurrentCart(CurrentStorefront, CurrentVisitorContext, updateCart);
            var result = new MiniCartBaseJsonResult(response.ServiceProviderResult);
            if (response.ServiceProviderResult.Success && response.Result != null)
            {
                result.Initialize(response.ServiceProviderResult.Cart);
            }

            return result;
        }

        public CSCartBaseJsonResult UpdateLineItem(UpdateCartLineInputModel inputModel)
        {
            var response = _cartManager.ChangeLineQuantity(CurrentStorefront, CurrentVisitorContext, inputModel);
            var result = new CSCartBaseJsonResult(response.ServiceProviderResult);
            if (response.ServiceProviderResult.Success && response.Result != null)
            {
                result.Initialize(response.Result, _productResolver);

                if (response.Result.HasBasketErrors())
                {
                    // We clear the cart from the cache when basket errors are detected.  This stops the message from being displayed over and over as the
                    // cart will be retrieved again from CS and the pipelines will be executed.
                    _cartCacheService.Invalidate(CurrentVisitorContext.GetCustomerId());

                }
            }

            return result;
        }
    }
}