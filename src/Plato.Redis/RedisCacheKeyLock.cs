// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Cache.Interfaces;
using Plato.Core.Miscellaneous;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace Plato.Redis
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Cache.Interfaces.ICacheKeyLock" />    
    public class RedisCacheKeyLock : ICacheKeyLock
    {
        internal IDatabase Db { get; private set; }
        internal RedisKey LockKey { get; private set; }
        internal RedisValue LockValue { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="RedisCacheKeyLock"/> is disposed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if disposed; otherwise, <c>false</c>.
        /// </value>
        public bool Disposed { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="RedisCacheKeyLock"/> is locked.
        /// </summary>
        /// <value>
        ///   <c>true</c> if locked; otherwise, <c>false</c>.
        /// </value>
        public bool Locked { get; internal set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RedisCacheKeyLock"/> class.
        /// </summary>
        /// <param name="db">The database.</param>
        /// <param name="key">The key.</param>
        internal RedisCacheKeyLock(IDatabase db, RedisKey key)
        {
            Guard.AgainstNull(() => db);
            Guard.AgainstNull(() => key);

            Disposed = false;
            Locked = false;

            Db = db;
            LockKey = $"{key}.plato.lock";
            LockValue = $"{key}.plato.lock.value";
        }

        #region Dispose       
        /// <summary>
        /// Finalizes an instance of the <see cref="RedisCacheKeyLock"/> class.
        /// </summary>
        ~RedisCacheKeyLock()
        {
            Dispose(false);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!Disposed)
            {
                Disposed = true;                            
                GC.SuppressFinalize(this);
                Unlock();            
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion

        /// <summary>
        /// Unlocks this instance.
        /// </summary>
        public void Unlock()
        {
            if (Locked)
            {
                Locked = false;
                Db.LockRelease(LockKey, LockValue);                
            }
        }

        /// <summary>
        /// Unlocks the asynchronous.
        /// </summary>
        /// <returns></returns>
        public async Task UnlockAsync()
        {
            if (Locked)
            {
                Locked = false;
                await Db.LockReleaseAsync(LockKey, LockValue);                
            }
        }
    }
}
