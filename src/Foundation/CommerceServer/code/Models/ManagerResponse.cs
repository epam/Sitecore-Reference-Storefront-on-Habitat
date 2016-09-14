using Sitecore.Commerce.Services;

namespace Sitecore.Foundation.CommerceServer.Models
{
    /// <summary>
    /// Defined the ManagerResponse class.
    /// </summary>
    /// <typeparam name="TServiceProviderResult">The type of the service provider result.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public class ManagerResponse<TServiceProviderResult, TResult>
        where TServiceProviderResult : ServiceProviderResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ManagerResponse{TServiceProviderResult, TResult}"/> class.
        /// </summary>
        /// <param name="serviceProviderResult">The service provider result instance.</param>
        /// <param name="result">The result.</param>
        public ManagerResponse(TServiceProviderResult serviceProviderResult, TResult result)
        {
            this.ServiceProviderResult = serviceProviderResult;
            this.Result = result;
        }

        /// <summary>
        /// Gets or sets the service provider result.
        /// </summary>
        /// <value>
        /// The service provider result.
        /// </value>
        public TServiceProviderResult ServiceProviderResult { get; set; }

        /// <summary>
        /// Gets or sets the result.
        /// </summary>
        /// <value>
        /// The result.
        /// </value>
        public TResult Result { get; set; }
    }
}