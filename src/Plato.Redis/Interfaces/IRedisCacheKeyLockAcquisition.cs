// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace Plato.Redis.Interfaces
{
    public interface IRedisCacheKeyLockAcquisition
    {
        RedisCacheKeyLock AcquireLock(IDatabase db, RedisKey key, TimeSpan? retryTimeout = null, TimeSpan? expiry = null);
        Task<RedisCacheKeyLock> AcquireLockAsync(IDatabase db, RedisKey key, TimeSpan? retryTimeout = null, TimeSpan? expiry = null);
    }
}