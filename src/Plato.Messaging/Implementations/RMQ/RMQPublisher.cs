// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Messaging.Implementations.RMQ.Interfaces;
using Plato.Messaging.Implementations.RMQ.Settings;
using Plato.Messaging.Interfaces;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.IO;

namespace Plato.Messaging.Implementations.RMQ
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Messaging.Implementations.RMQ.RMQPublisherSubscriber" />
    /// <seealso cref="Plato.Messaging.Implementations.RMQ.Interfaces.IRMQPublisher" />
    public class RMQPublisher: RMQPublisherSubscriber, IRMQPublisher
    {
        /// <summary>
        /// Gets or sets the on return.
        /// </summary>
        /// <value>
        /// The on return.
        /// </value>
        public Action<BasicReturnEventArgs> OnReturn { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RMQPublisher"/> class.
        /// </summary>
        /// <param name="connectionFactory">The connection factory.</param>
        /// <param name="connectionSettings">The connection settings.</param>
        /// <param name="exchangeSettings">The exchange settings.</param>
        /// <param name="queueSettings">The queue settings.</param>
        public RMQPublisher(
            IRMQConnectionFactory connectionFactory,
            RMQConnectionSettings connectionSettings,
            RMQExchangeSettings exchangeSettings,
            RMQQueueSettings queueSettings = null)
            : base(connectionFactory, connectionSettings, exchangeSettings, queueSettings)
        {
        }

        /// <summary>
        /// Does the on return.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="BasicReturnEventArgs"/> instance containing the event data.</param>
        private void DoOnReturn(object sender, BasicReturnEventArgs args)
        {
            OnReturn?.Invoke(args);
        }

        /// <summary>
        /// Opens this instance.
        /// </summary>
        public override void Open()
        {
            base.Open();
            _channel.BasicReturn += DoOnReturn;
        }

        /// <summary>
        /// Sends the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="action">The action.</param>
        protected void _Send(byte[] data, Action<ISenderProperties> action = null)
        {
            while (true)
            {
                if (!IsOpen())
                {
                    Open();
                }

                try
                {

                    var senderProperties = new RMQSenderProperties()
                    {
                        Properties = null,
                        Exchange = _exchangeSettings.ExchangeName,
                        RoutingKey = string.Empty,
                        Mandatory = false,
                    };

                    if (action != null)
                    {
                        action(senderProperties);
                    }

                    _channel.BasicPublish(
                        senderProperties.Exchange,
                        senderProperties.RoutingKey,
                        senderProperties.Mandatory,
                        null,
                        data);

                    return;
                }
                catch (Exception ex)
                {
                    if ((ex is AlreadyClosedException) || (ex is IOException))
                    {
                        // retry
                        continue;
                    }

                    var newException = RMQExceptionHandler.ExceptionHandler(_connection, ex);
                    if (newException != null)
                    {
                        throw newException;
                    }

                    throw;
                }
            }
        }
    }
}
