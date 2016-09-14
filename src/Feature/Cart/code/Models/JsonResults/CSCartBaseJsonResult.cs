using Sitecore.Foundation.CommerceServer.Interfaces;

namespace Sitecore.Feature.Cart.Models.JsonResults
{
    using System;
    using System.Linq;
    using Foundation.CommerceServer.Extensions;
    using Foundation.CommerceServer.Managers;
    using Sitecore.Commerce.Connect.CommerceServer.Orders.Models;
    using Sitecore.Commerce.Services;

    /// <summary>
    /// Defines the CSCartBaseJsonResult class.
    /// </summary>
    public class CSCartBaseJsonResult : CartBaseJsonResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CSCartBaseJsonResult"/> class.
        /// </summary>
        public CSCartBaseJsonResult()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CSCartBaseJsonResult"/> class.
        /// </summary>
        /// <param name="result">The result.</param>
        public CSCartBaseJsonResult(ServiceProviderResult result)
            : base(result)
        {
        }

        /// <summary>
        /// Initializes this object based on the data contained in the provided cart.
        /// </summary>
        /// <param name="cart">The cart used to initialize this object.</param>
        public override void Initialize(Commerce.Entities.Carts.Cart cart, IProductResolver productResolver)
        {
            base.Initialize(cart, productResolver);

            CommerceCart commerceCart = cart as CommerceCart;
            if (commerceCart == null)
            {
                return;
            }

            if (commerceCart.OrderForms.Count > 0)
            {
                foreach (var promoCode in (commerceCart.OrderForms[0].PromoCodes ?? Enumerable.Empty<String>()))
                {
                    this.PromoCodes.Add(promoCode);
                }
            }

            decimal totalSavings = 0;
            foreach (var lineitem in cart.Lines)
            {
                totalSavings += ((CommerceTotal)lineitem.Total).LineItemDiscountAmount;
            }

            this.Discount = totalSavings.ToCurrency(StorefrontManager.GetCustomerCurrency());
        }
    }
}