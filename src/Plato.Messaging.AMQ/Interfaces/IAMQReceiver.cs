// Plato.NET
// Copyright (c) 2018 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Apache.NMS;
using Plato.Messaging.Enums;
using Plato.Messaging.Interfaces;
using System.Threading;

namespace Plato.Messaging.AMQ.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Messaging.Interfaces.IMessageReceiverSender" />
    public interface IAMQReceiver: IMessageReceiverSender
    {
        /// <summary>
        /// Gets or sets the mode.
        /// </summary>
        /// <value>
        /// The mode.
        /// </value>
        ConsumerMode Mode { get; set; }

        /// <summary>
        /// Clears the cache buffer.
        /// </summary>
        void ClearCacheBuffer();

        /// <summary>
        /// Receives the message.
        /// </summary>
        /// <param name="msecTimeout">The msec timeout.</param>
        /// <returns></returns>
        IMessage ReceiveMessage(int msecTimeout = Timeout.Infinite);

        /// <summary>
        /// Receives the result.
        /// </summary>
        /// <param name="msecTimeout">The msec timeout.</param>
        /// <returns></returns>
        AMQReceiverResult ReceiveResult(int msecTimeout = Timeout.Infinite);
    }
}
