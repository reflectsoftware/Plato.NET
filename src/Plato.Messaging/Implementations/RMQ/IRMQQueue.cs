// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using RabbitMQ.Client;

namespace Plato.Messaging.Implementations.RMQ.Interfaces
{
    public interface IRMQQueue
    {
        /// <summary>
        /// Queues the information.
        /// </summary>
        /// <param name="queueName">Name of the queue.</param>
        /// <returns></returns>
        QueueDeclareOk QueueInfo(string queueName = null);
    }
}
