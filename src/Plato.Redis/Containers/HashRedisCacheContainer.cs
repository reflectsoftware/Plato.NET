// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Core.Miscellaneous;
using Plato.Redis.Interfaces;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace Plato.Redis.Containers
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Redis.Interfaces.IRedisCacheContainer" />
    public class HashRedisCacheContainer : IRedisCacheContainer
    {
        private readonly IRedisConnection _connection;
        private readonly IDatabase _redisDb;        
        private readonly string _name;

        public bool SupportsExpiration => false;

        /// <summary>
        /// Initializes a new instance of the <see cref="HashRedisCacheContainer" /> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="name">The name.</param>
        public HashRedisCacheContainer(            
            IRedisConnection connection,             
            string name = null)
        {
            Guard.AgainstNull(() => connection);
                        
            _connection = connection;            
            _redisDb = connection.GetDatabase();            
            _name = string.IsNullOrWhiteSpace(name) ? "HashRedisCache" : name;
        }      

        /// <summary>
        /// Gets the time to live.
        /// </summary>
        /// <param name="keepAlive">The keep alive.</param>
        /// <returns></returns>
        protected TimeSpan? GetTimeToLive(TimeSpan? keepAlive)
        {
            return keepAlive = keepAlive == TimeSpan.Zero ? null : keepAlive;
        }

        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public RedisValue Get(string key)
        {            
            return _redisDb.HashGet(_name, key);
        }

        /// <summary>
        /// Gets the asynchronous.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public async Task<RedisValue> GetAsync(string key)
        {
            return await _redisDb.HashGetAsync(_name, key);
        }

        /// <summary>
        /// Sets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="keepAlive">The keep alive.</param>
        /// <returns></returns>
        public bool Set(string key, RedisValue value, TimeSpan? keepAlive = null)
        {
            return _redisDb.HashSet(_name, key, value);            
        }

        /// <summary>
        /// Sets the asynchronous.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="item">The item.</param>
        /// <param name="keepAlive">The keep alive.</param>
        /// <returns></returns>
        public async Task<bool> SetAsync(string key, RedisValue value, TimeSpan? keepAlive = null)
        {
            return await _redisDb.HashSetAsync(_name, key, value);
        }

        /// <summary>
        /// Removes the specified name.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public bool Remove(string key)
        {
            return _redisDb.HashDelete(_name, key);
        }

        /// <summary>
        /// Removes the asynchronous.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public async Task<bool> RemoveAsync(string key)
        {
            return await _redisDb.HashDeleteAsync(_name, key);
        }
    }
}
