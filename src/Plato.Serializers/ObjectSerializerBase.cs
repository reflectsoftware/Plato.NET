// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Serializers.Interfaces;
using System;
using System.IO;
using System.Text;

namespace Plato.Serializers
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="Plato.Serializers.Interfaces.IObjectSerializer"/>
    public abstract class ObjectSerializerBase : IObjectSerializer
    {
        /// <summary>
        /// Serializes the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="obj">The object.</param>
        public abstract void Serialize(Stream stream, object obj);

        /// <summary>
        /// Deserializes the specified b object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bObj">The b object.</param>
        /// <returns></returns>
        public abstract T Deserialize<T>(byte[] bObj);

        /// <summary>
        /// Deserializes the specified iostream.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="iostream">The iostream.</param>
        /// <returns></returns>
        public abstract T Deserialize<T>(Stream iostream);

        /// <summary>
        /// Deserializes the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="iostream">The iostream.</param>
        /// <returns></returns>
        public abstract object Deserialize(Type type, Stream iostream);

        /// <summary>
        /// Deserializes the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="bObj">The b object.</param>
        /// <returns></returns>
        public abstract object Deserialize(Type type, byte[] bObj);

        /// <summary>
        /// Serializes the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public byte[] Serialize(object obj)
        {
            using (var ms = new MemoryStream())
            {
                Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Serializes the to64 base string.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public string SerializeTo64BaseString(object obj)
        {
            return Convert.ToBase64String(Serialize(obj));
        }

        /// <summary>
        /// Deserialize64s the base string.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sObj">The s object.</param>
        /// <returns></returns>
        public T Deserialize64BaseString<T>(string sObj)
        {
            return Deserialize<T>(Convert.FromBase64String(sObj));
        }

        /// <summary>
        /// Serializes the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="encoder">The encoder.</param>
        /// <returns></returns>
        public string Serialize(object obj, Encoding encoder)
        {
            return encoder.GetString(Serialize(obj));
        }

        /// <summary>
        /// Deserializes the specified s object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sObj">The s object.</param>
        /// <param name="encoder">The encoder.</param>
        /// <returns></returns>
        public T Deserialize<T>(string sObj, Encoding encoder)
        {
            return Deserialize<T>(encoder.GetBytes(sObj));
        }
    }
}
