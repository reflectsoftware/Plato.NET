// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Messaging.Implementations.RMQ.Interfaces;
using Plato.Messaging.Implementations.RMQ.Settings;

namespace Plato.Messaging.Implementations.RMQ.Factories
{
    /// <summary>
    /// 
    /// </summary>
    public class RMQProducerFactory : IRMQProducerFactory
    {
        private readonly IRMQConnectionFactory _connectionFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="RMQProducerFactory"/> class.
        /// </summary>
        /// <param name="connectionFactory">The connection factory.</param>
        public RMQProducerFactory(IRMQConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        /// <summary>
        /// Creates the byte.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="connectionName">Name of the connection.</param>
        /// <returns></returns>
        public IRMQProducerByte CreateByte(RMQQueueSettings settings, string connectionName)
        {
            return new RMQProducerByte(_connectionFactory, connectionName, settings);
        }

        /// <summary>
        /// Creates the text.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="connectionName">Name of the connection.</param>
        /// <returns></returns>
        public IRMQProducerText CreateText(RMQQueueSettings settings, string connectionName)
        {
            return new RMQProducerText(_connectionFactory, connectionName, settings);
        }
    }
}
