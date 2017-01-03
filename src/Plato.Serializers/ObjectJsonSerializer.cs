// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System;
using System.IO;
using System.Runtime.Serialization.Json;

namespace Plato.Serializers
{
    public class ObjectJsonSerializer : ObjectSerializerBase
    {
        /// <summary>
        /// Serializes the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="obj">The object.</param>
        public override void Serialize(Stream stream, object obj)
        {
            new DataContractJsonSerializer(obj.GetType()).WriteObject(stream, obj);
        }

        /// <summary>
        /// Deserializes the specified b object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bObj">The b object.</param>
        /// <returns></returns>
        public override T Deserialize<T>(byte[] bObj)
        {
            using (var ms = new MemoryStream(bObj))
            {
                return (T)new DataContractJsonSerializer(typeof(T)).ReadObject(ms);
            }
        }

        /// <summary>
        /// Deserializes the specified iostream.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="iostream">The iostream.</param>
        /// <returns></returns>
        public override T Deserialize<T>(Stream iostream)
        {
            return (T)new DataContractJsonSerializer(typeof(T)).ReadObject(iostream);
        }

        /// <summary>
        /// Deserializes the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="iostream">The iostream.</param>
        /// <returns></returns>
        public override object Deserialize(Type type, Stream iostream)
        {
            return new DataContractJsonSerializer(type).ReadObject(iostream);
        }

        /// <summary>
        /// Deserializes the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="bObj">The b object.</param>
        /// <returns></returns>
        public override object Deserialize(Type type, byte[] bObj)
        {
            using (var ms = new MemoryStream(bObj))
            {
                return new DataContractJsonSerializer(type).ReadObject(ms);
            }
        }
    }
}
