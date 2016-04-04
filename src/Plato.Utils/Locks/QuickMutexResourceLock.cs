// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Utils.Locks.Enums;
using Plato.Utils.Locks.Interfaces;
using System;

namespace Plato.Utils.Locks
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="Plato.Locks.Interfaces.IQuickMutexResourceLock"/>
    public class QuickMutexResourceLock : IQuickMutexResourceLock
    {
        private MutexResourceLock _resourceLock;

        /// <summary>
        /// Gets a value indicating whether this <see cref="QuickMutexResourceLock"/> is disposed.
        /// </summary>
        /// <value>
        /// <c>true</c> if disposed; otherwise, <c>false</c>.
        /// </value>
        public bool Disposed { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="QuickMutexResourceLock"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="rType">Type of the r.</param>
        public QuickMutexResourceLock(string name, MutexResourceLockType rType = MutexResourceLockType.Global)
        {
            Disposed = false;
            _resourceLock = new MutexResourceLock(name, rType);
            _resourceLock.Lock();
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose()
        {
            lock (this)
            {
                if (!Disposed)
                {
                    Disposed = true;
                    GC.SuppressFinalize(this);

                    _resourceLock?.Unlock();
                    _resourceLock?.Dispose();
                    _resourceLock = null;
                }
            }
        }
    }
}
