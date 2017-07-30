// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Cache.Interfaces;
using Plato.Redis.Interfaces;
using StackExchange.Redis;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Plato.Redis
{
    /// <summary>
    /// 
    /// </summary>
    public class RedisCacheKeyLockAcquisition : IRedisCacheKeyLockAcquisition
    {
        /// <summary>
        /// Acquires the lock.
        /// </summary>
        /// <param name="db">The database.</param>
        /// <param name="key">The key.</param>
        /// <param name="retryTimeout">The retry timeout.</param>
        /// <param name="expiry">The expiry.</param>
        /// <returns></returns>
        public ICacheKeyLock AcquireLock(IDatabase db, RedisKey key, TimeSpan? retryTimeout = null, TimeSpan? expiry = null)
        {
            var cacheLocked = new RedisCacheKeyLock(db, key);
            var now = DateTime.Now;
            var rnd = new Random((int)now.Ticks);

            expiry = expiry ?? TimeSpan.FromMinutes(2);
            retryTimeout = retryTimeout ?? TimeSpan.MaxValue;

            while (true)
            {
                var locked = db.LockTake(cacheLocked.LockKey, cacheLocked.LockValue, expiry.Value);
                if (locked)
                {
                    cacheLocked.Locked = true;
                    break;
                }

                if (DateTime.Now.Subtract(now) >= retryTimeout)
                {
                    break;
                }
                
                Thread.Sleep(rnd.Next(0, 3));
            }

            return cacheLocked;
        }

        /// <summary>
        /// Acquires the lock asynchronous.
        /// </summary>
        /// <param name="db">The database.</param>
        /// <param name="key">The key.</param>
        /// <param name="retryTimeout">The retry timeout.</param>
        /// <param name="expiry">The expiry.</param>
        /// <returns></returns>
        public Task<ICacheKeyLock> AcquireLockAsync(IDatabase db, RedisKey key, TimeSpan? retryTimeout = null, TimeSpan? expiry = null)
        {
            return Task.FromResult(AcquireLock(db, key, retryTimeout, expiry));
        }
    }
}
