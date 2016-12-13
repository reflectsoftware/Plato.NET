// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Plato.Messaging.Implementations.RMQ
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Messaging.Implementations.RMQ.RMQReceiverResult{System.Byte[]}" />
    public class RMQReceiverResultBytes : RMQReceiverResult<byte[]>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RMQReceiverResultBytes"/> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="channel">The channel.</param>
        /// <param name="args">The <see cref="BasicDeliverEventArgs"/> instance containing the event data.</param>
        /// <param name="queueName">Name of the queue.</param>
        internal RMQReceiverResultBytes(IConnection connection, IModel channel, BasicDeliverEventArgs args, string queueName)
            : base(connection, channel, args, queueName)
        {
        }

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        public override byte[] Data
        {
            get
            {
                return DeliverEventArgs.Body;
            }
        }
    }
}
