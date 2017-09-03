// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using MsgPack;
using MsgPack.Serialization;
using Plato.Redis.Interfaces;
using StackExchange.Redis;
using System;
using System.IO;

namespace Plato.Redis.Serializers
{
    /// <summary>
    /// 
    /// </summary>    
    /// <seealso cref="Plato.Redis.Interfaces.IRedisSerializer" />
    public class MsgPackRedisSerializer : IRedisSerializer
    {
        /// <summary>
        /// Serializes the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public RedisValue Serialize(object data)
        {
            if (data != null)
            {
                using (var ms = new MemoryStream())
                {
                    MessagePackSerializer.Get(data.GetType()).Pack(ms, data);
                    return ms.ToArray();
                }
            }

            return RedisValue.EmptyString;
        }

        /// <summary>
        /// Deserializes the specified data.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public T Deserialize<T>(RedisValue data)
        {
            if (data.HasValue)
            {
                using (var ms = new MemoryStream(data))
                {
                    return MessagePackSerializer.Get<T>().Unpack(ms);
                }
            }

            return default(T);
        }

        /// <summary>
        /// Deserializes the specified data.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public T Deserialize<T>(object data)
        {
            return MessagePackSerializer.Get<T>().FromMessagePackObject((MessagePackObject)data);
        }

        /// <summary>
        /// Deserializes the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public object Deserialize(object data, Type type)
        {
            return MessagePackSerializer.Get(type).FromMessagePackObject((MessagePackObject)data);
        }
    }
}
