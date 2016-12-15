// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Messaging.Implementations.RMQ.Interfaces;
using Plato.Messaging.Implementations.RMQ.Settings;
using System;

namespace Plato.Messaging.Implementations.RMQ
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Messaging.Implementations.RMQ.RMQReceiverSender" />
    public abstract class RMQQueue : RMQReceiverSender
    {
        protected readonly RMQQueueSettings _queueSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="RMQQueue"/> class.
        /// </summary>
        /// <param name="connectionFactory">The connection factory.</param>
        /// <param name="connectionName">Name of the connection.</param>
        /// <param name="queueSettings">The queue settings.</param>
        public RMQQueue(
            IRMQConnectionFactory connectionFactory, 
            string connectionName, 
            RMQQueueSettings queueSettings) 
            : base(connectionFactory, connectionName)
        {
            _queueSettings = queueSettings;
        }

        /// <summary>
        /// Opens the channel.
        /// </summary>
        protected override void OpenChannel()
        {
            base.OpenChannel();

            try
            {
                _channel.QueueDeclare(
                    queue: _queueSettings.QueueName,
                    durable: _queueSettings.Durable,
                    exclusive: _queueSettings.Exclusive,
                    autoDelete: _queueSettings.AutoDelete,
                    arguments: _queueSettings.Arguments);
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
