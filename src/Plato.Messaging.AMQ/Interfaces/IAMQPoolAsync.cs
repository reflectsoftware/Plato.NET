// Plato.NET
// Copyright (c) 2018 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Messaging.Interfaces;
using System;
using System.Threading.Tasks;

namespace Plato.Messaging.AMQ.Interfaces
{
    public interface IAMQPoolAsync: IDisposable
    {
        Task<IAMQPoolContainer<T>> GetAsync<T>(string connectionName, string queueName) where T : IMessageReceiverSender;
    }
}