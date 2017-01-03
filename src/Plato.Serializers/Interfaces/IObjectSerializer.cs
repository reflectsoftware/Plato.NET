// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System;
using System.IO;
using System.Text;

namespace Plato.Serializers.Interfaces
{
    /// <summary>
    ///
    /// </summary>
    public interface IObjectSerializer
    {
        /// <summary>
        /// Serializes the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="obj">The object.</param>
        void Serialize(Stream stream, object obj);

        /// <summary>
        /// Serializes the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        byte[] Serialize(object obj);

        /// <summary>
        /// Serializes the to64 base string.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        string SerializeTo64BaseString(object obj);

        /// <summary>
        /// Serializes the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="encoder">The encoder.</param>
        /// <returns></returns>
        string Serialize(object obj, Encoding encoder);

        /// <summary>
        /// Deserializes the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        object Deserialize(Type type, Stream stream);

        /// <summary>
        /// Deserializes the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="bObj">The b object.</param>
        /// <returns></returns>
        object Deserialize(Type type, byte[] bObj);

        /// <summary>
        /// Deserializes the specified b object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bObj">The b object.</param>
        /// <returns></returns>
        T Deserialize<T>(byte[] bObj);

        /// <summary>
        /// Deserializes the specified stream.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        T Deserialize<T>(Stream stream);

        /// <summary>
        /// Deserialize64s the base string.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sObj">The s object.</param>
        /// <returns></returns>
        T Deserialize64BaseString<T>(string sObj);

        /// <summary>
        /// Deserializes the specified s object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sObj">The s object.</param>
        /// <param name="encoder">The encoder.</param>
        /// <returns></returns>
        T Deserialize<T>(string sObj, Encoding encoder);
    }
}
