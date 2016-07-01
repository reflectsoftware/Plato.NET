// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Apache.NMS;
using Plato.Messaging.Interfaces;
using System;

namespace Plato.Messaging.Implementations.AMQ.Interfaces
{
    public interface IAMQSender : IMessageReceiverSender
    {
        /// <summary>
        /// Sends the specified action.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="createMessage">The create message.</param>
        void Send(Action<ISenderProperties> action, Func<ISession, IMessage> createMessage);
    }
}
