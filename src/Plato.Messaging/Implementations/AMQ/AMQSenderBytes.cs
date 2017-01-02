// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Messaging.Implementations.AMQ.Interfaces;
using Plato.Messaging.Implementations.AMQ.Settings;
using Plato.Messaging.Interfaces;
using System;

namespace Plato.Messaging.Implementations.AMQ
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Messaging.Implementations.AMQ.Interfaces.IAMQSenderBytes" />
    /// <seealso cref="Plato.Messaging.Implementations.AMQ.AMQSender" />
    public class AMQSenderBytes : AMQSender, IAMQSenderBytes
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AMQSenderText" /> class.
        /// </summary>
        /// <param name="connectionFactory">The connection factory.</param>
        /// <param name="connectionSettings">The connection settings.</param>
        /// <param name="destination">The destination.</param>
        public AMQSenderBytes(IAMQConnectionFactory connectionFactory, AMQConnectionSettings connectionSettings, AMQDestinationSettings destination) : base(connectionFactory, connectionSettings, destination)
        {
        }

        /// <summary>
        /// Sends the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="action">The action.</param>
        public void Send(byte[] data, Action<ISenderProperties> action = null)
        {
            Send(action, (session) =>
            {
                return session.CreateBytesMessage(data);
            });
        }
    }
}
