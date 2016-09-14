using System.Linq;
using FluentAssertions;
using Sitecore.Data;
using Sitecore.Feature.Cart.Models.InputModels;
using Sitecore.Feature.Cart.Repositories;
using Sitecore.Foundation.CommerceServer.Interfaces;
using Sitecore.Foundation.CommerceServer.Models.InputModels;
using Sitecore.Security.Accounts;
using Xunit;

namespace Sitecore.Feature.Cart.Tests.Repositories
{
    public class CheckoutRepositoryTests
    {
        private readonly Mocks _mocks;
        private readonly ICheckoutRepository _checkoutRepository;
        private readonly IVisitorContext _visitorContext;

        public CheckoutRepositoryTests()
        {
            _mocks = new Mocks();
            _checkoutRepository = new CheckoutRepository(
                _mocks.AccountManager,
                _mocks.ContactFactory,
                _mocks.CartManager,
                _mocks.OrderManager,
                _mocks.PaymentManager,
                _mocks.ShippingManager,
                _mocks.ProductResolver);
        }

        [Theory]
        [AutoCartDbData]
        public void GetAvailableStates_ShoudReturn_SuccessEqualsTrue_WhenServiceProviderResultIsSuccess(Database db)
        {
            _mocks.MockContexts(db);

            var result = _checkoutRepository.GetAvailableStates(new GetAvailableStatesInputModel { CountryCode = "test" });

            result.Success.Should().BeTrue();
        }

        [Theory]
        [AutoCartDbData]
        public void GetAvailableStates_ShoudReturn_SuccessEqualsFalse_WhenServiceProviderResultIsNotSuccess(Database db)
        {
            _mocks.MockContexts(db);

            var result = _checkoutRepository.GetAvailableStates(new GetAvailableStatesInputModel());

            result.Success.Should().BeFalse();
        }

        [Theory]
        [AutoCartDbData]
        public void GetAvailableStates_ShoudReturn_AvailableState(Database db)
        {
            _mocks.MockContexts(db);

            var result = _checkoutRepository.GetAvailableStates(new GetAvailableStatesInputModel { CountryCode = "test" });

            result.States.Should().NotBeNull();
        }

        [Theory]
        [AutoCartDbData]
        public void GetCheckoutData_ShoudReturn_SuccessEqualsTrue_WhenServiceProviderResultIsSuccess(Database db)
        {
            _mocks.MockContexts(db);

            var result = _checkoutRepository.GetCheckoutData(User.FromName("test", true));

            result.Success.Should().BeTrue();
        }

        [Theory]
        [AutoCartDbData]
        public void GetCheckoutData_ShoudReturn_OrderShippingOptions(Database db)
        {
            _mocks.MockContexts(db);

            var result = _checkoutRepository.GetCheckoutData(User.FromName("test", true));

            result.OrderShippingOptions.Should().NotBeNull();
        }

        [Theory]
        [AutoCartDbData]
        public void GetCheckoutData_ShoudReturn_LineShippingOptions(Database db)
        {
            _mocks.MockContexts(db);

            var result = _checkoutRepository.GetCheckoutData(User.FromName("test", true));

            result.LineShippingOptions.Should().NotBeNull();
        }

        [Theory]
        [AutoCartDbData]
        public void GetCheckoutData_ShoudReturn_ShipToStoreDeliveryMethod(Database db)
        {
            _mocks.MockContexts(db);

            var result = _checkoutRepository.GetCheckoutData(User.FromName("test", true));

            result.ShipToStoreDeliveryMethod.Should().NotBeNull();
        }

        [Theory]
        [AutoCartDbData]
        public void GetCheckoutData_ShoudReturn_Countries(Database db)
        {
            _mocks.MockContexts(db);

            var result = _checkoutRepository.GetCheckoutData(User.FromName("test", true));

            result.Countries.Should().NotBeNull();
        }

        [Theory]
        [AutoCartDbData]
        public void GetCheckoutData_ShoudReturn_PaymentOptions(Database db)
        {
            _mocks.MockContexts(db);

            var result = _checkoutRepository.GetCheckoutData(User.FromName("test", true));

            result.PaymentOptions.Should().NotBeNull();
        }

        [Theory]
        [AutoCartDbData]
        public void GetCheckoutData_ShoudReturn_PaymentMethods(Database db)
        {
            _mocks.MockContexts(db);

            var result = _checkoutRepository.GetCheckoutData(User.FromName("test", true));

            result.PaymentMethods.Should().NotBeNull();
        }

        [Theory]
        [AutoCartDbData]
        public void GetCheckoutData_ShoudReturn_UserAddresses(Database db)
        {
            _mocks.MockContexts(db);

            var result = _checkoutRepository.GetCheckoutData(User.FromName("test", true));

            result.UserAddresses.Should().NotBeNull();
        }

        [Theory]
        [AutoCartDbData]
        public void GetCheckoutData_ShoudReturn_UserEmail(Database db)
        {
            _mocks.MockContexts(db);

            var result = _checkoutRepository.GetCheckoutData(User.FromName("test", true));

            result.UserEmail.Should().NotBeNull();
        }

        [Theory]
        [AutoCartDbData]
        public void GetCommerceCart_ShoudReturn_CommerceCart(Database db)
        {
            _mocks.MockContexts(db);

            var result = _checkoutRepository.GetCommerceCart();

            result.Should().NotBeNull();
        }

        [Theory]
        [AutoCartDbData]
        public void GetCommerceCart_ShoudReturn_EmptyParties(Database db)
        {
            _mocks.MockContexts(db);

            var result = _checkoutRepository.GetCommerceCart();

            result.Parties.Count.Should().Be(0);
        }

        [Theory]
        [AutoCartDbData]
        public void GetCommerceCart_ShoudReturn_EmptyPayments(Database db)
        {
            _mocks.MockContexts(db);

            var result = _checkoutRepository.GetCommerceCart();

            result.Payment.Count.Should().Be(0);
        }

        [Theory]
        [AutoCartDbData]
        public void GetCommerceCart_ShoudReturn_EmptyShipment(Database db)
        {
            _mocks.MockContexts(db);

            var result = _checkoutRepository.GetCommerceCart();

            result.Shipping.Count.Should().Be(0);
        }

        [Theory]
        [AutoCartDbData]
        public void GetCommerceOrder_ShoudReturn_CommerceOrder(Database db)
        {
            _mocks.MockContexts(db);

            var result = _checkoutRepository.GetCommerceOrder("test");

            result.Should().NotBeNull();
        }

        [Theory]
        [AutoCartDbData]
        public void GetShippingMethods_ShoudReturn_SuccessEqualsTrue_WhenServiceProviderResultIsSuccess(Database db)
        {
            _mocks.MockContexts(db);

            var result = _checkoutRepository.GetShippingMethods(new GetShippingMethodsInputModel());

            result.Success.Should().BeTrue();
        }

        [Theory]
        [AutoCartDbData]
        public void GetShippingMethods_ShoudReturn_SuccessEqualsFalse_WhenServiceProviderResultIsNotSuccess(Database db)
        {
            _mocks.MockContexts(db);

            var result = _checkoutRepository.GetShippingMethods(null);

            result.Success.Should().BeTrue();
        }

        [Theory]
        [AutoCartDbData]
        public void GetShippingMethods_ShoudReturn_ShippingMethods(Database db)
        {
            _mocks.MockContexts(db);

            var result = _checkoutRepository.GetShippingMethods(new GetShippingMethodsInputModel());

            result.ShippingMethods.Count().Should().Be(Models.ShippingMethods.Count);
        }

        [Theory]
        [AutoCartDbData]
        public void SetPaymentMethods_ShoudReturn_CartResult(Database db)
        {
            _mocks.MockContexts(db);

            var result = _checkoutRepository.SetPaymentMethods(new PaymentInputModel());

            result.Should().NotBeNull();
        }

        [Theory]
        [AutoCartDbData]
        public void SetPaymentMethods_ShoudReturn_SuccessEqualsTrue_WhenServiceProviderResultIsSuccess(Database db)
        {
            _mocks.MockContexts(db);

            var result = _checkoutRepository.SetPaymentMethods(new PaymentInputModel());

            result.Success.Should().BeTrue();
        }

        [Theory]
        [AutoCartDbData]
        public void SetPaymentMethods_ShoudReturn_SuccessEqualsFalse_WhenServiceProviderResultIsNotSuccess(Database db)
        {
            _mocks.MockContexts(db);

            var result = _checkoutRepository.SetPaymentMethods(null);

            result.Success.Should().BeFalse();
        }

        [Theory]
        [AutoCartDbData]
        public void SetShippingMethods_ShoudReturn_CartResult(Database db)
        {
            _mocks.MockContexts(db);

            var result = _checkoutRepository.SetShippingMethods(new SetShippingMethodsInputModel());

            result.Should().NotBeNull();
        }

        [Theory]
        [AutoCartDbData]
        public void SetShippingMethods_ShoudReturn_SuccessEqualsTrue_WhenServiceProviderResultIsSuccess(Database db)
        {
            _mocks.MockContexts(db);

            var result = _checkoutRepository.SetShippingMethods(new SetShippingMethodsInputModel());

            result.Success.Should().BeTrue();
        }

        [Theory]
        [AutoCartDbData]
        public void SetShippingMethods_ShoudReturn_SuccessEqualsFalse_WhenServiceProviderResultIsNotSuccess(Database db)
        {
            _mocks.MockContexts(db);

            var result = _checkoutRepository.SetShippingMethods(null);

            result.Success.Should().BeFalse();
        }

        //***/*//*/*

        [Theory]
        [AutoCartDbData]
        public void SubmitOrder_ShoudReturn_SubmitResult(Database db)
        {
            _mocks.MockContexts(db);

            var result = _checkoutRepository.SubmitOrder(new SubmitOrderInputModel());

            result.Should().NotBeNull();
        }

        [Theory]
        [AutoCartDbData]
        public void SubmitOrder_ShoudReturn_SuccessEqualsTrue_WhenServiceProviderResultIsSuccess(Database db)
        {
            _mocks.MockContexts(db);

            var result = _checkoutRepository.SubmitOrder(new SubmitOrderInputModel());

            result.Success.Should().BeTrue();
        }

        [Theory]
        [AutoCartDbData]
        public void SubmitOrder_ShoudReturn_SuccessEqualsFalse_WhenServiceProviderResultIsNotSuccess(Database db)
        {
            _mocks.MockContexts(db);

            var result = _checkoutRepository.SubmitOrder(null);

            result.Success.Should().BeFalse();
        }
    }
}
