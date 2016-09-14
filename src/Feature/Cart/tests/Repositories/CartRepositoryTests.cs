using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentAssertions;
using NSubstitute;
using Sitecore.Commerce.Connect.CommerceServer.Orders.Models;
using Sitecore.Data;
using Sitecore.Feature.Cart.Models.InputModels;
using Sitecore.Feature.Cart.Repositories;
using Sitecore.Foundation.CommerceServer.Extensions;
using Sitecore.Foundation.CommerceServer.Interfaces;
using Sitecore.Foundation.CommerceServer.Managers;
using Sitecore.Foundation.CommerceServer.Models.InputModels;
using Sitecore.Foundation.Testing.Attributes;
using Xunit;

namespace Sitecore.Feature.Cart.Tests.Repositories
{
    public class CartRepositoryTests
    {
        private readonly ICartRepository _cartRepository;
        private readonly IVisitorContext _visitorContextMock;
        private readonly Mocks _mocks;

        public CartRepositoryTests()
        {
            _mocks = new Mocks();
            var cartManagerMock = _mocks.CartManager;
            _visitorContextMock = _mocks.VisitorContext;

            _cartRepository = new CartRepository(
                _mocks.AccountManager,
                _mocks.ContactFactory,
                cartManagerMock,
                _mocks.CartCacheService,
                _mocks.ProductResolver);
        }

        [Theory]
        [AutoDbData]
        public void AddCartLine_ShoudReturn_SuccessEqualTrue_WhenServiceProviderResultIsSuccess(Database db)
        {
            _mocks.MockContexts(db);

            var result = _cartRepository.AddCartLine(new AddCartLineInputModel());

            result.Success.Should().BeTrue();
        }

        [Theory]
        [AutoDbData]
        public void AddCartLine_ShoudReturn_SuccessEqualFalse_WhenServiceProviderResultIsNotSuccess(Database db)
        {
            _mocks.MockContexts(db);

            var result = _cartRepository.AddCartLine(null);

            result.Success.Should().BeFalse();
        }

        [Theory]
        [AutoDbData]
        public void AddCartLines_ShoudReturn_SuccessEqualsTrue_WhenServiceProviderResultIsSuccess(Database db)
        {
            _mocks.MockContexts(db);

            var result = _cartRepository.AddCartLines(new List<AddCartLineInputModel> { new AddCartLineInputModel() });

            result.Success.Should().BeTrue();
        }

        [Theory]
        [AutoDbData]
        public void AddCartLines_ShoudReturn_SuccessEqualsFalse_WhenServiceProviderResultIsNotSuccess(Database db)
        {
            _mocks.MockContexts(db);

            var result = _cartRepository
                .AddCartLines(new List<AddCartLineInputModel> { null });

            result.Success.Should().BeFalse();
        }

        [Theory]
        [AutoCartDbData]
        public void ApplyDiscount_ShoudReturn_ModelWithIncrementedPromoCodesCount(Database db)
        {
            _mocks.MockContexts(db);
            var expectedCount = Models.CommerceCartStub.OrderForms[0].PromoCodes.Count + 1;
            var model = new DiscountInputModel
            {
                PromoCode = Models.TestPromoCode
            };
            Models.AddPromoToCommerceCart(model.PromoCode);

            var result = _cartRepository.ApplyDiscount(model);
            Models.RefreshCommerceCart();

            result.PromoCodes.Count.Should().Be(expectedCount);
        }

        [Theory]
        [AutoCartDbData]
        public void ApplyDiscount_ShoudReturn_ModelThatContainingPassedPromoCode(Database db)
        {
            _mocks.MockContexts(db);
            var model = new DiscountInputModel
            {
                PromoCode = Models.TestPromoCode
            };
            Models.AddPromoToCommerceCart(model.PromoCode);

            var result = _cartRepository.ApplyDiscount(model);
            Models.RefreshCommerceCart();

            result.PromoCodes.Any(pc => pc.Equals(model.PromoCode)).Should().BeTrue();
        }

        [Theory]
        [AutoCartDbData]
        public void ApplyDiscount_ShoudReturn_SuccessEqualsTrue_WhenServiceProviderResultIsSuccess(Database db)
        {
            _mocks.MockContexts(db);

            var result = _cartRepository.ApplyDiscount(new DiscountInputModel { PromoCode = "test" });

            result.Success.Should().BeTrue();
        }

        [Theory]
        [AutoCartDbData]
        public void ApplyDiscount_ShoudReturn_SuccessEqualsFalse_WhenServiceProviderResultIsNotSuccess(Database db)
        {
            _mocks.MockContexts(db);

            var result = _cartRepository.ApplyDiscount(new DiscountInputModel { PromoCode = string.Empty });

            result.Success.Should().BeFalse();
        }

        [Theory]
        [AutoCartDbData]
        public void DeleteLineItem_ShoudReturn_SeuccessEqualTrue_WhenServiceProviderResultIsSuccess(Database db)
        {
            _mocks.MockContexts(db);

            var result = _cartRepository.DeleteLineItem(new DeleteCartLineInputModel { ExternalCartLineId = "test" });

            result.Success.Should().BeTrue();
        }

        [Theory]
        [AutoCartDbData]
        public void DeleteLineItem_ShoudReturn_SeuccessEqualFalse_WhenServiceProviderResultIsNotSuccess(Database db)
        {
            _mocks.MockContexts(db);

            var result = _cartRepository.DeleteLineItem(new DeleteCartLineInputModel());

            result.Success.Should().BeFalse();
        }

        [Theory]
        [AutoCartDbData]
        public void DeleteLineItem_ShoudReturn_ModelWithDecrementedCartLinesCount(Database db)
        {
            _mocks.MockContexts(db);
            var expectedCount = Models.CommerceCartStub.OrderForms[0].CartLines.Count - 1;
            Models.RemoveCartLineFromCommerceCart();

            var result = _cartRepository.DeleteLineItem(new DeleteCartLineInputModel { ExternalCartLineId = "test" });
            Models.RefreshCommerceCart();

            result.Lines.Count.Should().Be(expectedCount);
        }

        [Theory]
        [AutoCartDbData]
        public void GetCurrentCart_ShoudReturn_SuccessEqualTrue_WhenServiceProviderResultIsSuccessAndCartExistInCache(Database db)
        {
            _mocks.MockContexts(db);

            var result = _cartRepository.GetCurrentCart();

            result.Success.Should().BeTrue();
        }

        [Theory]
        [AutoCartDbData]
        public void GetCurrentCart_ShoudReturn_SuccessEqualsTrue_WhenServiceProviderResultIsSuccessAndCartNotExistInCacheAndCookieExist(Database db)
        {
            _mocks.MockContexts(db);
            MockCookie(Models.UserWithEmptyCache);
            _visitorContextMock.GetCustomerId().Returns(Models.UserWithEmptyCache);

            var result = _cartRepository.GetCurrentCart();

            result.Success.Should().BeTrue();
        }

        [Theory]
        [AutoCartDbData]
        public void GetCurrentCart_ShoudReturn_SuccessEqualsFalse_WhenServiceProviderResultIsNotSuccessAndCartNotExistInCacheAndCookieExist(Database db)
        {
            _mocks.MockContexts(db);
            MockCookie(Models.UserWithEmptyCache);
            _visitorContextMock.GetCustomerId().Returns(Models.UserWithEmptyCache);
            _mocks.MockCartManagerGetCurrentCartToFalseSuccess();

            var result = _cartRepository.GetCurrentCart();

            result.Success.Should().BeFalse();
        }

        [Theory]
        [AutoCartDbData]
        public void RemoveDiscount_ShoudReturn_SuccessEqualsTrue_WhenServiceProviderResultIsSuccess(Database db)
        {
            _mocks.MockContexts(db);

            var result = _cartRepository.RemoveDiscount(new DiscountInputModel { PromoCode = "test" });

            result.Success.Should().BeTrue();
        }

        [Theory]
        [AutoCartDbData]
        public void RemoveDiscount_ShoudReturn_SuccessEqualsFale_WhenServiceProviderResultIsNotSuccess(Database db)
        {
            _mocks.MockContexts(db);

            var result = _cartRepository.RemoveDiscount(new DiscountInputModel { PromoCode = string.Empty });

            result.Success.Should().BeFalse();
        }

        [Theory]
        [AutoCartDbData]
        public void RemoveDiscount_ShoudReturn_CartResult_WithoutRemovedCode(Database db)
        {
            _mocks.MockContexts(db);

            var result = _cartRepository.RemoveDiscount(new DiscountInputModel { PromoCode = Models.TestPromoCode });
            Models.RefreshCommerceCart();

            result.PromoCodes.Any(pm => pm.Equals(Models.TestPromoCode)).Should().BeFalse();
        }

        [Theory]
        [AutoCartDbData]
        public void UpdateMiniCart_ShoudReturn_SuccessEqualsTrue_WhenServiceProviderResultIsSuccess(Database db)
        {
            _mocks.MockContexts(db);

            var result = _cartRepository.UpdateMiniCart(true);

            result.Success.Should().BeTrue();
        }

        [Theory]
        [AutoCartDbData]
        public void UpdateMiniCart_ShoudReturn_SuccessEqualsFalse_WhenServiceProviderResultIsNotSuccess(Database db)
        {
            _mocks.MockContexts(db);
            _mocks.MockCartManagerGetCurrentCartToFalseSuccess();

            var result = _cartRepository.UpdateMiniCart(true);

            result.Success.Should().BeFalse();
        }

        [Theory]
        [AutoCartDbData]
        public void UpdateMiniCart_ShoudReturn_TotalEqualsCartTotal(Database db)
        {
            _mocks.MockContexts(db);
            var expectedResult = ((CommerceTotal)Models.CommerceCartStub.Total)
                .Subtotal.ToCurrency(StorefrontManager.GetCustomerCurrency());

            var result = _cartRepository.UpdateMiniCart(true);

            result.Total.Should().Be(expectedResult);
        }

        [Theory]
        [AutoCartDbData]
        public void UpdateMiniCart_ShoudReturn_LineItemCountEqualToCartLinesCount(Database db)
        {
            _mocks.MockContexts(db);

            var result = _cartRepository.UpdateMiniCart(true);

            result.LineItemCount.Should().Be(Models.CommerceCartStub.LineItemCount);
        }

        [Theory]
        [AutoCartDbData]
        public void UpdateLineItem_ShoudReturn_SuccessEqualsTrue_WhenServiceProviderResultIsSuccess(Database db)
        {
            _mocks.MockContexts(db);

            var result = _cartRepository.UpdateLineItem(new UpdateCartLineInputModel());

            result.Success.Should().BeTrue();
        }

        [Theory]
        [AutoCartDbData]
        public void UpdateLineItem_ShoudReturn_SuccessEqualsFalse_WhenServiceProviderResultIsNotSuccess(Database db)
        {
            _mocks.MockContexts(db);

            var result = _cartRepository.UpdateLineItem(null);

            result.Success.Should().BeFalse();
        }

        private void MockCookie(string userId)
        {
            HttpContext.Current.Request.Cookies.Add(new HttpCookie("_minicart")
            {
                Values =
                {
                    { "VisitorId", userId }
                }
            });
        }
    }
}