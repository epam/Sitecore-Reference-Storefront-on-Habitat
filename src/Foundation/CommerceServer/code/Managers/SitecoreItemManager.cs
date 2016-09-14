namespace Sitecore.Foundation.CommerceServer.Managers
{
    using System;
    using Sitecore.Data;
    using Sitecore.Data.Items;

    /// <summary>
    /// SitecoreItemManager class
    /// </summary>
    public class SitecoreItemManager
    {
        private static SitecoreItemManager _sitecoreItemManager;

        /// <summary>
        /// Instances this instance.
        /// </summary>
        /// <returns>Sitecore item manager</returns>
        public static SitecoreItemManager Instance()
        {
            return _sitecoreItemManager ?? (_sitecoreItemManager = new SitecoreItemManager());
        }

        /// <summary>
        /// Gets the item.
        /// </summary>
        /// <param name="itemIdOrPath">The item identifier or path.</param>
        /// <returns>The item</returns>
        public Item GetItem(string itemIdOrPath)
        {
            Guid itemGuid;
            return Guid.TryParse(itemIdOrPath, out itemGuid) ? Context.Database.GetItem(ID.Parse(itemGuid)) : Context.Database.GetItem(itemIdOrPath);
        }

        /// <summary>
        /// Gets the item.
        /// </summary>
        /// <typeparam name="T">Thje container type of the item.</typeparam>
        /// <param name="itemIdOrPath">The item identifier or path.</param>
        /// <returns>
        /// The item
        /// </returns>
        public T GetItem<T>(string itemIdOrPath)
        {
            Item item = this.GetItem(itemIdOrPath);
            var sitecoreItem = (T)Activator.CreateInstance(typeof(T), item);
            return sitecoreItem;
        }
    }
}