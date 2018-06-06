// Plato.NET
// Copyright (c) 2018 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Messaging.AMQ.Interfaces;
using Plato.Messaging.AMQ.Settings;
using Plato.Messaging.Interfaces;
using System;
using System.Threading.Tasks;

namespace Plato.Messaging.AMQ
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Messaging.AMQ.AMQSender" />
    /// <seealso cref="Plato.Messaging.AMQ.Interfaces.IAMQSenderText" />
    public class AMQSenderText: AMQSender, IAMQSenderText
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AMQSenderText" /> class.
        /// </summary>
        /// <param name="connectionFactory">The connection factory.</param>
        /// <param name="connectionSettings">The connection settings.</param>
        /// <param name="destination">The destination.</param>
        public AMQSenderText(IAMQConnectionFactory connectionFactory, AMQConnectionSettings connectionSettings, AMQDestinationSettings destination) : base(connectionFactory, connectionSettings, destination)
        {
        }

        /// <summary>
        /// Sends the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="action">The action.</param>
        public void Send(string text, Action<ISenderProperties> action = null)
        {
            Send(action, (session) =>
            {
                return session.CreateTextMessage(text);
            });
        }

        public async Task SendAsync(string text, Action<ISenderProperties> action = null)
        {
            await SendAsync(action, (session) =>
            {
                return session.CreateTextMessage(text);
            });
        }
    }
}
