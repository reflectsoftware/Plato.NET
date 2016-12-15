// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Messaging.Implementations.RMQ.Interfaces;
using Plato.Messaging.Implementations.RMQ.Settings;
using Plato.Messaging.Interfaces;
using System;

namespace Plato.Messaging.Implementations.RMQ
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Messaging.Implementations.RMQ.RMQProducer" />
    /// <seealso cref="Plato.Messaging.Implementations.RMQ.Interfaces.IRMQProducerByte" />
    public class RMQProducerByte : RMQProducer, IRMQProducerByte
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RMQProducerByte"/> class.
        /// </summary>
        /// <param name="connectionFactory">The connection factory.</param>
        /// <param name="connectionName">Name of the connection.</param>
        /// <param name="settings">The settings.</param>
        public RMQProducerByte(
            IRMQConnectionFactory connectionFactory, 
            string connectionName,             
            RMQQueueSettings settings) 
            : base(connectionFactory, connectionName, settings)
        {
        }

        /// <summary>
        /// Sends the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="action">The action.</param>
        public void Send(byte[] data, Action<ISenderProperties> action = null)
        {
            _Send(data, action);
        }
    }
}
