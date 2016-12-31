// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
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
    /// <seealso cref="Plato.Redis.RedisControl" />
    /// <seealso cref="Plato.Redis.Interfaces.IRedisControl" />
    /// <seealso cref="System.Collections.Generic.IList{T}" />
    public class RedisList<T> : RedisControl, IRedisControl, IList<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RedisList{T}" /> class.
        /// </summary>
        /// <param name="redisDb">The redis database.</param>
        /// <param name="redisKey">The key.</param>
        public RedisList(IDatabase redisDb, string redisKey) : base(redisDb, redisKey)
        {
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
        /// Inserts an item to the <see cref="T:System.Collections.Generic.IList`1" /> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item" /> should be inserted.</param>
        /// <param name="item">The object to insert into the <see cref="T:System.Collections.Generic.IList`1" />.</param>
        public void Insert(int index, T item)
        {
            if (RedisDb.ListLength(RedisKey) > index)
            {
                var before = RedisDb.ListGetByIndex(RedisKey, index);
                RedisDb.ListInsertBefore(RedisKey, before, Serialize(item));
            }
            else
            {
                throw new IndexOutOfRangeException($"Index: '{index}' for Redis list: '{RedisKey}' is out of range.");
            }
        }

        /// <summary>
        /// Removes the <see cref="T:System.Collections.Generic.IList`1" /> item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param>
        public void RemoveAt(int index)
        {            
            var value = RedisDb.ListGetByIndex(RedisKey, index);
            if (!value.IsNull)
            {
                RedisDb.ListRemove(RedisKey, value);
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="T"/> at the specified index.
        /// </summary>
        /// <value>
        /// The <see cref="T"/>.
        /// </value>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public T this[int index]
        {
            get
            {
                var value = RedisDb.ListGetByIndex(RedisKey, index);
                return value.HasValue ? Deserialize(value.ToString()) : default(T);
            }
            set
            {
                Insert(index, value);
            }
        }

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        public void Add(T item)
        {
            RedisDb.ListRightPush(RedisKey, Serialize(item));
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        public void Clear()
        {
            RedisDb.KeyDelete(RedisKey);
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
            for (int i = 0; i < Count; i++)
            {
                var value = RedisDb.ListGetByIndex(RedisKey, i);
                if(!value.HasValue)
                {
                    // we reached an are of the list where there are no longer any values.
                    break;
                }

                if(value.ToString().Equals(Serialize(item)))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1" /> to an <see cref="T:System.Array" />, starting at a particular <see cref="T:System.Array" /> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
        /// <param name="index">The index.</param>
        public void CopyTo(T[] array, int index)
        {
            var values = RedisDb.ListRange(RedisKey);
            for(var i=0; i< values.Length; i++)
            {
                array[index + i] = values[i].HasValue ? Deserialize(values[i].ToString()) : default(T);
            }
        }

        /// <summary>
        /// Determines the index of a specific item in the <see cref="T:System.Collections.Generic.IList`1" />.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.IList`1" />.</param>
        /// <returns>
        /// The index of <paramref name="item" /> if found in the list; otherwise, -1.
        /// </returns>
        public int IndexOf(T item)
        {
            for (int i = 0; i < Count; i++)
            {
                var value = RedisDb.ListGetByIndex(RedisKey, i);
                if(!value.HasValue)
                {
                    return -1;
                }

                if (value.ToString().Equals(Serialize(item)))
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        public int Count
        {
            get { return (int)RedisDb.ListLength(RedisKey); }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is read only.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is read only; otherwise, <c>false</c>.
        /// </value>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        /// <returns>
        /// true if <paramref name="item" /> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, false. This method also returns false if <paramref name="item" /> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </returns>
        public bool Remove(T item)
        {
            return RedisDb.ListRemove(RedisKey, Serialize(item)) > 0;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
            {
                var value = RedisDb.ListGetByIndex(RedisKey, i);
                yield return value.HasValue ? Deserialize(value.ToString()) : default(T);
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
