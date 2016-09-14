namespace Sitecore.Foundation.CommerceServer.Managers
{
    using System.Collections.Generic;
    using System.Linq;
    using Sitecore.Commerce.Entities.Carts;
    using Sitecore.Commerce.Entities.Shipping;
    using Sitecore.Commerce.Services.Shipping;
    using Sitecore.Commerce.Services.Shipping.Generics;
    using Sitecore.Diagnostics;
    using Sitecore.Foundation.CommerceServer.Extensions;
    using Sitecore.Foundation.CommerceServer.Interfaces;
    using Sitecore.Foundation.CommerceServer.Models;
    using Sitecore.Foundation.CommerceServer.Models.InputModels;

    /// <summary>
    /// Defines the ShippingManager class.
    /// </summary>
    public class ShippingManager : IShippingManager
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShippingManager"/> class.
        /// </summary>
        /// <param name="shippingServiceProvider">The shipping service provider.</param>
        /// <param name="cartManager">The cart manager.</param>
        public ShippingManager(
            [NotNull] ShippingServiceProvider shippingServiceProvider,
            [NotNull] ICartManager cartManager)
        {
            Assert.ArgumentNotNull(shippingServiceProvider, "shippingServiceProvider");
            Assert.ArgumentNotNull(cartManager, "cartManager");

            this._shippingServiceProvider = shippingServiceProvider;
            this._cartManager = cartManager;
        }

        /// <summary>
        /// Gets or sets the shipping service provider.
        /// </summary>
        /// <value>
        /// The shipping service provider.
        /// </value>
        private readonly ShippingServiceProvider _shippingServiceProvider;

        /// <summary>
        /// Gets or sets the cart manager.
        /// </summary>
        /// <value>
        /// The cart manager.
        /// </value>
        private readonly ICartManager _cartManager;

        /// <summary>
        /// Gets the shipping preferences.
        /// </summary>
        /// <param name="cart">The cart.</param>
        /// <returns>The manager response where the shipping options are returned in the Result.</returns>
        public ManagerResponse<GetShippingOptionsResult, List<ShippingOption>> GetShippingPreferences([NotNull] Cart cart)
        {
            Assert.ArgumentNotNull(cart, "cart");

            var request = new GetShippingOptionsRequest(cart);
            var result = this._shippingServiceProvider.GetShippingOptions(request);
            if (result.Success && result.ShippingOptions != null)
            {
                return new ManagerResponse<GetShippingOptionsResult, List<ShippingOption>>(result, result.ShippingOptions.ToList());
            }

            //Helpers.LogSystemMessages(result.SystemMessages, result);
            return new ManagerResponse<GetShippingOptionsResult, List<ShippingOption>>(result, null);
        }

        /// <summary>
        /// Gets the shipping methods.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="visitorContext">The visitor context.</param>
        /// <param name="inputModel">The input model.</param>
        /// <returns>
        /// The manager response where the shipping methods are returned in the Result.
        /// </returns>
        public ManagerResponse<GetShippingMethodsResult, IReadOnlyCollection<ShippingMethod>> GetShippingMethods([NotNull] CommerceStorefront storefront, [NotNull] IVisitorContext visitorContext, [NotNull] GetShippingMethodsInputModel inputModel)
        {
            Assert.ArgumentNotNull(storefront, "storefront");
            Assert.ArgumentNotNull(visitorContext, "visitorContext");
            Assert.ArgumentNotNull(inputModel, "inputModel");

            var errorResult = new GetShippingMethodsResult { Success = false };
            var cartResult = this._cartManager.GetCurrentCart(storefront, visitorContext);
            if (!cartResult.ServiceProviderResult.Success || cartResult.Result == null)
            {
                errorResult.SystemMessages.ToList().AddRange(cartResult.ServiceProviderResult.SystemMessages);
                return new ManagerResponse<GetShippingMethodsResult, IReadOnlyCollection<ShippingMethod>>(errorResult, null);
            }

            var cart = cartResult.Result;

            var preferenceType = InputModelExtension.GetShippingOptionType(inputModel.ShippingPreferenceType);
            if (inputModel.Lines != null && inputModel.Lines.Any())
            {
                // We only support a single line at a time, hence accessing element 0 is ok.
                preferenceType = InputModelExtension.GetShippingOptionType(inputModel.Lines[0].ShippingPreferenceType);
            }

            // TODO: Remove hard coded language - will be fixed in connect.
            var request = new Helpers.GetShippingMethodsRequest(
                "en-us",
                new ShippingOption { ShippingOptionType = preferenceType },
                (inputModel.ShippingAddress != null) ? inputModel.ShippingAddress.ToParty() : null)
            {
                Cart = cart,
                Lines = (inputModel.Lines != null) ? inputModel.Lines.ToCommerceCartLines().Cast<CartLine>().ToList() : null
            };

            var result = this._shippingServiceProvider.GetShippingMethods<Helpers.GetShippingMethodsRequest, GetShippingMethodsResult>(request);

            //Helpers.LogSystemMessages(errorResult.SystemMessages, errorResult);
            return new ManagerResponse<GetShippingMethodsResult, IReadOnlyCollection<ShippingMethod>>(result, result.ShippingMethods);
        }

        /// <summary>
        /// Gets the shipping methods.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="visitorContext">The visitor context.</param>
        /// <param name="option">The option.</param>
        /// <returns>The manager response where the shipping methods are returned in the result.</returns>
        public ManagerResponse<GetShippingMethodsResult, IReadOnlyCollection<ShippingMethod>> GetShippingMethods([NotNull] CommerceStorefront storefront, [NotNull] IVisitorContext visitorContext, [NotNull] ShippingOption option)
        {
            Assert.ArgumentNotNull(storefront, "storefront");
            Assert.ArgumentNotNull(visitorContext, "visitorContext");
            Assert.ArgumentNotNull(option, "option");

            // TODO: Remove hard coded language - will be fixed in connect.
            var request = new Helpers.GetShippingMethodsRequest("en-us", option, null);

            var result = this._shippingServiceProvider.GetShippingMethods<Helpers.GetShippingMethodsRequest, GetShippingMethodsResult>(request);

            //Helpers.LogSystemMessages(result.SystemMessages, result);
            return new ManagerResponse<GetShippingMethodsResult, IReadOnlyCollection<ShippingMethod>>(result, result.ShippingMethods);
        }
    }
}

