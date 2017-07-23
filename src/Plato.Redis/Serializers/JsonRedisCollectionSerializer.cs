using Newtonsoft.Json;
using Plato.Redis.Interfaces;
using StackExchange.Redis;

namespace Plato.Redis.Serializers
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="Plato.Redis.Interfaces.IRedisCollectionSerializer{T}" />
    public class JsonRedisCollectionSerializer<T> : IRedisCollectionSerializer<T>
    {
        /// <summary>
        /// Serializes the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public RedisValue Serialize(object data)
        {
            return JsonConvert.SerializeObject(data);
        }

        /// <summary>
        /// Deserializes the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public T Deserialize(RedisValue data)
        {
            return JsonConvert.DeserializeObject<T>(data);
        }
    }
}
