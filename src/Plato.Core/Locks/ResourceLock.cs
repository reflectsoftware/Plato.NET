// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Core.Locks.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Plato.Core.Locks
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="Plato.Core.Locks.Interfaces.IResourceLock"/>
    public class ResourceLock : IResourceLock
    {
        private class ResourceLockInfo : IDisposable
        {
            private long _referenceCount;

            public bool Disposed { get; private set; }
            public string NamedKey { get; private set; }

            /// <summary>
            /// Gets or sets the lock.
            /// </summary>
            /// <value>
            /// The lock.
            /// </value>
            public ReaderWriterLockSlim Lock { get; set; }

            /// <summary>
            /// Initializes a new instance of the <see cref="ResourceLockInfo"/> class.
            /// </summary>
            /// <param name="name">The name.</param>
            public ResourceLockInfo(string name)
            {
                NamedKey = name.ToLower();
                _referenceCount = 0;
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

                        Lock?.Dispose();
                        Lock = null;
                    }
                }
            }

            /// <summary>
            /// Gets the reference count.
            /// </summary>
            /// <value>
            /// The reference count.
            /// </value>
            public long ReferenceCount
            {
                get
                {
                    return Interlocked.Read(ref _referenceCount);
                }
            }

            /// <summary>
            /// Increments the reference count.
            /// </summary>
            public void IncrementReferenceCount()
            {
                Interlocked.Increment(ref _referenceCount);
            }

            /// <summary>
            /// Decrements the reference count.
            /// </summary>
            public void DecrementReferenceCount()
            {
                Interlocked.Decrement(ref _referenceCount);
            }

            /// <summary>
            /// Determines whether this instance can dispose.
            /// </summary>
            /// <returns></returns>
            public bool CanDispose()
            {
                return ReferenceCount == 0;
            }
        }

        private static Dictionary<string, ResourceLockInfo> _resources { get; set; }
        private static ReaderWriterLockSlim _resourcesLock { get; set; }
        private ResourceLockInfo _lockInfo { get; set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ResourceLock"/> is disposed.
        /// </summary>
        /// <value>
        /// <c>true</c> if disposed; otherwise, <c>false</c>.
        /// </value>
        public bool Disposed { get; private set; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; private set; }

        /// <summary>
        /// Initializes the <see cref="ResourceLock"/> class.
        /// </summary>
        static ResourceLock()
        {
            _resources = new Dictionary<string, ResourceLockInfo>();
            _resourcesLock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceLock"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public ResourceLock(string name)
        {
            Disposed = false;
            Name = name;
            _lockInfo = GetLockInfo(Name.ToLower());
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
                    DisposeLock(_lockInfo);
                }
            }
        }

        /// <summary>
        /// Gets the lock information.
        /// </summary>
        /// <param name="nameKey">The name key.</param>
        /// <returns></returns>
        private static ResourceLockInfo GetLockInfo(string nameKey)
        {
            _resourcesLock.EnterReadLock();
            try
            {
                if (_resources.ContainsKey(nameKey))
                {
                    var lockInfo = _resources[nameKey];
                    lockInfo.IncrementReferenceCount();

                    return lockInfo;
                }
            }
            finally
            {
                _resourcesLock.ExitReadLock();
            }
            
            _resourcesLock.EnterWriteLock();
            try
            {
                ResourceLockInfo lockInfo;

                if (_resources.ContainsKey(nameKey))
                {
                    lockInfo = _resources[nameKey];
                    lockInfo.IncrementReferenceCount();

                    return lockInfo;
                }

                lockInfo = new ResourceLockInfo(nameKey) { Lock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion) };
                lockInfo.IncrementReferenceCount();

                _resources.Add(nameKey, lockInfo);

                return lockInfo;
            }
            finally
            {
                _resourcesLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Disposes the lock.
        /// </summary>
        /// <param name="lockInfo">The lock information.</param>
        private static void DisposeLock(ResourceLockInfo lockInfo)
        {
            _resourcesLock.EnterWriteLock();
            try
            {
                lockInfo.DecrementReferenceCount();
                if (lockInfo.CanDispose())
                {
                    _resources.Remove(lockInfo.NamedKey);
                    lockInfo.Dispose();
                }
            }
            finally
            {
                _resourcesLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Enters the read lock.
        /// </summary>
        public void EnterReadLock()
        {
            _lockInfo.Lock.EnterReadLock();
        }

        /// <summary>
        /// Exits the read lock.
        /// </summary>
        public void ExitReadLock()
        {
            _lockInfo.Lock.ExitReadLock();
        }

        /// <summary>
        /// Enters the write lock.
        /// </summary>
        public void EnterWriteLock()
        {
            _lockInfo.Lock.EnterWriteLock();
        }

        /// <summary>
        /// Exits the write lock.
        /// </summary>
        public void ExitWriteLock()
        {
            _lockInfo.Lock.ExitWriteLock();
        }
    }
}
