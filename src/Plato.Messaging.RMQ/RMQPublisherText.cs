// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Messaging.RMQ.Interfaces;
using Plato.Messaging.RMQ.Settings;
using Plato.Messaging.Interfaces;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Plato.Messaging.RMQ
{
    public class RMQPublisherText : RMQPublisher, IRMQPublisherText
    {
        public RMQPublisherText(
            IRMQConnectionFactory connectionFactory,
            RMQConnectionSettings connectionSettings,
            RMQExchangeSettings exchangeSettings,
            RMQQueueSettings queueSettings = null)
            : base(connectionFactory, connectionSettings, exchangeSettings, queueSettings)
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

        /// <summary>
        /// Sends the asynchronous.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="action">The action.</param>
        /// <returns></returns>
        public Task SendAsync(string text, Action<ISenderProperties> action = null)
        {
            var data = Encoding.UTF8.GetBytes(text);
            _Send(data, action);

            return Task.CompletedTask;
        }
    }
}
