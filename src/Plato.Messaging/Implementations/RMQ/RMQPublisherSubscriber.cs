// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Messaging.Implementations.RMQ.Interfaces;
using Plato.Messaging.Implementations.RMQ.Settings;
using System;

namespace Plato.Messaging.Implementations.RMQ
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Messaging.Implementations.RMQ.RMQReceiverSender" />
    public abstract class RMQPublisherSubscriber : RMQReceiverSender
    {
        protected readonly RMQExchangeSettings _exchangeSettings;
        protected readonly RMQQueueSettings _queueSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="RMQPublisherSubscriber"/> class.
        /// </summary>
        /// <param name="connectionFactory">The connection factory.</param>
        /// <param name="connectionName">Name of the connection.</param>
        /// <param name="exchangeSettings">The exchange settings.</param>
        /// <param name="queueSettings">The queue settings.</param>
        public RMQPublisherSubscriber(
            IRMQConnectionFactory connectionFactory, 
            string connectionName,
            RMQExchangeSettings exchangeSettings,
            RMQQueueSettings queueSettings = null)
            : base(connectionFactory, connectionName)
        {
            _exchangeSettings = exchangeSettings;
            _queueSettings = queueSettings;
        }

        /// <summary>
        /// Opens the channel.
        /// </summary>
        protected override void OpenChannel()
        {
            base.OpenChannel();

            try
            {
                _channel.ExchangeDeclare(
                    _exchangeSettings.ExchangeName,
                    _exchangeSettings.Type,
                    _exchangeSettings.Durable,
                    _exchangeSettings.AutoDelete,
                    _exchangeSettings.Arguments);

                if (_queueSettings != null)
                {
                    _channel.QueueDeclare(
                        queue: _queueSettings.QueueName,
                        durable: _queueSettings.Durable,
                        exclusive: _queueSettings.Exclusive,
                        autoDelete: _queueSettings.AutoDelete,
                        arguments: _queueSettings.Arguments);

                    foreach (var routingKey in _queueSettings.RoutingKeys)
                    {
                        _channel.QueueBind(
                            _queueSettings.QueueName,
                            _exchangeSettings.ExchangeName, 
                            routingKey ?? string.Empty, 
                            null);
                    }
                }
            }
            catch (Exception ex)
            {
                var newException = RMQExceptionHandler.ExceptionHandler(_connection, ex);
                if (newException != null)
                {
                    throw newException;
                }

                throw;
            }
        }
    }
}
