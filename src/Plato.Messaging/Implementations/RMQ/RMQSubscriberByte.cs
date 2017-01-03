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
    /// <seealso cref="Plato.Messaging.Implementations.RMQ.Interfaces.IRMQSubscriberByte" />
    public class RMQSubscriberByte : RMQSubscriber, IRMQSubscriberByte
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RMQSubscriberByte"/> class.
        /// </summary>
        /// <param name="connectionFactory">The connection factory.</param>
        /// <param name="connectionSettings">The connection settings.</param>
        /// <param name="exchangeSettings">The exchange settings.</param>
        /// <param name="queueSettings">The queue settings.</param>
        public RMQSubscriberByte(
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
        public RMQReceiverResultByte Receive(int msecTimeout = Timeout.Infinite)
        {
            var result = _Receive(msecTimeout);
            return new RMQReceiverResultByte(_connection, _channel, result, _queueSettings.QueueName);
        }
    }
}
