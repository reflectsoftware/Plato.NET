// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Messaging.Implementations.RMQ.Interfaces;
using Plato.Messaging.Implementations.RMQ.Settings;
using Plato.Messaging.Interfaces;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;

namespace Plato.Messaging.Implementations.RMQ
{

    public class RMQPublisher: RMQPublisherSubscriber, IRMQPublisher
    {
        public Action<BasicReturnEventArgs> OnReturn { get; set; }

        public RMQPublisher(
            IRMQConnectionFactory connctionFactory, 
            string connectionName,
            RMQExchangeSettings exchangeSettings,
            RMQQueueSettings queueSettings = null,
            IEnumerable<string> routingKeys = null) 
            : base(connctionFactory, connectionName, exchangeSettings, queueSettings, routingKeys)
        {
        }

        public RMQPublisher(
            IRMQConnectionFactory connctionFactory,
            string connectionName,
            RMQExchangeSettings exchangeSettings,
            RMQQueueSettings queueSettings = null,
            string routingKeys = "")
            : base(connctionFactory, connectionName, exchangeSettings, queueSettings, routingKeys)
        {
        }

        private void DoOnReturn(object sender, BasicReturnEventArgs args)
        {
            OnReturn?.Invoke(args);
        }

        public override void Open()
        {
            base.Open();
            _channel.BasicReturn += DoOnReturn;
        }

        protected void _Send(byte[] data, Action<ISenderProperties> action = null)
        {
            try
            {
                if (!IsOpen())
                {
                    Open();
                }

                var props = _channel.CreateBasicProperties();

                var senderProperties = new RMQSenderProperties()
                {
                    Properties = props,
                    Exchange = _exchangeSettings.ExchangeName,
                    RoutingKey = string.Empty,
                    Mandatory = false,
                };

                if (action != null)
                {
                    action(senderProperties);
                }

                _channel.BasicPublish(
                    senderProperties.Exchange, 
                    senderProperties.RoutingKey, 
                    senderProperties.Mandatory, 
                    props, 
                    data);
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
