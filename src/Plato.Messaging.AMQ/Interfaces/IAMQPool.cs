// Plato.NET
// Copyright (c) 2018 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System;

namespace Plato.Messaging.AMQ.Interfaces
{
    public interface IAMQPool: IDisposable
    {
        IAMQPoolContainer<IAMQReceiverBytes> GetBytesConsumer(string connectionName, string queueName);
        IAMQPoolContainer<IAMQSenderBytes> GetBytesProducer(string connectionName, string queueName);
        IAMQPoolContainer<IAMQReceiverText> GetTextConsumer(string connectionName, string queueName);
        IAMQPoolContainer<IAMQSenderText> GetTextProducer(string connectionName, string queueName);
    }
}