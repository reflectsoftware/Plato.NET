using MsgPack.Serialization;
using Plato.Redis.Interfaces;
using StackExchange.Redis;
using System.IO;

namespace Plato.Redis.Serializers
{
    /// <summary>
    /// 
    /// </summary>    
    /// <seealso cref="Plato.Redis.Interfaces.IRedisCollectionSerializer" />
    public class MsgPackRedisCollectionSerializer : IRedisCollectionSerializer
    {
        /// <summary>
        /// Serializes the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public RedisValue Serialize(object data)
        {
            using (var ms = new MemoryStream())
            {
                MessagePackSerializer.Get(data.GetType()).Pack(ms, data);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Deserializes the specified data.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public T Deserialize<T>(RedisValue data)
        {
            using (var ms = new MemoryStream(data))
            {
                return MessagePackSerializer.Get<T>().Unpack(ms);
            }
        }
    }
}
