// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Messaging.Implementations.RMQ.Interfaces;
using Plato.Messaging.Implementations.RMQ.Settings;
using System.Collections.Generic;

namespace Plato.Messaging.Implementations.RMQ.Factories
{
    /// <summary>
    /// 
    /// </summary>
    public class RMQPublisherFactory : IRMQPublisherFactory
    {
        private readonly IRMQConnectionFactory _connectionFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="RMQProducerFactory"/> class.
        /// </summary>
        /// <param name="connectionFactory">The connection factory.</param>
        public RMQPublisherFactory(IRMQConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public IRMQPublisherByte CreateByte(
            string connectionName, 
            RMQExchangeSettings exchangeSettings, 
            RMQQueueSettings queueSettings,
            IEnumerable<string> routingKeys = null)
        {
            return new RMQPublisherByte(_connectionFactory, connectionName, exchangeSettings, queueSettings, routingKeys);
        }

        public IRMQPublisherByte CreateByte(
            string connectionName,
            RMQExchangeSettings exchangeSettings,
            RMQQueueSettings queueSettings,
            string routingKey = "")
        {
            return new RMQPublisherByte(_connectionFactory, connectionName, exchangeSettings, queueSettings, routingKey);
        }

        public IRMQPublisherText CreateText(
            string connectionName,
            RMQExchangeSettings exchangeSettings,
            RMQQueueSettings queueSettings,
            IEnumerable<string> routingKeys = null)
        {
            return new RMQPublisherText(_connectionFactory, connectionName, exchangeSettings, queueSettings, routingKeys);
        }

        public IRMQPublisherText CreateText(
            string connectionName,
            RMQExchangeSettings exchangeSettings,
            RMQQueueSettings queueSettings,
            string routingKey = "")
        {
            return new RMQPublisherText(_connectionFactory, connectionName, exchangeSettings, queueSettings, routingKey);
        }
    }
}
