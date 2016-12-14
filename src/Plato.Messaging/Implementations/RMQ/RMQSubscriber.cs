// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Messaging.Implementations.RMQ.Interfaces;
using Plato.Messaging.Implementations.RMQ.Settings;
using RabbitMQ.Client.Events;
using System;
using System.Threading;

namespace Plato.Messaging.Implementations.RMQ
{

    public class RMQSubscriber : RMQPublisherSubscriber, IRMQSubscriber
    {
        protected RMQBasicConsumer _queueingConsumer;

        public RMQSubscriber(
            IRMQConnectionFactory connctionFactory,
            string connectionName,
            RMQExchangeSettings exchangeSettings,
            RMQQueueSettings queueSettings)
            : base(connctionFactory, connectionName, exchangeSettings, queueSettings)
        {
        }

        private void OnCancelConsumer(object sender, ConsumerEventArgs args)
        {
            _queueingConsumer = null;
        }

        public override void Open()
        {
            try
            {
                if (IsOpen() && _queueingConsumer != null)
                {
                    return;
                }

                base.Open();

                try
                {
                    _queueingConsumer = new RMQBasicConsumer(_channel);

                    _channel.BasicConsume(_queueSettings.QueueName,
                        _queueSettings.ConsumerSettings.NoAck,
                        _queueSettings.ConsumerSettings.Tag,
                        _queueSettings.ConsumerSettings.NoLocal,
                        _queueSettings.ConsumerSettings.Exclusive,
                        _queueSettings.ConsumerSettings.Arguments,
                        _queueingConsumer);

                    _queueingConsumer.ConsumerCancelled += OnCancelConsumer;
                }
                catch (Exception)
                {
                    _queueingConsumer = null;
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

        public void ClearCacheBuffer()
        {
            try
            {
                if (IsOpen())
                {
                    CloseChannel();
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

        protected BasicDeliverEventArgs _Receive(int msecTimeout = Timeout.Infinite)
        {
            try
            {
                Open();

                var deliverArgs = (BasicDeliverEventArgs)null;
                var status = _queueingConsumer.Queue.Dequeue(msecTimeout, out deliverArgs);
                if (status)
                {
                    return deliverArgs;
                }

                throw _TimeoutException;
            }
            catch (TimeoutException)
            {
                throw;
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
