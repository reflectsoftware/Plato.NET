// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System.Threading.Tasks;

namespace Plato.Async.Collections
{
    public interface IAsyncConcurrentList<T> : IAsyncConcurrentCollection<T>
    {
        Task<T> GetAtAsync(int index);
        Task<int> IndexOfAsync(T item);
        Task InsertAsync(int index, T item);
        Task RemoveAtAsync(int index);
    }
}
