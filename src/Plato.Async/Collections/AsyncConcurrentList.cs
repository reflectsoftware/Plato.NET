// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Nito.AsyncEx;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plato.Async.Collections
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="Plato.Async.Collections.IAsyncConcurrentList{T}" />
    public class AsyncConcurrentList<T> : IAsyncConcurrentList<T>
    {
        private List<T> _list;
        private AsyncLock _locker;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncConcurrentList{T}"/> class.
        /// </summary>
        public AsyncConcurrentList()
        {
            _list = new List<T>();
            _locker = new AsyncLock();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncConcurrentList{T}"/> class.
        /// </summary>
        /// <param name="capacity">The capacity.</param>
        public AsyncConcurrentList(int capacity)
        {
            _list = new List<T>(capacity);
            _locker = new AsyncLock();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncConcurrentList{T}"/> class.
        /// </summary>
        /// <param name="collection">The collection.</param>
        public AsyncConcurrentList(IEnumerable<T> collection)
        {
            _list = new List<T>(collection);
            _locker = new AsyncLock();
        }

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public async Task AddAsync(T item)
        {
            using (await _locker.LockAsync())
            {
                _list.Add(item);
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
                _list.Clear();
            }
        }

        /// <summary>
        /// Determines whether the specified item contains asynchronous.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public async Task<bool> ContainsAsync(T item)
        {
            using (await _locker.LockAsync())
            {
                return _list.Contains(item);
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
                return _list.Count();
            }
        }

        /// <summary>
        /// Gets at asynchronous.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public async Task<T> GetAtAsync(int index)
        {
            using (await _locker.LockAsync())
            {
                return _list[index];
            }
        }

        /// <summary>
        /// Indexes the of asynchronous.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public async Task<int> IndexOfAsync(T item)
        {
            using (await _locker.LockAsync())
            {
                return _list.IndexOf(item);
            }            
        }

        /// <summary>
        /// Inserts the asynchronous.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public async Task InsertAsync(int index, T item)
        {
            using (await _locker.LockAsync())
            {
                _list.Insert(index, item);               
            }
        }

        /// <summary>
        /// Removes the asynchronous.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public async Task<bool> RemoveAsync(T item)
        {
            using (await _locker.LockAsync())
            {
                return _list.Remove(item);
            }
        }

        /// <summary>
        /// Removes at asynchronous.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public async Task RemoveAtAsync(int index)
        {
            using (await _locker.LockAsync())
            {
                _list.RemoveAt(index);
            }
        }

        /// <summary>
        /// Values the asynchronous.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<T>> ValuesAsync()
        {
            using (await _locker.LockAsync())
            {
                return _list.ToList();
            }
        }
    }
}
