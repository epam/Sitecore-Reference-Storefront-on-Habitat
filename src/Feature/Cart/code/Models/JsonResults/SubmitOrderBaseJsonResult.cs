namespace Sitecore.Feature.Cart.Models.JsonResults
{
    using Sitecore.Commerce.Services;
    using Sitecore.Diagnostics;
    using Sitecore.Feature.Base.Models.JsonResults;

    /// <summary>
    /// The Json result of a Submit Order request.
    /// </summary>
    public class SubmitOrderBaseJsonResult : BaseJsonResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SubmitOrderBaseJsonResult"/> class.
        /// </summary>
        public SubmitOrderBaseJsonResult()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SubmitOrderBaseJsonResult"/> class.
        /// </summary>
        /// <param name="result">The service provider result.</param>
        public SubmitOrderBaseJsonResult(ServiceProviderResult result)
            : base(result)
        {
        }

        /// <summary>
        /// Gets or sets the order confirmation page URL.
        /// </summary>
        public string ConfirmUrl { get; set; }

        /// <summary>
        /// Initializes the specified confirm URL.
        /// </summary>
        /// <param name="confirmUrl">The confirm URL.</param>
        public virtual void Initialize(string confirmUrl)
        {
            Assert.ArgumentNotNullOrEmpty(confirmUrl, "confirmUrl");

            this.ConfirmUrl = confirmUrl;
        }
    }
}