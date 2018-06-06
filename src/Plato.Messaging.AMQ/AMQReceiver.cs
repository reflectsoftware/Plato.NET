﻿// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Apache.NMS;
using Apache.NMS.Util;
using Plato.Messaging.AMQ.Interfaces;
using Plato.Messaging.AMQ.Settings;
using Plato.Messaging.Enums;
using System;
using System.Threading;

namespace Plato.Messaging.AMQ
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Messaging.AMQ.AMQReceiverSender" />
    /// <seealso cref="Plato.Messaging.AMQ.Interfaces.IAMQReceiver" />
    public class AMQReceiver: AMQReceiverSender, IAMQReceiver
    {
        private readonly TimeoutException _timeoutException;
        private readonly AMQDestinationSettings _destination;
        private IMessageConsumer _consumer;

        /// <summary>
        /// Gets or sets the mode.
        /// </summary>
        /// <value>
        /// The mode.
        /// </value>
        public ConsumerMode Mode { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AMQReceiver" /> class.
        /// </summary>
        /// <param name="connectionFactory">The connection factory.</param>
        /// <param name="connectionSettings">The connection settings.</param>
        /// <param name="destination">The destination.</param>
        public AMQReceiver(IAMQConnectionFactory connectionFactory, AMQConnectionSettings connectionSettings, AMQDestinationSettings destination) : base(connectionFactory, connectionSettings)
        {
            Mode = ConsumerMode.OnNoMessage_Timeout;
            _timeoutException = new TimeoutException();
            _destination = destination;
            _consumer = null;            
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public override void Close()
        {
            if (_consumer != null)
            {
                _consumer.Close();
                _consumer.Dispose();
                _consumer = null;
            }

            base.Close();
        }

        /// <summary>
        /// Gets the delivery mode.
        /// </summary>
        /// <value>
        /// The delivery mode.
        /// </value>
        protected override AcknowledgementMode AcknowledgementMode
        {
            get { return _destination.AckMode; }
        }

        /// <summary>
        /// Clears the cache buffer.
        /// </summary>
        public void ClearCacheBuffer()
        {
            if (_session != null)
            {
                try
                {
                    _session.Recover();
                }
                catch (Exception ex)
                {
                    Close();

                    var newException = AMQExceptionHandler.ExceptionHandler(_connection, ex);
                    if (newException != null)
                    {
                        throw newException;
                    }

                    throw;
                }
            }
        }

        /// <summary>
        /// Receives the message.
        /// </summary>
        /// <param name="msecTimeout">The msec timeout.</param>
        /// <returns></returns>
        public IMessage ReceiveMessage(int msecTimeout = Timeout.Infinite)
        {
            try
            {
                Open();

                if (_consumer == null)
                {
                    var destination = SessionUtil.GetDestination(_session, _destination.Path);
                    if (_destination.Durable && destination.IsTopic)
                    {
                        _consumer = _session.CreateDurableConsumer((ITopic)destination, _destination.SubscriberId, _destination.Selector, false);
                    }
                    else
                    {
                        _consumer = _session.CreateConsumer(destination, _destination.Selector, false);
                    }
                }

                var message = (IMessage)null;
                if (msecTimeout != Timeout.Infinite)
                {
                    message = _consumer.Receive(TimeSpan.FromMilliseconds(msecTimeout));
                }
                else
                {
                    message = _consumer.Receive();
                }

                if (message == null && Mode == ConsumerMode.OnNoMessage_Timeout)
                {
                    throw _timeoutException;
                }

                return message;
            }
            catch (TimeoutException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Close();

                var newException = AMQExceptionHandler.ExceptionHandler(_connection, ex);
                if (newException != null)
                {
                    throw newException;
                }

                throw;
            }
        }

        /// <summary>
        /// Receives the result.
        /// </summary>
        /// <param name="msecTimeout">The msec timeout.</param>
        /// <returns></returns>
        public AMQReceiverResult ReceiveResult(int msecTimeout = Timeout.Infinite)
        {
            var message = ReceiveMessage(msecTimeout);
            return message != null ? new AMQReceiverResult(message) : null;
        }
    }
}
