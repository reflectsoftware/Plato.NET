// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Messaging.Implementations.RMQ.Interfaces;
using Plato.Messaging.Implementations.RMQ.Settings;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System;

namespace Plato.Messaging.Implementations.RMQ
{
    public abstract class RMQQueue : RMQReceiverSender, IRMQQueue
    {
        protected readonly RMQQueueSettings _queueSettings;
        
        public RMQQueue(
            IRMQConnectionFactory connectionFactory,
            RMQConnectionSettings connectionSettings,
            RMQQueueSettings queueSettings) 
            : base(connectionFactory, connectionSettings)
        {
            _queueSettings = queueSettings;
        }       

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

        /// <summary>
        /// Queues the information.
        /// </summary>
        /// <param name="queueName">Name of the queue.</param>
        /// <returns></returns>
        public QueueDeclareOk QueueInfo(string queueName = null)
        {
            Open();

            try
            {
                var qname = queueName ?? _queueSettings.QueueName;
                try
                {
                    var info = _channel.QueueDeclarePassive(qname);
                    return info;
                }
                catch (OperationInterruptedException ex)
                {
                    if (ex.ShutdownReason.ReplyCode == 404)
                    {
                        // not found
                        return new QueueDeclareOk(qname, 0, 0);
                    }

                    throw;
                }
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
