// Plato.NET
// Copyright (c) 2018 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Cache;
using Plato.Core.Miscellaneous;
using Plato.Messaging.AMQ.Interfaces;
using Plato.Messaging.Interfaces;
using System;

namespace Plato.Messaging.AMQ.Pool
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="Plato.Messaging.AMQ.Interfaces.IAMQPoolContainer{T}" />
    public class AMQPoolContainer<T> : IAMQPoolContainer<T> where T: IMessageReceiverSender
    {
        private readonly PoolInstance<AMQObjectPoolData, object> _container;
        private bool _disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="AMQPoolContainer{T}"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public AMQPoolContainer(PoolInstance<AMQObjectPoolData, object> container)
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
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public T Instance
        {
            get
            {
                return (T)_container.Instance.Instance;
            }
        }

        #region Dispose
        /// <summary>
        /// Finalizes an instance of the <see cref="AMQPoolContainer{T}"/> class.
        /// </summary>
        ~AMQPoolContainer()
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
