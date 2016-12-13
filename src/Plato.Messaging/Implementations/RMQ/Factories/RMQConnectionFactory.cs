// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Messaging.Implementations.RMQ.Interfaces;
using Plato.Messaging.Interfaces;
using RabbitMQ.Client;

namespace Plato.Messaging.Implementations.RMQ.Factories
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Messaging.Implementations.AMQ.Interfaces.IRMQConnectionFactory" />
    public class RMQConnectionFactory : IRMQConnectionFactory
    {
        private readonly IRMQConnectionManager _connectionManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="RMQConnectionFactory" /> class.
        /// </summary>
        /// <param name="connectionManager">The connection manager.</param>
        public RMQConnectionFactory(IRMQConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }

        /// <summary>
        /// Declares the connection.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        /// <exception cref="MessageException">
        /// </exception>
        public IConnection CreateConnection(string name)
        {
            return _connectionManager.DeclareConnection(name);
        }

        /// <summary>
        /// Removes the connection.
        /// </summary>
        /// <param name="name">The name.</param>
        public void RemoveConnection(string name)
        {
            _connectionManager.RemoveConnection(name);
        }
    }
}
