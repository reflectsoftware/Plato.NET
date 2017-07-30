// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Cache;
using Plato.Cache.Interfaces;
using Plato.Core.Miscellaneous;
using Plato.Redis.Containers;
using Plato.Redis.Interfaces;
using Plato.Redis.Serializers;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace Plato.Redis
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Cache.Interfaces.IGlobalCache" />
    public class RedisCache : IGlobalCache
    {
        private readonly IRedisConnection _connection;
        private readonly IRedisCacheKeyLockAcquisition _cacheKeyLockAcquisition;
        private readonly IRedisCacheContainer _container;
        private readonly IRedisSerializer _serialier;
        private readonly IDatabase _redisDb;

        /// <summary>
        /// Gets a value indicating whether this <see cref="RedisCache"/> is disposed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if disposed; otherwise, <c>false</c>.
        /// </value>
        public bool Disposed { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RedisCache" /> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="cacheKeyLockAcquisition">The cache key lock acquisition.</param>
        /// <param name="container">The container.</param>
        /// <param name="serializer">The serializer.</param>
        public RedisCache(
            IRedisConnection connection,
            IRedisCacheKeyLockAcquisition cacheKeyLockAcquisition,
            IRedisCacheContainer container = null,
            IRedisSerializer serializer = null)
        {
            Guard.AgainstNull(() => connection);
            Guard.AgainstNull(() => cacheKeyLockAcquisition);

            Disposed = false;

            _connection = connection;
            _cacheKeyLockAcquisition = cacheKeyLockAcquisition;
            _redisDb = connection.GetDatabase();
            _container = container ?? new StringRedisCacheContainer(_connection);
            _serialier = serializer ?? new JsonRedisSerializer();
        }

        #region Dispose
        /// <summary>
        /// Finalizes an instance of the <see cref="RedisCache"/> class.
        /// </summary>
        ~RedisCache()
        {
            Dispose(false);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            lock (this)
            {
                if (!Disposed)
                {
                    Disposed = true;
                    GC.SuppressFinalize(this);
                }
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
        /// Determines whether the specified c information has expired.
        /// </summary>
        /// <param name="cInfo">The c information.</param>
        /// <returns>
        ///   <c>true</c> if the specified c information has expired; otherwise, <c>false</c>.
        /// </returns>
        private bool HasExpired(CacheDataInfo cInfo)
        {
            return cInfo.KeepAlive != TimeSpan.Zero && DateTime.Now.Subtract(cInfo.CachedDateTime) >= cInfo.KeepAlive;
        }
        
        /// <summary>
        /// Gets the specified name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">The name.</param>
        /// <param name="callback">The callback.</param>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        public T Get<T>(string name, Func<string, object[], CacheDataInfo<T>> callback = null, params object[] args)
        {            
            var value = _container.Get(name);
            if(value.HasValue)
            {
                var cInfo = _serialier.Deserialize<CacheDataInfo<T>>(value);
                if(_container.SupportsExpiration || !HasExpired(cInfo))
                {
                    return cInfo.NewCacheData;
                }

                Remove(name);
            }

            if (callback == null)
            {
                return default(T);
            }

            using (var cacheLock = _cacheKeyLockAcquisition.AcquireLock(_redisDb, name))
            {
                value = _container.Get(name);
                if (!value.HasValue)
                {
                    var cData = callback(name, args);
                    if (cData == null)
                    {
                        return default(T);
                    }

                    Set(name, cData.NewCacheData, cData.KeepAlive);
                    return cData.NewCacheData;
                }
                
                var cInfo = _serialier.Deserialize<CacheDataInfo<T>>(value);
                return cInfo.NewCacheData;
            }
        }

        /// <summary>
        /// Gets the asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">The name.</param>
        /// <param name="callbackAsync">The callback asynchronous.</param>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        public async Task<T> GetAsync<T>(string name, Func<string, object[], Task<CacheDataInfo<T>>> callbackAsync = null, params object[] args)
        {
            var value = await _container.GetAsync(name);
            if (value.HasValue)
            {
                var cInfo = _serialier.Deserialize<CacheDataInfo<T>>(value);
                if (_container.SupportsExpiration || !HasExpired(cInfo))
                {
                    return cInfo.NewCacheData;
                }

                await RemoveAsync(name);
            }

            if (callbackAsync == null)
            {
                return default(T);
            }

            using (var cacheLock = await _cacheKeyLockAcquisition.AcquireLockAsync(_redisDb, name))
            {
                value = await _container.GetAsync(name);
                if (!value.HasValue)
                {
                    var cData = await callbackAsync(name, args);
                    if (cData == null)
                    {
                        return default(T);
                    }

                    await SetAsync(name, cData.NewCacheData, cData.KeepAlive);
                    return cData.NewCacheData;
                }

                var cInfo = _serialier.Deserialize<CacheDataInfo<T>>(value);
                return cInfo.NewCacheData;
            }
        }

        /// <summary>
        /// Removes the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public bool Remove(string name)
        {
            return _container.Remove(name);
        }

        /// <summary>
        /// Removes the asynchronous.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public async Task<bool> RemoveAsync(string name)
        {
            return await _container.RemoveAsync(name);
        }

        /// <summary>
        /// Sets the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="item">The item.</param>
        /// <param name="keepAlive">The keep alive.</param>
        public void Set(string name, object item, TimeSpan? keepAlive = null)
        {
            var cInfo = new CacheDataInfo<object>
            {
                CachedDateTime = DateTime.Now,
                KeepAlive = keepAlive ?? TimeSpan.Zero,
                NewCacheData = item,
            };

            _container.Set(name, _serialier.Serialize(cInfo), keepAlive);

            // _container.Set(name, _serialier.Serialize(item), keepAlive);            
        }

        /// <summary>
        /// Sets the asynchronous.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="item">The item.</param>
        /// <param name="keepAlive">The keep alive.</param>
        /// <returns></returns>
        public async Task SetAsync(string name, object item, TimeSpan? keepAlive = null)
        {
            await _container.SetAsync(name, _serialier.Serialize(item), keepAlive);            
        }
    }
}
