namespace Sitecore.Feature.Cart.Models.JsonResults
{
    using Foundation.CommerceServer.Extensions;
    using Foundation.CommerceServer.Managers;
    using Sitecore.Commerce.Connect.CommerceServer.Orders.Models;
    using Sitecore.Commerce.Entities.Carts;
    using Sitecore.Commerce.Services;
    using Sitecore.Diagnostics;
    using Sitecore.Feature.Base.Models.JsonResults;

    /// <summary>
    /// Defines the MiniCartJsonResult class.
    /// </summary>
    public class MiniCartBaseJsonResult : BaseJsonResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MiniCartBaseJsonResult"/> class.
        /// </summary>
        public MiniCartBaseJsonResult()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MiniCartBaseJsonResult"/> class.
        /// </summary>
        /// <param name="result">The service provider result.</param>
        public MiniCartBaseJsonResult(ServiceProviderResult result)
            : base(result)
        {
        }

        /// <summary>
        /// Gets or sets the line item count.
        /// </summary>
        /// <value>
        /// The line item count.
        /// </value>
        public int LineItemCount { get; set; }

        /// <summary>
        /// Gets or sets the total.
        /// </summary>
        /// <value>
        /// The total.
        /// </value>
        public string Total { get; set; }

        /// <summary>
        /// Initializes the specified cart.
        /// </summary>
        /// <param name="cart">The cart.</param>
        public virtual void Initialize(Cart cart)
        {
            Assert.ArgumentNotNull(cart, "cart");

            this.LineItemCount = ((CommerceCart)cart).LineItemCount;
            this.Total = ((CommerceTotal)cart.Total).Subtotal.ToCurrency(StorefrontManager.GetCustomerCurrency());
        }
    }
}