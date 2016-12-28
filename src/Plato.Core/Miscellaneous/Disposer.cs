// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Core.Interfaces;
using System;

namespace Plato.Core.Miscellaneous
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="Plato.Core.Interfaces.IDisposer{T}" />
    public class Disposer<T> : IDisposer<T> where T : class
    {
        /// <summary>
        /// Gets a value indicating whether this <see cref="Disposer{T}"/> is disposed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if disposed; otherwise, <c>false</c>.
        /// </value>
        public bool Disposed { get; private set; }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public T Instance { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Disposer{T}"/> class.
        /// </summary>
        /// <param name="instance">The instance.</param>
        public Disposer(T instance)
        {
            Disposed = false;
            Instance = instance;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="Disposer{T}"/> class.
        /// </summary>
        ~Disposer()
        {
            Dispose(false);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            lock (this)
            {
                if (!Disposed)
                {
                    Disposed = true;
                    if (disposing)
                    {
                        GC.SuppressFinalize(this);
                    }

                    (Instance as IDisposable)?.Dispose();
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

    public class DisposerFactory : IDisposerFactory
    {
        public IDisposer<T> Create<T>(T instance) where T : class
        {
            return new Disposer<T>(instance);
        }
    }
}
