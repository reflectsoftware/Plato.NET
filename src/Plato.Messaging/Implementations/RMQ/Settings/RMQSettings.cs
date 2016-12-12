// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Messaging.Implementations.RMQ.Interfaces;
using Plato.Messaging.Interfaces;

namespace Plato.Messaging.Implementations.RMQ.Settings
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Messaging.Interfaces.IMessageSettings" />
    public class RMQSettings : IMessageSettings
    {
        /// <summary>
        /// Gets or sets the name of the connection.
        /// </summary>
        /// <value>
        /// The name of the connection.
        /// </value>
        public string ConnectionName { get; set; }

        /// <summary>
        /// Gets or sets the connection factory.
        /// </summary>
        /// <value>
        /// The connection factory.
        /// </value>
        public IRMQConnectionFactory ConnectionFactory { get; set; }

        /// <summary>
        /// Gets or sets the exchange settings.
        /// </summary>
        /// <value>
        /// The exchange settings.
        /// </value>
        public RMQExchangeSettings ExchangeSettings { get; set; }

        /// <summary>
        /// Gets or sets the queue settings.
        /// </summary>
        /// <value>
        /// The queue settings.
        /// </value>
        public RMQQueueSettings QueueSettings { get; set; }
    }
}
