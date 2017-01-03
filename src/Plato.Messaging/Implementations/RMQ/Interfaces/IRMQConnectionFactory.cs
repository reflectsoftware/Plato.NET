// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Messaging.Implementations.RMQ.Settings;
using RabbitMQ.Client;

namespace Plato.Messaging.Implementations.RMQ.Interfaces
{
    public interface IRMQConnectionFactory 
    {
        IConnection CreateConnection(RMQConnectionSettings settings);
    }
}
