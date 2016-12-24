// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Core.Locks.Enums;
using Plato.Core.Locks.Interfaces;
using System;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading;

namespace Plato.Core.Locks
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="Plato.Locks.Interfaces.IMutexResourceLock"/>
    public class MutexResourceLock : IMutexResourceLock
    {
        private readonly static MutexSecurity FFileMutexSec;
        private Mutex _mutex;

        /// <summary>
        /// Gets a value indicating whether this <see cref="MutexResourceLock"/> is disposed.
        /// </summary>
        /// <value>
        /// <c>true</c> if disposed; otherwise, <c>false</c>.
        /// </value>
        public bool Disposed { get; private set; }
        
        /// <summary>
        /// Initializes the <see cref="MutexResourceLock"/> class.
        /// </summary>
        static MutexResourceLock()
        {
            FFileMutexSec = new MutexSecurity();
            FFileMutexSec.AddAccessRule(new MutexAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), MutexRights.Synchronize | MutexRights.FullControl, AccessControlType.Allow));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MutexResourceLock"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="rType">Type of the r.</param>
        public MutexResourceLock(string name, MutexResourceLockType rType = MutexResourceLockType.Global)
        {
            Disposed = false;
            var sType = rType == MutexResourceLockType.Local ? "Local" : "Global";

            bool bCreatedNew;
            _mutex = new Mutex(false, string.Format(@"{0}\Plato.ResourceLock.Resource:{1}", sType, name.ToLower()), out bCreatedNew, FFileMutexSec);
        }

        /// <summary>
        /// Locks this instance.
        /// </summary>
        public void Lock()
        {
            _mutex?.WaitOne();
        }

        /// <summary>
        /// Unlocks this instance.
        /// </summary>
        public void Unlock()
        {
            _mutex?.ReleaseMutex();
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

                    _mutex?.Close();
                    _mutex?.Dispose();
                    _mutex = null;
                }
            }
        }
    }
}
