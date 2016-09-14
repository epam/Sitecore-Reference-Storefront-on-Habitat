namespace Sitecore.Foundation.CommerceServer.Extensions
{
    using Sitecore.Analytics;
    using Sitecore.Foundation.CommerceServer.Exceptions;
    using Sitecore.Foundation.CommerceServer.Infrastructure.Constants;
    using Sitecore.Foundation.CommerceServer.Managers;

    /// <summary>
    /// Defines the TrackerExtensions class.
    /// </summary>
    public static class TrackerExtensions
    {
        /// <summary>
        /// Checks if the Tracker.Current object os null.
        /// </summary>
        /// <param name="tracker">The tracker.</param>
        /// <returns>In the context of the Runtime site, will trow a TrackingNotEnabledException; Otherwise null is returned
        /// in the context of the page editor.</returns>
        /// <exception cref="TrackingNotEnabledException">Tracker is null we are in the context of the runtime site.</exception>
        public static ITracker CheckForNull(this ITracker tracker)
        {
            if (tracker == null && !Sitecore.Context.PageMode.IsExperienceEditor)
            {
                throw new TrackingNotEnabledException(StorefrontManager.GetSystemMessage(StorefrontConstants.SystemMessages.TrackingNotEnabled));
            }

            return tracker;
        }
    }
}
