namespace Sitecore.Foundation.CommerceServer.Extensions
{
    using System;
    using System.Web.Mvc;
    using System.Web.UI;

    using Sitecore.Web.UI.WebControls;

    /// <summary>
    /// Used to create integration of the editor frame into an MVC site
    /// </summary>
    public class FrameEditor : IDisposable
    {
        private EditFrame _editFrameControl;
        private HtmlHelper _html;

        /// <summary>
        /// Initializes a new instance of the <see cref="FrameEditor"/> class.
        /// </summary>
        /// <param name="html">The html helper managing the current request</param>
        /// <param name="dataSource">A path to the Sitecore item that the frame is for</param>
        /// <param name="buttons">A path to the edit frame buttons in the core database that need to be shown</param>
        public FrameEditor(HtmlHelper html, string dataSource = null, string buttons = null)
        {
            this._html = html;

            this._editFrameControl = new EditFrame
            {
                DataSource = dataSource ?? "/sitecore/content/home",
                Buttons = buttons ?? "/sitecore/content/Applications/WebEdit/Edit Frame Buttons/Default"
            };

            var output = new HtmlTextWriter(html.ViewContext.Writer);

            this._editFrameControl.RenderFirstPart(output);
        }

        /// <summary>
        /// Disposes of the current class appropriately
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this._editFrameControl != null)
                {
                    this._editFrameControl.RenderLastPart(new HtmlTextWriter(this._html.ViewContext.Writer));
                    this._editFrameControl.Dispose();
                }
            }
        }
    }
}