// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Redis.Interfaces;
using StackExchange.Redis;
using System;
using System.Threading;

namespace Plato.Redis
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Redis.Interfaces.IRedisControl" />
    public class RedisControl : IRedisControl
    {
        private int _lockCount;

        /// <summary>
        /// Gets the redis database.
        /// </summary>
        /// <value>
        /// The redis database.
        /// </value>
        public IDatabase RedisDb { get; private set; }

        /// <summary>
        /// Gets the redis key.
        /// </summary>
        /// <value>
        /// The redis key.
        /// </value>
        public string RedisKey { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RedisControl"/> class.
        /// </summary>
        /// <param name="redisDb">The redis database.</param>
        /// <param name="redisKey">The redis key.</param>
        public RedisControl(IDatabase redisDb, string redisKey)
        {
            RedisDb = redisDb;
            RedisKey = redisKey;
            _lockCount = 0;
        }

        /// <summary>
        /// Locks the specified timeout.
        /// </summary>
        /// <param name="timeout">The timeout.</param>
        /// <param name="lockLength">The lock length.</param>
        /// <returns></returns>
        public bool Lock(int timeout = Timeout.Infinite, TimeSpan? lockLength = null)
        {
            if(_lockCount > 0)
            {
                _lockCount++;
                return true;
            }

            var sleeped = 0;
            var ttl = lockLength ?? TimeSpan.FromSeconds(5);
            var result = false;

            while (true)
            {
                if(RedisDb.StringSet($"{RedisKey}.locked", "locked", ttl, when: When.NotExists))
                {
                    _lockCount = 1;
                    result = true;
                    break;
                }

                Thread.Sleep(10);
                sleeped += 10;

                if (timeout != Timeout.Infinite && sleeped >= timeout)
                {
                    result = false;
                    break;
                }
            }

            return result;            
        }

        /// <summary>
        /// Unlocks this instance.
        /// </summary>
        public void Unlock()
        {
            if(_lockCount > 0)
            {
                _lockCount--;
                if(_lockCount == 0)
                {
                    RedisDb.KeyDelete($"{RedisKey}.locked");
                }
            }
        }
    }
}
