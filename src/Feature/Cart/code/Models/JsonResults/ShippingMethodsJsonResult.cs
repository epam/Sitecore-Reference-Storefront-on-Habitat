namespace Sitecore.Feature.Cart.Models.JsonResults
{
    using Sitecore.Commerce.Entities.Shipping;
    using Sitecore.Commerce.Services.Shipping;
    using System.Collections.Generic;

    /// <summary>
    /// The Json result of a request to retrieve nearby store locations.
    /// </summary>
    public class ShippingMethodsJsonResult : ShippingMethodsBaseJsonResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShippingMethodsJsonResult"/> class.
        /// </summary>
        public ShippingMethodsJsonResult()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShippingMethodsJsonResult"/> class.
        /// </summary>
        /// <param name="result">The service provider result.</param>
        public ShippingMethodsJsonResult(GetShippingMethodsResult result)
            : base(result)
        {
        }

        /// <summary>
        /// Gets or sets the available line item shipping methods.
        /// </summary>
        public IEnumerable<ShippingMethodPerItem> LineShippingMethods { get; set; }

        /// <summary>
        /// Initilizes the specified shipping methods.
        /// </summary>
        /// <param name="shippingMethods">The shipping methods.</param>
        /// <param name="shippingMethodsPerItem">The shipping methods per item.</param>
        public virtual void Initialize(IEnumerable<ShippingMethod> shippingMethods, IEnumerable<ShippingMethodPerItem> shippingMethodsPerItem)
        {
            base.Initialize(shippingMethods);

            this.LineShippingMethods = shippingMethodsPerItem;
        }
    }
}