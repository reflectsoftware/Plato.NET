// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Messaging.Implementations.RMQ.Interfaces;
using Plato.Messaging.Implementations.RMQ.Settings;
using System.Threading;

namespace Plato.Messaging.Implementations.RMQ
{
    public class RMQComsumerByte : RMQConsumer
    {
        public RMQComsumerByte(IRMQConnectionFactory connctionFactory, string connectionName, RMQQueueSettings settings) : base(connctionFactory, connectionName, settings)
        {
        }

        public RMQReceiverResultBytes Receive(int msecTimeout = Timeout.Infinite)
        {
            var deliveryArgs = _Receive(msecTimeout);
            return new RMQReceiverResultBytes(_connection, _channel, deliveryArgs, _settings.QueueName);
        }
    }
}
