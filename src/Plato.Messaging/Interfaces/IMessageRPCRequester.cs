// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System;
using System.Threading;

namespace Plato.Messaging.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Messaging.Interfaces.IMessageReceiverSender" />
    public interface IMessageRPCRequester<TData> : IMessageReceiverSender
    {
        /// <summary>
        /// Requests the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="action">The action.</param>
        /// <param name="msecTimeout">The msec timeout.</param>
        /// <returns></returns>
        TData Request(TData data, Action<ISenderProperties> action = null, int msecTimeout = Timeout.Infinite);
    }
}
