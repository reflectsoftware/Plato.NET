// Plato.NET
// Copyright (c) 2018 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Messaging.Interfaces;
using System;
using System.Collections.Generic;

namespace Plato.Messaging.RMQ.Interfaces
{
    public interface IRMQPool: IDisposable
    {
        IRMQPoolContainer<T> Get<T>(
            string connectionName, 
            string queueName, 
            string exchangeName = null,
            IDictionary<string, object> queueArgs = null,
            IDictionary<string, object> exchangeArgs = null) where T : IMessageReceiverSender;
    }
}