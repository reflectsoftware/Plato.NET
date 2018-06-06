// Plato.NET
// Copyright (c) 2018 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Messaging.AMQ.Interfaces;
using Plato.Messaging.AMQ.Settings;

namespace Plato.Messaging.AMQ.Pool
{
    public class AMQPoolStates
    {        
        public AMQConnectionSettings Connection { get; set; }
        public AMQDestinationSettings Destination { get; set; }
        internal IAMQSenderFactory SenderFactory { get; set; }
        internal IAMQReceiverFactory ReceiverFactory { get; set; }
    }
}
