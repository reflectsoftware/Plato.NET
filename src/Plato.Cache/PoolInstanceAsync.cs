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
    public class PoolInstanceAsync<T, TData> : IDisposable where T: class
    {
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
        public GenericObjectPoolAsync<T, TData> Pool { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PoolInstance{T}"/> class.
        /// </summary>
        /// <param name="pool">The pool.</param>
        internal PoolInstanceAsync(GenericObjectPoolAsync<T, TData> pool, T poolObject)
        {
            Pool = pool;
            Instance = poolObject;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Pool.ReturnAsync(Instance).Wait();
            GC.SuppressFinalize(this);
        }
    }
}
