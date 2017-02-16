// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Nito.AsyncEx;
using System;
using System.Threading;

namespace Plato.Core.Locks.Interfaces
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="System.IDisposable"/>
    public interface IResourceLockAsync : IDisposable
    {
        /// <summary>
        /// Readers the lock.
        /// </summary>
        /// <returns></returns>
        IDisposable ReaderLock();

        /// <summary>
        /// Readers the lock.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        IDisposable ReaderLock(CancellationToken cancellationToken);

        /// <summary>
        /// Writers the lock.
        /// </summary>
        /// <returns></returns>
        IDisposable WriterLock();

        /// <summary>
        /// Writers the lock.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        IDisposable WriterLock(CancellationToken cancellationToken);
        
        /// <summary>
        /// Readers the lock asynchronous.
        /// </summary>
        /// <returns></returns>
        AwaitableDisposable<IDisposable> ReaderLockAsync();

        /// <summary>
        /// Readers the lock asynchronous.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        AwaitableDisposable<IDisposable> ReaderLockAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Writers the lock asynchronous.
        /// </summary>
        /// <returns></returns>
        AwaitableDisposable<IDisposable> WriterLockAsync();

        /// <summary>
        /// Writers the lock asynchronous.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        AwaitableDisposable<IDisposable> WriterLockAsync(CancellationToken cancellationToken);
    }
}
