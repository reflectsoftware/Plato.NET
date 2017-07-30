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
    public class StringRedisCacheContainer : IRedisCacheContainer
    {
        private readonly IRedisConnection _connection;
        private readonly IDatabase _redisDb;        
        private readonly string _prefixName;

        public bool SupportsExpiration => true;

        /// <summary>
        /// Initializes a new instance of the <see cref="StringRedisCacheContainer" /> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="name">The name.</param>
        public StringRedisCacheContainer(            
            IRedisConnection connection,             
            string name = null)
        {
            Guard.AgainstNull(() => connection);
                        
            _connection = connection;            
            _redisDb = connection.GetDatabase();            
            _prefixName = string.IsNullOrWhiteSpace(name) ? "StringRedisCache" : name;
        }

        /// <summary>
        /// Prefixes the name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        protected string PrefixName(string name)
        {
            return $"{_prefixName}:{name}";
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
            return _redisDb.StringGet(PrefixName(key));
        }

        /// <summary>
        /// Gets the asynchronous.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public async Task<RedisValue> GetAsync(string key)
        {
            return await _redisDb.StringGetAsync(PrefixName(key));
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
            return _redisDb.StringSet(PrefixName(key), value, GetTimeToLive(keepAlive));
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
            return await _redisDb.StringSetAsync(PrefixName(key), value, GetTimeToLive(keepAlive));
        }

        /// <summary>
        /// Removes the specified name.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public bool Remove(string key)
        {
            return _redisDb.KeyDelete(PrefixName(key));
        }

        /// <summary>
        /// Removes the asynchronous.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public async Task<bool> RemoveAsync(string key)
        {
            return await _redisDb.KeyDeleteAsync(PrefixName(key));
        }
    }
}
