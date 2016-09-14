namespace Sitecore.Foundation.CommerceServer.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Defines the DefaultCurrencyNotSetException class.
    /// </summary>
    [Serializable]
    public class DefaultCurrencyNotSetException : Exception
    {
        #region Constructors (public)

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultCurrencyNotSetException"/> class.
        /// </summary>
        public DefaultCurrencyNotSetException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultCurrencyNotSetException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public DefaultCurrencyNotSetException(string message)
            : this(message, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultCurrencyNotSetException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public DefaultCurrencyNotSetException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        #endregion

        #region Constructor (protected)

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultCurrencyNotSetException" /> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        protected DefaultCurrencyNotSetException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion

        #region Overrides

        /// <summary>
        /// When overridden in a derived class, sets the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> with information about the exception.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        public override void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

        #endregion
    }
}