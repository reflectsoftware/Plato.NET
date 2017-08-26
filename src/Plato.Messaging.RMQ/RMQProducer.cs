// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Messaging.Enums;
using Plato.Messaging.Exceptions;
using Plato.Messaging.Interfaces;
using Plato.Messaging.RMQ.Interfaces;
using Plato.Messaging.RMQ.Settings;
using System;

namespace Plato.Messaging.RMQ
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Messaging.RMQ.RMQQueue" />
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
            try
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

                    action?.Invoke(senderProperties);

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
                    var newException = RMQExceptionHandler.ExceptionHandler(_connection, ex);
                    if (newException != null)
                    {
                        throw newException;
                    }

                    throw;
                }
            }
            catch (MessageException ex)
            {
                switch (ex.ExceptionCode)
                {
                    case MessageExceptionCode.LostConnection:
                        Close();
                        break;
                }

                throw;
            }
        }
    }
}
