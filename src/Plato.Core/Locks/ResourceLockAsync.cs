// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Nito.AsyncEx;
using Plato.Core.Locks.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Plato.Core.Locks
{
    /// <summary>
    /// https://github.com/StephenCleary/AsyncEx/wiki/AsyncReaderWriterLock
    /// </summary>
    /// <seealso cref="Plato.Core.Locks.Interfaces.IResourceLockAsync"/>
    public class ResourceLockAsync : IResourceLockAsync
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
            public AsyncReaderWriterLock Lock { get; set; }

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
        private static AsyncReaderWriterLock _resourcesLock { get; set; }
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
        /// Initializes the <see cref="ResourceLockAsync"/> class.
        /// </summary>
        static ResourceLockAsync()
        {
            _resources = new Dictionary<string, ResourceLockInfo>();
            _resourcesLock = new AsyncReaderWriterLock();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceLockAsync"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public ResourceLockAsync(string name)
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
            using (_resourcesLock.ReaderLock())
            {
                if (_resources.ContainsKey(nameKey))
                {
                    var lockInfo = _resources[nameKey];
                    lockInfo.IncrementReferenceCount();

                    return lockInfo;
                }
            }

            using (_resourcesLock.WriterLock())
            {
                ResourceLockInfo lockInfo;

                if (_resources.ContainsKey(nameKey))
                {
                    lockInfo = _resources[nameKey];
                    lockInfo.IncrementReferenceCount();

                    return lockInfo;
                }

                lockInfo = new ResourceLockInfo(nameKey) { Lock = new AsyncReaderWriterLock() };
                lockInfo.IncrementReferenceCount();

                _resources.Add(nameKey, lockInfo);

                return lockInfo;
            }
        }

        /// <summary>
        /// Disposes the lock.
        /// </summary>
        /// <param name="lockInfo">The lock information.</param>
        private static void DisposeLock(ResourceLockInfo lockInfo)
        {
            using (_resourcesLock.WriterLock())
            {
                lockInfo.DecrementReferenceCount();
                if (lockInfo.CanDispose())
                {
                    _resources.Remove(lockInfo.NamedKey);
                    lockInfo.Dispose();
                }
            }
        }

        /// <summary>
        /// Readers the lock.
        /// </summary>
        /// <returns></returns>
        public IDisposable ReaderLock()
        {
            return _lockInfo.Lock.ReaderLock();
        }
        /// <summary>
        /// Readers the lock.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public IDisposable ReaderLock(CancellationToken cancellationToken)
        {
            return _lockInfo.Lock.ReaderLock(cancellationToken);
        }

        /// <summary>
        /// Writers the lock.
        /// </summary>
        /// <returns></returns>
        public IDisposable WriterLock()
        {
            return _lockInfo.Lock.WriterLock();
        }

        /// <summary>
        /// Writers the lock.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public IDisposable WriterLock(CancellationToken cancellationToken)
        {
            return _lockInfo.Lock.WriterLock(cancellationToken);
        }
        
        /// <summary>
        /// Readers the lock asynchronous.
        /// </summary>
        /// <returns></returns>
        public AwaitableDisposable<IDisposable> ReaderLockAsync()
        {
            return _lockInfo.Lock.ReaderLockAsync();
        }

        /// <summary>
        /// Readers the lock asynchronous.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public AwaitableDisposable<IDisposable> ReaderLockAsync(CancellationToken cancellationToken)
        {
            return _lockInfo.Lock.ReaderLockAsync(cancellationToken);
        }

        /// <summary>
        /// Writers the lock asynchronous.
        /// </summary>
        /// <returns></returns>
        public AwaitableDisposable<IDisposable> WriterLockAsync()
        {
            return _lockInfo.Lock.WriterLockAsync();
        }

        /// <summary>
        /// Writers the lock asynchronous.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public AwaitableDisposable<IDisposable> WriterLockAsync(CancellationToken cancellationToken)
        {
            return _lockInfo.Lock.WriterLockAsync(cancellationToken);
        }
    }
}
