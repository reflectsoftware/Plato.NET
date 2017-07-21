// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System;
using System.Threading.Tasks;

namespace Plato.Cache.Interfaces
{
    public interface IMemoryCache : ICache
    {
        /// <summary>
        /// Purges the expired items.
        /// </summary>
        void PurgeExpiredItems();

        /// <summary>
        /// Purges the expired items asynchronous.
        /// </summary>
        /// <returns></returns>
        Task PurgeExpiredItemsAsync();

        /// <summary>
        /// Gets the specified name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">The name.</param>
        /// <param name="bSlidingTimeWindow">if set to <c>true</c> [b sliding time window].</param>
        /// <returns></returns>
        T Get<T>(string name, bool bSlidingTimeWindow = false);

        /// <summary>
        /// Gets the asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">The name.</param>
        /// <param name="bSlidingTimeWindow">if set to <c>true</c> [b sliding time window].</param>
        /// <returns></returns>
        Task<T> GetAsync<T>(string name, bool bSlidingTimeWindow = false);

        /// <summary>
        /// Gets the specified name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">The name.</param>
        /// <param name="bSlidingTimeWindow">if set to <c>true</c> [b sliding time window].</param>
        /// <param name="callback">The callback.</param>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        T Get<T>(string name, bool bSlidingTimeWindow, Func<string, object[], ObtainCacheDataInfo> callback, params object[] args);

        /// <summary>
        /// Gets the asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">The name.</param>
        /// <param name="bSlidingTimeWindow">if set to <c>true</c> [b sliding time window].</param>
        /// <param name="callbackAsync">The callback asynchronous.</param>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        Task<T> GetAsync<T>(string name, bool bSlidingTimeWindow, Func<string, object[], Task<ObtainCacheDataInfo>> callbackAsync, params object[] args);
    }
}
