// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Apache.NMS;
using Plato.Messaging.Implementations.AMQ.Interfaces;
using Plato.Messaging.Implementations.AMQ.Settings;
using Plato.Messaging.Interfaces;
using System.Threading;

namespace Plato.Messaging.Implementations.AMQ
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Messaging.Implementations.AMQ.AMQReceiver" />
    /// <seealso cref="Plato.Messaging.Implementations.AMQ.Interfaces.IAMQReceiverText" />
    public class AMQReceiverText : AMQReceiver, IAMQReceiverText
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AMQReceiverText" /> class.
        /// </summary>
        /// <param name="connectionFactory">The connection factory.</param>
        /// <param name="connectionSettings">The connection settings.</param>
        /// <param name="destination">The destination.</param>
        public AMQReceiverText(IAMQConnectionFactory connectionFactory, AMQConnectionSettings connectionSettings, AMQDestinationSettings destination) : base(connectionFactory, connectionSettings, destination)
        {
        }

        /// <summary>
        /// Receives the specified msec timeout.
        /// </summary>
        /// <param name="msecTimeout">The msec timeout.</param>
        /// <returns></returns>
        public IMessageReceiveResult<string> Receive(int msecTimeout = Timeout.Infinite)
        {
            return new AMQReceiverTextResult((ITextMessage)ReceiveMessage(msecTimeout));
        }
    }
}
