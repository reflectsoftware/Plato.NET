// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Messaging.Implementations.RMQ.Interfaces;
using Plato.Messaging.Implementations.RMQ.Settings;
using Plato.Messaging.Interfaces;
using System;

namespace Plato.Messaging.Implementations.RMQ
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Messaging.Implementations.RMQ.RMQQueue" />
    public abstract class RMQProducer : RMQQueue
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RMQProducer"/> class.
        /// </summary>
        /// <param name="connectionFactory">The conndction factory.</param>
        /// <param name="connectionName">Name of the connection.</param>
        /// <param name="settings">The settings.</param>
        public RMQProducer(
            IRMQConnectionFactory connectionFactory, 
            string connectionName,             
            RMQQueueSettings settings) 
            : base(connectionFactory, connectionName, settings)
        {
        }

        /// <summary>
        /// Sends the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="action">The action.</param>
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
