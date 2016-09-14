namespace Sitecore.Foundation.CommerceServer.Interfaces
{
    using System.Collections.Generic;
    using Sitecore.Commerce.Connect.CommerceServer.Orders.Models;
    using Sitecore.Commerce.Connect.CommerceServer.Orders.Pipelines;
    using Sitecore.Commerce.Entities;
    using Sitecore.Commerce.Entities.Carts;
    using Sitecore.Commerce.Services.Carts;
    using Sitecore.Foundation.CommerceServer.Managers;
    using Sitecore.Foundation.CommerceServer.Models;
    using Sitecore.Foundation.CommerceServer.Models.InputModels;

    public interface ICartManager
    {
        /// <summary>
        /// Gets or sets the inventory manager.
        /// </summary>
        /// <value>
        /// The inventory manager.
        /// </value>
        //IInventoryManager InventoryManager { get; }

        /// <summary>
        /// Gets or sets the cart service provider.
        /// </summary>
        /// <value>
        /// The cart service provider.
        /// </value>
        //CartServiceProvider CartServiceProvider { get; }

        /// <summary>
        /// Returns the current user cart.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="visitorContext">The visitor context.</param>
        /// <param name="refresh">if set to <c>true</c> [refresh].</param>
        /// <returns>
        /// The manager response where the modified CommerceCart is returned in the Result.
        /// </returns>
        ManagerResponse<CartResult, CommerceCart> GetCurrentCart([NotNull] CommerceStorefront storefront, [NotNull] IVisitorContext visitorContext, bool refresh = false);

        /// <summary>
        /// Returns the current user cart.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="customerId">The customer identifier.</param>
        /// <param name="refresh">if set to <c>true</c> [refresh].</param>
        /// <returns>
        /// The manager response where the modified CommerceCart is returned in the Result.
        /// </returns>
        ManagerResponse<CartResult, CommerceCart> GetCurrentCart([NotNull] CommerceStorefront storefront, [NotNull] string customerId, bool refresh = false);

        /// <summary>
        /// Updates the cart currency.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="visitorContext">The visitor context.</param>
        /// <param name="currencyCode">The currency code.</param>
        /// <returns>
        /// The manager response.
        /// </returns>
        ManagerResponse<CartResult, bool> UpdateCartCurrency([NotNull] CommerceStorefront storefront, [NotNull] IVisitorContext visitorContext, [NotNull] string currencyCode);

        /// <summary>
        /// Adds the line item to cart.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="visitorContext">The visitor context.</param>
        /// <param name="inputModelList">The input model.</param>
        /// <returns>
        /// The manager response where the result is retuned indicating the success or failure of the operation.
        /// </returns>
        ManagerResponse<CartResult, bool> AddLineItemsToCart([NotNull] CommerceStorefront storefront, [NotNull] IVisitorContext visitorContext, IEnumerable<AddCartLineInputModel> inputModelList);

        /// <summary>
        /// Removes the line item from cart.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="visitorContext">The visitor context.</param>
        /// <param name="externalCartLineId">The external cart line identifier.</param>
        /// <returns>
        /// The manager response where the modified CommerceCart is returned in the Result.
        /// </returns>
        ManagerResponse<CartResult, CommerceCart> RemoveLineItemFromCart([NotNull] CommerceStorefront storefront, [NotNull] IVisitorContext visitorContext, [NotNull] string externalCartLineId);

        /// <summary>
        /// Changes the line quantity.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="visitorContext">The visitor context.</param>
        /// <param name="inputModel">The input model.</param>
        /// <returns>
        /// The manager response where the modified CommerceCart is returned in the Result.
        /// </returns>
        ManagerResponse<CartResult, CommerceCart> ChangeLineQuantity([NotNull] CommerceStorefront storefront, [NotNull] IVisitorContext visitorContext, [NotNull] UpdateCartLineInputModel inputModel);

        /// <summary>
        /// Adds the promo code to cart.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="visitorContext">The visitor context.</param>
        /// <param name="promoCode">The promo code.</param>
        /// <returns>
        /// The manager response where the modified CommerceCart is returned in the Result.
        /// </returns>
        ManagerResponse<AddPromoCodeResult, CommerceCart> AddPromoCodeToCart([NotNull] CommerceStorefront storefront, [NotNull] IVisitorContext visitorContext, string promoCode);

        /// <summary>
        /// Removes the promo code from cart.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="visitorContext">The visitor context.</param>
        /// <param name="promoCode">The promo code.</param>
        /// <returns>
        /// The manager response where the modified CommerceCart is returned in the Result.
        /// </returns>
        ManagerResponse<RemovePromoCodeResult, CommerceCart> RemovePromoCodeFromCart([NotNull] CommerceStorefront storefront, [NotNull] IVisitorContext visitorContext, string promoCode);

        /// <summary>
        /// Sets the shipping methods.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="visitorContext">The visitor context.</param>
        /// <param name="inputModel">The input model.</param>
        /// <returns>
        /// The manager response where the modified CommerceCart is returned in the Result.
        /// </returns>
        ManagerResponse<AddShippingInfoResult, CommerceCart> SetShippingMethods([NotNull] CommerceStorefront storefront, [NotNull] IVisitorContext visitorContext, [NotNull] SetShippingMethodsInputModel inputModel);

        /// <summary>
        /// Sets the payment methods.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="visitorContext">The visitor context.</param>
        /// <param name="inputModel">The input model.</param>
        /// <returns>The manager response with a cart in the result.</returns>
        ManagerResponse<CartResult, CommerceCart> SetPaymentMethods([NotNull] CommerceStorefront storefront, [NotNull] IVisitorContext visitorContext, [NotNull] PaymentInputModel inputModel);

        /// <summary>
        /// Merges the carts.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="visitorContext">The visitor context.</param>
        /// <param name="anonymousVisitorId">The anonymous visitor identifier.</param>
        /// <param name="anonymousVisitorCart">The anonymous visitor cart.</param>
        /// <returns>
        /// The manager response where the merged cart is returned in the result.
        /// </returns>
        ManagerResponse<CartResult, CommerceCart> MergeCarts([NotNull] CommerceStorefront storefront, [NotNull] IVisitorContext visitorContext, string anonymousVisitorId, Cart anonymousVisitorCart);

        /// <summary>
        /// Adds a party to a cart
        /// </summary>
        /// <param name="storefront">The Storefront Context</param>
        /// <param name="visitorContext">The Visitor Context</param>
        /// <param name="cart">the cart</param>
        /// <param name="party">the party info</param>
        /// <returns>the updated cart</returns>
        ManagerResponse<CartResult, CommerceCart> AddPartyToCart([NotNull] CommerceStorefront storefront, [NotNull] IVisitorContext visitorContext, [NotNull] CommerceCart cart, [NotNull] CommerceParty party);

        /// <summary>
        /// Removes party info from a cart
        /// </summary>
        /// <param name="storefront">The Storefront Context</param>
        /// <param name="visitorContext">The Visitor Context</param>
        /// <param name="cart">the cart</param>
        /// <param name="parties">The parties.</param>
        /// <returns>
        /// the updated cart
        /// </returns>
        ManagerResponse<CartResult, CommerceCart> RemovePartiesFromCart([NotNull] CommerceStorefront storefront, [NotNull] IVisitorContext visitorContext, [NotNull] CommerceCart cart, [NotNull] List<Party> parties);

        /// <summary>
        /// Removes all shipping parties.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="visitorContext">The visitor context.</param>
        /// <param name="cart">The cart.</param>
        /// <returns>The manager response with a cart in the result.</returns>
        ManagerResponse<CartResult, CommerceCart> RemoveAllShippingParties([NotNull] CommerceStorefront storefront, [NotNull] IVisitorContext visitorContext, [NotNull] CommerceCart cart);

        /// <summary>
        /// Removes all billing parties.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="visitorContext">The visitor context.</param>
        /// <param name="cart">The cart.</param>
        /// <returns>The manager response with a cart in the result.</returns>
        ManagerResponse<CartResult, CommerceCart> RemoveAllBillingParties([NotNull] CommerceStorefront storefront, [NotNull] IVisitorContext visitorContext, [NotNull] CommerceCart cart);

        /// <summary>
        /// Removes all payment methods.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="visitorContext">The visitor context.</param>
        /// <param name="cart">The cart.</param>
        /// <returns>The manager response with a cart in the result.</returns>
        ManagerResponse<CartResult, CommerceCart> RemoveAllPaymentMethods([NotNull] CommerceStorefront storefront, [NotNull] IVisitorContext visitorContext, [NotNull] CommerceCart cart);

        /// <summary>
        /// Removes all shipping methods.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="visitorContext">The visitor context.</param>
        /// <param name="cart">The cart.</param>
        /// <returns>The manager response with the cart result and returned cart.</returns>
        ManagerResponse<CartResult, CommerceCart> RemoveAllShippingMethods([NotNull] CommerceStorefront storefront, [NotNull] IVisitorContext visitorContext, [NotNull] CommerceCart cart);

        /// <summary>
        /// Adds payment info to a cart
        /// </summary>
        /// <param name="storefront">The Storefront Context</param>
        /// <param name="visitorContext">The Visitor Context</param>
        /// <param name="cart">the cart</param>
        /// <param name="info">the payment info</param>
        /// <param name="party">the party info</param>
        /// <param name="refreshCart">if set to <c>true</c> the cart will be re-calculated using the Commerce Server pipelines.</param>
        /// <returns>
        /// the updated cart
        /// </returns>
        ManagerResponse<CartResult, CommerceCart> AddPaymentInfoToCart([NotNull] CommerceStorefront storefront, [NotNull] IVisitorContext visitorContext, [NotNull] CommerceCart cart, [NotNull] PaymentInfo info, [NotNull] CommerceParty party, bool refreshCart = false);

        /// <summary>
        /// Updates the cart.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="visitorContext">The visitor context.</param>
        /// <param name="cart">The cart.</param>
        /// <param name="cartChanges">The cart changes.</param>
        /// <returns>The manager response with the updated cart.</returns>
        ManagerResponse<CartResult, CommerceCart> UpdateCart([NotNull] CommerceStorefront storefront, [NotNull] IVisitorContext visitorContext, [NotNull] CommerceCart cart, [NotNull] CommerceCart cartChanges);
    }
}