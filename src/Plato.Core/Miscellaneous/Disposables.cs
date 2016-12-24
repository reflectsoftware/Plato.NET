// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Core.Interfaces;
using System;
using System.Collections.Concurrent;

namespace Plato.Core.Miscellaneous
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Telmetrics.BusinessActivity.Subscriber.Core.IDisposables" />
    public class Disposables : IDisposables
    {
        private readonly ConcurrentQueue<object> _disposables;

        public bool Disposed { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Disposables"/> class.
        /// </summary>
        public Disposables()
        {
            Disposed = false;
            _disposables = new ConcurrentQueue<object>();
        }

        #region Dispose
        /// <summary>
        /// Finalizes an instance of the <see cref="Disposables"/> class.
        /// </summary>
        ~Disposables()
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
                    GC.SuppressFinalize(this);

                    DisposeAll();
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
        /// Adds the specified disposable.
        /// </summary>
        /// <param name="disposable">The disposable.</param>
        public void Add(object disposable)
        {
            _disposables.Enqueue(disposable);
        }

        /// <summary>
        /// Disposes all.
        /// </summary>
        public void DisposeAll()
        {
            lock (this)
            {
                object item;
                while (_disposables.TryDequeue(out item))
                {
                    (item as IDisposable)?.Dispose();
                }
            }
        }
    }
}
