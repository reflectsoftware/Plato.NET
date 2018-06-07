// Plato.NET
// Copyright (c) 2018 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Cache;
using Plato.Core.Miscellaneous;
using Plato.Messaging.RMQ.Interfaces;
using System;
using System.Threading.Tasks;

namespace Plato.Messaging.RMQ.Pool
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RMQPoolContainerAsync<T> : IRMQPoolContainerAsync<T> where T : class
    {
        private readonly PoolInstanceAsync<T, RMQPoolStates> _container;
        private bool _disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="RMQPoolContainerAsync{T}"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public RMQPoolContainerAsync(PoolInstanceAsync<T, RMQPoolStates> container)
        {
            Guard.AgainstNull(() => container);

            _container = container;
        }

        /// <summary>
        /// Gets the pool identifier.
        /// </summary>
        /// <value>
        /// The pool identifier.
        /// </value>
        public Guid PoolId
        {
            get
            {
                return _container.Pool.Id;
            }
        }

        /// <summary>
        /// Totals the pool size asynchronous.
        /// </summary>
        /// <returns></returns>
        public Task<long> TotalPoolSizeAsync()
        {
            return _container.Pool.TotalPoolSizeAsync();
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public T Instance
        {
            get
            {
                return _container.Instance;
            }
        }

        #region Dispose
        /// <summary>
        /// Finalizes an instance of the <see cref="RMQPoolContainerAsync{T}"/> class.
        /// </summary>
        ~RMQPoolContainerAsync()
        {
            Dispose(false);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected void Dispose(bool disposing)
        {
            lock (this)
            {
                if (!_disposed)
                {
                    _disposed = true;
                    GC.SuppressFinalize(this);

                    _container?.Dispose();
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
    }
}
