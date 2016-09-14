namespace Sitecore.Feature.Cart.Models.JsonResults
{
    using Sitecore.Commerce.Entities.Shipping;
    using Sitecore.Commerce.Services.Shipping;
    using Sitecore.Feature.Base.Models.JsonResults;
    using System.Collections.Generic;

    /// <summary>
    /// The Json result of a request to retrieve nearby store locations.
    /// </summary>
    public class ShippingMethodsBaseJsonResult : BaseJsonResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShippingMethodsBaseJsonResult"/> class.
        /// </summary>
        public ShippingMethodsBaseJsonResult()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShippingMethodsBaseJsonResult"/> class.
        /// </summary>
        /// <param name="result">The service provider result.</param>
        public ShippingMethodsBaseJsonResult(GetShippingMethodsResult result)
            : base(result)
        {
        }

        /// <summary>
        /// Gets or sets the available order-level shipping methods.
        /// </summary>
        public IEnumerable<ShippingMethod> ShippingMethods { get; set; }

        /// <summary>
        /// Initilizes the specified shipping methods.
        /// </summary>
        /// <param name="shippingMethods">The shipping methods.</param>        
        public virtual void Initialize(IEnumerable<ShippingMethod> shippingMethods)
        {
            this.ShippingMethods = shippingMethods;
        }
    }
}