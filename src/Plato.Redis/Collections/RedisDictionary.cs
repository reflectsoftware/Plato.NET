// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Redis.Interfaces;
using Plato.Redis.Serializers;
using StackExchange.Redis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Plato.Redis.Collections
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <seealso cref="Plato.Redis.Interfaces.IRedisCollection" />
    /// <seealso cref="Plato.Redis.Interfaces.IRedisDictionary{TKey, TValue}" />
    public class RedisDictionary<TKey, TValue> : IRedisCollection, IRedisDictionary<TKey, TValue>
    {
        public IDatabase RedisDb { get; private set; }
        public RedisKey RedisKey { get; private set; }
        public IRedisSerializer ValueSerializer { get; private set; }
        public IRedisSerializer KeySerializer { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RedisDictionary{TKey, TValue}" /> class.
        /// </summary>
        /// <param name="redisDb">The redis database.</param>
        /// <param name="redisKey">The redis key.</param>
        /// <param name="valueSerializer">The value serializer.</param>
        public RedisDictionary(
            IDatabase redisDb, 
            RedisKey redisKey,      
            IRedisSerializer valueSerializer = null)
        {
            RedisDb = redisDb;
            RedisKey = redisKey;
            ValueSerializer = valueSerializer ?? new JsonRedisSerializer();
            KeySerializer = new JsonRedisSerializer();            
        }

        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="when">The when.</param>
        /// <returns></returns>
        public bool Add(TKey key, TValue value, When when)
        {
            return RedisDb.HashSet(RedisKey, KeySerializer.Serialize(key), ValueSerializer.Serialize(value), when: when);
        }

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="when">The when.</param>
        /// <returns></returns>
        public async Task<bool> AddAsync(TKey key, TValue value, When when)
        {
            return await RedisDb.HashSetAsync(RedisKey, KeySerializer.Serialize(key), ValueSerializer.Serialize(value), when: when);
        }

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="tran">The tran.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="when">The when.</param>
        /// <returns></returns>
        public async Task<bool> AddAsync(ITransaction tran, TKey key, TValue value, When when)
        {
            return await tran.HashSetAsync(RedisKey, KeySerializer.Serialize(key), ValueSerializer.Serialize(value), when: when);
        }

        /// <summary>
        /// Adds an element with the provided key and value to the <see cref="T:System.Collections.Generic.IDictionary`2" />.
        /// </summary>
        /// <param name="key">The object to use as the key of the element to add.</param>
        /// <param name="value">The object to use as the value of the element to add.</param>
        public void Add(TKey key, TValue value)
        {
            Add(key, value, When.Always);            
        }

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public async Task<bool> AddAsync(TKey key, TValue value)
        {
            return await AddAsync(key, value, When.Always);
        }

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="tran">The tran.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public async Task<bool> AddAsync(ITransaction tran, TKey key, TValue value)
        {
            return await AddAsync(tran, key, value, When.Always);
        }

        /// <summary>
        /// Removes the element with the specified key from the <see cref="T:System.Collections.Generic.IDictionary`2" />.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns>
        /// true if the element is successfully removed; otherwise, false.  This method also returns false if <paramref name="key" /> was not found in the original <see cref="T:System.Collections.Generic.IDictionary`2" />.
        /// </returns>
        public bool Remove(TKey key)
        {
            return RedisDb.HashDelete(RedisKey, KeySerializer.Serialize(key));
        }

        /// <summary>
        /// Removes the asynchronous.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public async Task<bool> RemoveAsync(TKey key)
        {
            return await RedisDb.HashDeleteAsync(RedisKey, KeySerializer.Serialize(key));
        }

        /// <summary>
        /// Removes the asynchronous.
        /// </summary>
        /// <param name="tran">The tran.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public async Task<bool> RemoveAsync(ITransaction tran, TKey key)
        {
            return await tran.HashDeleteAsync(RedisKey, KeySerializer.Serialize(key));
        }

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public async Task AddAsync(KeyValuePair<TKey, TValue> item)
        {
            await AddAsync(item.Key, item.Value);
        }

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="tran">The tran.</param>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public async Task AddAsync(ITransaction tran, KeyValuePair<TKey, TValue> item)
        {
            await AddAsync(tran, item.Key, item.Value);
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
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        /// <returns>
        /// true if <paramref name="item" /> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, false. This method also returns false if <paramref name="item" /> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </returns>
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return Remove(item.Key);
        }

        /// <summary>
        /// Removes the asynchronous.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public async Task<bool> RemoveAsync(KeyValuePair<TKey, TValue> item)
        {
            return await RemoveAsync(item.Key);
        }

        /// <summary>
        /// Removes the asynchronous.
        /// </summary>
        /// <param name="tran">The tran.</param>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public async Task<bool> RemoveAsync(ITransaction tran, KeyValuePair<TKey, TValue> item)
        {
            return await RemoveAsync(tran, item.Key);
        }

        /// <summary>
        /// Adds the multiple.
        /// </summary>
        /// <param name="items">The items.</param>
        public void AddMultiple(IEnumerable<KeyValuePair<TKey, TValue>> items)
        {
            RedisDb.HashSet(RedisKey, items.Select(i => new HashEntry(KeySerializer.Serialize(i.Key), ValueSerializer.Serialize(i.Value))).ToArray());
        }

        /// <summary>
        /// Adds the multiple asynchronous.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <returns></returns>
        public async Task AddMultipleAsync(IEnumerable<KeyValuePair<TKey, TValue>> items)
        {
            await RedisDb.HashSetAsync(RedisKey, items.Select(i => new HashEntry(KeySerializer.Serialize(i.Key), ValueSerializer.Serialize(i.Value))).ToArray());
        }

        /// <summary>
        /// Adds the multiple asynchronous.
        /// </summary>
        /// <param name="tran">The tran.</param>
        /// <param name="items">The items.</param>
        /// <returns></returns>
        public async Task AddMultipleAsync(ITransaction tran, IEnumerable<KeyValuePair<TKey, TValue>> items)
        {
            await tran.HashSetAsync(RedisKey, items.Select(i => new HashEntry(KeySerializer.Serialize(i.Key), ValueSerializer.Serialize(i.Value))).ToArray());
        }

        /// <summary>
        /// Gets the value asynchronous.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public async Task<TValue> GetAsync(TKey key)
        {
            var redisValue = await RedisDb.HashGetAsync(RedisKey, KeySerializer.Serialize(key));
            return redisValue.IsNull ? default(TValue) : ValueSerializer.Deserialize<TValue>(redisValue);
        }

        /// <summary>
        /// Gets the values asynchronous.
        /// </summary>
        /// <returns></returns>
        public async Task<ICollection<TValue>> GetValuesAsync()
        {
            return new Collection<TValue>((await RedisDb.HashValuesAsync(RedisKey)).Select(h => ValueSerializer.Deserialize<TValue>(h)).ToList());
        }

        /// <summary>
        /// Gets the keys asynchronous.
        /// </summary>
        /// <returns></returns>
        public async Task<ICollection<TKey>> GetKeysAsync()
        {
            return new Collection<TKey>((await RedisDb.HashKeysAsync(RedisKey)).Select(h => KeySerializer.Deserialize<TKey>(h)).ToList());
        }

        /// <summary>
        /// Determines whether [contains key asynchronous] [the specified key].
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public async Task<bool> ContainsKeyAsync(TKey key)
        {
            return await RedisDb.HashExistsAsync(RedisKey, KeySerializer.Serialize(key));
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key whose value to get.</param>
        /// <param name="value">When this method returns, the value associated with the specified key, if the key is found; otherwise, the default value for the type of the <paramref name="value" /> parameter. This parameter is passed uninitialized.</param>
        /// <returns>
        /// true if the object that implements <see cref="T:System.Collections.Generic.IDictionary`2" /> contains an element with the specified key; otherwise, false.
        /// </returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            var redisValue = RedisDb.HashGet(RedisKey, KeySerializer.Serialize(key));
            if (redisValue.IsNull)
            {
                value = default(TValue);
                return false;
            }

            value = ValueSerializer.Deserialize<TValue>(redisValue);
            return true;
        }

        /// <summary>
        /// Gets the or add.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="addFunction">The add function.</param>
        /// <returns></returns>
        public TValue GetOrAdd(TKey key, Func<TKey, TValue> addFunction)
        {
            TValue value;
            if (!TryGetValue(key, out value))
            {
                value = addFunction(key);
                Add(key, value);
            }

            return value;
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.IDictionary`2" /> contains an element with the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the <see cref="T:System.Collections.Generic.IDictionary`2" />.</param>
        /// <returns>
        /// true if the <see cref="T:System.Collections.Generic.IDictionary`2" /> contains an element with the key; otherwise, false.
        /// </returns>
        public bool ContainsKey(TKey key)
        {
            return RedisDb.HashExists(RedisKey, KeySerializer.Serialize(key));
        }

        /// <summary>
        /// Gets an <see cref="T:System.Collections.Generic.ICollection`1" /> containing the values in the <see cref="T:System.Collections.Generic.IDictionary`2" />.
        /// </summary>
        public ICollection<TValue> Values
        {
            get { return new Collection<TValue>(RedisDb.HashValues(RedisKey).Select(h => ValueSerializer.Deserialize<TValue>(h)).ToList()); }
        }

        /// <summary>
        /// Gets an <see cref="T:System.Collections.Generic.ICollection`1" /> containing the keys of the <see cref="T:System.Collections.Generic.IDictionary`2" />.
        /// </summary>
        public ICollection<TKey> Keys
        {
            get { return new Collection<TKey>(RedisDb.HashKeys(RedisKey).Select(h => KeySerializer.Deserialize<TKey>(h)).ToList()); }
        }

        /// <summary>
        /// Gets or sets the <see cref="TValue"/> with the specified key.
        /// </summary>
        /// <value>
        /// The <see cref="TValue"/>.
        /// </value>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public TValue this[TKey key]
        {
            get
            {
                var redisValue = RedisDb.HashGet(RedisKey, KeySerializer.Serialize(key));
                return redisValue.IsNull ? default(TValue) : ValueSerializer.Deserialize<TValue>(redisValue);
            }
            set
            {
                Add(key, value);
            }
        }        

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1" /> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        /// <returns>
        /// true if <paramref name="item" /> is found in the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, false.
        /// </returns>
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return RedisDb.HashExists(RedisKey, KeySerializer.Serialize(item.Key));
        }

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1" /> to an <see cref="T:System.Array" />, starting at a particular <see cref="T:System.Array" /> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            var values = RedisDb.HashGetAll(RedisKey);
            for (var i = 0; i < values.Length; i++)
            {
                var key = KeySerializer.Deserialize<TKey>(values[i].Name);
                var value = ValueSerializer.Deserialize<TValue>(values[i].Value);

                array[i + arrayIndex] = new KeyValuePair<TKey, TValue>(key, value);
            }
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        public int Count
        {
            get { return (int)RedisDb.HashLength(RedisKey); }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            foreach (var hashKey in RedisDb.HashKeys(RedisKey))
            {
                var redisValue = RedisDb.HashGet(RedisKey, hashKey);
                yield return new KeyValuePair<TKey, TValue>(KeySerializer.Deserialize<TKey>(hashKey), ValueSerializer.Deserialize<TValue>(redisValue));
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
            yield return GetEnumerator();
        }
    }
}