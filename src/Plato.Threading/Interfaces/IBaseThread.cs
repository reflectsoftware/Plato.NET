// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Threading.Enums;
using Plato.Utils.Logging.Interfaces;
using System;
using System.Threading;

namespace Plato.Threading.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public interface IBaseThread : IDisposable
    {
        /// <summary>
        /// Gets a value indicating whether this <see cref="IBaseThread"/> is disposed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if disposed; otherwise, <c>false</c>.
        /// </value>
        bool Disposed { get; }

        /// <summary>
        /// Gets a value indicating whether [process exiting].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [process exiting]; otherwise, <c>false</c>.
        /// </value>
        bool ProcessExiting { get; }

        /// <summary>
        /// Gets the do work execution count.
        /// </summary>
        /// <value>
        /// The do work execution count.
        /// </value>
        ulong DoWorkExecutionCount { get; }

        /// <summary>
        /// Gets the state of the thread.
        /// </summary>
        /// <value>
        /// The state of the thread.
        /// </value>
        BaseThreadState ThreadState { get; }

        string Name { get; }

        /// <summary>
        /// Gets the notification.
        /// </summary>
        /// <value>
        /// The notification.
        /// </value>
        ILogNotification Notification { get; }

        /// <summary>
        /// Gets the active thread.
        /// </summary>
        /// <value>
        /// The active thread.
        /// </value>
        Thread ActiveThread { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="IBaseThread"/> is terminated.
        /// </summary>
        /// <value>
        ///   <c>true</c> if terminated; otherwise, <c>false</c>.
        /// </value>
        bool Terminated { get; }

        /// <summary>
        /// Waits the state of for thread.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <param name="mSec">The m sec.</param>
        /// <returns></returns>
        bool WaitForThreadState(BaseThreadState state, int mSec);

        /// <summary>
        /// Starts the specified b start in background.
        /// </summary>
        /// <param name="bStartInBackground">if set to <c>true</c> [b start in background].</param>
        void Start(bool bStartInBackground = false);

        /// <summary>
        /// Stops this instance.
        /// </summary>
        void Stop();
    }
}
