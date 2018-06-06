﻿// Plato.NET
// Copyright (c) 2018 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System;
using System.Threading.Tasks;

namespace Plato.Messaging.AMQ.Interfaces
{
    public interface IAMQPoolContainerAsync<T> : IDisposable where T : class
    {
        Guid PoolId { get; }        
        Task<long> TotalPoolSizeAsync();
        T Instance { get; }
    }
}
