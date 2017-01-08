// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Messaging.Interfaces;
using RabbitMQ.Client.Events;
using System;

namespace Plato.Messaging.RMQ.Interfaces
{
    public interface IRMQPublisher : IMessageReceiverSender
    {
        Action<BasicReturnEventArgs> OnReturn { get; set; }
    }
}
