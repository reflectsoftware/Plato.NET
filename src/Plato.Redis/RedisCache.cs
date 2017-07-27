// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Cache;
using Plato.Cache.Interfaces;
using Plato.Core.Miscellaneous;
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
        private readonly IRedisCollectionSerializer _valueSerializer;
        private readonly IDatabase _redisDb;
        private readonly string _prefixName;

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
        /// <param name="serializer">The serializer.</param>
        /// <param name="prefixName">Name of the prefix.</param>
        public RedisCache(
            IRedisConnection connection,
            IRedisCacheKeyLockAcquisition cacheKeyLockAcquisition,             
            IRedisCollectionSerializer serializer = null,
            string prefixName = "RedisCache")
        {        
            Guard.AgainstNull(() => connection);
            Guard.AgainstNull(() => cacheKeyLockAcquisition);            
            
            Disposed = false;

            _connection = connection;
            _cacheKeyLockAcquisition = cacheKeyLockAcquisition;            
            _redisDb = connection.GetDatabase();            
            _valueSerializer = serializer ?? new JsonRedisCollectionSerializer();
            _prefixName = string.IsNullOrWhiteSpace(prefixName) ? "RedisCache" : prefixName;
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
            lock(this)
            {
                if(!Disposed)
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
        /// Gets the time span.
        /// </summary>
        /// <param name="keepAlive">The keep alive.</param>
        /// <returns></returns>
        protected TimeSpan? GetTimeToLive(TimeSpan? keepAlive)
        {            
            return keepAlive = keepAlive == TimeSpan.Zero ? null : keepAlive;
        }

        /// <summary>
        /// Suffixes the name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        protected string PrefixName(string name)
        {
            return $"{_prefixName}:{name}";
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
            var key = PrefixName(name);
            var value = _redisDb.StringGet(key);
            if(value.HasValue)
            {
                return _valueSerializer.Deserialize<T>(value);
            }

            if(callback == null)
            {
                return default(T);
            }

            using (var cacheLock = _cacheKeyLockAcquisition.AcquireLock(_redisDb, key))
            {
                value = _redisDb.StringGet(key);
                if(!value.HasValue)
                {
                    var cData = callback(name, args);
                    Set(name, cData.NewCacheData, cData.KeepAlive);

                    return cData.NewCacheData;
                }

                return _valueSerializer.Deserialize<T>(value);
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
            var key = PrefixName(name);
            var value = await _redisDb.StringGetAsync(key);
            if (value.HasValue)
            {
                return _valueSerializer.Deserialize<T>(value);
            }

            if (callbackAsync == null)
            {
                return default(T);
            }

            using (var cacheLock = await _cacheKeyLockAcquisition.AcquireLockAsync(_redisDb, key))
            {
                value = await _redisDb.StringGetAsync(key);
                if (!value.HasValue)
                {
                    var cData = await callbackAsync(name, args);
                    await SetAsync(name, cData.NewCacheData, cData.KeepAlive);

                    return cData.NewCacheData;
                }

                return _valueSerializer.Deserialize<T>(value);
            }
        }

        /// <summary>
        /// Removes the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public bool Remove(string name)
        {
            return _redisDb.KeyDelete(PrefixName(name));
        }

        /// <summary>
        /// Removes the asynchronous.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public async Task<bool> RemoveAsync(string name)
        {
            return await _redisDb.KeyDeleteAsync(PrefixName(name));
        }

        /// <summary>
        /// Sets the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="item">The item.</param>
        /// <param name="keepAlive">The keep alive.</param>
        public void Set(string name, object item, TimeSpan? keepAlive)
        {            
            _redisDb.StringSet(PrefixName(name), _valueSerializer.Serialize(item), GetTimeToLive(keepAlive));        
        }

        /// <summary>
        /// Sets the asynchronous.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="item">The item.</param>
        /// <param name="keepAlive">The keep alive.</param>
        /// <returns></returns>
        public async Task SetAsync(string name, object item, TimeSpan? keepAlive)
        {
            await _redisDb.StringSetAsync(PrefixName(name), _valueSerializer.Serialize(item), GetTimeToLive(keepAlive));
        }
    }
}
