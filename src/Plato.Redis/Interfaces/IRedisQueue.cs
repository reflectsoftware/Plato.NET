// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using StackExchange.Redis;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Plato.Redis.Interfaces
{
    public interface IRedisQueue<T> : IEnumerable<T>, IEnumerable, ICollection
    {
        void Clear();
        bool Contains(T item);
        T Dequeue();
        void Enqueue(T item);
        Task ClearAsync();
        Task ClearAsync(ITransaction tran);
        Task EnqueueAsync(T item);
        Task EnqueueAsync(ITransaction tran, T item);
        Task<T> DequeueAsync();
        T Peek();
        Task<T> PeekAsync();

        T[] ToArray();
    }
}
