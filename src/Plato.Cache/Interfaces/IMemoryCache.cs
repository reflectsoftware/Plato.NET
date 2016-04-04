// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System;

namespace Plato.Cache.Interfaces
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="System.IDisposable"/>
    public interface IMemoryCache : IDisposable
    {
        /// <summary>
        /// Purges the expired items.
        /// </summary>
        void PurgeExpiredItems();

        /// <summary>
        /// Removes the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        bool Remove(string name);

        /// <summary>
        /// Gets the specified name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">The name.</param>
        /// <param name="bSlidingTimeWindow">if set to <c>true</c> [b sliding time window].</param>
        /// <returns></returns>
        T Get<T>(string name, bool bSlidingTimeWindow);

        /// <summary>
        /// Gets the specified name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        T Get<T>(string name);

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
        /// Gets the specified name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">The name.</param>
        /// <param name="callback">The callback.</param>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        T Get<T>(string name, Func<string, object[], ObtainCacheDataInfo> callback, params object[] args);

        /// <summary>
        /// Sets the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="item">The item.</param>
        /// <param name="keepAlive">The keep alive.</param>
        void Set(string name, object item, TimeSpan keepAlive);

        /// <summary>
        /// Sets the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="item">The item.</param>
        void Set(string name, object item);
    }
}
