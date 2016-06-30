// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Apache.NMS;
using Apache.NMS.Util;
using Plato.Messaging.Implementations.AMQ.Interfaces;
using Plato.Messaging.Implementations.AMQ.Settings;
using Plato.Messaging.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Plato.Messaging.Implementations.AMQ
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Messaging.Implementations.AMQ.AMQReceiverSender" />
    /// <seealso cref="Plato.Messaging.Interfaces.IMessageSender{System.String}" />
    public class AMQSender<TData> : AMQReceiverSender, IAMQSender<TData>
    {
        private readonly AMQDestinationSettings _destination;
        private IMessageProducer _producer;

        /// <summary>
        /// Initializes a new instance of the <see cref="AMQSender"/> class.
        /// </summary>
        /// <param name="connectionFactory">The connection factory.</param>
        /// <param name="connectionName">Name of the connection.</param>
        /// <param name="destination">The destination.</param>
        public AMQSender(IAMQConnectionFactory connectionFactory, string connectionName, AMQDestinationSettings destination) : base(connectionFactory, connectionName)
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
        /// Creates the message.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        private IMessage CreateMessage(TData data)
        {
            IMessage message = null;
            if (data is string)
            {
                message = _session.CreateTextMessage(data as string);
            }
            else if (data is byte[])
            {
                message = _session.CreateBytesMessage(data as byte[]);
            }
            else if (data is IDictionary<string, string>)
            {
                var map = (data as IDictionary<string, string>);
                var messageMap = _session.CreateMapMessage();
                foreach (var key in map.Keys)
                {
                    messageMap.Body.SetString(key, map[key]);
                }

                message = messageMap;
            }
            else
            {
                throw new NotImplementedException($"Send data type not support for AMQSender: '{data.GetType().FullName}'");
            }

            return message;
        }

        /// <summary>
        /// Sends the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="action">The action.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Send(TData data, Action<ISenderProperties> action = null)
        {
            try
            {
                Open();

                if (_producer == null)
                {
                    var destination = SessionUtil.GetDestination(_session, _destination.Path);
                    _producer = _session.CreateProducer(destination);
                }

                var senderProperties = new AMQSenderProperties()
                {
                    Properties = new NameValueCollection()
                };

                if (action != null)
                {
                    action(senderProperties);
                }

                var request = CreateMessage(data);
                request.NMSCorrelationID = senderProperties.CorrelationId;
                request.NMSTimeToLive = senderProperties.TTL;

                foreach (var key in senderProperties.Properties.AllKeys)
                {
                    request.Properties[key] = senderProperties.Properties[key];
                }

                _producer.Send(request);
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
}
