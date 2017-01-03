// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Serializers.FormatterPools;
using Plato.Serializers.Interfaces;
using System;
using System.IO;

namespace Plato.Serializers
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="Plato.Serializers.ObjectSerializerBase"/>
    public class ObjectFastSerializer : ObjectSerializerBase
    {
        /// <summary>
        /// Serializes the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="obj">The object.</param>
        public override void Serialize(Stream stream, object obj)
        {
            FastFormatterPool.Pool.Serialize(stream, obj as IFastBinarySerializable);
        }

        /// <summary>
        /// Deserializes the specified b object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bObj">The b object.</param>
        /// <returns></returns>
        public override T Deserialize<T>(byte[] bObj)
        {
            return FastFormatterPool.Pool.Deserialize<T>(bObj);
        }

        /// <summary>
        /// Deserializes the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="bObj">The b object.</param>
        /// <returns></returns>
        public override object Deserialize(Type type, byte[] bObj)
        {
            return FastFormatterPool.Pool.Deserialize(bObj, type);
        }

        /// <summary>
        /// Deserializes the specified stream.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        public override T Deserialize<T>(Stream stream)
        {
            return FastFormatterPool.Pool.Deserialize<T>(stream);
        }

        /// <summary>
        /// Deserializes the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        public override object Deserialize(Type type, Stream stream)
        {
            return FastFormatterPool.Pool.Deserialize(stream, type);
        }
    }
}
