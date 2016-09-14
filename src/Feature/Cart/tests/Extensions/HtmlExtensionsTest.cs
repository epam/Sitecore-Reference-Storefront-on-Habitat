namespace Sitecore.Feature.Cart.Tests.Extensions
{
    using System.Web;
    using System.Web.Mvc;
    using FluentAssertions;
    using Sitecore.Analytics;
    using Sitecore.Analytics.Tracking;
    using Sitecore.Common;
    using Sitecore.Diagnostics;
    using Sitecore.FakeDb.Sites;
    using Sitecore.Feature.Cart.Extentions;
    using Sitecore.Mvc.Helpers;
    using Xunit;

    public class HtmlExtensionsTest
    {
        [Fact]
        public void AnalyticsVisitorIdentification_ShoudReturnEmptyString_TrackingIsTrue()
        {
            var htmlHelper = new HtmlHelper(new ViewContext(), new ViewPage());
            var sitecoreHelper = new SitecoreHelper(htmlHelper);
            var expectedResult = new HtmlString(
                "<link href=\"/layouts/System/VisitorIdentification.aspx\" rel=\"stylesheet\" type=\"text/css\" />")
                .ToString();

            Context.Site = null; Context.Site = new FakeSiteContext("fake");
            Tracer.EndSession();
            Profiler.EndSession();
            var emptySession = new EmptySession();
            var tracker = new NullTracker(emptySession);
            Switcher<ITracker, TrackerSwitcher>.Enter(tracker);
            Tracker.Initialize();
            Tracker.IsActive = true;
            var result = sitecoreHelper.AnalyticsVisitorIdentification();

            result.ToString().Should().Be(expectedResult);
        }

        [Fact]
        public void AnalyticsVisitorIdentification_ShoudReturnDebuggingMessage_WhenTrcaingIsTrue()
        {
            var htmlHelper = new HtmlHelper(new ViewContext(), new ViewPage());
            var sitecoreHelper = new SitecoreHelper(htmlHelper);
            var expectedResult = new HtmlString("<!-- Visitor identification is disabled because debugging is active. -->")
                .ToString();

            Context.Site = new FakeSiteContext("fake");
            Tracer.StartSession();
            Context.Diagnostics.Tracing = true;
            var result = sitecoreHelper.AnalyticsVisitorIdentification();

            result.ToString().Should().Be(expectedResult);
        }

        [Fact]
        public void AnalyticsVisitorIdentification_ShoudReturnDebuggingMessage_WhenProfilingIsTrue()
        {
            var htmlHelper = new HtmlHelper(new ViewContext(), new ViewPage());
            var sitecoreHelper = new SitecoreHelper(htmlHelper);
            var expectedResult = "<!-- Visitor identification is disabled because debugging is active. -->";

            Context.Site = new FakeSiteContext("fake");
            Profiler.StartSession();
            Context.Diagnostics.Profiling = true;
            var result = sitecoreHelper.AnalyticsVisitorIdentification();

            result.ToString().Should().Be(expectedResult);
        }

        [Fact]
        public void AnalyticsVisitorIdentification_ShoudReturnEmptyString_TrackingIsFalse()
        {
            var htmlHelper = new HtmlHelper(new ViewContext(), new ViewPage());
            var sitecoreHelper = new SitecoreHelper(htmlHelper);


            var result = sitecoreHelper.AnalyticsVisitorIdentification();

            result.ToString().Should().Be(string.Empty);
        }
    }
}
