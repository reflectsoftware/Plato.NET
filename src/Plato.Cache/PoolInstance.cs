// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System;

namespace Plato.Cache
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="System.IDisposable"/>
    public class PoolInstance<T> : IDisposable where T: class
    {
        /// <summary>
        /// Gets a value indicating whether this <see cref="PoolInstance{T}"/> is disposed.
        /// </summary>
        /// <value>
        /// <c>true</c> if disposed; otherwise, <c>false</c>.
        /// </value>
        public bool Disposed { get; private set; }
        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public T Instance { get; private set; }

        /// <summary>
        /// Gets the pool.
        /// </summary>
        /// <value>
        /// The pool.
        /// </value>
        public GenericObjectPool<T> Pool { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PoolInstance{T}"/> class.
        /// </summary>
        /// <param name="pool">The pool.</param>
        internal PoolInstance(GenericObjectPool<T> pool)
        {
            Pool = pool;
            Instance = Pool.Take();
            Disposed = false;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            lock (this)
            {
                if (!Disposed)
                {
                    Disposed = true;
                    GC.SuppressFinalize(this);

                    Pool.Return(Instance);
                }
            }
        }
    }
}
