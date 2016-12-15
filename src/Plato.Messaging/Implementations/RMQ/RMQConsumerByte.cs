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
    /// <seealso cref="Plato.Messaging.Implementations.RMQ.RMQConsumer" />
    /// <seealso cref="Plato.Messaging.Implementations.RMQ.Interfaces.IRMQConsumerByte" />
    public class RMQConsumerByte : RMQConsumer, IRMQConsumerByte
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RMQConsumerByte"/> class.
        /// </summary>
        /// <param name="connectionFactory">The connection factory.</param>
        /// <param name="connectionName">Name of the connection.</param>
        /// <param name="settings">The settings.</param>
        public RMQConsumerByte(
            IRMQConnectionFactory connectionFactory, 
            string connectionName, 
            RMQQueueSettings settings) 
            : base(connectionFactory, connectionName, settings)
        {
        }

        /// <summary>
        /// Receives the specified msec timeout.
        /// </summary>
        /// <param name="msecTimeout">The msec timeout.</param>
        /// <returns></returns>
        public RMQReceiverResultByte Receive(int msecTimeout = Timeout.Infinite)
        {
            var deliveryArgs = _Receive(msecTimeout);
            return new RMQReceiverResultByte(_connection, _channel, deliveryArgs, _queueSettings.QueueName);
        }
    }
}
