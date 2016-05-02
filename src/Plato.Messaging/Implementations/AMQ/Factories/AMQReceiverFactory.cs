// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Messaging.Implementations.AMQ.Interfaces;
using Plato.Messaging.Implementations.AMQ.Settings;

namespace Plato.Messaging.Implementations.AMQ.Factories
{    
    /// <summary>
    /// 
    /// </summary>
    public class AMQReceiverFactory : IAMQReceiverFactory
    {
        private readonly IAMQConnectionFactory _connectionFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="AMQReceiverFactory" /> class.
        /// </summary>
        /// <param name="connectionFactory">The connection factory.</param>
        public AMQReceiverFactory(IAMQConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        /// <summary>
        /// Creates the specified settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="connectionName">Name of the connection.</param>
        /// <returns></returns>
        public IAMQReceiver Create(AMQDestinationSettings settings, string connectionName)
        {
            return new AMQReceiver(_connectionFactory, connectionName, settings);
        }
    }
}
