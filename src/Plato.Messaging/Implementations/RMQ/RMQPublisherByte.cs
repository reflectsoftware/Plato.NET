// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Messaging.Implementations.RMQ.Interfaces;
using Plato.Messaging.Implementations.RMQ.Settings;
using Plato.Messaging.Interfaces;
using System;

namespace Plato.Messaging.Implementations.RMQ
{
    public class RMQPublisherByte : RMQPublisher, IRMQPublisherByte
    {
        public RMQPublisherByte(
            IRMQConnectionFactory connctionFactory, 
            string connectionName,
            RMQExchangeSettings exchangeSettings,
            RMQQueueSettings queueSettings = null)
            : base(connctionFactory, connectionName, exchangeSettings, queueSettings)
        {
        }

        public void Send(byte[] data, Action<ISenderProperties> action = null)
        {
            _Send(data, action);
        }
    }
}
