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
        protected readonly RMQQueueSettings _settings;
        
        public RMQQueue(
            IRMQConnectionFactory connctionFactory, 
            string connectionName, 
            RMQQueueSettings settings) 
            : base(connctionFactory, connectionName)
        {
            _settings = settings;
        }       

        protected override void OpenChannel()
        {
            base.OpenChannel();

            try
            {
                _channel.QueueDeclare(
                    queue: _settings.QueueName,
                    durable: _settings.Durable,
                    exclusive: _settings.Exclusive,
                    autoDelete: _settings.AutoDelete,
                    arguments: _settings.Arguments);
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
