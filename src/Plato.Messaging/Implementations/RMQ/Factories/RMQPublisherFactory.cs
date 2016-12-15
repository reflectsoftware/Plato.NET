﻿// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Messaging.Implementations.RMQ.Interfaces;
using Plato.Messaging.Implementations.RMQ.Settings;

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

        /// <summary>
        /// Creates the byte.
        /// </summary>
        /// <param name="connectionName">Name of the connection.</param>
        /// <param name="exchangeSettings">The exchange settings.</param>
        /// <param name="queueSettings">The queue settings.</param>
        /// <returns></returns>
        public IRMQPublisherByte CreateByte(
            string connectionName, 
            RMQExchangeSettings exchangeSettings, 
            RMQQueueSettings queueSettings)
        {
            return new RMQPublisherByte(_connectionFactory, connectionName, exchangeSettings, queueSettings);
        }

        /// <summary>
        /// Creates the text.
        /// </summary>
        /// <param name="connectionName">Name of the connection.</param>
        /// <param name="exchangeSettings">The exchange settings.</param>
        /// <param name="queueSettings">The queue settings.</param>
        /// <returns></returns>
        public IRMQPublisherText CreateText(
            string connectionName,
            RMQExchangeSettings exchangeSettings,
            RMQQueueSettings queueSettings)
        {
            return new RMQPublisherText(_connectionFactory, connectionName, exchangeSettings, queueSettings);
        }
    }
}
