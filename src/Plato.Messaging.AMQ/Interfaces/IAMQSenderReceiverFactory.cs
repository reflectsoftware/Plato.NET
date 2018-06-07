// Plato.NET
// Copyright (c) 2018 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Messaging.AMQ.Settings;
using Plato.Messaging.Interfaces;
using System;

namespace Plato.Messaging.AMQ.Interfaces
{
    public interface IAMQSenderReceiverFactory
    {
        IMessageReceiverSender Create(Type type, AMQConnectionSettings connection, AMQDestinationSettings destination);
        T Create<T>(AMQConnectionSettings connection, AMQDestinationSettings destination) where T : IMessageReceiverSender;
    }
}
