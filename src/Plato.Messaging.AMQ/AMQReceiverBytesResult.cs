// Plato.NET
// Copyright (c) 2018 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Apache.NMS;
using Plato.Messaging.Interfaces;

namespace Plato.Messaging.AMQ
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Messaging.Interfaces.IMessageReceiveResult" />
    public class AMQReceiverBytesResult : AMQReceiverResult, IMessageReceiveResult<byte[]>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AMQReceiverBytesResult"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        internal AMQReceiverBytesResult(IMessage message) : base(message)
        {
        }

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        public byte[] Data
        {
            get
            {
                return (Message as IBytesMessage).Content;
            }
        }
    }
}
