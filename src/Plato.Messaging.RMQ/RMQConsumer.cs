﻿// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Messaging.Enums;
using Plato.Messaging.Exceptions;
using Plato.Messaging.RMQ.Interfaces;
using Plato.Messaging.RMQ.Settings;
using RabbitMQ.Client.Events;
using System;
using System.Threading;

namespace Plato.Messaging.RMQ
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Messaging.RMQ.RMQQueue" />
    /// <seealso cref="Plato.Messaging.RMQ.Interfaces.IRMQConsumer" />
    public abstract class RMQConsumer : RMQQueue, IRMQConsumer
    {
        protected RMQBasicConsumer _queueingConsumer;

        /// <summary>
        /// Gets or sets the mode.
        /// </summary>
        /// <value>
        /// The mode.
        /// </value>
        public ConsumerMode Mode { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RMQConsumer"/> class.
        /// </summary>
        /// <param name="connectionFactory">The connection factory.</param>
        /// <param name="connectionSettings">The connection settings.</param>
        /// <param name="queueSettings">The queue settings.</param>
        public RMQConsumer(
            IRMQConnectionFactory connectionFactory,
            RMQConnectionSettings connectionSettings,
            RMQQueueSettings queueSettings) 
            : base(connectionFactory, connectionSettings, queueSettings)
        {
            Mode = ConsumerMode.OnNoMessage_Timeout;
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

        /// <summary>
        /// Clears the cache buffer.
        /// </summary>
        public void ClearCacheBuffer()
        {
            try
            {
                Close();
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
        /// Receives the specified msec timeout.
        /// </summary>
        /// <param name="msecTimeout">The msec timeout.</param>
        /// <returns></returns>
        protected BasicDeliverEventArgs _Receive(int msecTimeout = Timeout.Infinite)
        {
            try
            {
                try
                {
                    Open();

                    // there's a possibility that the _queueingConsumer may be set to null on a different thread if the connection is closed
                    // by assigning to a variable at the point of state, we must have a valid queueingConsumer even though the connection is closed.
                    var queueingConsumer = _queueingConsumer;
                    if(queueingConsumer == null)
                    {                        
                        return null;
                    }

                    var deliverArgs = (BasicDeliverEventArgs)null;
                    var status = queueingConsumer.Queue.Dequeue(msecTimeout, out deliverArgs);
                    if (status || Mode == ConsumerMode.OnNoMessage_ReturnNull)
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
