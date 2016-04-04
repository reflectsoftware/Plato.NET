// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.SQL.Enums;

namespace Plato.SQL
{
    /// <summary>
    ///
    /// </summary>
    public class DatabaseDeadlockGuardManager
    {
        private int _deadlockCount { get; set; }

        /// <summary>
        /// Gets the retries.
        /// </summary>
        /// <value>
        /// The retries.
        /// </value>
        public int Retries { get; internal set; }

        /// <summary>
        /// Gets the retry delay.
        /// </summary>
        /// <value>
        /// The retry delay.
        /// </value>
        public int RetryDelay { get; internal set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseDeadlockGuardManager"/> class.
        /// </summary>
        public DatabaseDeadlockGuardManager()
        {
            _deadlockCount = 0;
            Retries = 3;
            RetryDelay = 250;
        }

        /// <summary>
        /// Guards the completed.
        /// </summary>
        public void GuardCompleted()
        {
            _deadlockCount--;
        }

        /// <summary>
        /// Creates the deadlock guard.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="guardType">Type of the guard.</param>
        /// <returns></returns>
        public DatabaseDeadlockGuard CreateDeadlockGuard(string id, DatabaseDeadlockGuardType guardType)
        {
            var isAmbient = _deadlockCount > 0 && guardType != DatabaseDeadlockGuardType.Isolated;
            _deadlockCount++;

            return new DatabaseDeadlockGuard(this, id, isAmbient, Retries, RetryDelay);
        }

        /// <summary>
        /// Creates the deadlock guard.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public DatabaseDeadlockGuard CreateDeadlockGuard(string id)
        {
            return CreateDeadlockGuard(id, DatabaseDeadlockGuardType.Ambient);
        }
    }
}
