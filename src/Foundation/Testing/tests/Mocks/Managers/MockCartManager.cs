namespace Sitecore.Foundation.Testing.Mocks.Managers
{
    using System.Collections.Generic;
    using Sitecore.Commerce.Connect.CommerceServer.Orders.Models;
    using Sitecore.Commerce.Connect.CommerceServer.Orders.Pipelines;
    using Sitecore.Commerce.Entities;
    using Sitecore.Commerce.Entities.Carts;
    using Sitecore.Commerce.Services.Carts;
    using Sitecore.Foundation.CommerceServer.Interfaces;
    using Sitecore.Foundation.CommerceServer.Managers;
    using Sitecore.Foundation.CommerceServer.Models;
    using Sitecore.Foundation.CommerceServer.Models.InputModels;

    public class MockCartManager : ICartManager
    {
        private readonly IInventoryManager _inventoryManager;
        private readonly CartServiceProvider _cartServiceProvider;

        public MockCartManager(IInventoryManager inventoryManager, CartServiceProvider cartServiceProvider)
        {
            _inventoryManager = inventoryManager;
            _cartServiceProvider = cartServiceProvider;
        }

        public ManagerResponse<CartResult, CommerceCart> GetCurrentCart(CommerceStorefront storefront, IVisitorContext visitorContext, bool refresh = false)
        {
            return null;
        }

        public ManagerResponse<CartResult, CommerceCart> GetCurrentCart(CommerceStorefront storefront, string customerId, bool refresh = false)
        {
            return null;
        }

        public ManagerResponse<CartResult, bool> UpdateCartCurrency(CommerceStorefront storefront, IVisitorContext visitorContext, string currencyCode)
        {
            return null;
        }

        public ManagerResponse<CartResult, bool> AddLineItemsToCart(CommerceStorefront storefront, IVisitorContext visitorContext, IEnumerable<AddCartLineInputModel> inputModelList)
        {
            return null;
        }

        public ManagerResponse<CartResult, CommerceCart> RemoveLineItemFromCart(CommerceStorefront storefront, IVisitorContext visitorContext, string externalCartLineId)
        {
            return null;
        }

        public ManagerResponse<CartResult, CommerceCart> ChangeLineQuantity(CommerceStorefront storefront, IVisitorContext visitorContext, UpdateCartLineInputModel inputModel)
        {
            return null;
        }

        public ManagerResponse<AddPromoCodeResult, CommerceCart> AddPromoCodeToCart(CommerceStorefront storefront, IVisitorContext visitorContext, string promoCode)
        {
            return null;
        }

        public ManagerResponse<RemovePromoCodeResult, CommerceCart> RemovePromoCodeFromCart(CommerceStorefront storefront, IVisitorContext visitorContext, string promoCode)
        {
            return null;
        }

        public ManagerResponse<AddShippingInfoResult, CommerceCart> SetShippingMethods(CommerceStorefront storefront, IVisitorContext visitorContext, SetShippingMethodsInputModel inputModel)
        {
            return null;
        }

        public ManagerResponse<CartResult, CommerceCart> SetPaymentMethods(CommerceStorefront storefront, IVisitorContext visitorContext, PaymentInputModel inputModel)
        {
            return null;
        }

        public ManagerResponse<CartResult, CommerceCart> MergeCarts(CommerceStorefront storefront, IVisitorContext visitorContext, string anonymousVisitorId, Cart anonymousVisitorCart)
        {
            return null;
        }

        public ManagerResponse<CartResult, CommerceCart> AddPartyToCart(CommerceStorefront storefront, IVisitorContext visitorContext, CommerceCart cart, CommerceParty party)
        {
            return null;
        }

        public ManagerResponse<CartResult, CommerceCart> RemovePartiesFromCart(CommerceStorefront storefront, IVisitorContext visitorContext, CommerceCart cart, List<Party> parties)
        {
            return null;
        }

        public ManagerResponse<CartResult, CommerceCart> RemoveAllShippingParties(CommerceStorefront storefront, IVisitorContext visitorContext, CommerceCart cart)
        {
            return null;
        }

        public ManagerResponse<CartResult, CommerceCart> RemoveAllBillingParties(CommerceStorefront storefront, IVisitorContext visitorContext, CommerceCart cart)
        {
            return null;
        }

        public ManagerResponse<CartResult, CommerceCart> RemoveAllPaymentMethods(CommerceStorefront storefront, IVisitorContext visitorContext, CommerceCart cart)
        {
            return null;
        }

        public ManagerResponse<CartResult, CommerceCart> RemoveAllShippingMethods(CommerceStorefront storefront, IVisitorContext visitorContext, CommerceCart cart)
        {
            return null;
        }

        public ManagerResponse<CartResult, CommerceCart> AddPaymentInfoToCart(CommerceStorefront storefront, IVisitorContext visitorContext, CommerceCart cart, PaymentInfo info, CommerceParty party, bool refreshCart = false)
        {
            return null;
        }

        public ManagerResponse<CartResult, CommerceCart> UpdateCart(CommerceStorefront storefront, IVisitorContext visitorContext, CommerceCart cart, CommerceCart cartChanges)
        {
            return null;
        }

        public MockCartManager()
        {
        }
    }
}
