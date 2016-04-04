// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Threading.Interfaces;
using System;

namespace Plato.Threading.WorkManagement
{
    /// <summary>
    /// 
    /// </summary>
    internal class ThreadWatcherInfo
    {
        /// <summary>
        /// Gets or sets the worker.
        /// </summary>
        /// <value>
        /// The worker.
        /// </value>
        public IBaseWorker Worker { get; set; }

        /// <summary>
        /// Gets or sets the maximum alive window.
        /// </summary>
        /// <value>
        /// The maximum alive window.
        /// </value>
        public TimeSpan MaxAliveWindow { get; set; }

        /// <summary>
        /// Gets or sets the last response time.
        /// </summary>
        /// <value>
        /// The last response time.
        /// </value>
        public DateTime LastResponseTime { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadWatcherInfo"/> class.
        /// </summary>
        /// <param name="worker">The worker.</param>
        /// <param name="aliveWindowSeconds">The alive window seconds.</param>
        public ThreadWatcherInfo(IBaseWorker worker, int aliveWindowSeconds)
        {
            Worker = worker;
            MaxAliveWindow = new TimeSpan(0, 0, aliveWindowSeconds);
            LastResponseTime = DateTime.Now;
        }
    }
}
