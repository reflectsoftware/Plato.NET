// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Plato.Redis.Interfaces
{
    public interface IRedisDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        TValue GetOrAdd(TKey key, Func<TKey, TValue> addFunction);
        bool Add(TKey key, TValue value, When when);
        void AddMultiple(IEnumerable<KeyValuePair<TKey, TValue>> items);
        Task<bool> AddAsync(TKey key, TValue value, When when);
        Task<bool> AddAsync(ITransaction tran, TKey key, TValue value, When when);
        Task<bool> AddAsync(TKey key, TValue value);
        Task<bool> AddAsync(ITransaction tran, TKey key, TValue value);
        Task<bool> RemoveAsync(TKey key);
        Task<bool> RemoveAsync(ITransaction tran, TKey key);
        Task AddAsync(KeyValuePair<TKey, TValue> item);
        Task AddAsync(ITransaction tran, KeyValuePair<TKey, TValue> item);
        Task ClearAsync();
        Task ClearAsync(ITransaction tran);
        Task<bool> RemoveAsync(KeyValuePair<TKey, TValue> item);
        Task<bool> RemoveAsync(ITransaction tran, KeyValuePair<TKey, TValue> item);
        Task AddMultipleAsync(IEnumerable<KeyValuePair<TKey, TValue>> items);
        Task AddMultipleAsync(ITransaction tran, IEnumerable<KeyValuePair<TKey, TValue>> items);
        Task<TValue> GetAsync(TKey key);
        Task<bool> ContainsKeyAsync(TKey key);
        Task<ICollection<TKey>> GetKeysAsync();
        Task<ICollection<TValue>> GetValuesAsync();
    }
}
