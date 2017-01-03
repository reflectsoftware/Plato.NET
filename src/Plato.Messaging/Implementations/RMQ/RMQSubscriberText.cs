// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Messaging.Implementations.RMQ.Interfaces;
using Plato.Messaging.Implementations.RMQ.Settings;
using System.Threading;

namespace Plato.Messaging.Implementations.RMQ
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Messaging.Implementations.RMQ.RMQSubscriber" />
    /// <seealso cref="Plato.Messaging.Implementations.RMQ.Interfaces.IRMQSubscriberText" />
    public class RMQSubscriberText : RMQSubscriber, IRMQSubscriberText
    {
        public RMQSubscriberText(
            IRMQConnectionFactory connectionFactory,
            RMQConnectionSettings connectionSettings,
            RMQExchangeSettings exchangeSettings,
            RMQQueueSettings queueSettings)
            : base(connectionFactory, connectionSettings, exchangeSettings, queueSettings)
        {
        }

        /// <summary>
        /// Receives the specified msec timeout.
        /// </summary>
        /// <param name="msecTimeout">The msec timeout.</param>
        /// <returns></returns>
        public RMQReceiverResultText Receive(int msecTimeout = Timeout.Infinite)
        {
            var result = _Receive(msecTimeout);
            return new RMQReceiverResultText(_connection, _channel, result, _queueSettings.QueueName);
        }
    }
}
