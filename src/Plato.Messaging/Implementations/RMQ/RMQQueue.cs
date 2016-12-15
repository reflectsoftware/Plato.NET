// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Messaging.Implementations.RMQ.Interfaces;
using Plato.Messaging.Implementations.RMQ.Settings;
using System;

namespace Plato.Messaging.Implementations.RMQ
{
    public abstract class RMQQueue : RMQReceiverSender
    {
        protected readonly RMQQueueSettings _queueSettings;
        
        public RMQQueue(
            IRMQConnectionFactory connctionFactory, 
            string connectionName, 
            RMQQueueSettings queueSettings) 
            : base(connctionFactory, connectionName)
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
    }
}
