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
    public class AMQSenderFactory : IAMQSenderFactory
    {
        private readonly IAMQConnectionFactory _connectionFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="AMQSenderFactory"/> class.
        /// </summary>
        /// <param name="connectionFactory">The connection factory.</param>
        public AMQSenderFactory(IAMQConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        /// <summary>
        /// Creates the specified settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="connectionName">Name of the connection.</param>
        /// <returns></returns>
        public IAMQSender Create(AMQDestinationSettings settings, string connectionName)
        {
            return new AMQSender(_connectionFactory, connectionName, settings);
        }

        /// <summary>
        /// Creates the text.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="connectionName">Name of the connection.</param>
        /// <returns></returns>
        public IAMQSenderText CreateText(AMQDestinationSettings settings, string connectionName)
        {
            return new AMQSenderText(_connectionFactory, connectionName, settings);
        }

        /// <summary>
        /// Creates the bytes.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="connectionName">Name of the connection.</param>
        /// <returns></returns>
        public IAMQSenderBytes CreateBytes(AMQDestinationSettings settings, string connectionName)
        {
            return new AMQSenderBytes(_connectionFactory, connectionName, settings);
        }
    }
}
