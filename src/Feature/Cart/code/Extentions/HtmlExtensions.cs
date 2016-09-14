namespace Sitecore.Feature.Cart.Extentions
{
    using System.Web;
    using System.Web.Mvc;
    using Analytics;
    using Sitecore;
    /// <summary>
    /// Extensions for working with HTML and HtmlHelpers.
    /// </summary>
    public static class HtmlExtensions
    {
        /// <summary>
        /// Returns the visitor identification snippet if needed
        /// </summary>
        /// <param name="sitecoreHelper">The sitecore helper.</param>
        /// <returns>
        /// HtmlString with the identification snippet
        /// </returns>
        public static HtmlString AnalyticsVisitorIdentification(this Mvc.Helpers.SitecoreHelper sitecoreHelper)
        {
            if (Context.Diagnostics.Tracing || Context.Diagnostics.Profiling)
            {
                return new HtmlString("<!-- Visitor identification is disabled because debugging is active. -->");
            }

            if (!Tracker.IsActive)
            {
                return MvcHtmlString.Empty;
            }

            return new HtmlString("<link href=\"/layouts/System/VisitorIdentification.aspx\" rel=\"stylesheet\" type=\"text/css\" />");
        }
    }
}