// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Messaging.Implementations.RMQ.Interfaces;
using Plato.Messaging.Implementations.RMQ.Settings;
using System;

namespace Plato.Messaging.Implementations.RMQ
{
    public abstract class RMQPublisherSubscriber : RMQReceiverSender
    {
        protected readonly RMQExchangeSettings _exchangeSettings;
        protected readonly RMQQueueSettings _queueSettings;
        
        public RMQPublisherSubscriber(
            IRMQConnectionFactory connectionFactory,
            RMQConnectionSettings connectionSettings,
            RMQExchangeSettings exchangeSettings,
            RMQQueueSettings queueSettings = null)
            : base(connectionFactory, connectionSettings)
        {
            _exchangeSettings = exchangeSettings;
            _queueSettings = queueSettings;
        }
        
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
