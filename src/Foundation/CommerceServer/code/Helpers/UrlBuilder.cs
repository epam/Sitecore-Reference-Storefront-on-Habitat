namespace Sitecore.Foundation.CommerceServer.Helpers
{
    using Collections;
    using Interfaces;
    using Sitecore.Foundation.CommerceServer.Utils;
    using System;
    using System.Globalization;

    /// <summary>
    /// Used to help in the building and modification of urls
    /// </summary>
    public class UrlBuilder : UriBuilder
    {
        private QueryStringCollection _query = new QueryStringCollection();

        #region Constructor overloads

        /// <summary>
        /// Initializes a new instance of the <see cref="UrlBuilder"/> class.
        /// </summary>
        public UrlBuilder()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UrlBuilder"/> class.
        /// </summary>
        /// <param name="uri">The URI to work with.</param>
        public UrlBuilder(string uri)
            : base(uri)
        {
            this.PopulateQueryString(uri);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UrlBuilder"/> class.
        /// </summary>
        /// <param name="uri">The URI to work with.</param>
        public UrlBuilder(Uri uri)
            : base(uri)
        {
            this.PopulateQueryString(uri.AbsoluteUri);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UrlBuilder"/> class.
        /// </summary>
        /// <param name="schemeName">Name of the scheme.</param>
        /// <param name="hostName">Name of the host.</param>
        public UrlBuilder(string schemeName, string hostName)
            : base(schemeName, hostName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UrlBuilder"/> class.
        /// </summary>
        /// <param name="scheme">The scheme of the url.</param>
        /// <param name="host">The host of the url.</param>
        /// <param name="portNumber">The port number.</param>
        public UrlBuilder(string scheme, string host, int portNumber)
            : base(scheme, host, portNumber)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UrlBuilder"/> class.
        /// </summary>
        /// <param name="scheme">The scheme of the url.</param>
        /// <param name="host">The host of the url.</param>
        /// <param name="port">The port of the url.</param>
        /// <param name="pathValue">The path value.</param>
        public UrlBuilder(string scheme, string host, int port, string pathValue)
            : base(scheme, host, port, pathValue)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UrlBuilder"/> class.
        /// </summary>
        /// <param name="scheme">An Internet access protocol.</param>
        /// <param name="host">A DNS-style domain name or IP address.</param>
        /// <param name="port">An IP port number for the service.</param>
        /// <param name="path">The path to the Internet resource.</param>
        /// <param name="extraValue">A query string or fragment identifier.</param>
        /// <exception cref="T:System.ArgumentException">
        /// 	<paramref name="extraValue"/> is neither null nor <see cref="F:System.String.Empty"/>, nor does a valid fragment identifier begin with a number sign (#), nor a valid query string begin with a question mark (?).
        /// </exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// 	<paramref name="port"/> is less than 0.
        /// </exception>
        public UrlBuilder(string scheme, string host, int port, string path, string extraValue)
            : base(scheme, host, port, path, extraValue)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UrlBuilder"/> class.
        /// </summary>
        /// <param name="request">The web request.</param>
        protected UrlBuilder(System.Web.HttpRequest request) :
            base(request.Url.Scheme, request.Url.Host, request.Url.Port, request.Url.LocalPath)
        {
            this.PopulateQueryString(request);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a url build for the request url in the current context.
        /// </summary>
        /// <value>The current URL.</value>
        public static UrlBuilder CurrentUrl
        {
            get
            {
                var siteContext = ContextTypeLoader.CreateInstance<ISiteContext>();
                return new UrlBuilder(siteContext.CurrentContext.Request.Url);
            }
        }

        /// <summary>
        /// Gets the querystring list.
        /// </summary>
        /// <value>The querystring list.</value>
        public QueryStringCollection QueryList
        {
            get
            {
                return this._query;
            }
        }

        /// <summary>
        /// Gets or sets the name of the page.
        /// </summary>
        /// <value>The name of the page.</value>
        /// TODO:remove unused
        public string PageName
        {
            get
            {
                string path = this.Path;
                return path.Substring(path.LastIndexOf("/", StringComparison.OrdinalIgnoreCase) + 1);
            }

            set
            {
                string path = this.Path;
                path = path.Substring(0, path.LastIndexOf("/", StringComparison.OrdinalIgnoreCase));
                this.Path = string.Concat(path, "/", value);
            }
        }

        /// <summary>
        /// Gets the domain of the url e.g. http://www.abc.com:81.
        /// </summary>
        /// <value>The domain.</value>
        /// TODO:remove unused
        public string UrlDomain
        {
            get
            {
                var urlDomain = string.Concat(this.Scheme + "://" + this.Uri.Host);
                if (!(this.Port == 80 && this.Scheme == "http") || !(this.Port == 443 && this.Scheme == "https"))
                {
                    urlDomain = string.Concat(urlDomain, ":", this.Port.ToString(CultureInfo.InvariantCulture));
                }

                return urlDomain;
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        /// <PermissionSet>
        /// 	<IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/>
        /// </PermissionSet>
        public new string ToString()
        {
            this.SetQuery();
            return this.Uri.AbsoluteUri;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        /// <param name="relative">True returns an absolute url, Fale returns a relative one</param>
        /// <PermissionSet>
        /// 	<IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/>
        /// </PermissionSet>
        public string ToString(bool relative)
        {
            if (relative)
            {
                this.SetQuery();
                return this.Uri.PathAndQuery;
            }
            else
            {
                return this.ToString();
            }
        }

        /// <summary>
        /// Navigates this instance.
        /// </summary>
        /// TODO:remove unused
        public void Navigate()
        {
            this.NavigateInternal(true);
        }

        /// <summary>
        /// Navigates the specified end response.
        /// </summary>
        /// <param name="endResponse">if set to <c>true</c> [end response].</param>
        /// TODO:remove unused
        public void Navigate(bool endResponse)
        {
            this.NavigateInternal(endResponse);
        }

        /// <summary>
        /// Toggles the scheme.
        /// </summary>
        /// <param name="toHttps">if set to <c>true</c> [to HTTPS].</param>
        /// TODO:remove unused
        public void ToggleScheme(bool toHttps)
        {
            if (toHttps)
            {
                this.Scheme = "https";
                this.Port = 443;
            }
            else
            {
                this.Scheme = "http";
                this.Port = 80;
            }
        }

        #endregion

        #region Private methods

        private void PopulateQueryString(System.Web.HttpRequest request)
        {
            this._query = new QueryStringCollection();

            foreach (string key in request.QueryString.AllKeys)
            {
                if (!string.IsNullOrEmpty(key))
                {
                    this._query.Add(key, request.QueryString[key]);
                }
            }

            this.SetQuery();
        }

        /// <summary>
        /// Populates the query string.
        /// </summary>
        /// <param name="url">The URL to get the query string from.</param>
        private void PopulateQueryString(string url)
        {
            this._query = new QueryStringCollection(url);
            this.SetQuery();
        }

        /// <summary>
        /// Gets the query string.
        /// </summary>
        private void SetQuery()
        {
            this.Query = this._query.ToString().TrimStart(new[] { '?' });
        }

        /// <summary>
        /// Navigates the specified end response.
        /// </summary>
        /// <param name="endResponse">if set to <c>true</c> [end response].</param>
        private void NavigateInternal(bool endResponse)
        {
            string uri = this.ToString();
            var siteContext = ContextTypeLoader.CreateInstance<ISiteContext>();
            siteContext.CurrentContext.Response.Redirect(uri, endResponse);
        }

        #endregion
    }
}