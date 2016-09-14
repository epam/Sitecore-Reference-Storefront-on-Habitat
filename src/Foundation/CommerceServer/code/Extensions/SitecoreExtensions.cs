namespace Sitecore.Foundation.CommerceServer.Extensions
{
    using Sitecore.Data.Items;
    using Sitecore.Resources.Media;
    using System;
    using System.Web.Mvc;

    /// <summary>
    /// Extensions for working with Sitecore objects in the MVC Site
    /// </summary>
    public static class SitecoreExtensions
    {
        /// <summary>
        /// Gets the full size image paths based on the BaseUrl in the app settings and the short path in the delimited property Image_Filename in the Product.
        /// </summary>
        /// <param name="item">The item on which the image is contained.</param>
        /// <param name="width">The width of the image to draw.</param>
        /// <param name="height">The height of the image to draw.</param>
        /// <returns>
        /// The full size image paths.
        /// </returns>
        public static string GetImageUrl(this MediaItem item, int width, int height)
        {
            if (item == null)
            {
                //This happens if there is a trailing pipe '|' at the end of an image list
                return string.Empty;
            }

            var options = new Sitecore.Resources.Media.MediaUrlOptions() { Height = height, Width = width };
            var url = Sitecore.Resources.Media.MediaManager.GetMediaUrl(item, options);
            var cleanUrl = Sitecore.StringUtil.EnsurePrefix('/', url);
            var hashedUrl = HashingUtils.ProtectAssetUrl(cleanUrl);

            return hashedUrl;
        }

        /// <summary>
        /// Creates a Sitecore Editframe for an item
        /// </summary>
        /// <param name="html">The html helper managing the current request</param>
        /// <param name="dataSource">A path to the Sitecore item that the frame is for</param>
        /// <param name="buttons">A path to the edit frame buttons in the core database that need to be shown</param>
        /// <returns>The editor for disposal</returns>
        //TODO:remove unused
        public static IDisposable EditFrame(this HtmlHelper html, string dataSource = null, string buttons = null)
        {
            return new FrameEditor(html, dataSource, buttons);
        }
    }
}