// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Messaging.Interfaces;
using Plato.Messaging.Implementations.RMQ.Settings;
using RabbitMQ.Client.Events;
using System;

namespace Plato.Messaging.Implementations.RMQ
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Messaging.RMQ.RMQReceiverSender" />
    /// <seealso cref="Plato.Messaging.Interfaces.IMessageSender" />
    public class RMQSender : RMQReceiverSender, IMessageSender<byte[]>
    {
        /// <summary>
        /// Gets or sets the on return.
        /// </summary>
        /// <value>
        /// The on return.
        /// </value>
        public Action<BasicReturnEventArgs> OnReturn { get; set; }

        public RMQSender(RMQSettings settings) : base(settings)
        {
        }

        /// <summary>
        /// Does the on return.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="BasicReturnEventArgs"/> instance containing the event data.</param>
        private void DoOnReturn(object sender, BasicReturnEventArgs args)
        {
            if (OnReturn != null)
            {
                OnReturn(args);
            }
        }

        /// <summary>
        /// Opens this instance.
        /// </summary>
        public override void Open()
        {
            try
            {
                base.Open();
                _channel.BasicReturn += DoOnReturn;
            }
            catch (Exception ex)
            {
                var newException = RMQExceptionHandler.ExceptionHandler(_connection, ex);
                if (newException != null)
                {
                    throw newException;
                }

                throw;
            }
        }

        /// <summary>
        /// Sends the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="action">The action.</param>
        public void Send(byte[] data, Action<ISenderProperties> action = null)
        {
            try
            {
                if (!IsOpen())
                {
                    Open();
                }

                var props = _channel.CreateBasicProperties();

                var senderProperties = new RMQSenderProperties()
                {
                    Properties = props,
                    Exchange = _settings.ExchangeSettings != null ? _settings.ExchangeSettings.Name : string.Empty,
                    RoutingKey = string.Empty,
                    Mandatory = false,
                };

                if (action != null)
                {
                    action(senderProperties);
                }

                _channel.BasicPublish(senderProperties.Exchange, senderProperties.RoutingKey, senderProperties.Mandatory, props, data);
            }
            catch (Exception ex)
            {
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
