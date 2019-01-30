// Plato.NET
// Copyright (c) 2018 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Messaging.Interfaces;
using Plato.Messaging.RMQ.Settings;
using System;

namespace Plato.Messaging.RMQ.Interfaces
{
    public interface IRMQSenderReceiverFactory
    {
        IMessageReceiverSender Create(Type type, RMQConnectionSettings connection, RMQQueueSettings destination);
        T Create<T>(RMQConnectionSettings connection, RMQQueueSettings destination) where T : IMessageReceiverSender;

        IMessageReceiverSender Create(Type type, RMQConnectionSettings connection, RMQExchangeSettings exchange);
        T Create<T>(RMQConnectionSettings connection, RMQExchangeSettings exchange) where T : IMessageReceiverSender;

        IMessageReceiverSender Create(Type type, RMQConnectionSettings connection, RMQExchangeSettings exchange, RMQQueueSettings destination);
        T Create<T>(RMQConnectionSettings connection, RMQExchangeSettings exchange, RMQQueueSettings destination) where T : IMessageReceiverSender;
    }
}
