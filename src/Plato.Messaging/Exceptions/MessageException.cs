// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Messaging.Enums;
using System;
using System.Runtime.Serialization;

namespace Plato.Messaging.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.ApplicationException" />
    /// <seealso cref="System.Runtime.Serialization.ISerializable" />
    [Serializable]
    public class MessageException : ApplicationException, ISerializable
    {
        /// <summary>
        /// Gets the exception code.
        /// </summary>
        /// <value>
        /// The exception code.
        /// </value>
        public MessageExceptionCode ExceptionCode { get; private set; }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageException"/> class.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="message">The message.</param>
        public MessageException(MessageExceptionCode code, string message) : base(message)
        {
            ExceptionCode = code;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageException"/> class.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public MessageException(MessageExceptionCode code, string message, Exception innerException): base(message, innerException)
        {
            ExceptionCode = code;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageException"/> class.
        /// </summary>
        /// <param name="info">The information.</param>
        /// <param name="context">The context.</param>
        protected MessageException(SerializationInfo info, StreamingContext context): base(info, context)
        {
            ExceptionCode = (MessageExceptionCode)info.GetInt32("ExceptionCode");
        }

        /// <summary>
        /// Gets the object data.
        /// </summary>
        /// <param name="info">The information.</param>
        /// <param name="context">The context.</param>
        void ISerializable.GetObjectData( SerializationInfo info, StreamingContext context )
        {
            info.AddValue("ExceptionCode", (int)ExceptionCode);
            base.GetObjectData( info, context );
        }
    }
}
