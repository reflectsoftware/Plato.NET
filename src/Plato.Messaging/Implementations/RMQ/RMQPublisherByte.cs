// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Messaging.Implementations.RMQ.Interfaces;
using Plato.Messaging.Implementations.RMQ.Settings;
using Plato.Messaging.Interfaces;
using System;

namespace Plato.Messaging.Implementations.RMQ
{
    public class RMQPublisherByte : RMQPublisher, IRMQPublisherByte
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RMQPublisherByte" /> class.
        /// </summary>
        /// <param name="connectionFactory">The connection factory.</param>
        /// <param name="connectionSettings">The connection settings.</param>
        /// <param name="exchangeSettings">The exchange settings.</param>
        /// <param name="queueSettings">The queue settings.</param>
        public RMQPublisherByte(
            IRMQConnectionFactory connectionFactory,
            RMQConnectionSettings connectionSettings,
            RMQExchangeSettings exchangeSettings,
            RMQQueueSettings queueSettings = null)
            : base(connectionFactory, connectionSettings, exchangeSettings, queueSettings)
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
