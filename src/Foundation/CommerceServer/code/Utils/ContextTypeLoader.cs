using CommerceContext = Sitecore.Commerce.Connect.CommerceServer;

namespace Sitecore.Foundation.CommerceServer.Utils
{
    using System;
    using Sitecore.Commerce.Connect.CommerceServer.Caching;

    /// <summary>
    /// Wraps <see cref="CommerceContext.CommerceTypeLoader"/> methods.
    /// </summary>
    public static class ContextTypeLoader
    {
        /// <summary>
        /// Creates instance of specified interface type. 
        /// </summary>
        /// <typeparam name="T">Interface type to be loaded.</typeparam>
        /// <returns>An instance of type T.</returns>
        public static T CreateInstance<T>()
        {
           return CommerceContext.CommerceTypeLoader.CreateInstance<T>();
        }

        /// <summary>
        /// Gets the cache provider by specified cache name. 
        /// </summary>
        /// <param name="cacheName">Name of the cache to get.</param>
        /// <returns>ICacheProvider instance for the given cache name.</returns>
        public static ICacheProvider GetCacheProvider(string cacheName)
        {
            return CommerceContext.CommerceTypeLoader.GetCacheProvider(cacheName);
        }

        /// <summary>
        /// Creates new instance of type specified type. 
        /// </summary>
        /// <typeparam name="T">Object type to be loaded.</typeparam>
        /// <param name="args">The arguments.</param>
        /// <returns>An instance of type T. </returns>
        public static T CreateInstance<T>(params object[] args)
        {
            return CommerceContext.CommerceTypeLoader.CreateInstance<T>(args);
        }
    }
}