﻿// Plato.NET
// Copyright (c) 2018 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System;

namespace Plato.Messaging.AMQ.Interfaces
{
    public interface IAMQPool: IDisposable
    {        
        IAMQPoolContainer<IAMQReceiverText> GetConsumer(string connectionName, string queueName);
        IAMQPoolContainer<IAMQSenderText> GetProducer(string connectionName, string queueName);
    }
}