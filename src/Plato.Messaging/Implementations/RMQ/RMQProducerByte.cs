// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Messaging.Implementations.RMQ.Interfaces;
using Plato.Messaging.Implementations.RMQ.Settings;
using Plato.Messaging.Interfaces;
using System;

namespace Plato.Messaging.Implementations.RMQ
{
    public class RMQProducerByte : RMQProducer, IRMQProducerByte
    {
        public RMQProducerByte(
            IRMQConnectionFactory connctionFactory, 
            string connectionName,             
            RMQQueueSettings settings) 
            : base(connctionFactory, connectionName, settings)
        {
        }

        public void Send(byte[] data, Action<ISenderProperties> action = null)
        {
            _Send(data, action);
        }
    }
}
