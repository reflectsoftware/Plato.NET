// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using StackExchange.Redis;
using System;
using System.Collections.Generic;

namespace Plato.Redis.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>    
    /// <seealso cref="System.Collections.Generic.IDictionary{TKey, TValue}" />
    public interface IRedisDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        /// <summary>
        /// Gets the or add.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="addFunction">The add function.</param>
        /// <returns></returns>
        TValue GetOrAdd(TKey key, Func<TKey, TValue> addFunction);

        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="when">The when.</param>
        /// <returns></returns>
        bool Add(TKey key, TValue value, When when);

        /// <summary>
        /// Adds the multiple.
        /// </summary>
        /// <param name="items">The items.</param>
        void AddMultiple(IEnumerable<KeyValuePair<TKey, TValue>> items);
    }
}
