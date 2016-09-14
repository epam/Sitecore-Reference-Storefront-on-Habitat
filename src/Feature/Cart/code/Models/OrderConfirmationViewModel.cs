namespace Sitecore.Feature.Cart.Models
{
    using Sitecore.Commerce.Connect.CommerceServer.Orders.Models;
    using Sitecore.Foundation.CommerceServer.Managers;
    using Sitecore.Mvc.Presentation;

    /// <summary>
    /// Defines the OrderConfirmationViewModel class.
    /// </summary>
    public class OrderConfirmationViewModel : RenderingModel
    {
        /// <summary>
        /// Gets or sets the confirmation identifier.
        /// </summary>
        /// <value>
        /// The confirmation identifier.
        /// </value>
        public string ConfirmationId { get; set; }

        /// <summary>
        /// Gets or sets the order status.
        /// </summary>
        /// <value>
        /// The order status.
        /// </value>
        public string OrderStatus { get; set; }

        /// <summary>
        /// Initializes the specified renderings.
        /// </summary>
        /// <param name="renderings">The renderings.</param>
        /// <param name="confirmationId">The confirmation identifier.</param>
        /// <param name="order">The order.</param>
        public void Initialize(Rendering renderings, string confirmationId, CommerceOrder order)
        {
            base.Initialize(renderings);

            this.ConfirmationId = confirmationId;
            this.OrderStatus = StorefrontManager.GetOrderStatusName(order.Status);
        }
    }
}
