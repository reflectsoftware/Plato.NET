// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Messaging.Implementations.RMQ.Interfaces;
using Plato.Messaging.Implementations.RMQ.Settings;
using Plato.Messaging.Interfaces;
using System;

namespace Plato.Messaging.Implementations.RMQ
{
    public abstract class RMQProducer : RMQQueue
    {
        public RMQProducer(
            IRMQConnectionFactory connctionFactory, 
            string connectionName,             
            RMQQueueSettings settings) 
            : base(connctionFactory, connectionName, settings)
        {
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
                props.Persistent = _queueSettings.Persistent;

                var senderProperties = new RMQSenderProperties()
                {
                    Properties = props,
                    Exchange = string.Empty,
                    RoutingKey = _queueSettings.QueueName,
                    Mandatory = false,
                };

                if (action != null)
                {
                    action(senderProperties);
                }

                _channel.BasicPublish(
                    string.Empty, 
                    _queueSettings.QueueName, 
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
