﻿// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Cache.Interfaces;
using Plato.Core.Locks;
using Plato.Core.Miscellaneous;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plato.Cache
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="Plato.Cache.Interfaces.ILocalMemoryCache"/>
    /// <seealso cref="Plato.Cache.Interfaces.IMemoryCacheExtension"/>
    public class LocalMemoryCache : ILocalMemoryCache, IMemoryCacheExtension
    {
        private const int DISPOSE_OBJECT_TIME = 5;

        /// <summary>
        ///
        /// </summary>
        /// <seealso cref="System.IDisposable"/>
        private class CacheNode : IDisposable
        {
            public bool Disposed { get; private set; }
            public string Name { get; set; }
            public object Data { get; set; }
            public TimeSpan KeepAlive { get; set; }
            public DateTime CachedDateTime { get; set; }

            /// <summary>
            /// Initializes a new instance of the <see cref="CacheNode"/> class.
            /// </summary>
            /// <param name="name">The name.</param>
            /// <param name="data">The data.</param>
            /// <param name="keepAlive">The keep alive.</param>
            public CacheNode(string name, object data, TimeSpan keepAlive)
            {
                Disposed = false;
                Name = name;
                Data = data;
                KeepAlive = keepAlive;
                CachedDateTime = DateTime.Now;
            }

            /// <summary>
            /// Finalizes an instance of the <see cref="CacheNode"/> class.
            /// </summary>
            ~CacheNode()
            {
                Dispose(false);
            }

            /// <summary>
            /// Resets the cached date time.
            /// </summary>
            public void ResetCachedDateTime()
            {
                CachedDateTime = DateTime.Now;
            }

            /// <summary>
            /// Releases unmanaged and - optionally - managed resources.
            /// </summary>
            /// <param name="bDisposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
            protected virtual void Dispose(bool bDisposing)
            {
                lock (this)
                {
                    if (!Disposed)
                    {
                        Disposed = true;
                        GC.SuppressFinalize(this);

                        if (Data != null)
                        {
                            MiscHelper.DisposeObject(Data);
                            Data = null;
                        }
                    }
                }
            }

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public void Dispose()
            {
                Dispose(true);
            }
        }

        private Dictionary<string, CacheNode> CacheContainer { get; set; }
        private List<CacheNode> DelayDisposeItems { get; set; }
        private TimeSpan LastExpiredScanWindow { get; set; }
        private DateTime LastExpiredCheck { get; set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="LocalMemoryCache"/> is disposed.
        /// </summary>
        /// <value>
        /// <c>true</c> if disposed; otherwise, <c>false</c>.
        /// </value>
        public bool Disposed { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalMemoryCache"/> class.
        /// </summary>
        public LocalMemoryCache()
        {
            Disposed = false;
            LastExpiredScanWindow = new TimeSpan(0, DISPOSE_OBJECT_TIME, 0);
            LastExpiredCheck = DateTime.Now;
            CacheContainer = new Dictionary<string, CacheNode>();
            DelayDisposeItems = new List<CacheNode>();
        }

        #region Dispose
        /// <summary>
        /// Finalizes an instance of the <see cref="LocalMemoryCache"/> class.
        /// </summary>
        ~LocalMemoryCache()
        {
            Dispose(false);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="bDisposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool bDisposing)
        {
            lock (this)
            {
                if (!Disposed)
                {
                    Disposed = true;
                    GC.SuppressFinalize(this);

                    if (bDisposing)
                    {
                        _ForceDispose();
                    }
                }
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion Dispose

        /// <summary>
        /// Adds to delay disposable.
        /// </summary>
        /// <param name="node">The node.</param>
        private void AddToDelayDisposable(CacheNode node)
        {
            node.KeepAlive = new TimeSpan(0, DISPOSE_OBJECT_TIME, 0);
            node.CachedDateTime = DateTime.Now;

            DelayDisposeItems.Add(node);
        }

        /// <summary>
        /// Checks the expired if necessary.
        /// </summary>
        private void CheckExpiredIfNecessary()
        {
            lock (this)
            {
                if (DateTime.Now.Subtract(LastExpiredCheck) > LastExpiredScanWindow)
                {
                    foreach (var cInfo in CacheContainer.Values.ToList())
                    {
                        if (cInfo.KeepAlive == TimeSpan.Zero)
                        {
                            continue;
                        }

                        if (DateTime.Now.Subtract(cInfo.CachedDateTime) > cInfo.KeepAlive)
                        {
                            CacheContainer.Remove(cInfo.Name);
                            AddToDelayDisposable(cInfo);
                        }
                    }

                    foreach (var cInfo in DelayDisposeItems.ToList())
                    {
                        if (DateTime.Now.Subtract(cInfo.CachedDateTime) > cInfo.KeepAlive)
                        {
                            DelayDisposeItems.Remove(cInfo);
                            cInfo.Dispose();
                        }
                    }

                    LastExpiredCheck = DateTime.Now;
                }
            }
        }

        /// <summary>
        /// force dispose.
        /// </summary>
        private void _ForceDispose()
        {
            lock (this)
            {
                foreach (var node in CacheContainer.Values)
                {
                    node.Dispose();
                }

                foreach (var node in DelayDisposeItems)
                {
                    node.Dispose();
                }

                CacheContainer.Clear();
                DelayDisposeItems.Clear();
            }
        }

        /// <summary>
        /// Prepares the name of the object.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        private static string PrepareObjectName(string name)
        {
            return name.Trim().ToLower();
        }

        /// <summary>
        /// Gets the node.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        private CacheNode GetNode(string name)
        {
            var key = PrepareObjectName(name);
            return CacheContainer.ContainsKey(key) ? CacheContainer[key] : null;
        }

        /// <summary>
        /// Purges the expired items.
        /// </summary>
        public void PurgeExpiredItems()
        {
            CheckExpiredIfNecessary();
        }

        /// <summary>
        /// Purges the expired items asynchronous.
        /// </summary>
        /// <returns></returns>
        public Task PurgeExpiredItemsAsync()
        {
            PurgeExpiredItems();
            return Task.CompletedTask;
        }
        
        /// <summary>
        /// Removes the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public bool Remove(string name)
        {
            lock (this)
            {
                var node = GetNode(name);
                if (node == null)
                {
                    return false;
                }

                CacheContainer.Remove(node.Name);
                AddToDelayDisposable(node);

                return true;
            }
        }

        /// <summary>
        /// Removes the asynchronous.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public Task<bool> RemoveAsync(string name)
        {
            var result = Remove(name);
            return Task.FromResult(result);
        }

        private CacheNode GetCachedNode(string name, bool bSlidingTimeWindow = false)
        {
            lock (this)
            {
                PurgeExpiredItems();

                var node = GetNode(name);
                if(node != null)
                {
                    if (node.KeepAlive != TimeSpan.Zero && DateTime.Now.Subtract(node.CachedDateTime) > node.KeepAlive)
                    {
                        Remove(name);
                        node = null;
                    }
                    else if (bSlidingTimeWindow)
                    {
                        node.ResetCachedDateTime();
                    }
                }

                return node;
            }
        }

        /// <summary>
        /// Gets the specified name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">The name.</param>
        /// <param name="bSlidingTimeWindow">if set to <c>true</c> [b sliding time window].</param>
        /// <returns></returns>
        public T Get<T>(string name, bool bSlidingTimeWindow = false)
        {
            lock (this)
            {
                PurgeExpiredItems();

                var node = GetCachedNode(name);
                if (node == null)
                {
                    return default(T);
                }                

                return (T)node.Data;
            }
        }

        /// <summary>
        /// Gets the asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">The name.</param>
        /// <param name="bSlidingTimeWindow">if set to <c>true</c> [b sliding time window].</param>
        /// <returns></returns>
        public Task<T> GetAsync<T>(string name, bool bSlidingTimeWindow = false)
        {
            var result = Get<T>(name, bSlidingTimeWindow);
            return Task.FromResult(result);
        }

        /// <summary>
        /// Gets the specified name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">The name.</param>
        /// <param name="bSlidingTimeWindow">if set to <c>true</c> [b sliding time window].</param>
        /// <param name="callback">The callback.</param>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">callback</exception>
        public T Get<T>(string name, bool bSlidingTimeWindow, Func<string, object[], CacheDataInfo<T>> callback = null, params object[] args)
        {            
            var node = GetCachedNode(name, bSlidingTimeWindow);
            if(node == null)
            {
                if(callback == null)
                {
                    return default(T);
                }

                using (var rLock = new ResourceLock(name))
                {
                    rLock.EnterWriteLock();
                    try
                    {
                        node = GetCachedNode(name, bSlidingTimeWindow);
                        if (node == null)
                        {
                            var cData = callback(name, args);
                            if (cData == null)
                            {
                                return default(T);
                            }

                            Set(name, cData.NewCacheData, cData.KeepAlive);
                            return cData.NewCacheData;
                        }
                    }
                    finally
                    {
                        rLock.ExitWriteLock();
                    }
                }
            }

            return (T)node.Data;
        }

        /// <summary>
        /// Gets the asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">The name.</param>
        /// <param name="bSlidingTimeWindow">if set to <c>true</c> [b sliding time window].</param>
        /// <param name="callbackAsync">The callback asynchronous.</param>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        public async Task<T> GetAsync<T>(string name, bool bSlidingTimeWindow, Func<string, object[], Task<CacheDataInfo<T>>> callbackAsync = null, params object[] args)
        {                        
            var node = GetCachedNode(name, bSlidingTimeWindow);
            if (node == null)
            {
                if (callbackAsync == null)
                {
                    return default(T);
                }

                using (var rLock = new ResourceLockAsync(name))
                {
                    using (await rLock.WriterLockAsync())
                    {
                        node = GetCachedNode(name, bSlidingTimeWindow);
                        if (node == null)
                        {
                            var cData = await callbackAsync(name, args);
                            if (cData == null)
                            {
                                return default(T);
                            }

                            Set(name, cData.NewCacheData, cData.KeepAlive);
                            return cData.NewCacheData;
                        }
                    }
                }
            }

            return (T)node.Data;
        }

        /// <summary>
        /// Gets the specified name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">The name.</param>
        /// <param name="callback">The callback.</param>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        public T Get<T>(string name, Func<string, object[], CacheDataInfo<T>> callback = null, params object[] args)
        {
            return Get(name, false, callback, args);
        }

        /// <summary>
        /// Gets the asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">The name.</param>
        /// <param name="callbackAsync">The callback asynchronous.</param>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        public Task<T> GetAsync<T>(string name, Func<string, object[], Task<CacheDataInfo<T>>> callbackAsync = null, params object[] args)
        {
            return GetAsync(name, false, callbackAsync, args);
        }

        /// <summary>
        /// Sets the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="item">The item.</param>
        /// <param name="keepAlive">The keep alive.</param>
        public void Set(string name, object item, TimeSpan? keepAlive = null)
        {
            lock (this)
            {
                PurgeExpiredItems();

                keepAlive = keepAlive ?? TimeSpan.Zero;

                var node = GetNode(name);
                if (node != null && ReferenceEquals(node.Data, item))
                {
                    node.KeepAlive = keepAlive.Value;
                    node.ResetCachedDateTime();
                }
                else
                {
                    Remove(name);

                    var preName = PrepareObjectName(name);
                    CacheContainer[preName] = new CacheNode(preName, item, keepAlive.Value);
                }
            }
        }

        /// <summary>
        /// Sets the asynchronous.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="item">The item.</param>
        /// <param name="keepAlive">The keep alive.</param>
        /// <returns></returns>
        public Task SetAsync(string name, object item, TimeSpan? keepAlive = null)
        {
            Set(name, item, keepAlive);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        void IMemoryCacheExtension.Clear()
        {
            _ForceDispose();
        }

        /// <summary>
        /// Clears the asynchronous.
        /// </summary>
        /// <returns></returns>
        Task IMemoryCacheExtension.ClearAsync()
        {
            (this as IMemoryCacheExtension).Clear();
            return Task.CompletedTask;
        }
    }
}
