namespace Sitecore.Foundation.CommerceServer.Interfaces
{
    using System;
    using Sitecore.Commerce.Connect.CommerceServer;

    public interface ILogger
    {
        /// <summary>
        /// Logs an informational message into Commerce Server. 
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="owner">The message owner.</param>
        void LogInfo(string message, object owner);

        /// <summary>
        /// Logs a warning message into Commerce Server. 
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="owner">The message owner.</param>
        void LogWarning(string message, object owner);

        /// <summary>
        /// Logs an error message into Commerce Server. 
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="owner">The message owner.</param>
        void LogError(string message, object owner);

        /// <summary>
        /// Logs an error message into Commerce Server. 
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="ownerType">The message owner type.</param>
        /// <param name="exception">The source exception to log.</param>
        void LogError(string message, Type ownerType, Exception exception);

        /// <summary>
        /// Logs an error message into Commerce Server. 
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="ownerType">The message owner.</param>
        void LogError(string message, Type ownerType);

        /// <summary>
        /// Logs an error message into Commerce Server. 
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="owner">The message owner.</param>
        /// <param name="exception">The source exception to log.</param>
        void LogError(string message, object owner, Exception exception);
    }
}