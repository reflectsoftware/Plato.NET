// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Messaging.Enums;
using System.Threading;

namespace Plato.Messaging.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Messaging.Interfaces.IMessageReceiverSender" />
    public interface IMessageReceiver<TData> : IMessageReceiverSender
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
        /// Receive result with the specified msec timeout.
        /// </summary>
        /// <param name="msecTimeout">The msec timeout.</param>
        /// <returns></returns>
        IMessageReceiveResult<TData> Receive(int msecTimeout = Timeout.Infinite);
    }
}
