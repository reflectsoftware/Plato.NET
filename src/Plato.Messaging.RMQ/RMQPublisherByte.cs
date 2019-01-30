// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Messaging.RMQ.Interfaces;
using Plato.Messaging.RMQ.Settings;
using Plato.Messaging.Interfaces;
using System;
using System.Threading.Tasks;

namespace Plato.Messaging.RMQ
{
    public class RMQPublisherByte : RMQPublisher, IRMQPublisherBytes
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RMQPublisherByte" /> class.
        /// </summary>
        /// <param name="connectionFactory">The connection factory.</param>
        /// <param name="connectionSettings">The connection settings.</param>
        /// <param name="exchangeSettings">The exchange settings.</param>
        public RMQPublisherByte(
            IRMQConnectionFactory connectionFactory,
            RMQConnectionSettings connectionSettings,
            RMQExchangeSettings exchangeSettings)
            : base(connectionFactory, connectionSettings, exchangeSettings)
        {
        }

        /// <summary>
        /// Sends the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="action">The action.</param>
        public void Send(byte[] data, Action<ISenderProperties> action = null)
        {
            _Send(data, action);
        }

        /// <summary>
        /// Sends the asynchronous.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="action">The action.</param>
        /// <returns></returns>
        public Task SendAsync(byte[] data, Action<ISenderProperties> action = null)
        {
            _Send(data, action);

            return Task.CompletedTask;
        }
    }
}
