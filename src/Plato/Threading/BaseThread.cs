// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Core.Logging;
using Plato.Core.Logging.Enums;
using Plato.Core.Logging.Interfaces;
using Plato.Core.Miscellaneous;
using Plato.Interfaces;
using Plato.Threading.Enums;
using Plato.Threading.Exceptions;
using System;
using System.Threading;

namespace Plato.Threading
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Interfaces.IBaseThread" />
    public abstract class BaseThread : IBaseThread
    {
        private readonly object _threadLock;
        protected bool _terminated;

        /// <summary>
        /// Gets a value indicating whether this <see cref="IBaseThread" /> is disposed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if disposed; otherwise, <c>false</c>.
        /// </value>
        public bool Disposed { get; private set; }              

        /// <summary>
        /// Gets the do work execution count.
        /// </summary>
        /// <value>
        /// The do work execution count.
        /// </value>
        public ulong DoWorkExecutionCount { get; private set; }

        /// <summary>
        /// Gets the state of the thread.
        /// </summary>
        /// <value>
        /// The state of the thread.
        /// </value>
        public BaseThreadState ThreadState { get; private set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets the notification.
        /// </summary>
        /// <value>
        /// The notification.
        /// </value>
        public ILogNotification Notification { get; set; }

        /// <summary>
        /// Gets the active thread.
        /// </summary>
        /// <value>
        /// The active thread.
        /// </value>
        public Thread ActiveThread { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseThread"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="notification">The notification.</param>
        public BaseThread(string name, ILogNotification notification)
        {
            _threadLock = new object();
            _terminated = true;

            Disposed = false;            
            Name = name;
            ActiveThread = null;            
            Notification = notification ?? new LogNotification();
            ThreadState = BaseThreadState.Stopped;
            DoWorkExecutionCount = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseThread"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public BaseThread(string name) : this(name, new LogNotification())
        {
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="bDisposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        /// <exception cref="Plato.Threading.Exceptions.ThreadWorkException"></exception>
        protected virtual void Dispose(bool bDisposing )
        {
            lock (this)
            {
                if (!Disposed)
                {
                    Disposed = true;
                    GC.SuppressFinalize(this);

                    if (bDisposing)
                    {
                        try
                        {
                            OnDispose();
                        }
                        catch (Exception ex)
                        {
                            var eMsg = string.Format("OnDispose failed: An unhandled exception was detected in Thread '{0} -> {1}'.", Name, ex.Message);
                            throw new ThreadWorkException(eMsg, ex);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }
        
        /// <summary>
        /// Threads the begin sequence.
        /// </summary>
        private void ThreadBeginSequence()
        {
            try
            {
                OnInitializeThread();
                OnStartedThread();
                Notification.SendInformation("Thread '{0}' was successfully started.", Name);
                ThreadState = BaseThreadState.Started;
            }
            catch (Exception ex)
            {
                Notification.SendException(ex);
            }
        }

        /// <summary>
        /// Threads the end sequence.
        /// </summary>
        private void ThreadEndSequence()
        {
            try
            {
                OnStoppedThread();
                OnUninitializeThread();
                Notification.SendInformation("Thread '{0}' was successfully stopped.", Name);
                ThreadState = BaseThreadState.Stopped;
            }
            catch (Exception ex)
            {
                Notification.SendException(ex);
            }
        }

        /// <summary>
        /// _s the safe thread stop.
        /// </summary>
        private void _SafeThreadStop()
        {
            Stop();
        }

        /// <summary>
        /// Executes this instance.
        /// </summary>
        private void Execute()
        {
            ThreadBeginSequence();

            try
            {
                _terminated = false;
                while (!_terminated)
                {
                    try
                    {
                        OnWork();
                        PassiveSleep(GetWorkSleepValue());
                    }
                    catch (ThreadAbortException)
                    {
                        Notification.SendMessage(string.Format("The following thread '{0}' was aborted.", Name), NotificationType.Fatal);
                        _terminated = true;
                        break;
                    }
                    catch (Exception ex)
                    {
                        var action = OnHandleException(ex);
                        if (action == HandleExceptionType.None)
                        {
                            return;
                        }
                        if (action == HandleExceptionType.Default || action == HandleExceptionType.DefaultAndAbort)
                        {
                            var eMsg = string.Format("Thread Execution: An unhandled exception was detected in thread '{0}' -> {1}.", Name, ex.Message);
                            Notification.SendException(new ThreadWorkException(eMsg, ex), false);
                        }

                        if (action == HandleExceptionType.DefaultAndAbort)
                        {
                            Notification.SendMessage(string.Format("The following thread '{0}' will be aborted.", Name), NotificationType.Fatal);
                            _terminated = true;
                        }
                    }
                    finally
                    {
                        DoWorkExecutionCount++;
                    }
                }
            }
            finally
            {
                ThreadEndSequence();
            }
        }

        /// <summary>
        /// Safes the thread stop.
        /// </summary>
        private void SafeThreadStop()
        {
            _terminated = true;
            new Thread(_SafeThreadStop).Start();
        }

        /// <summary>
        /// Called when [work].
        /// </summary>
        protected abstract void OnWork();

        /// <summary>
        /// Called when [handle exception].
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns></returns>
        protected virtual HandleExceptionType OnHandleException(Exception ex)
        {
            return HandleExceptionType.Default;
        }

        /// <summary>
        /// Called when [dispose].
        /// </summary>
        protected virtual void OnDispose()
        {
        }

        /// <summary>
        /// Called when [initialize thread].
        /// </summary>
        protected virtual void OnInitializeThread()
        {
        }

        /// <summary>
        /// Called when [uninitialize thread].
        /// </summary>
        protected virtual void OnUninitializeThread()
        {
        }

        /// <summary>
        /// Called when [started thread].
        /// </summary>
        protected virtual void OnStartedThread()
        {
        }

        /// <summary>
        /// Called when [stopped thread].
        /// </summary>
        protected virtual void OnStoppedThread()
        {
        }

        /// <summary>
        /// Called when [starting].
        /// </summary>
        protected virtual void OnStarting()
        {
        }

        /// <summary>
        /// Called when [started].
        /// </summary>
        protected virtual void OnStarted()
        {
        }

        /// <summary>
        /// Called when [stopping].
        /// </summary>
        protected virtual void OnStopping()
        {
        }

        /// <summary>
        /// Called when [stopped].
        /// </summary>
        protected virtual void OnStopped()
        {
        }

        /// <summary>
        /// Passives the sleep.
        /// </summary>
        /// <param name="msecSleep">The msec sleep.</param>
        protected virtual void PassiveSleep(int msecSleep)
        {
            MiscHelper.PassiveSleep(msecSleep, ref _terminated);
        }

        /// <summary>
        /// Gets the work sleep value.
        /// </summary>
        /// <returns></returns>
        protected virtual int GetWorkSleepValue()
        {
            return 1000;
        }

        /// <summary>
        /// Gets the wait on terminate thread value.
        /// </summary>
        /// <returns></returns>
        protected virtual int GetWaitOnTerminateThreadValue()
        {
            return 10000;
        }

        /// <summary>
        /// Waits the state of for thread.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <param name="mSec">The m sec.</param>
        /// <returns></returns>
        public bool WaitForThreadState(BaseThreadState state, int mSec)
        {
            if (mSec != 0)
            {
                long sleep = mSec;
                while (sleep > 0 || mSec == Timeout.Infinite)
                {
                    if (ThreadState == state)
                    {
                        break;
                    }

                    Thread.Sleep(100);
                    sleep -= 100;
                }
            }

            return ThreadState == state;
        }

        /// <summary>
        /// Starts the specified b start in background.
        /// </summary>
        /// <param name="bStartInBackground">if set to <c>true</c> [b start in background].</param>
        public void Start(bool bStartInBackground = false)
        {
            lock (_threadLock)
            {
                if (ActiveThread == null)
                {
                    OnStarting();
                    try
                    {
                        _terminated = false;
                        var tmpThread = new Thread(Execute) { IsBackground = bStartInBackground };
                        tmpThread.Start();

                        ActiveThread = tmpThread;
                    }
                    finally
                    {
                        OnStarted();
                    }
                }
            }
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void Stop()
        {
            lock (_threadLock)
            {
                if (ActiveThread != null)
                {
                    if (ActiveThread.ManagedThreadId != Thread.CurrentThread.ManagedThreadId)
                    {
                        OnStopping();
                        try
                        {
                            _terminated = true;
                            WaitForThreadState(BaseThreadState.Stopped, GetWaitOnTerminateThreadValue());

                            if (ThreadState != BaseThreadState.Stopped)
                            {
                                ActiveThread.Abort();
                            }

                            ActiveThread = null;
                        }
                        finally
                        {
                            OnStopped();
                        }
                    }
                    else
                    {
                        SafeThreadStop();
                    }
                }
            }
        }

        /// <summary>
        /// Terminates this instance.
        /// </summary>
        internal void Terminate()
        {
            _terminated = true;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="IBaseThread" /> is terminated.
        /// </summary>
        /// <value>
        ///   <c>true</c> if terminated; otherwise, <c>false</c>.
        /// </value>
        public bool Terminated
        {
            get
            {
                return _terminated;
            }
        }
    }
}
