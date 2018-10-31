// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using StackExchange.Redis;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Plato.Redis.Interfaces
{
    /// <seealso cref="System.Collections.Generic.IReadOnlyCollection{T}" />
    public interface IRedisStack<T> : IEnumerable<T>, IEnumerable, ICollection
    {
        void Clear();
        bool Contains(T item);
        T Pop();
        Task<T> PopAsync();
        void Push(T item);        
        T Peek();
        Task<T> PeekAsync();
        T[] ToArray();
        Task ClearAsync();
        Task ClearAsync(ITransaction tran);
        Task PushAsync(T item);
        Task PushAsync(ITransaction tran, T item);
    }
}
