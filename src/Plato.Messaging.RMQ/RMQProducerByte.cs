// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Messaging.RMQ.Interfaces;
using Plato.Messaging.RMQ.Settings;
using Plato.Messaging.Interfaces;
using System;

namespace Plato.Messaging.RMQ
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Messaging.RMQ.RMQProducer" />
    /// <seealso cref="Plato.Messaging.RMQ.Interfaces.IRMQProducerByte" />
    public class RMQProducerByte : RMQProducer, IRMQProducerByte
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RMQProducerByte"/> class.
        /// </summary>
        /// <param name="connectionFactory">The connection factory.</param>
        /// <param name="connectionSettings">The connection settings.</param>
        /// <param name="queueSettings">The queue settings.</param>
        public RMQProducerByte(
            IRMQConnectionFactory connectionFactory,
            RMQConnectionSettings connectionSettings,
            RMQQueueSettings queueSettings) 
            : base(connectionFactory, connectionSettings, queueSettings)
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
