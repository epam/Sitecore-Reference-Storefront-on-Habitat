namespace Sitecore.Foundation.CommerceServer.Logging
{
    using System;
    using Sitecore.Commerce.Connect.CommerceServer;
    using Sitecore.Foundation.CommerceServer.Interfaces;

    /// <summary>
    /// Provides logic to log Sitecore messages into Commerce.
    /// </summary>
    public class Logger : ILogger
    {
        /// <summary>
        /// Gets the currently configured logger. 
        /// </summary>
        private static ICommerceLog Current => CommerceLog.Current;

        /// <summary>
        /// Logs an informational message into Commerce Server. 
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="owner">The message owner.</param>
        public void LogInfo(string message, object owner)
        {
            Current.Info(message, owner);
        }

        /// <summary>
        /// Logs a warning message into Commerce Server. 
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="owner">The message owner.</param>
        public void LogWarning(string message, object owner)
        {
            Current.Warning(message, owner);
        }

        /// <summary>
        /// Logs an error message into Commerce Server. 
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="owner">The message owner.</param>
        public void LogError(string message, object owner)
        {
            Current.Error(message, owner);
        }

        /// <summary>
        /// Logs an error message into Commerce Server. 
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="ownerType">The message owner type.</param>
        /// <param name="exception">The source exception to log.</param>
        public void LogError(string message, Type ownerType, Exception exception)
        {
            Current.Error(message, ownerType, exception);
        }

        /// <summary>
        /// Logs an error message into Commerce Server. 
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="ownerType">The message owner.</param>
        public void LogError(string message, Type ownerType)
        {
            Current.Error(message, ownerType);
        }

        /// <summary>
        /// Logs an error message into Commerce Server. 
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="owner">The message owner.</param>
        /// <param name="exception">The source exception to log.</param>
        public void LogError(string message, object owner, Exception exception)
        {
            Current.Error(message, owner, exception);
        }
    }
}