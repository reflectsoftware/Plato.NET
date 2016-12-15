// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Messaging.Interfaces;
using RabbitMQ.Client.Events;
using System;

namespace Plato.Messaging.Implementations.RMQ.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Messaging.Interfaces.IMessageReceiverSender" />
    public interface IRMQPublisher : IMessageReceiverSender
    {
        /// <summary>
        /// Gets or sets the on return.
        /// </summary>
        /// <value>
        /// The on return.
        /// </value>
        Action<BasicReturnEventArgs> OnReturn { get; set; }
    }
}
