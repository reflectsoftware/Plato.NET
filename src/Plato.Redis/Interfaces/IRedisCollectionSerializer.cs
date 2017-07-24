using StackExchange.Redis;

namespace Plato.Redis.Interfaces
{
    public interface IRedisCollectionSerializer
    {
        RedisValue Serialize(object data);
        T Deserialize<T>(RedisValue value);
    }
}
