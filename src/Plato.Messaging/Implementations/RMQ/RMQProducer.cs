// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Messaging.Exceptions;
using Plato.Messaging.Implementations.RMQ.Interfaces;
using Plato.Messaging.Implementations.RMQ.Settings;
using Plato.Messaging.Interfaces;
using RabbitMQ.Client.Exceptions;
using System;
using System.IO;

namespace Plato.Messaging.Implementations.RMQ
{
    public abstract class RMQProducer : RMQQueue
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RMQProducer" /> class.
        /// </summary>
        /// <param name="connectionFactory">The connection factory.</param>
        /// <param name="connectionSettings">The connection settings.</param>
        /// <param name="queueSettings">The settings.</param>
        public RMQProducer(
            IRMQConnectionFactory connectionFactory,
            RMQConnectionSettings connectionSettings,
            RMQQueueSettings queueSettings) 
            : base(connectionFactory, connectionSettings, queueSettings)
        {
        }

        /// <summary>
        /// Sends the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="action">The action.</param>
        protected void _Send(byte[] data, Action<ISenderProperties> action = null)
        {
            while (true)
            {
                if (!IsOpen())
                {
                    Open();
                }

                try
                {
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

                    return;
                }
                catch (Exception ex)
                {
                    if ((ex is AlreadyClosedException) || (ex is IOException))
                    {
                        // retry
                        continue;
                    }

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
}
