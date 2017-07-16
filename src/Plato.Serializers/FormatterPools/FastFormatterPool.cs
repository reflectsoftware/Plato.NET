// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Cache;
using Plato.Serializers.Interfaces;
using System;
using System.IO;

namespace Plato.Serializers.FormatterPools
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="Plato.Cache.GenericObjectPool{Plato.Serializers.FastBinaryFormatter}"/>
    public class FastFormatterPool : GenericObjectPool<FastBinaryFormatter, object>
    {
        /// <summary>
        /// Gets the pool.
        /// </summary>
        /// <value>
        /// The pool.
        /// </value>
        public static FastFormatterPool Pool { get; private set; }

        /// <summary>
        /// Initializes the <see cref="FastFormatterPool"/> class.
        /// </summary>
        static FastFormatterPool()
        {
            Pool = new FastFormatterPool();
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="FastFormatterPool"/> class from being created.
        /// </summary>
        private FastFormatterPool() : base(3, 25)
        {
        }

        /// <summary>
        /// Creates the pool object.
        /// </summary>
        /// <returns></returns>
        protected override FastBinaryFormatter CreatePoolObject()
        {
            return new FastBinaryFormatter();
        }

        /// <summary>
        /// Serializes the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="obj">The object.</param>
        public void Serialize(Stream stream, IFastBinarySerializable obj)
        {
            using (var container = Pool.Container())
            {
                container.Instance.Serialize(stream, obj);
            }
        }

        /// <summary>
        /// Serializes the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public byte[] Serialize(IFastBinarySerializable obj)
        {
            using (var container = Pool.Container())
            {
                return container.Instance.Serialize(obj);
            }
        }

        /// <summary>
        /// Deserializes the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="oType">Type of the o.</param>
        /// <returns></returns>
        public IFastBinarySerializable Deserialize(Stream stream, Type oType)
        {
            using (var container = Pool.Container())
            {
                return container.Instance.Deserialize(stream, oType);
            }
        }

        /// <summary>
        /// Deserializes the specified graph.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="oType">Type of the o.</param>
        /// <returns></returns>
        public IFastBinarySerializable Deserialize(byte[] graph, Type oType)
        {
            using (var container = Pool.Container())
            {
                return container.Instance.Deserialize(graph, oType);
            }
        }

        /// <summary>
        /// Deserializes the specified stream.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        public T Deserialize<T>(Stream stream)
        {
            using (var container = Pool.Container())
            {
                return container.Instance.Deserialize<T>(stream);
            }
        }

        /// <summary>
        /// Deserializes the specified data.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public T Deserialize<T>(byte[] data)
        {
            using (var container = Pool.Container())
            {
                return container.Instance.Deserialize<T>(data);
            }
        }
    }
}
