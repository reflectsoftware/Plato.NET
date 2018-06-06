// Plato.NET
// Copyright (c) 2018 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Apache.NMS;
using Apache.NMS.Util;
using Plato.Messaging.AMQ.Interfaces;
using Plato.Messaging.AMQ.Settings;
using Plato.Messaging.Enums;
using Plato.Messaging.Exceptions;
using Plato.Messaging.Interfaces;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Threading.Tasks;

namespace Plato.Messaging.AMQ
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Messaging.AMQ.AMQReceiverSender" />
    /// <seealso cref="Plato.Messaging.AMQ.Interfaces.IAMQSender" />
    public class AMQSender : AMQReceiverSender, IAMQSender
    {
        private readonly AMQDestinationSettings _destination;
        private IMessageProducer _producer;

        /// <summary>
        /// Initializes a new instance of the <see cref="AMQSender" /> class.
        /// </summary>
        /// <param name="connectionFactory">The connection factory.</param>
        /// <param name="connectionSettings">The connection settings.</param>
        /// <param name="destination">The destination.</param>
        public AMQSender(IAMQConnectionFactory connectionFactory, AMQConnectionSettings connectionSettings, AMQDestinationSettings destination) : base(connectionFactory, connectionSettings)
        {
            _destination = destination;
            _producer = null;
        }              

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public override void Close()
        {
            if(_producer != null)
            {
                _producer.Close();
                _producer.Dispose();
                _producer = null;
            }

            base.Close();
        }

        /// <summary>
        /// Creates the map.
        /// </summary>
        /// <returns></returns>
        public IMapMessage CreateMapMessage()
        {
            while (true)
            {
                Open();

                try
                {
                    return _session.CreateMapMessage();
                }
                catch (Exception ex)
                {
                    Close();

                    if ((ex is NMSConnectionException) || (ex is IOException))
                    {
                        // retry
                        continue;
                    }

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
        /// Sends the specified data.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="createMessage">The create message.</param>
        public void Send(Action<ISenderProperties> action, Func<ISession, IMessage> createMessage)
        {
            try
            {
                var connectionRetry = true;
                while (true)
                {
                    Open();

                    try
                    {
                        if (_producer == null)
                        {
                            var destination = SessionUtil.GetDestination(_session, _destination.Path);
                            _producer = _session.CreateProducer(destination);
                        }

                        var senderProperties = new AMQSenderProperties()
                        {
                            Properties = new NameValueCollection()
                        };

                        action?.Invoke(senderProperties);

                        var request = createMessage(_session);
                        request.NMSCorrelationID = senderProperties.CorrelationId;
                        request.NMSTimeToLive = senderProperties.TTL;

                        foreach (var key in senderProperties.Properties.AllKeys)
                        {
                            request.Properties[key] = senderProperties.Properties[key];
                        }

                        _producer.Send(request);

                        return;
                    }
                    catch (Exception ex)
                    {
                        Close();

                        var newException = AMQExceptionHandler.ExceptionHandler(_connection, ex);
                        if (newException != null)
                        {
                            if (newException.ExceptionCode == MessageExceptionCode.LostConnection
                              && connectionRetry)
                            {
                                // try the reconnection cycle
                                connectionRetry = false;
                                continue;
                            }

                            throw newException;
                        }

                        throw;
                    }
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

        /// <summary>
        /// Sends the asynchronous.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="createMessage">The create message.</param>
        /// <returns></returns>
        public Task SendAsync(Action<ISenderProperties> action, Func<ISession, IMessage> createMessage)
        {
            Send(action, createMessage);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Sends the specified action.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="message">The message.</param>
        public void Send(Action<ISenderProperties> action, IMessage message)
        {
            Send(action, (session) => message);
        }

        /// <summary>
        /// Sends the asynchronous.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        public Task SendAsync(Action<ISenderProperties> action, IMessage message)
        {
            return SendAsync(action, (session) => message);
        }
    }
}
