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
        /// <param name="connectionSettings">The connection settings.</param>
        /// <param name="destinationSettings">The settings.</param>
        /// <returns></returns>
        public IAMQSender Create(AMQConnectionSettings connectionSettings, AMQDestinationSettings destinationSettings)
        {
            return new AMQSender(_connectionFactory, connectionSettings, destinationSettings);
        }

        /// <summary>
        /// Creates the text.
        /// </summary>
        /// <param name="connectionSettings">The connection settings.</param>
        /// <param name="destinationSettings">The destination settings.</param>
        /// <returns></returns>
        public IAMQSenderText CreateText(AMQConnectionSettings connectionSettings, AMQDestinationSettings destinationSettings)
        {
            return new AMQSenderText(_connectionFactory, connectionSettings, destinationSettings);
        }

        /// <summary>
        /// Creates the bytes.
        /// </summary>
        /// <param name="connectionSettings">The connection settings.</param>
        /// <param name="destinationSettings">The destination settings.</param>
        /// <returns></returns>
        public IAMQSenderBytes CreateBytes(AMQConnectionSettings connectionSettings, AMQDestinationSettings destinationSettings)
        {
            return new AMQSenderBytes(_connectionFactory, connectionSettings, destinationSettings);
        }
    }
}
