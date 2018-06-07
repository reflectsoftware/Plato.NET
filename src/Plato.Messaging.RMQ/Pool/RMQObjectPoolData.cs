// Plato.NET
// Copyright (c) 2018 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Messaging.Interfaces;
using System;

namespace Plato.Messaging.RMQ.Pool
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public class RMQObjectPoolData : IDisposable
    {
        public IMessageReceiverSender Instance { get; set; }
        protected bool _disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="RMQObjectPoolData"/> class.
        /// </summary>
        public RMQObjectPoolData()
        {
            _disposed = false;
        }

        #region Dispose
        /// <summary>
        /// Finalizes an instance of the <see cref="RMQObjectPoolData"/> class.
        /// </summary>
        ~RMQObjectPoolData()
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
                if (!_disposed)
                {
                    _disposed = false;
                    GC.SuppressFinalize(this);
                    Instance?.Dispose();
                }
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        void IDisposable.Dispose()
        {
            Dispose(true);
        }
        #endregion Dispose
    }
}
