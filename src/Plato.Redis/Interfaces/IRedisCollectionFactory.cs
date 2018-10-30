using Plato.Redis.Interfaces;
using StackExchange.Redis;

namespace Plato.Redis.Interfaces
{
    public interface IRedisCollectionFactory
    {
        IRedisDictionary<TKey, TValue> CreateDictionary<TKey, TValue>(IDatabase redisDb, RedisKey redisKey, IRedisSerializer valueSerializer = null);
        IRedisList<TValue> CreateList<TValue>(IDatabase redisDb, RedisKey redisKey, IRedisSerializer valueSerializer = null);
        IRedisQueue<TValue> CreateQueue<TValue>(IDatabase redisDb, RedisKey redisKey, IRedisSerializer valueSerializer = null);
        IRedisStack<TValue> CreateStack<TValue>(IDatabase redisDb, RedisKey redisKey, IRedisSerializer valueSerializer = null);
    }
}