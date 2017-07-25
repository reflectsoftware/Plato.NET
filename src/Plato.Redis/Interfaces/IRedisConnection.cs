// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using StackExchange.Redis;
using System;

namespace Plato.Redis.Interfaces
{
    public interface IRedisConnection: IDisposable
    {
        bool Disposed { get; }
        IConnectionMultiplexer Connection { get; }                        
        IDatabase GetDatabase(int db = -1, object asyncState = null, RedisDatabaseProxyConfiguration config = null);
    }
}