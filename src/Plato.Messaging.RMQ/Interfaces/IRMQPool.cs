// Plato.NET
// Copyright (c) 2018 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System;

namespace Plato.Messaging.RMQ.Interfaces
{
    public interface IRMQPool: IDisposable
    {
        IRMQPoolContainer<IRMQPublisherBytes> GetBytesPublisher(string connectionName, string queueName);
        IRMQPoolContainer<IRMQSubscriberBytes> GetBytesSubscriber(string connectionName, string queueName);
        IRMQPoolContainer<IRMQPublisherText> GetTextPublisher(string connectionName, string queueName);
        IRMQPoolContainer<IRMQSubscriberText> GetTextSubscriber(string connectionName, string queueName);
        IRMQPoolContainer<IRMQReceiverBytes> GetBytesConsumer(string connectionName, string queueName);
        IRMQPoolContainer<IRMQProducerBytes> GetBytesProducer(string connectionName, string queueName);
        IRMQPoolContainer<IRMQReceiverText> GetTextConsumer(string connectionName, string queueName);
        IRMQPoolContainer<IRMQProducerText> GetTextProducer(string connectionName, string queueName);
    }
}