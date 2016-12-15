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
    /// <seealso cref="Plato.Messaging.Implementations.RMQ.Interfaces.IRMQConsumerFactory" />
    public class RMQConsumerFactory : IRMQConsumerFactory
    {
        private readonly IRMQConnectionFactory _connectionFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="RMQConsumerFactory" /> class.
        /// </summary>
        /// <param name="connectionFactory">The connection factory.</param>
        public RMQConsumerFactory(IRMQConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        /// <summary>
        /// Creates the byte.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="connectionName">Name of the connection.</param>
        /// <returns></returns>
        public IRMQConsumerByte CreateByte(RMQQueueSettings settings, string connectionName)
        {
            return new RMQConsumerByte(_connectionFactory, connectionName, settings);
        }

        /// <summary>
        /// Creates the text.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="connectionName">Name of the connection.</param>
        /// <returns></returns>
        public IRMQConsumerText CreateText(RMQQueueSettings settings, string connectionName)
        {
            return new RMQConsumerText(_connectionFactory, connectionName, settings);
        }
    }
}
