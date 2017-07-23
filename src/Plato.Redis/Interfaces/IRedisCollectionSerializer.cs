using StackExchange.Redis;

namespace Plato.Redis.Interfaces
{
    public interface IRedisCollectionSerializer<T>
    {
        RedisValue Serialize(object data);
        T Deserialize(RedisValue value);

    }
}
