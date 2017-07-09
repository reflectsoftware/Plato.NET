// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System;
using System.Threading.Tasks;

namespace Plato.Messaging.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Messaging.Interfaces.IMessageReceiverSender" />
    public interface IMessageSender<TData> : IMessageReceiverSender
    {
        /// <summary>
        /// Sends the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="action">The action.</param>
        void Send(TData data, Action<ISenderProperties> action = null);

        /// <summary>
        /// Sends the asynchronous.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="action">The action.</param>
        /// <returns></returns>
        Task SendAsync(TData data, Action<ISenderProperties> action = null);
    }
}
