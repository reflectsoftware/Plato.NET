// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System.Collections;
using System.Collections.Generic;

namespace Plato.Redis.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="Plato.Redis.Interfaces.IRedisControl" />
    /// <seealso cref="System.Collections.Generic.IEnumerable{T}" />
    /// <seealso cref="System.Collections.IEnumerable" />
    /// <seealso cref="System.Collections.ICollection" />
    /// <seealso cref="System.Collections.Generic.IReadOnlyCollection{T}" />
    public interface IRedisQueue<T> : IRedisControl, IEnumerable<T>, IEnumerable, ICollection, IReadOnlyCollection<T>
    {
        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        void Clear();

        /// <summary>
        /// Determines whether [contains] [the specified item].
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>
        ///   <c>true</c> if [contains] [the specified item]; otherwise, <c>false</c>.
        /// </returns>
        bool Contains(T item);

        /// <summary>
        /// Dequeues this instance.
        /// </summary>
        /// <returns></returns>
        T Dequeue();

        /// <summary>
        /// Enqueues the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        void Enqueue(T item);

        /// <summary>
        /// Peeks this instance.
        /// </summary>
        /// <returns></returns>
        T Peek();

        /// <summary>
        /// To the array.
        /// </summary>
        /// <returns></returns>
        T[] ToArray();
    }
}
