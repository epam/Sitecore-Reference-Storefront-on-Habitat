namespace Sitecore.Feature.Cart.Models
{
    using Sitecore.Commerce.Connect.CommerceServer.Orders.Models;
    using Sitecore.Mvc.Presentation;

    /// <summary>
    /// Defines the CartRenderingModel class.
    /// </summary>
    public class CartRenderingModel : Sitecore.Mvc.Presentation.RenderingModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CartRenderingModel"/> class.
        /// </summary>
        /// <param name="cart">The cart.</param>
        public CartRenderingModel(CommerceCart cart)
        {
            this.Cart = cart;
        }

        /// <summary>
        /// Gets or sets the cart.
        /// </summary>
        /// <value>
        /// The cart.
        /// </value>
        public CommerceCart Cart { get; set; }

        /// <summary>
        /// Gets the specified cart.
        /// </summary>
        /// <param name="cart">The cart.</param>
        /// <param name="rendering">The rendering.</param>
        /// <returns>Gets an instance of the CartRenderingModel with initialized Sitecore rendering.</returns>
        public static CartRenderingModel Get(CommerceCart cart, Rendering rendering)
        {
            CartRenderingModel model = new CartRenderingModel(cart);

            model.Initialize(rendering);

            return model;
        }
    }
}