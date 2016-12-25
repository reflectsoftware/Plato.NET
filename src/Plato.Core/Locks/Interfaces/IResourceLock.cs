// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System;

namespace Plato.Core.Locks.Interfaces
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="System.IDisposable"/>
    public interface IResourceLock : IDisposable
    {
        /// <summary>
        /// Enters the read lock.
        /// </summary>
        void EnterReadLock();

        /// <summary>
        /// Exits the read lock.
        /// </summary>
        void ExitReadLock();

        /// <summary>
        /// Enters the write lock.
        /// </summary>
        void EnterWriteLock();

        /// <summary>
        /// Exits the write lock.
        /// </summary>
        void ExitWriteLock();
    }
}
