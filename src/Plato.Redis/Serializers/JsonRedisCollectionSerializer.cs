using Newtonsoft.Json;
using Plato.Redis.Interfaces;
using StackExchange.Redis;

namespace Plato.Redis.Serializers
{
    /// <summary>
    /// 
    /// </summary>    
    /// <seealso cref="Plato.Redis.Interfaces.IRedisCollectionSerializer" />
    public class JsonRedisCollectionSerializer : IRedisCollectionSerializer
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
        /// <typeparam name="T"></typeparam>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public T Deserialize<T>(RedisValue data)
        {
            return JsonConvert.DeserializeObject<T>(data);
        }
    }
}
