// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Newtonsoft.Json;
using Plato.Redis.Interfaces;
using StackExchange.Redis;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Plato.Redis.Collections
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="Plato.Redis.Interfaces.IRedisStack{T}" />
    public class RedisStack<T> : IRedisCollection, IRedisStack<T>
    {
        public IDatabase RedisDb => _redisList.RedisDb;
        public string RedisKey => _redisList.RedisKey;

        private readonly RedisList<T> _redisList;

        /// <summary>
        /// Initializes a new instance of the <see cref="RedisQueue{T}"/> class.
        /// </summary>
        /// <param name="redisDb">The redis database.</param>
        /// <param name="_redisKey">The redis key.</param>
        public RedisStack(IDatabase redisDb, string _redisKey) 
        {
            _redisList = new RedisList<T>(redisDb, _redisKey);
        }

        /// <summary>
        /// Serializes the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        protected virtual string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// De-serializes the specified serialized.
        /// </summary>
        /// <param name="serialized">The serialized.</param>
        /// <returns></returns>
        protected virtual T Deserialize(string serialized)
        {
            return JsonConvert.DeserializeObject<T>(serialized);
        }

        /// <summary>
        /// Gets a value indicating whether access to the <see cref="T:System.Collections.ICollection" /> is synchronized (thread safe).
        /// </summary>
        public bool IsSynchronized
        {
            get { return true; }
        }

        /// <summary>
        /// Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection" />.
        /// </summary>
        public object SyncRoot
        {
            get { return this; }
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        public void Clear()
        {
            _redisList.Clear();
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1" /> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        /// <returns>
        /// true if <paramref name="item" /> is found in the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, false.
        /// </returns>
        public bool Contains(T item)
        {
            return _redisList.Contains(item);
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.ICollection" />.
        /// </summary>
        public int Count
        {
            get { return _redisList.Count; }
        }
        
        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.ICollection" /> to an <see cref="T:System.Array" />, starting at a particular <see cref="T:System.Array" /> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.ICollection" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
        /// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins.</param>
        public void CopyTo(Array array, int index)
        {
            var copyArray = ToArray();
            Array.Copy(array, index, copyArray, 0, copyArray.Length);
        }

        /// <summary>
        /// Enqueues the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Push(T item)
        {
            _redisList.RedisDb.ListLeftPush(_redisList.RedisKey, Serialize(item));
        }

        /// <summary>
        /// Dequeues this instance.
        /// </summary>
        /// <returns></returns>
        public T Pop()
        {
            var value = _redisList.RedisDb.ListLeftPop(_redisList.RedisKey);
            return value.HasValue ? Deserialize(value.ToString()) : default(T);
        }

        /// <summary>
        /// Peeks this instance.
        /// </summary>
        /// <returns></returns>
        public T Peek()
        {
            return _redisList[0];
        }

        /// <summary>
        /// To the array.
        /// </summary>
        /// <returns></returns>
        public T[] ToArray()
        {
            var array = new List<T>();
            foreach (var value in _redisList.RedisDb.ListRange(_redisList.RedisKey))
            {
                if(value.HasValue)
                {
                    array.Add(Deserialize(value.ToString()));
                }
            }

            return array.ToArray();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<T> GetEnumerator()
        {
            return _redisList.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _redisList.GetEnumerator();
        }
    }    
}
