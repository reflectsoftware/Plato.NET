// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Messaging.RMQ.Interfaces;
using Plato.Messaging.RMQ.Settings;
using Plato.Messaging.Interfaces;
using System;
using System.Text;

namespace Plato.Messaging.RMQ
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Messaging.RMQ.RMQProducer" />
    /// <seealso cref="Plato.Messaging.RMQ.Interfaces.IRMQProducerText" />
    public class RMQProducerText : RMQProducer, IRMQProducerText
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RMQProducerText"/> class.
        /// </summary>
        /// <param name="connectionFactory">The connection factory.</param>
        /// <param name="connectionSettings">The connection settings.</param>
        /// <param name="queueSettings">The queue settings.</param>
        public RMQProducerText(
            IRMQConnectionFactory connectionFactory,
            RMQConnectionSettings connectionSettings,
            RMQQueueSettings queueSettings) 
            : base(connectionFactory, connectionSettings, queueSettings)
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
