// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Messaging.Implementations.RMQ.Interfaces;
using Plato.Messaging.Implementations.RMQ.Settings;
using System.Threading;

namespace Plato.Messaging.Implementations.RMQ
{
    public class RMQConsumerText : RMQConsumer, IRMQConsumerText
    {
        public RMQConsumerText(
            IRMQConnectionFactory connctionFactory, 
            string connectionName, 
            RMQQueueSettings settings) 
            : base(connctionFactory, connectionName, settings)
        {
        }

        public RMQReceiverResultText Receive(int msecTimeout = Timeout.Infinite)
        {
            var deliveryArgs = _Receive(msecTimeout);
            return new RMQReceiverResultText(_connection, _channel, deliveryArgs, _queueSettings.QueueName);
        }
    }
}
