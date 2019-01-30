// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Messaging.RMQ.Interfaces;
using Plato.Messaging.RMQ.Settings;
using System;

namespace Plato.Messaging.RMQ
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Messaging.RMQ.RMQPublisherSubscriber" />
    /// <seealso cref="Plato.Messaging.RMQ.Interfaces.IRMQSubscriber" />
    public class RMQSubscriber : RMQConsumer, IRMQSubscriber
    {
        protected readonly RMQExchangeSettings _exchangeSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="RMQSubscriber"/> class.
        /// </summary>
        /// <param name="connectionFactory">The connection factory.</param>
        /// <param name="connectionSettings">The connection settings.</param>
        /// <param name="exchangeSettings">The exchange settings.</param>
        /// <param name="queueSettings">The queue settings.</param>
        public RMQSubscriber(
            IRMQConnectionFactory connectionFactory,
            RMQConnectionSettings connectionSettings,
            RMQExchangeSettings exchangeSettings,
            RMQQueueSettings queueSettings)
            : base(connectionFactory, connectionSettings, queueSettings)
        {
            _exchangeSettings = exchangeSettings;
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
