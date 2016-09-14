namespace Sitecore.Foundation.Feature.Models.JsonResults
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;
    using Sitecore.Commerce.Services;
    using Sitecore.Foundation.CommerceServer.Managers;

    /// <summary>
    /// Defines the BaseJsonResult class.
    /// </summary>
    public class BaseJsonResult : JsonResult
    {
        private readonly List<string> _errors = new List<string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseJsonResult"/> class.
        /// </summary>
        public BaseJsonResult()
        {
            this.Success = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseJsonResult"/> class.
        /// </summary>
        /// <param name="result">The service provider result.</param>
        public BaseJsonResult(ServiceProviderResult result)
        {
            this.Success = true;

            if (result != null)
            {
                this.SetErrors(result);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseJsonResult" /> class.
        /// </summary>
        /// <param name="area">The area.</param>
        /// <param name="exception">The exception.</param>
        public BaseJsonResult(string area, Exception exception)
        {
            this.Success = false;

            this.SetErrors(area, exception);
        }

        /// <summary>
        /// Gets the errors.
        /// </summary>
        /// <value>
        /// The errors.
        /// </value>
        public List<string> Errors
        {
            get { return _errors; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has errors.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has errors; otherwise, <c>false</c>.
        /// </value>
        public bool HasErrors
        {
            get { return this._errors != null && this._errors.Any(); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="BaseJsonResult"/> is success.
        /// </summary>
        /// <value>
        ///   <c>true</c> if success; otherwise, <c>false</c>.
        /// </value>
        public bool Success { get; set; }

        /// <summary>
        /// Sets the errors.
        /// </summary>
        /// <param name="result">The result.</param>
        public void SetErrors(ServiceProviderResult result)
        {
            this.Success = result.Success;
            if (result.SystemMessages.Count <= 0)
            {
                return;
            }

            var errors = result.SystemMessages;
            foreach (var error in errors)
            {
                var message = StorefrontManager.GetSystemMessage(error.Message, false);
                this.Errors.Add(string.IsNullOrEmpty(message) ? error.Message : message);
            }
        }

        /// <summary>
        /// Sets the errors.
        /// </summary>
        /// <param name="area">The area.</param>
        /// <param name="exception">The exception.</param>
        public void SetErrors(string area, Exception exception)
        {
            this._errors.Add(string.Format(CultureInfo.InvariantCulture, "{0}: {1}", area, exception.Message));
            this.Success = false;
        }

        /// <summary>
        /// Sets the errors.
        /// </summary>
        /// <param name="errors">The errors.</param>
        public void SetErrors(List<string> errors)
        {
            if (!errors.Any())
            {
                return;
            }

            this.Success = false;
            this._errors.AddRange(errors);
        }
    }
}