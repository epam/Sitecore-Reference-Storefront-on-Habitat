
using Sitecore.Foundation.CommerceServer.Interfaces;

namespace Sitecore.Feature.Cart.Models.JsonResults
{
    using Foundation.CommerceServer.Extensions;
    using Foundation.CommerceServer.Managers;
    using Sitecore.Commerce.Connect.CommerceServer.Orders.Models;
    using Sitecore.Commerce.Entities.Carts;
    using Sitecore.Commerce.Services;
    using Sitecore.Feature.Base.Models.JsonResults;
    using Sitecore.Foundation.CommerceServer.Utils;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Emits the Json result of a Cart request.
    /// </summary>
    public class CartBaseJsonResult : BaseJsonResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CartBaseJsonResult"/> class.
        /// </summary>
        public CartBaseJsonResult()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CartBaseJsonResult"/> class.
        /// </summary>
        /// <param name="result">The result.</param>
        public CartBaseJsonResult(ServiceProviderResult result)
            : base(result)
        {
        }

        /// <summary>
        /// Gets or sets a value indicating whether the request is in preview mode.
        /// </summary>
        public bool IsPreview { get; set; }

        /// <summary>
        /// Gets or sets the lines.
        /// </summary>
        /// <value>
        /// The lines.
        /// </value>
        public List<CartLineBaseJsonResult> Lines { get; set; }

        /// <summary>
        /// Gets or sets the list of cart adjustments.
        /// </summary>
        public List<CartAdjustmentBaseJsonResult> Adjustments { get; set; }

        /// <summary>
        /// Gets or sets the sub total.
        /// </summary>
        /// <value>
        /// The sub total.
        /// </value>
        public string Subtotal { get; set; }

        /// <summary>
        /// Gets or sets the tax total.
        /// </summary>
        /// <value>
        /// The tax total.
        /// </value>
        public string TaxTotal { get; set; }

        /// <summary>
        /// Gets or sets the total.
        /// </summary>
        /// <value>
        /// The total.
        /// </value>
        public string Total { get; set; }

        /// <summary>
        /// Gets or sets the total amount.
        /// </summary>
        /// <value>
        /// The total amount.
        /// </value>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Gets or sets the discount.
        /// </summary>
        /// <value>
        /// The discount.
        /// </value>
        public string Discount { get; set; }

        /// <summary>
        /// Gets or sets the shipping total.
        /// </summary>
        public string ShippingTotal { get; set; }

        /// <summary>
        /// Gets or sets the promo codes.
        /// </summary>
        /// <value>
        /// The promo codes.
        /// </value>
        public List<string> PromoCodes { get; set; }

        /// <summary>
        /// Initializes this object based on the data contained in the provided cart.
        /// </summary>
        /// <param name="cart">The cart used to initialize this object.</param>
        /// <param name="productResolver"></param>
        public virtual void Initialize(Cart cart, IProductResolver productResolver)
        {
            this.Lines = new List<CartLineBaseJsonResult>();
            this.Adjustments = new List<CartAdjustmentBaseJsonResult>();
            this.PromoCodes = new List<string>();
            var currencyCode = StorefrontManager.GetCustomerCurrency();

            this.Subtotal = 0.0M.ToCurrency(currencyCode);
            this.TaxTotal = 0.0M.ToCurrency(currencyCode);
            this.Total = 0.0M.ToCurrency(currencyCode);
            this.TotalAmount = 0.0M;
            this.Discount = 0.0M.ToCurrency(currencyCode);
            this.ShippingTotal = 0.0M.ToCurrency(currencyCode);

            if (cart == null)
            {
                return;
            }

            foreach (var line in (cart.Lines ?? Enumerable.Empty<CartLine>()))
            {
                var cartLine = ContextTypeLoader.CreateInstance<CartLineBaseJsonResult>(line, productResolver);
                this.Lines.Add(cartLine);
            }

            foreach (var adjustment in (cart.Adjustments ?? Enumerable.Empty<CartAdjustment>()))
            {
                this.Adjustments.Add(new CartAdjustmentBaseJsonResult(adjustment));
            }

            var commerceTotal = (CommerceTotal)cart.Total;

            this.Subtotal = commerceTotal.Subtotal.ToCurrency(currencyCode);
            this.TaxTotal = cart.Total.TaxTotal.Amount.ToCurrency(currencyCode);
            this.Total = cart.Total.Amount.ToCurrency(currencyCode);
            this.TotalAmount = cart.Total.Amount;
            this.Discount = commerceTotal.OrderLevelDiscountAmount.ToCurrency(currencyCode);
            this.ShippingTotal = commerceTotal.ShippingTotal.ToCurrency(currencyCode);
        }
    }
}