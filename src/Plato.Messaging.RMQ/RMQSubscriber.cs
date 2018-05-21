// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Messaging.RMQ.Interfaces;
using Plato.Messaging.RMQ.Settings;

namespace Plato.Messaging.RMQ
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Messaging.RMQ.RMQPublisherSubscriber" />
    /// <seealso cref="Plato.Messaging.RMQ.Interfaces.IRMQSubscriber" />
    public class RMQSubscriber : RMQPublisherSubscriber, IRMQSubscriber
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RMQSubscriber"/> class.
        /// </summary>
        /// <param name="connectionFactory">The connection factory.</param>
        /// <param name="connectionSettings">The connection settings.</param>
        /// <param name="exchangeSettings">The exchange settings.</param>
        /// <param name="queueSettings">The queue settings.</param>
        public RMQSubscriber(
            IRMQConnectionFactory connectionFactory,
            RMQConnectionSettings connectionSettings,
            RMQExchangeSettings exchangeSettings,
            RMQQueueSettings queueSettings)
            : base(connectionFactory, connectionSettings, exchangeSettings, queueSettings)
        {            
        }
    }
}
