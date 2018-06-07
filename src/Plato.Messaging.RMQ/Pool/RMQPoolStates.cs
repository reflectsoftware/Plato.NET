// Plato.NET
// Copyright (c) 2018 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Messaging.RMQ.Interfaces;
using Plato.Messaging.RMQ.Settings;

namespace Plato.Messaging.RMQ.Pool
{
    public class RMQPoolStates
    {        
        public RMQConnectionSettings Connection { get; set; }
        public RMQQueueSettings Queue { get; set; }
        internal IRMQProducerFactory ProducerFactory { get; set; }
        internal IRMQConsumerFactory ConsumerFactory { get; set; }
        internal IRMQPublisherFactory PublisherFactory { get; set; }
        internal IRMQSubscriberFactory SubscriberFactory { get; set; }
    }
}
