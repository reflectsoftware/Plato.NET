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
        /// <param name="connectionSettings">The connection settings.</param>
        /// <param name="queueSettings">The settings.</param>
        /// <returns></returns>
        public IRMQProducerByte CreateByte(RMQConnectionSettings connectionSettings, RMQQueueSettings queueSettings)
        {
            return new RMQProducerByte(_connectionFactory, connectionSettings, queueSettings);
        }

        /// <summary>
        /// Creates the text.
        /// </summary>
        /// <param name="connectionSettings">The connection settings.</param>
        /// <param name="queueSettings">The queue settings.</param>
        /// <returns></returns>
        public IRMQProducerText CreateText(RMQConnectionSettings connectionSettings, RMQQueueSettings queueSettings)
        {
            return new RMQProducerText(_connectionFactory, connectionSettings, queueSettings);
        }
    }
}
