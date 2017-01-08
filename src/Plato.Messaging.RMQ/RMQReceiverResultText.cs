// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Plato.Messaging.RMQ
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Messaging.RMQ.RMQReceiverResult{System.String}" />
    public class RMQReceiverResultText : RMQReceiverResult<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RMQReceiverResultText"/> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="channel">The channel.</param>
        /// <param name="args">The <see cref="BasicDeliverEventArgs"/> instance containing the event data.</param>
        /// <param name="queueName">Name of the queue.</param>
        internal RMQReceiverResultText(IConnection connection, IModel channel, BasicDeliverEventArgs args, string queueName)
            : base(connection, channel, args, queueName)
        {
        }

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        public override string Data
        {
            get
            {
                return Encoding.UTF8.GetString(DeliverEventArgs.Body);
            }
        }
    }
}
