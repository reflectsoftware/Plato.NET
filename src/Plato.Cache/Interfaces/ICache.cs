// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System;
using System.Threading.Tasks;

namespace Plato.Cache.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public interface ICache : IDisposable
    {
        /// <summary>
        /// Removes the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        bool Remove(string name);

        /// <summary>
        /// Removes the asynchronous.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        Task<bool> RemoveAsync(string name);

        /// <summary>
        /// Gets the specified name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">The name.</param>
        /// <param name="callback">The callback.</param>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        T Get<T>(string name, Func<string, object[], CacheDataInfo<T>> callback = null, params object[] args);

        /// <summary>
        /// Gets the asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">The name.</param>
        /// <param name="callbackAsync">The callback asynchronous.</param>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        Task<T> GetAsync<T>(string name, Func<string, object[], Task<CacheDataInfo<T>>> callbackAsync = null, params object[] args);

        /// <summary>
        /// Sets the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="item">The item.</param>
        /// <param name="keepAlive">The keep alive.</param>
        void Set(string name, object item, TimeSpan? keepAlive = null);

        /// <summary>
        /// Sets the asynchronous.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="item">The item.</param>
        /// <param name="keepAlive">The keep alive.</param>
        /// <returns></returns>
        Task SetAsync(string name, object item, TimeSpan? keepAlive = null);
    }
}
