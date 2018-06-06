// Plato.NET
// Copyright (c) 2018 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Apache.NMS;
using Plato.Messaging.Interfaces;
using System;
using System.Threading.Tasks;

namespace Plato.Messaging.AMQ.Interfaces
{
    public interface IAMQSender : IMessageReceiverSender
    {
        /// <summary>
        /// Creates the map message.
        /// </summary>
        /// <returns></returns>
        IMapMessage CreateMapMessage();

        /// <summary>
        /// Sends the specified action.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="createMessage">The create message.</param>
        void Send(Action<ISenderProperties> action, Func<ISession, IMessage> createMessage);

        /// <summary>
        /// Sends the asynchronous.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="createMessage">The create message.</param>
        /// <returns></returns>
        Task SendAsync(Action<ISenderProperties> action, Func<ISession, IMessage> createMessage);

        /// <summary>
        /// Sends the specified action.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="message">The message.</param>
        void Send(Action<ISenderProperties> action, IMessage message);

        /// <summary>
        /// Sends the asynchronous.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        Task SendAsync(Action<ISenderProperties> action, IMessage message);
    }
}
