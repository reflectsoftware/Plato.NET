// Plato.NET
// Copyright (c) 2018 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System;
using System.Threading.Tasks;

namespace Plato.Messaging.AMQ.Interfaces
{
    public interface IAMQPoolAsync: IDisposable
    {
        Task<IAMQPoolContainerAsync<IAMQReceiverBytes>> GetBytesConsumerAsync(string connectionName, string queueName);
        Task<IAMQPoolContainerAsync<IAMQSenderBytes>> GetBytesProducerAsync(string connectionName, string queueName);
        Task<IAMQPoolContainerAsync<IAMQReceiverText>> GetTextConsumerAsync(string connectionName, string queueName);
        Task<IAMQPoolContainerAsync<IAMQSenderText>> GetTextProducerAsync(string connectionName, string queueName);
    }
}