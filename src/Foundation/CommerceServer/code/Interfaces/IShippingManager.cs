using System.Collections.Generic;
using Sitecore.Commerce.Entities.Carts;
using Sitecore.Commerce.Entities.Shipping;
using Sitecore.Commerce.Services.Shipping;
using Sitecore.Foundation.CommerceServer.Interfaces;
using Sitecore.Foundation.CommerceServer.Models;
using Sitecore.Foundation.CommerceServer.Models.InputModels;

namespace Sitecore.Foundation.CommerceServer.Managers
{
    public interface IShippingManager
    {
        /// <summary>
        /// Gets the shipping preferences.
        /// </summary>
        /// <param name="cart">The cart.</param>
        /// <returns>The manager response where the shipping options are returned in the Result.</returns>
        ManagerResponse<GetShippingOptionsResult, List<ShippingOption>> GetShippingPreferences([NotNull] Cart cart);

        /// <summary>
        /// Gets the shipping methods.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="visitorContext">The visitor context.</param>
        /// <param name="inputModel">The input model.</param>
        /// <returns>
        /// The manager response where the shipping methods are returned in the Result.
        /// </returns>
        ManagerResponse<GetShippingMethodsResult, IReadOnlyCollection<ShippingMethod>> GetShippingMethods([NotNull] CommerceStorefront storefront, [NotNull] IVisitorContext visitorContext, [NotNull] GetShippingMethodsInputModel inputModel);

        /// <summary>
        /// Gets the shipping methods.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="visitorContext">The visitor context.</param>
        /// <param name="option">The option.</param>
        /// <returns>The manager response where the shipping methods are returned in the result.</returns>
        ManagerResponse<GetShippingMethodsResult, IReadOnlyCollection<ShippingMethod>> GetShippingMethods([NotNull] CommerceStorefront storefront, [NotNull] IVisitorContext visitorContext, [NotNull] ShippingOption option);
    }
}