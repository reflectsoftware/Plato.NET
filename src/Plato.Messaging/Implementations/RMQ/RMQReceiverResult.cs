// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Messaging.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;

namespace Plato.Messaging.Implementations.RMQ
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Messaging.Interfaces.IMessageReceiveResult" />
    public class RMQReceiverResult : IMessageReceiveResult<byte[]>
    {
        /// <summary>
        /// Gets the channel.
        /// </summary>
        /// <value>
        /// The channel.
        /// </value>
        public IModel Channel { get; private set; }

        /// <summary>
        /// Gets the connection.
        /// </summary>
        /// <value>
        /// The connection.
        /// </value>
        public IConnection Connection { get; private set; }

        /// <summary>
        /// Gets the deliver event arguments.
        /// </summary>
        /// <value>
        /// The deliver event arguments.
        /// </value>
        public BasicDeliverEventArgs DeliverEventArgs { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance has acknowledged.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has acknowledged; otherwise, <c>false</c>.
        /// </value>
        public bool HasAcknowledged { get; private set; }

        /// <summary>
        /// Gets the name of the queue.
        /// </summary>
        /// <value>
        /// The name of the queue.
        /// </value>
        public string QueueName { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RMQReceiverResult"/> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="channel">The channel.</param>
        /// <param name="args">The <see cref="BasicDeliverEventArgs"/> instance containing the event data.</param>
        /// <param name="queueName">Name of the queue.</param>
        internal RMQReceiverResult(IConnection connection, IModel channel, BasicDeliverEventArgs args, string queueName)
        {
            Channel = channel;
            Connection = connection;
            DeliverEventArgs = args;
            HasAcknowledged = false;
            QueueName = queueName;
        }

        /// <summary>
        /// Gets the headers.
        /// </summary>
        /// <value>
        /// The headers.
        /// </value>
        public IDictionary<string, object> Headers
        {
            get
            {
                return DeliverEventArgs.BasicProperties.Headers;
            }
        }

        /// <summary>
        /// Gets the header.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public T GetHeader<T>(string key, T defaultValue = default(T))
        {
            return Headers != null && Headers.ContainsKey(key) ? (T)Headers[key] : defaultValue;
        }

        /// <summary>
        /// Gets the name of the exchange.
        /// </summary>
        /// <value>
        /// The name of the exchange.
        /// </value>
        public string ExchangeName
        {
            get
            {
                return DeliverEventArgs.Exchange;
            }
        }

        /// <summary>
        /// Gets the routing key.
        /// </summary>
        /// <value>
        /// The routing key.
        /// </value>
        public string RoutingKey
        {
            get
            {
                return DeliverEventArgs.RoutingKey;
            }
        }

        /// <summary>
        /// Gets the message identifier.
        /// </summary>
        /// <value>
        /// The message identifier.
        /// </value>
        public string MessageId
        {
            get
            {
                return DeliverEventArgs.BasicProperties.MessageId;
            }
        }

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        public byte[] Data
        {
            get
            {
                return DeliverEventArgs.Body;
            }
        }

        /// <summary>
        /// Acknowledges this instance.
        /// </summary>
        public void Acknowledge()
        {
            try
            {
                if (!HasAcknowledged)
                {
                    Channel.BasicAck(DeliverEventArgs.DeliveryTag, false);
                    HasAcknowledged = true;
                }
            }
            catch (Exception ex)
            {
                var newException = RMQExceptionHandler.ExceptionHandler(Connection, ex);
                if (newException != null)
                {
                    throw newException;
                }

                throw;
            }
        }

        /// <summary>
        /// Rejects the specified requeue.
        /// </summary>
        /// <param name="requeue">if set to <c>true</c> [requeue].</param>
        public void Reject(bool requeue = false)
        {
            try
            {
                if (!HasAcknowledged)
                {
                    Channel.BasicNack(DeliverEventArgs.DeliveryTag, false, requeue);
                    HasAcknowledged = true;
                }
            }
            catch (Exception ex)
            {
                var newException = RMQExceptionHandler.ExceptionHandler(Connection, ex);
                if (newException != null)
                {
                    throw newException;
                }

                throw;
            }
        }
    }
}
