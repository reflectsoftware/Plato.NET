// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Plato.Async.Collections
{
    public interface IAsyncConcurrentDictionary<TKey, TValue> : IAsyncConcurrentCollection<KeyValuePair<TKey, TValue>>
    {
        Task<TValue> GetAsync(TKey key);
        Task<TValue> GetOrAddAsync(TKey key, Func<TKey, Task<TValue>> asyncAction);
        Task<ICollection<TKey>> GetKeysAsync();
        Task<ICollection<TValue>> GetValuesAsync();
        Task AddAsync(TKey key, TValue value);
        Task<bool> ContainsKeyAsync(TKey key);
        Task<bool> RemoveAsync(TKey key);
    }
}
