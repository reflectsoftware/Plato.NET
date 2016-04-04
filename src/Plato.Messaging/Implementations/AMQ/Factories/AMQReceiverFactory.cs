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
        private readonly string _connectionName;

        /// <summary>
        /// Initializes a new instance of the <see cref="AMQReceiverFactory"/> class.
        /// </summary>
        /// <param name="connectionFactory">The connection factory.</param>
        /// <param name="connectionName">Name of the connection.</param>
        public AMQReceiverFactory(IAMQConnectionFactory connectionFactory, string connectionName)
        {
            _connectionFactory = connectionFactory;
            _connectionName = connectionName;
        }

        /// <summary>
        /// Creates the specified settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns></returns>
        public IAMQReceiver Create(AMQDestinationSettings settings)
        {
            return new AMQReceiver(_connectionFactory, _connectionName, settings);
        }
    }
}
