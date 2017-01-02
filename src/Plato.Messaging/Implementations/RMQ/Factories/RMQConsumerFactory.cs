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
    public class RMQConsumerFactory : IRMQConsumerFactory
    {
        private readonly IRMQConnectionFactory _connectionFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="RMQConsumerFactory"/> class.
        /// </summary>
        /// <param name="connectionFactory">The connection factory.</param>
        public RMQConsumerFactory(IRMQConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        /// <summary>
        /// Creates the byte.
        /// </summary>
        /// <param name="connectionSettings">The connection settings.</param>
        /// <param name="queueSettings">The settings.</param>
        /// <returns></returns>
        public IRMQConsumerByte CreateByte(RMQConnectionSettings connectionSettings, RMQQueueSettings queueSettings)
        {
            return new RMQConsumerByte(_connectionFactory, connectionSettings, queueSettings);
        }

        /// <summary>
        /// Creates the text.
        /// </summary>
        /// <param name="connectionSettings">The connection settings.</param>
        /// <param name="queueSettings">The queue settings.</param>
        /// <returns></returns>
        public IRMQConsumerText CreateText(RMQConnectionSettings connectionSettings, RMQQueueSettings queueSettings)
        {
            return new RMQConsumerText(_connectionFactory, connectionSettings, queueSettings);
        }
    }
}
