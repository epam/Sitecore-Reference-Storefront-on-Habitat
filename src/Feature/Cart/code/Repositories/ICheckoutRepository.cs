using Sitecore.Commerce.Connect.CommerceServer.Orders.Models;
using Sitecore.Feature.Cart.Models.InputModels;
using Sitecore.Feature.Cart.Models.JsonResults;
using Sitecore.Foundation.CommerceServer.Models.InputModels;
using Sitecore.Security.Accounts;

namespace Sitecore.Feature.Cart.Repositories
{
    public interface ICheckoutRepository
    {
        AvailableStatesBaseJsonResult GetAvailableStates(GetAvailableStatesInputModel model);

        CheckoutDataBaseJsonResult GetCheckoutData(User user);

        CommerceCart GetCommerceCart();

        CommerceOrder GetCommerceOrder(string confirmationId);

        ShippingMethodsBaseJsonResult GetShippingMethods(GetShippingMethodsInputModel inputModel);

        CSCartBaseJsonResult SetPaymentMethods(PaymentInputModel inputModel);

        CSCartBaseJsonResult SetShippingMethods(SetShippingMethodsInputModel inputModel);

        SubmitOrderBaseJsonResult SubmitOrder(SubmitOrderInputModel inputModel);
    }
}