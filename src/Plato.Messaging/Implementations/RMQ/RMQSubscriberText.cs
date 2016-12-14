// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Messaging.Implementations.RMQ.Interfaces;
using Plato.Messaging.Implementations.RMQ.Settings;
using System.Collections.Generic;
using System.Threading;

namespace Plato.Messaging.Implementations.RMQ
{
    public class RMQSubscriberText : RMQSubscriber, IRMQSubscriberText
    {
        public RMQSubscriberText(
            IRMQConnectionFactory connctionFactory,
            string connectionName,
            RMQExchangeSettings exchangeSettings,
            RMQQueueSettings queueSettings,
            IEnumerable<string> routingKeys = null)
            : base(connctionFactory, connectionName, exchangeSettings, queueSettings, routingKeys)
        {
        }

        public RMQSubscriberText(
            IRMQConnectionFactory connctionFactory,
            string connectionName,
            RMQExchangeSettings exchangeSettings,
            RMQQueueSettings queueSettings,
            string routingKeys = "")
            : base(connctionFactory, connectionName, exchangeSettings, queueSettings, routingKeys)
        {
        }

        public RMQReceiverResultText Receive(int msecTimeout = Timeout.Infinite)
        {
            var result = _Receive(msecTimeout);
            return new RMQReceiverResultText(_connection, _channel, result, _queueSettings.QueueName);
        }
    }
}
