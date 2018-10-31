// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Redis.Interfaces;
using Plato.Redis.Serializers;
using StackExchange.Redis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plato.Redis.Collections
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="Plato.Redis.Interfaces.IRedisCollection" />
    /// <seealso cref="Plato.Redis.Interfaces.IRedisList{T}" />
    public class RedisList<T> : IRedisCollection, IRedisList<T>
    {
        public IDatabase RedisDb { get; private set; }
        public RedisKey RedisKey { get; private set; }
        public IRedisSerializer Serializer { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RedisList{T}" /> class.
        /// </summary>
        /// <param name="redisDb">The redis database.</param>
        /// <param name="redisKey">The key.</param>
        /// <param name="serializer">The serializer.</param>
        public RedisList(IDatabase redisDb, RedisKey redisKey, IRedisSerializer serializer = null)
        {
            RedisDb = redisDb;
            RedisKey = redisKey;
            Serializer = serializer ?? new JsonRedisSerializer();
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
                RedisDb.ListInsertBefore(RedisKey, before, Serializer.Serialize(item));
            }
            else
            {
                throw new IndexOutOfRangeException($"Index: '{index}' for Redis list: '{RedisKey}' is out of range.");
            }
        }

        /// <summary>
        /// Inserts the asynchronous.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public async Task InsertAsync(int index, T item)
        {
            if (RedisDb.ListLength(RedisKey) > index)
            {
                var before = RedisDb.ListGetByIndex(RedisKey, index);
                await RedisDb.ListInsertBeforeAsync(RedisKey, before, Serializer.Serialize(item));
            }
            else
            {
                throw new IndexOutOfRangeException($"Index: '{index}' for Redis list: '{RedisKey}' is out of range.");
            }
        }

        /// <summary>
        /// Inserts the asynchronous.
        /// </summary>
        /// <param name="tran">The tran.</param>
        /// <param name="index">The index.</param>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public async Task InsertAsync(ITransaction tran, int index, T item)
        {
            if (RedisDb.ListLength(RedisKey) > index)
            {
                var before = RedisDb.ListGetByIndex(RedisKey, index);
                await tran.ListInsertBeforeAsync(RedisKey, before, Serializer.Serialize(item));
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
        /// Removes at asynchronous.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public async Task RemoveAtAsync(int index)
        {
            var value = RedisDb.ListGetByIndex(RedisKey, index);
            if (!value.IsNull)
            {
                await RedisDb.ListRemoveAsync(RedisKey, value);
            }
        }

        /// <summary>
        /// Removes at asynchronous.
        /// </summary>
        /// <param name="tran">The tran.</param>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public async Task RemoveAtAsync(ITransaction tran, int index)
        {
            var value = RedisDb.ListGetByIndex(RedisKey, index);
            if (!value.IsNull)
            {
                await tran.ListRemoveAsync(RedisKey, value);
            }
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
            return RedisDb.ListRemove(RedisKey, Serializer.Serialize(item)) > 0;
        }

        /// <summary>
        /// Removes the asynchronous.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public async Task<bool> RemoveAsync(T item)
        {
            return await RedisDb.ListRemoveAsync(RedisKey, Serializer.Serialize(item)) > 0;
        }

        /// <summary>
        /// Removes the asynchronous.
        /// </summary>
        /// <param name="tran">The tran.</param>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public async Task<bool> RemoveAsync(ITransaction tran, T item)
        {
            return await tran.ListRemoveAsync(RedisKey, Serializer.Serialize(item)) > 0;
        }

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        public void Add(T item)
        {
            RedisDb.ListRightPush(RedisKey, Serializer.Serialize(item));
        }

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public async Task AddAsync(T item)
        {
            await RedisDb.ListRightPushAsync(RedisKey, Serializer.Serialize(item));
        }

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="tran">The tran.</param>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public async Task AddAsync(ITransaction tran, T item)
        {
            await tran.ListRightPushAsync(RedisKey, Serializer.Serialize(item));
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        public void Clear()
        {
            RedisDb.KeyDelete(RedisKey);
        }

        /// <summary>
        /// Clears the asynchronous.
        /// </summary>
        /// <returns></returns>
        public async Task ClearAsync()
        {
            await RedisDb.KeyDeleteAsync(RedisKey);
        }

        /// <summary>
        /// Clears the asynchronous.
        /// </summary>
        /// <param name="tran">The tran.</param>
        /// <returns></returns>
        public async Task ClearAsync(ITransaction tran)
        {
            await tran.KeyDeleteAsync(RedisKey);
        }

        /// <summary>
        /// Peeks this instance.
        /// </summary>
        /// <returns></returns>
        public T Peek()
        {
            var value = RedisDb.ListGetByIndex(RedisKey, 0);
            return value.HasValue ? Serializer.Deserialize<T>(value) : default(T);
        }

        /// <summary>
        /// Peeks the asynchronous.
        /// </summary>
        /// <returns></returns>
        public async Task<T> PeekAsync()
        {
            var value = await RedisDb.ListGetByIndexAsync(RedisKey, 0);
            return value.HasValue ? Serializer.Deserialize<T>(value) : default(T);
        }

        /// <summary>
        /// Gets the asynchronous.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public async Task<T> GetAsync(int index)
        {
            var value = await RedisDb.ListGetByIndexAsync(RedisKey, index);
            return value.HasValue ? Serializer.Deserialize<T>(value) : default(T);
        }

        /// <summary>
        /// Get Values the asynchronous.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetValuesAsync()
        {
            var values = await RedisDb.ListRangeAsync(RedisKey);
            return values.Where(x => x.HasValue).Select(x => Serializer.Deserialize<T>(x));
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
                return value.HasValue ? Serializer.Deserialize<T>(value) : default(T);
            }
            set
            {
                RedisDb.ListSetByIndex(RedisKey, index, Serializer.Serialize(value));
            }
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
                if (!value.HasValue)
                {
                    // we reached an are of the list where there are no longer values.
                    break;
                }

                if (value.ToString().Equals(Serializer.Serialize(item)))
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
            for (var i = 0; i < values.Length; i++)
            {
                array[index + i] = values[i].HasValue ? Serializer.Deserialize<T>(values[i]) : default(T);
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
                if (!value.HasValue)
                {
                    return -1;
                }

                if (value.ToString().Equals(Serializer.Serialize(item)))
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
        /// Gets the values.
        /// </summary>
        /// <value>
        /// The values.
        /// </value>
        public IEnumerable<T> Values
        {
            get
            {
                var values = RedisDb.ListRange(RedisKey);
                return values.Where(x => x.HasValue).Select(x => Serializer.Deserialize<T>(x));
            }
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
                yield return value.HasValue ? Serializer.Deserialize<T>(value) : default(T);
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
