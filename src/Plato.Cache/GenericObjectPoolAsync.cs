// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Nito.AsyncEx;
using Plato.Core.Miscellaneous;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Plato.Cache
{
    /// <summary>
    /// GenericObjectPool class.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="System.IDisposable"/>
    public abstract class GenericObjectPoolAsync<T, TData> : IDisposable where T: class
    {
        private Stack<T> _objectPool;
        private AsyncLock _asyncLock;
        private AsyncSemaphore _poolSemaphore;
        private long _totalPoolSize;
        private long _availablePoolObjects;

        protected TData Data { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="GenericObjectPoolAsync{T}"/> is disposed.
        /// </summary>
        /// <value>
        /// <c>true</c> if disposed; otherwise, <c>false</c>.
        /// </value>
        public bool Disposed { get; private set; }

        /// <summary>
        /// Gets the maximum object usage.
        /// </summary>
        /// <value>
        /// The maximum object usage.
        /// </value>
        public long MaxObjectUsage { get; private set; }

        /// <summary>
        /// Gets the maximum size of the grow.
        /// </summary>
        /// <value>
        /// The maximum size of the grow.
        /// </value>
        public int MaxGrowSize { get; private set; }


        /// <summary>
        /// Gets the initial size of the pool.
        /// </summary>
        /// <value>
        /// The initial size of the pool.
        /// </value>
        public int InitialPoolSize { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericObjectPoolAsync{T, TData}"/> class.
        /// </summary>
        /// <param name="initialPoolSize">Initial size of the pool.</param>
        /// <param name="maxGrowSize">Maximum size of the grow.</param>
        /// <param name="data">The data.</param>
        public GenericObjectPoolAsync(int initialPoolSize, int maxGrowSize, TData data = default(TData))
        {
            _totalPoolSize = 0;
            _availablePoolObjects = 0;
            MaxObjectUsage = 0;

            Disposed = false;
            InitialPoolSize = initialPoolSize;
            MaxGrowSize = maxGrowSize;
            Data = data;

            _asyncLock = new AsyncLock();
            _poolSemaphore = new AsyncSemaphore(maxGrowSize);            

            Initialize(Data);
            CreateInitialPoolSet();
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="GenericObjectPoolAsync{T}"/> class.
        /// </summary>
        ~GenericObjectPoolAsync()
        {
            Dispose(false);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="bDisposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool bDisposing)
        {
            using (_asyncLock.Lock())
            {
                if (!Disposed)
                {
                    Disposed = true;
                    GC.SuppressFinalize(this);

                    Clear();
                    _objectPool = null;
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

        /// <summary>
        /// Clears this instance.
        /// </summary>
        private void Clear()
        {
            if (_objectPool != null)
            {
                foreach (T poolObject in _objectPool)
                {
                    MiscHelper.DisposeObject(poolObject);
                }

                _objectPool.Clear();

                _totalPoolSize -= _totalPoolSize;
                _availablePoolObjects -= _availablePoolObjects;
            }
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        /// <param name="data">The data.</param>
        protected virtual void Initialize(TData data)
        {            
        }

        /// <summary>
        /// Creates the initial pool set.
        /// </summary>
        private void CreateInitialPoolSet()
        {
            _objectPool = new Stack<T>();
            for (var i = 0; i < InitialPoolSize; i++)
            {
                var poolObject = CreatePoolObjectAsync().Result;
                AddObjectToPool(poolObject);
            }                
        }

        /// <summary>
        /// Adds the object to pool.
        /// </summary>
        /// <param name="poolObject">The pool object.</param>
        private void AddObjectToPool(T poolObject)
        {
            if (poolObject != null)
            {
                _objectPool.Push(poolObject);

                _totalPoolSize++;
                _availablePoolObjects++;
            }
        }

        /// <summary>
        /// Gets the total size of the pool.
        /// </summary>
        /// <value>
        /// The total size of the pool.
        /// </value>
        public async Task<long> TotalPoolSizeAsync() 
        {
            using (await _asyncLock.LockAsync())
            {
                return _totalPoolSize;
            }
        }

        /// <summary>
        /// Gets the available pool objects.
        /// </summary>
        /// <value>
        /// The available pool objects.
        /// </value>
        public async Task<long> AvailablePoolObjectsAsync()
        {
            using (await _asyncLock.LockAsync())
            {
                return _availablePoolObjects;
            }
        }

        /// <summary>
        /// Gets the taken object pool count.
        /// </summary>
        /// <value>
        /// The taken object pool count.
        /// </value>
        public async Task<long> TakenObjectPoolCountAsync()
        {
            using (await _asyncLock.LockAsync())
            {
                return _totalPoolSize - _availablePoolObjects;
            }
        }

        /// <summary>
        /// Takes this instance.
        /// </summary>
        /// <returns></returns>
        public virtual async Task<T> TakeAsync()
        {
            T poolObject = null;

            await _poolSemaphore.WaitAsync();

            using (await _asyncLock.LockAsync())
            {
                if (_objectPool.Count != 0)
                {
                    poolObject = _objectPool.Pop();
                    _availablePoolObjects--;                    
                }
                else
                {
                    if (_totalPoolSize < MaxGrowSize)
                    {
                        AddObjectToPool(await CreatePoolObjectAsync());
                        poolObject = _objectPool.Pop();
                        _availablePoolObjects--;
                    }
                }

                if ((_totalPoolSize - _availablePoolObjects) > MaxObjectUsage)
                {
                    MaxObjectUsage = (_totalPoolSize - _availablePoolObjects);
                }
            }

            return await PoolObjectBeforeGivenAsync(poolObject);
        }

        /// <summary>
        /// Returns the specified pool object.
        /// </summary>
        /// <param name="poolObject">The pool object.</param>
        public virtual async Task ReturnAsync(T poolObject)
        {
            if (poolObject == null)
            {
                return;
            }

            poolObject = await PoolObjectBeforeReturningAsync(poolObject);

            using (await _asyncLock.LockAsync())
            {
                if (poolObject != null)
                {
                    _objectPool.Push(poolObject);
                    _availablePoolObjects++;
                }
                else
                {
                    _totalPoolSize--;
                }

                if (_totalPoolSize != 0)
                {
                    _poolSemaphore.Release();
                }
            }
        }

        /// <summary>
        /// Containers the asynchronous.
        /// </summary>
        /// <returns></returns>
        public async Task<PoolInstanceAsync<T, TData>> ContainerAsync()
        {
            var poolObject = await TakeAsync();
            return new PoolInstanceAsync<T, TData>(this, poolObject);
        }

        /// <summary>
        /// Pools the object before given.
        /// </summary>
        /// <param name="poolObject">The pool object.</param>
        /// <returns></returns>
        protected virtual Task<T> PoolObjectBeforeGivenAsync(T poolObject)
        {
            return Task.FromResult(poolObject);
        }

        /// <summary>
        /// Pools the object before returning.
        /// </summary>
        /// <param name="poolObject">The pool object.</param>
        /// <returns></returns>
        protected virtual Task<T> PoolObjectBeforeReturningAsync(T poolObject)
        {
            return Task.FromResult(poolObject);
        }

        /// <summary>
        /// Creates the pool object.
        /// </summary>
        /// <returns></returns>
        protected abstract Task<T> CreatePoolObjectAsync();
    }
}
