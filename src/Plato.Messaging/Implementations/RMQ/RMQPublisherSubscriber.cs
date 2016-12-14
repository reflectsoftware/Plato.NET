// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Messaging.Implementations.RMQ.Interfaces;
using Plato.Messaging.Implementations.RMQ.Settings;
using System;
using System.Collections.Generic;

namespace Plato.Messaging.Implementations.RMQ
{
    public abstract class RMQPublisherSubscriber : RMQReceiverSender
    {
        protected readonly RMQExchangeSettings _exchangeSettings;
        protected readonly RMQQueueSettings _queueSettings;
        protected readonly List<string> _routingKeys;

        public RMQPublisherSubscriber(
            IRMQConnectionFactory connctionFactory, 
            string connectionName,
            RMQExchangeSettings exchangeSettings,
            RMQQueueSettings queueSettings = null,
            IEnumerable<string> routingKeys = null) 
            : base(connctionFactory, connectionName)
        {
            _exchangeSettings = exchangeSettings;
            _queueSettings = queueSettings;
            _routingKeys = new List<string>(routingKeys ?? new[] { string.Empty });
        }

        public RMQPublisherSubscriber(
            IRMQConnectionFactory connctionFactory,
            string connectionName,
            RMQExchangeSettings exchangeSettings,
            RMQQueueSettings queueSettings = null,
            string routingKey = "")
            : this(connctionFactory, connectionName, exchangeSettings, queueSettings, new string[] { routingKey })
        {
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

                    foreach (var routingKey in _routingKeys)
                    {
                        _channel.QueueBind(
                            _queueSettings.QueueName,
                            _exchangeSettings.ExchangeName, 
                            routingKey, 
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
