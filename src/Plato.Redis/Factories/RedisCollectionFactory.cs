using Plato.Redis.Collections;
using Plato.Redis.Interfaces;
using StackExchange.Redis;

namespace Plato.Redis.Factories
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Redis.Interfaces.IRedisCollectionFactory" />
    public class RedisCollectionFactory : IRedisCollectionFactory
    {
        /// <summary>
        /// Creates the dictionary.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="redisDb">The redis database.</param>
        /// <param name="redisKey">The redis key.</param>
        /// <param name="valueSerializer">The value serializer.</param>
        /// <returns></returns>
        public IRedisDictionary<TKey, TValue> CreateDictionary<TKey, TValue>(
            IDatabase redisDb,
            RedisKey redisKey,
            IRedisSerializer valueSerializer = null)
        {
            return new RedisDictionary<TKey, TValue>(redisDb, redisKey, valueSerializer);
        }

        /// <summary>
        /// Creates the list.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="redisDb">The redis database.</param>
        /// <param name="redisKey">The redis key.</param>
        /// <param name="valueSerializer">The value serializer.</param>
        /// <returns></returns>
        public IRedisList<TValue> CreateList<TValue>(
            IDatabase redisDb,
            RedisKey redisKey,
            IRedisSerializer valueSerializer = null)
        {
            return new RedisList<TValue>(redisDb, redisKey, valueSerializer);
        }

        /// <summary>
        /// Creates the queue.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="redisDb">The redis database.</param>
        /// <param name="redisKey">The redis key.</param>
        /// <param name="valueSerializer">The value serializer.</param>
        /// <returns></returns>
        public IRedisQueue<TValue> CreateQueue<TValue>(
            IDatabase redisDb,
            RedisKey redisKey,
            IRedisSerializer valueSerializer = null)
        {
            return new RedisQueue<TValue>(redisDb, redisKey, valueSerializer);
        }

        /// <summary>
        /// Creates the stack.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="redisDb">The redis database.</param>
        /// <param name="redisKey">The redis key.</param>
        /// <param name="valueSerializer">The value serializer.</param>
        /// <returns></returns>
        public IRedisStack<TValue> CreateStack<TValue>(
            IDatabase redisDb,
            RedisKey redisKey,
            IRedisSerializer valueSerializer = null)
        {
            return new RedisStack<TValue>(redisDb, redisKey, valueSerializer);
        }
    }
}
