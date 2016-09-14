namespace Sitecore.Foundation.CommerceServer.Managers
{
    using System.Collections.Generic;
    using System.Linq;
    using Sitecore.Commerce.Connect.CommerceServer.Orders.Pipelines;
    using Sitecore.Commerce.Entities.Payments;
    using Sitecore.Commerce.Services.Payments;
    using Sitecore.Diagnostics;
    using Sitecore.Foundation.CommerceServer.Interfaces;
    using Sitecore.Foundation.CommerceServer.Models;

    /// <summary>
    /// Defines the PaymentManager class.
    /// </summary>
    public class PaymentManager : IPaymentManager
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentManager" /> class.
        /// </summary>
        /// <param name="paymentServiceProvider">The payment service provider.</param>
        /// <param name="cartManager">The cart manager.</param>
        public PaymentManager(
            [NotNull] PaymentServiceProvider paymentServiceProvider,
            [NotNull] ICartManager cartManager)
        {
            Assert.ArgumentNotNull(paymentServiceProvider, "paymentServiceProvider");

            this._paymentServiceProvider = paymentServiceProvider;
            this._cartManager = cartManager;
        }

        /// <summary>
        /// Gets or sets the payment service provider.
        /// </summary>
        /// <value>
        /// The payment service provider.
        /// </value>
        private readonly PaymentServiceProvider _paymentServiceProvider;

        /// <summary>
        /// Gets or sets the cart manager.
        /// </summary>
        /// <value>
        /// The cart manager.
        /// </value>
        private readonly ICartManager _cartManager;

        /// <summary>
        /// Gets the payment options.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="visitorContext">The visitor context.</param>
        /// <returns>
        /// The manager response where the payment option list is returned in the Result.
        /// </returns>
        public ManagerResponse<GetPaymentOptionsResult, IEnumerable<PaymentOption>> GetPaymentOptions([NotNull] CommerceStorefront storefront, [NotNull] IVisitorContext visitorContext)
        {
            Assert.ArgumentNotNull(storefront, "storefront");
            Assert.ArgumentNotNull(visitorContext, "visitorContext");

            var result = new GetPaymentOptionsResult { Success = false };
            var cartResult = this._cartManager.GetCurrentCart(storefront, visitorContext);
            if (!cartResult.ServiceProviderResult.Success || cartResult.Result == null)
            {
                result.SystemMessages.ToList().AddRange(cartResult.ServiceProviderResult.SystemMessages);
                return new ManagerResponse<GetPaymentOptionsResult, IEnumerable<PaymentOption>>(result, null);
            }

            // TODO: CS Connect does not sport passing in a Cart.
            // var request = new GetPaymentOptionsRequest(storefront.ShopName, cart);
            var request = new GetPaymentOptionsRequest(storefront.ShopName);
            result = this._paymentServiceProvider.GetPaymentOptions(request);

            //Helpers.LogSystemMessages(result.SystemMessages, result);
            return new ManagerResponse<GetPaymentOptionsResult, IEnumerable<PaymentOption>>(result, result.PaymentOptions.ToList());
        }

        /// <summary>
        /// Gets the payment methods.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="visitorContext">The visitor context.</param>
        /// <param name="paymentOption">The payment option.</param>
        /// <returns>The manager response where the payment method list is returned in the Result.</returns>
        public ManagerResponse<GetPaymentMethodsResult, IEnumerable<PaymentMethod>> GetPaymentMethods([NotNull] CommerceStorefront storefront, [NotNull] IVisitorContext visitorContext, PaymentOption paymentOption)
        {
            Assert.ArgumentNotNull(storefront, "storefront");
            Assert.ArgumentNotNull(visitorContext, "visitorContext");
            Assert.ArgumentNotNull(paymentOption, "paymentOption");

            // TODO: Remove hard coded language - will be fixed in connect.
            var request = new CommerceGetPaymentMethodsRequest("en-us", paymentOption);
            var result = this._paymentServiceProvider.GetPaymentMethods(request);

            if (!result.Success)
            {
                //Helpers.LogSystemMessages(result.SystemMessages, result);
            }

            return new ManagerResponse<GetPaymentMethodsResult, IEnumerable<PaymentMethod>>(result, result.PaymentMethods.ToList());
        }
    }
}

