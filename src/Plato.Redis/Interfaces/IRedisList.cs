// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using StackExchange.Redis;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Plato.Redis.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="System.Collections.Generic.IList{T}" />
    public interface IRedisList<T> : IList<T>
    {
        IEnumerable<T> Values { get; }
        Task InsertAsync(int index, T item);
        Task InsertAsync(ITransaction tran, int index, T item);
        Task RemoveAtAsync(int index);
        Task RemoveAtAsync(ITransaction tran, int index);
        Task<bool> RemoveAsync(ITransaction tran, T item);
        Task<bool> RemoveAsync(T item);
        Task AddAsync(T item);
        Task AddAsync(ITransaction tran, T item);
        Task ClearAsync();
        Task ClearAsync(ITransaction tran);
        T Peek();
        Task<T> PeekAsync();
        Task<T> GetAsync(int index);        
        Task<IEnumerable<T>> GetValuesAsync();
    }
}
