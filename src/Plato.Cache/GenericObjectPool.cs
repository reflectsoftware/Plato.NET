// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Core.Miscellaneous;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Plato.Cache
{
    /// <summary>
    /// GenericObjectPool class.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="System.IDisposable"/>
    public abstract class GenericObjectPool<T, TData> : IDisposable where T: class
    {
        private Stack<T> _objectPool;
        private Semaphore _poolSemaphore;
        private long _totalPoolSize;
        private long _availablePoolObjects;

        protected TData Data { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="GenericObjectPool{T}"/> is disposed.
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
        /// Initializes a new instance of the <see cref="GenericObjectPool{T}"/> class.
        /// </summary>
        /// <param name="initialPoolSize">Initial size of the pool.</param>
        /// <param name="maxGrowSize">Maximum size of the grow.</param>
        public GenericObjectPool(int initialPoolSize, int maxGrowSize, TData data = default(TData))
        {
            _totalPoolSize = 0;
            _availablePoolObjects = 0;
            MaxObjectUsage = 0;

            Disposed = false;
            InitialPoolSize = initialPoolSize;
            MaxGrowSize = maxGrowSize;
            Data = data;
            _poolSemaphore = new Semaphore(0, maxGrowSize);

            Initialize(Data);
            CreateInitialPoolSet();
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="GenericObjectPool{T}"/> class.
        /// </summary>
        ~GenericObjectPool()
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

                    Clear();
                    _objectPool = null;

                    _poolSemaphore?.Close();
                    _poolSemaphore?.Dispose();
                    _poolSemaphore = null;
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
            lock (this)
            {
                if (_objectPool != null)
                {
                    foreach (T poolObject in _objectPool)
                    {
                        MiscHelper.DisposeObject(poolObject);
                    }

                    _objectPool.Clear();
                    Interlocked.Add(ref _totalPoolSize, -_totalPoolSize);
                    Interlocked.Add(ref _availablePoolObjects, -_availablePoolObjects);
                }
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
            lock (this)
            {
                _objectPool = new Stack<T>();
                for (var i = 0; i < InitialPoolSize; i++)
                {
                    AddObjectToPool(CreatePoolObject());
                }

                _poolSemaphore.Release(MaxGrowSize);
            }
        }

        /// <summary>
        /// Adds the object to pool.
        /// </summary>
        /// <param name="poolObject">The pool object.</param>
        private void AddObjectToPool(T poolObject)
        {
            lock (this)
            {
                if (poolObject != null)
                {
                    _objectPool.Push(poolObject);
                    Interlocked.Increment(ref _totalPoolSize);
                    Interlocked.Increment(ref _availablePoolObjects);
                }
            }
        }

        /// <summary>
        /// Gets the total size of the pool.
        /// </summary>
        /// <value>
        /// The total size of the pool.
        /// </value>
        public long TotalPoolSize 
        {
            get
            {
                return Interlocked.Read(ref _totalPoolSize);
            }
        }

        /// <summary>
        /// Gets the available pool objects.
        /// </summary>
        /// <value>
        /// The available pool objects.
        /// </value>
        public long AvailablePoolObjects
        {
            get
            {
                return Interlocked.Read(ref _availablePoolObjects);
            }
        }

        /// <summary>
        /// Gets the taken object pool count.
        /// </summary>
        /// <value>
        /// The taken object pool count.
        /// </value>
        public long TakenObjectPoolCount
        {
            get
            {
                lock (this)
                {
                    return TotalPoolSize - AvailablePoolObjects;
                }
            }
        }

        /// <summary>
        /// Takes this instance.
        /// </summary>
        /// <returns></returns>
        public virtual T Take()
        {
            T poolObject = null;

            _poolSemaphore.WaitOne();

            lock (this)
            {
                if (_objectPool.Count != 0)
                {
                    poolObject = _objectPool.Pop();
                    Interlocked.Decrement(ref _availablePoolObjects);
                }
                else
                {
                    if (TotalPoolSize < MaxGrowSize)
                    {
                        AddObjectToPool(CreatePoolObject());
                        poolObject = _objectPool.Pop();
                        Interlocked.Decrement(ref _availablePoolObjects);
                    }
                }

                if ((TotalPoolSize - AvailablePoolObjects) > MaxObjectUsage)
                {
                    MaxObjectUsage = (TotalPoolSize - AvailablePoolObjects);
                }
            }

            return PoolObjectBeforeGiven(poolObject);
        }

        /// <summary>
        /// Returns the specified pool object.
        /// </summary>
        /// <param name="poolObject">The pool object.</param>
        public virtual void Return(T poolObject)
        {
            if (poolObject == null)
            {
                return;
            }

            poolObject = PoolObjectBeforeReturning(poolObject);

            if (poolObject != null)
            {
                lock (this)
                {
                    _objectPool.Push(poolObject);
                    Interlocked.Increment(ref _availablePoolObjects);
                }
            }
            else
            {
                Interlocked.Decrement(ref _totalPoolSize);
            }

            if (TotalPoolSize != 0)
            {
                _poolSemaphore.Release();
            }
        }

        /// <summary>
        /// Pools the object before given.
        /// </summary>
        /// <param name="poolObject">The pool object.</param>
        /// <returns></returns>
        protected virtual T PoolObjectBeforeGiven(T poolObject)
        {
            return poolObject;
        }

        /// <summary>
        /// Pools the object before returning.
        /// </summary>
        /// <param name="poolObject">The pool object.</param>
        /// <returns></returns>
        protected virtual T PoolObjectBeforeReturning(T poolObject)
        {
            return poolObject;
        }

        /// <summary>
        /// Containers this instance.
        /// </summary>
        /// <returns></returns>
        public PoolInstance<T, TData> Container()
        {
            return new PoolInstance<T, TData>(this);
        }

        /// <summary>
        /// Creates the pool object.
        /// </summary>
        /// <returns></returns>
        protected abstract T CreatePoolObject();
    }
}
