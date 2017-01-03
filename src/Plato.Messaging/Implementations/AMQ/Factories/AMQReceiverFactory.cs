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
        /// <param name="connectionSettings">The connection settings.</param>
        /// <param name="destinationSettings">The settings.</param>
        /// <returns></returns>
        public IAMQReceiver Create(AMQConnectionSettings connectionSettings, AMQDestinationSettings destinationSettings)
        {
            return new AMQReceiver(_connectionFactory, connectionSettings, destinationSettings);
        }

        /// <summary>
        /// Creates the text.
        /// </summary>
        /// <param name="connectionSettings">The connection settings.</param>
        /// <param name="destinationSettings">The destination settings.</param>
        /// <returns></returns>
        public IAMQReceiverText CreateText(AMQConnectionSettings connectionSettings, AMQDestinationSettings destinationSettings)
        {
            return new AMQReceiverText(_connectionFactory, connectionSettings, destinationSettings);
        }

        /// <summary>
        /// Creates the bytes.
        /// </summary>
        /// <param name="connectionSettings">The connection settings.</param>
        /// <param name="destinationSettings">The destination settings.</param>
        /// <returns></returns>
        public IAMQReceiverBytes CreateBytes(AMQConnectionSettings connectionSettings, AMQDestinationSettings destinationSettings)
        {
            return new AMQReceiverBytes(_connectionFactory, connectionSettings, destinationSettings);
        }
    }
}
