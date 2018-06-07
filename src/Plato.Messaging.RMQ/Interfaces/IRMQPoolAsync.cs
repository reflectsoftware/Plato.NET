// Plato.NET
// Copyright (c) 2018 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System;
using System.Threading.Tasks;

namespace Plato.Messaging.RMQ.Interfaces
{
    public interface IRMQPoolAsync: IDisposable
    {
        Task<IRMQPoolContainer<IRMQPublisherBytes>> GetBytesPublisherAsync(string connectionName, string queueName);
        Task<IRMQPoolContainer<IRMQSubscriberBytes>> GetBytesSubscriberAsync(string connectionName, string queueName);
        Task<IRMQPoolContainer<IRMQPublisherText>> GetTextPublisherAsync(string connectionName, string queueName);
        Task<IRMQPoolContainer<IRMQSubscriberText>> GetTextSubscriberAsync(string connectionName, string queueName);
        Task<IRMQPoolContainerAsync<IRMQReceiverBytes>> GetBytesConsumerAsync(string connectionName, string queueName);
        Task<IRMQPoolContainerAsync<IRMQProducerBytes>> GetBytesProducerAsync(string connectionName, string queueName);
        Task<IRMQPoolContainerAsync<IRMQReceiverText>> GetTextConsumerAsync(string connectionName, string queueName);
        Task<IRMQPoolContainerAsync<IRMQProducerText>> GetTextProducerAsync(string connectionName, string queueName);
    }
}