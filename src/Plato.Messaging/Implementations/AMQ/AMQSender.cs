// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Apache.NMS;
using Apache.NMS.Util;
using Plato.Messaging.Implementations.AMQ.Interfaces;
using Plato.Messaging.Implementations.AMQ.Settings;
using Plato.Messaging.Interfaces;
using System;
using System.Collections.Specialized;

namespace Plato.Messaging.Implementations.AMQ
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Messaging.Implementations.AMQ.AMQReceiverSender" />
    /// <seealso cref="Plato.Messaging.Implementations.AMQ.Interfaces.IAMQSender" />
    public class AMQSender : AMQReceiverSender, IAMQSender
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
        /// Sends the specified data.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="createMessage">The create message.</param>
        public void Send(Action<ISenderProperties> action, Func<ISession, IMessage> createMessage)
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

                var request = createMessage(_session);
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
