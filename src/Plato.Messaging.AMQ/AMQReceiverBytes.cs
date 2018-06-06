// Plato.NET
// Copyright (c) 2018 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Apache.NMS;
using Plato.Messaging.AMQ.Interfaces;
using Plato.Messaging.AMQ.Settings;
using Plato.Messaging.Interfaces;
using System.Threading;

namespace Plato.Messaging.AMQ
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Messaging.AMQ.AMQReceiver" />
    /// <seealso cref="Plato.Messaging.AMQ.Interfaces.IAMQReceiverBytes" />
    public class AMQReceiverBytes : AMQReceiver, IAMQReceiverBytes
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AMQReceiverBytes" /> class.
        /// </summary>
        /// <param name="connectionFactory">The connection factory.</param>
        /// <param name="connectionSettings">The connection settings.</param>
        /// <param name="destination">The destination.</param>
        public AMQReceiverBytes(IAMQConnectionFactory connectionFactory, AMQConnectionSettings connectionSettings, AMQDestinationSettings destination) : base(connectionFactory, connectionSettings, destination)
        {
        }

        /// <summary>
        /// Receives the specified msec timeout.
        /// </summary>
        /// <param name="msecTimeout">The msec timeout.</param>
        /// <returns></returns>
        public IMessageReceiveResult<byte[]> Receive(int msecTimeout = Timeout.Infinite)
        {
            var message = ReceiveMessage(msecTimeout);
            return message != null ? new AMQReceiverBytesResult((IBytesMessage)message) : null;
        }
    }
}
