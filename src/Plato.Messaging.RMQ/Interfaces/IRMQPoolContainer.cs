// Plato.NET
// Copyright (c) 2018 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System;

namespace Plato.Messaging.RMQ.Interfaces
{
    public interface IRMQPoolContainer<T> : IDisposable where T : class
    {
        Guid PoolId { get; }        
        long TotalPoolSize();
        T Instance { get; }
    }
}
