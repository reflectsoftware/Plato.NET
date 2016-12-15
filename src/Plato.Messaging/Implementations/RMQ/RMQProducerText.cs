// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Messaging.Implementations.RMQ.Interfaces;
using Plato.Messaging.Implementations.RMQ.Settings;
using Plato.Messaging.Interfaces;
using System;
using System.Text;

namespace Plato.Messaging.Implementations.RMQ
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Messaging.Implementations.RMQ.RMQProducer" />
    /// <seealso cref="Plato.Messaging.Implementations.RMQ.Interfaces.IRMQProducerText" />
    public class RMQProducerText : RMQProducer, IRMQProducerText
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RMQProducerText"/> class.
        /// </summary>
        /// <param name="connectionFactory">The connection factory.</param>
        /// <param name="connectionName">Name of the connection.</param>
        /// <param name="settings">The settings.</param>
        public RMQProducerText(
            IRMQConnectionFactory connectionFactory, 
            string connectionName,             
            RMQQueueSettings settings) 
            : base(connectionFactory, connectionName, settings)
        {
        }

        /// <summary>
        /// Sends the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="action">The action.</param>
        public void Send(string text, Action<ISenderProperties> action = null)
        {
            var data = Encoding.UTF8.GetBytes(text);
            _Send(data, action);
        }
    }
}
