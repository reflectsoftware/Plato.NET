// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Messaging.Implementations.RMQ.Settings;
using Plato.Messaging.Interfaces;
using RabbitMQ.Client.Events;
using System;
using System.Threading;

namespace Plato.Messaging.Implementations.RMQ
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Messaging.RMQ.RMQReceiverSender" />
    /// <seealso cref="Plato.Messaging.Interfaces.IMessageRPCRequester" />
    public class RMQRPCRequester : RMQReceiverSender, IMessageRPCRequester<byte[]>
    {
        protected RMQBasicConsumer _replyQueueingConsumer;
        protected string _replyQueueName;

        /// <summary>
        /// Initializes a new instance of the <see cref="RMQRPCRequester"/> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        public RMQRPCRequester(RMQSettings settings): base(settings)
        {
        }

        /// <summary>
        /// Opens this instance.
        /// </summary>
        public override void Open()
        {
            try
            {
                if (IsOpen() && _replyQueueingConsumer != null)
                {
                    return;
                }

                try
                {
                    base.Open();

                    _replyQueueName = _channel.QueueDeclare();
                }
                catch (Exception)
                {
                    Close();
                    throw;
                }

                try
                {
                    _replyQueueingConsumer = new RMQBasicConsumer(_channel);
                    _channel.BasicConsume(_replyQueueName, true, _replyQueueingConsumer);
                }
                catch (Exception)
                {
                    _replyQueueingConsumer = null;
                    throw;
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

        /// <summary>
        /// Requests the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="action">The action.</param>
        /// <param name="msecTimeout">The msec timeout.</param>
        /// <returns></returns>
        public byte[] Request(byte[] data, Action<ISenderProperties> action = null, int msecTimeout = Timeout.Infinite)
        {
            if (!IsOpen())
            {
                Open();
            }

            try
            {
                var requestProps = _channel.CreateBasicProperties();
                requestProps.ReplyTo = _replyQueueName;
                requestProps.CorrelationId = Guid.NewGuid().ToString();

                var sendProperties = new RMQSenderProperties()
                {
                    Properties = requestProps,
                    Exchange = _settings.ExchangeSettings != null ? _settings.ExchangeSettings.Name : string.Empty,
                    RoutingKey = string.Empty,
                    Mandatory = false,
                };

                if (action != null)
                {
                    action(sendProperties);
                }

                _channel.BasicPublish(sendProperties.Exchange, sendProperties.RoutingKey, sendProperties.Mandatory, requestProps, data);

                while (true)
                {
                    BasicDeliverEventArgs deliverArgs;
                    var status = _replyQueueingConsumer.Queue.Dequeue(msecTimeout, out deliverArgs);
                    if (status)
                    {
                        if (deliverArgs.BasicProperties.CorrelationId != requestProps.CorrelationId)
                        {
                            continue;
                        }

                        return deliverArgs.Body;
                    }

                    throw _timeoutException;
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
