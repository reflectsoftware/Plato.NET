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
    public class AMQReceiverTextResult : AMQReceiverResult, IMessageReceiveResult<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AMQReceiverTextResult"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        internal AMQReceiverTextResult(IMessage message) : base(message)
        {
        }

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        public string Data
        {
            get
            {
                return (Message as ITextMessage).Text;
            }
        }
    }
}
