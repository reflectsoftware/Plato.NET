// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Messaging.Implementations.RMQ.Settings;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Plato.Messaging.Implementations.RMQ
{

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Messaging.Implementations.RMQ.RMQReceiverSender" />
    public abstract class RMQReceiverBase : RMQReceiverSender
    {
        private readonly List<string> _routingKeys;
        protected RMQBasicConsumer _queueingConsumer;

        /// <summary>
        /// Initializes a new instance of the <see cref="RMQReceiverBase"/> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="routingKeys">The routing keys.</param>
        public RMQReceiverBase(RMQSettings settings, IEnumerable<string> routingKeys = null) : base(settings)
        {
            _routingKeys = new List<string>(routingKeys ?? new[] { string.Empty });
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RMQReceiverBase"/> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="routingKey">The routing key.</param>
        public RMQReceiverBase(RMQSettings settings, string routingKey = "") : this(settings, new string[] { routingKey })
        {
        }

        /// <summary>
        /// Called when [cancel consumer].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="ConsumerEventArgs"/> instance containing the event data.</param>
        private void OnCancelConsumer(object sender, ConsumerEventArgs args)
        {
            _queueingConsumer = null;
        }

        /// <summary>
        /// Opens this instance.
        /// </summary>
        public override void Open()
        {
            try
            {
                if (IsOpen() && _queueingConsumer != null)
                {
                    return;
                }

                try
                {
                    OpenConnection();
                    OpenChannel();

                    _channel.QueueDeclare(_settings.QueueSettings.Name,
                        _settings.QueueSettings.Durable,
                        _settings.QueueSettings.Exclusive,
                        _settings.QueueSettings.AutoDelete,
                        _settings.QueueSettings.Arguments);

                    foreach (var routingKey in _routingKeys)
                    {
                        _channel.QueueBind(_settings.QueueSettings.Name, _settings.ExchangeSettings.Name, routingKey, null);
                    }
                }
                catch (Exception)
                {
                    Close();
                    throw;
                }

                try
                {
                    _queueingConsumer = new RMQBasicConsumer(_channel);
                    _channel.BasicConsume(_settings.QueueSettings.Name,
                        _settings.QueueSettings.ConsumerSettings.NoAck,
                        _settings.QueueSettings.ConsumerSettings.Tag,
                        _settings.QueueSettings.ConsumerSettings.NoLocal,
                        _settings.QueueSettings.ConsumerSettings.Exclusive,
                        _settings.QueueSettings.ConsumerSettings.Arguments,
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

        /// <summary>
        /// Clears the cache buffer.
        /// </summary>
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

        /// <summary>
        /// Reads the specified msec timeout.
        /// </summary>
        /// <param name="msecTimeout">The msec timeout.</param>
        /// <returns></returns>
        protected BasicDeliverEventArgs _Receive(int msecTimeout = Timeout.Infinite)
        {
            try
            {
                Open();

                BasicDeliverEventArgs deliverArgs;
                var status = _queueingConsumer.Queue.Dequeue(msecTimeout, out deliverArgs);
                if (status)
                {
                    return deliverArgs; 
                }

                throw _timeoutException;
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
