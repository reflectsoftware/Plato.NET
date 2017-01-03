// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System;

namespace Plato.Core.Locks.Interfaces
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="System.IDisposable"/>
    public interface IMutexResourceLock : IDisposable
    {
        /// <summary>
        /// Locks this instance.
        /// </summary>
        void Lock();

        /// <summary>
        /// Unlocks this instance.
        /// </summary>
        void Unlock();
    }
}
