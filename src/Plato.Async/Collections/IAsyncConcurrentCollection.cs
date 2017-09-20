// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Plato.Async.Collections
{
    public interface IAsyncConcurrentCollection<T>
    {
        Task<int> CountAsync();
        Task AddAsync(T item);
        Task ClearAsync();
        Task<bool> ContainsAsync(T item);
        Task<bool> RemoveAsync(T item);
        Task<IEnumerable<T>> ValuesAsync();
    }
}
