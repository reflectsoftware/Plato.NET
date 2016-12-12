// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Messaging.Implementations.RMQ.Interfaces;
using Plato.Messaging.Implementations.RMQ.Settings;
using Plato.Messaging.Interfaces;
using System.Collections.Generic;
using System.Threading;

namespace Plato.Messaging.Implementations.RMQ
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Messaging.Implementations.RMQ.RMQReceiverBase" />
    /// <seealso cref="Plato.Messaging.Implementations.RMQ.IRMQReceiver" />
    public class RMQReceiver : RMQReceiverBase, IRMQReceiver
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RMQReceiver"/> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="routingKeys">The routing keys.</param>
        public RMQReceiver(RMQSettings settings, IEnumerable<string> routingKeys = null) : base(settings, routingKeys)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RMQReceiver"/> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="routingKey">The routing key.</param>
        public RMQReceiver(RMQSettings settings, string routingKey = "") : base(settings, new string[] { routingKey })
        {
        }

        /// <summary>
        /// Receives the specified msec timeout.
        /// </summary>
        /// <param name="msecTimeout">The msec timeout.</param>
        /// <returns></returns>
        public IMessageReceiveResult<byte[]> Receive(int msecTimeout = Timeout.Infinite)
        {
            var deliveryArgs = _Receive(msecTimeout);
            return new RMQReceiverResultBytes(_connection, _channel, deliveryArgs, _settings.QueueSettings.Name);
        }
    }
}
