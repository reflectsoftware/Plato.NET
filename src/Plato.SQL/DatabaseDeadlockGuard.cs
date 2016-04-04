// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System;
using System.Data.SqlClient;
using System.Threading;

namespace Plato.SQL
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="System.IDisposable"/>
    public class DatabaseDeadlockGuard : IDisposable
    {
        private DatabaseDeadlockGuardManager _manager { get; set; }
        private bool _isAmbient { get; set; }
        private int _retries { get; set; }
        private int _retryDelay { get; set; }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Id { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is disposed.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is disposed; otherwise, <c>false</c>.
        /// </value>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseDeadlockGuard"/> class.
        /// </summary>
        /// <param name="manager">The manager.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="isAmbient">if set to <c>true</c> [is ambient].</param>
        /// <param name="retries">The retries.</param>
        /// <param name="retryDelay">The retry delay.</param>
        internal DatabaseDeadlockGuard(DatabaseDeadlockGuardManager manager, string id, bool isAmbient, int retries, int retryDelay)
        {
            _manager = manager;
            Id = id;
            _isAmbient = isAmbient;
            _retries = retries;
            _retryDelay = retryDelay;
            IsDisposed = false;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="DatabaseDeadlockGuard"/> class.
        /// </summary>
        ~DatabaseDeadlockGuard()
        {
            Dispose(false);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            lock (this)
            {
                if (!IsDisposed)
                {
                    IsDisposed = true;
                    GC.SuppressFinalize(this);
                    _manager.GuardCompleted();
                }
            }
        }

        /// <summary>
        /// Executes the specified operation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="operation">The operation.</param>
        /// <returns></returns>
        public T Execute<T>(Func<T> operation)
        {
            var deadlockRetries = _retries;

            while (true)
            {
                try
                {
                    return operation();
                }
                catch (SqlException ex)
                {
                    if (_isAmbient || ex.Number != SQLErrors.DEADLOCK || --deadlockRetries < 0)
                    {
                        throw;
                    }

                    Thread.Sleep(_retryDelay);
                    continue;
                }
            }
        }

        /// <summary>
        /// Executes the specified operation.
        /// </summary>
        /// <param name="operation">The operation.</param>
        public void Execute(Action operation)
        {
            Execute<object>(() =>
            {
                operation();
                return null;
            });
        }
    }
}
