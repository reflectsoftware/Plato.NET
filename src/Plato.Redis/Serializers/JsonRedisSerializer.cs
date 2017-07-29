// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Newtonsoft.Json;
using Plato.Redis.Interfaces;
using StackExchange.Redis;

namespace Plato.Redis.Serializers
{
    /// <summary>
    /// 
    /// </summary>    
    /// <seealso cref="Plato.Redis.Interfaces.IRedisSerializer" />
    public class JsonRedisSerializer : IRedisSerializer
    {
        /// <summary>
        /// Serializes the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public RedisValue Serialize(object data)
        {
            return data != null ? (RedisValue)JsonConvert.SerializeObject(data) : RedisValue.EmptyString;
        }

        /// <summary>
        /// Deserializes the specified data.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public T Deserialize<T>(RedisValue data)
        {
            return data.HasValue ? JsonConvert.DeserializeObject<T>(data) : default(T);
        }
    }
}
