namespace Sitecore.Feature.Cart.Models.JsonResults
{
    using Sitecore.Commerce.Services;
    using Sitecore.Diagnostics;
    using Sitecore.Feature.Base.Models.JsonResults;
    using System.Collections.Generic;

    /// <summary>
    /// The Json result of a request to retrieve the available states.
    /// </summary>
    public class AvailableStatesBaseJsonResult : BaseJsonResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AvailableStatesBaseJsonResult"/> class.
        /// </summary>
        public AvailableStatesBaseJsonResult()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AvailableStatesBaseJsonResult"/> class.
        /// </summary>
        /// <param name="result">The service provider result.</param>
        public AvailableStatesBaseJsonResult(ServiceProviderResult result)
            : base(result)
        {
        }

        /// <summary>
        /// Gets or sets the available states.
        /// </summary>
        public Dictionary<string, string> States { get; set; }

        /// <summary>
        /// Initializes the specified states.
        /// </summary>
        /// <param name="states">The states.</param>
        public virtual void Initialize(Dictionary<string, string> states)
        {
            Assert.ArgumentNotNull(states, "states");

            this.States = states;
        }
    }
}