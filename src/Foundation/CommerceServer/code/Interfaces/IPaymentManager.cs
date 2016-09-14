using System.Collections.Generic;
using Sitecore.Commerce.Entities.Payments;
using Sitecore.Commerce.Services.Payments;
using Sitecore.Foundation.CommerceServer.Models;

namespace Sitecore.Foundation.CommerceServer.Interfaces
{
    public interface IPaymentManager
    {
        /// <summary>
        /// Gets the payment options.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="visitorContext">The visitor context.</param>
        /// <returns>
        /// The manager response where the payment option list is returned in the Result.
        /// </returns>
        ManagerResponse<GetPaymentOptionsResult, IEnumerable<PaymentOption>> GetPaymentOptions([NotNull] CommerceStorefront storefront, [NotNull] IVisitorContext visitorContext);

        /// <summary>
        /// Gets the payment methods.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="visitorContext">The visitor context.</param>
        /// <param name="paymentOption">The payment option.</param>
        /// <returns>The manager response where the payment method list is returned in the Result.</returns>
        ManagerResponse<GetPaymentMethodsResult, IEnumerable<PaymentMethod>> GetPaymentMethods([NotNull] CommerceStorefront storefront, [NotNull] IVisitorContext visitorContext, PaymentOption paymentOption);
    }
}