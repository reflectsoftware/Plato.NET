// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Nito.AsyncEx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plato.Async.Collections
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <seealso cref="Plato.Async.Collections.IAsyncConcurrentDictionary{TKey, TValue}" />
    public class AsyncConcurrentDictionary<TKey, TValue> : IAsyncConcurrentDictionary<TKey, TValue>
    {        
        private AsyncLock _locker;
        private Dictionary<TKey, TValue> _dictionary;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncConcurrentDictionary{TKey, TValue}"/> class.
        /// </summary>
        public AsyncConcurrentDictionary()
        {
            _locker = new AsyncLock();
            _dictionary = new Dictionary<TKey, TValue>();
        }

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public async Task AddAsync(TKey key, TValue value)
        {
            using (await _locker.LockAsync())
            {
                _dictionary.Add(key, value);
            }
        }

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public async Task AddAsync(KeyValuePair<TKey, TValue> item)
        {
            using (await _locker.LockAsync())
            {
                _dictionary.Add(item.Key, item.Value);
            }
        }

        /// <summary>
        /// Gets the or add asynchronous.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="asyncAction">The async action.</param>
        /// <returns></returns>
        public async Task<TValue> GetOrAddAsync(TKey key, Func<TKey, Task<TValue>> asyncAction)
        {
            using (await _locker.LockAsync())
            {
                var value = default(TValue);
                if (!_dictionary.ContainsKey(key))
                {
                    value = await asyncAction(key);
                    _dictionary.Add(key, value);
                }
                else
                {
                    value = _dictionary[key];
                }

                return value;
            }
        }

        /// <summary>
        /// Clears the asynchronous.
        /// </summary>
        /// <returns></returns>
        public async Task ClearAsync()
        {
            using (await _locker.LockAsync())
            {
                _dictionary.Clear();
            }
        }

        /// <summary>
        /// Determines whether the specified item contains asynchronous.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public async Task<bool> ContainsAsync(KeyValuePair<TKey, TValue> item)
        {
            using (await _locker.LockAsync())
            {
                return _dictionary.ContainsKey(item.Key);
            }
        }

        /// <summary>
        /// Determines whether [contains key asynchronous] [the specified key].
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public async Task<bool> ContainsKeyAsync(TKey key)
        {
            using (await _locker.LockAsync())
            {
                return _dictionary.ContainsKey(key);
            }
        }

        /// <summary>
        /// Counts the asynchronous.
        /// </summary>
        /// <returns></returns>
        public async Task<int> CountAsync()
        {
            using (await _locker.LockAsync())
            {
                return _dictionary.Count;
            }
        }

        /// <summary>
        /// Gets the asynchronous.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public async Task<TValue> GetAsync(TKey key)
        {
            using (await _locker.LockAsync())
            {
                return _dictionary[key];
            }
        }

        /// <summary>
        /// Gets the keys asynchronous.
        /// </summary>
        /// <returns></returns>
        public async Task<ICollection<TKey>> GetKeysAsync()
        {
            using (await _locker.LockAsync())
            {
                return _dictionary.Keys;
            }
        }

        /// <summary>
        /// Gets the values asynchronous.
        /// </summary>
        /// <returns></returns>
        public async Task<ICollection<TValue>> GetValuesAsync()
        {
            using (await _locker.LockAsync())
            {
                return _dictionary.Values;
            }
        }

        /// <summary>
        /// Removes the asynchronous.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public async Task<bool> RemoveAsync(TKey key)
        {
            using (await _locker.LockAsync())
            {
                return _dictionary.Remove(key);
            }
        }

        /// <summary>
        /// Removes the asynchronous.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public async Task<bool> RemoveAsync(KeyValuePair<TKey, TValue> item)
        {
            using (await _locker.LockAsync())
            {
                return _dictionary.Remove(item.Key);
            }
        }

        /// <summary>
        /// Values the asynchronous.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<KeyValuePair<TKey, TValue>>> ValuesAsync()
        {
            using (await _locker.LockAsync())
            {
                return _dictionary.ToList();
            }
        }
    }
}
