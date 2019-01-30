// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Messaging.Enums;
using Plato.Messaging.Exceptions;
using Plato.Messaging.Interfaces;
using Plato.Messaging.RMQ.Interfaces;
using Plato.Messaging.RMQ.Settings;
using RabbitMQ.Client.Events;
using System;

namespace Plato.Messaging.RMQ
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Messaging.RMQ.RMQPublisherSubscriber" />
    /// <seealso cref="Plato.Messaging.RMQ.Interfaces.IRMQPublisher" />
    public class RMQPublisher: RMQReceiverSender, IRMQPublisher
    {
        protected readonly RMQExchangeSettings _exchangeSettings;

        /// <summary>
        /// Gets or sets the on return.
        /// </summary>
        /// <value>
        /// The on return.
        /// </value>
        public Action<BasicReturnEventArgs> OnReturn { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RMQPublisher" /> class.
        /// </summary>
        /// <param name="connectionFactory">The connection factory.</param>
        /// <param name="connectionSettings">The connection settings.</param>
        /// <param name="exchangeSettings">The exchange settings.</param>
        public RMQPublisher(
            IRMQConnectionFactory connectionFactory,
            RMQConnectionSettings connectionSettings,
            RMQExchangeSettings exchangeSettings)
            : base(connectionFactory, connectionSettings)
        {
            _exchangeSettings = exchangeSettings;
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
        /// Closes this instance.
        /// </summary>
        public override void Close()
        {
            if (_channel != null)
            {
                _channel.BasicReturn -= DoOnReturn;
            }

            base.Close();
        }

        /// <summary>
        /// Sends the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="action">The action.</param>
        protected void _Send(byte[] data, Action<ISenderProperties> action = null)
        {
            try
            {
                var connectionRetry = true;
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

                        action?.Invoke(senderProperties);

                        _channel.BasicPublish(
                            senderProperties.Exchange,
                            senderProperties.RoutingKey,
                            senderProperties.Mandatory,
                            null,
                            data);

                        break;
                    }
                    catch (Exception ex)
                    {
                        var newException = RMQExceptionHandler.ExceptionHandler(_connection, ex);
                        if (newException != null)
                        {
                            if (newException.ExceptionCode == MessageExceptionCode.LostConnection
                            && connectionRetry)
                            {
                                // try the reconnection cycle
                                connectionRetry = false;
                                continue;
                            }

                            throw newException;
                        }

                        throw;
                    }
                }
            }
            catch (MessageException ex)
            {
                switch (ex.ExceptionCode)
                {
                    case MessageExceptionCode.LostConnection:
                        Close();
                        break;
                }

                throw;
            }
        }
    }
}
