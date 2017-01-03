// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System;
using System.Collections.Generic;
using System.Threading;

namespace Plato.Threading.WorkManagement
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Threading.BaseThread" />
    internal class ThreadWatcher : BaseThread
    {
        private readonly List<ThreadWatcherInfo> _threads;
        private readonly WorkManager _workManager;
        private int _workSleep;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadWatcher"/> class.
        /// </summary>
        /// <param name="manager">The manager.</param>
        public ThreadWatcher(WorkManager manager) : base("Plato.Threading.WorkManagement.ThreadWatcher")
        {
            _threads = new List<ThreadWatcherInfo>();
            _workSleep = 250;
            _workManager = manager;
        }

        /// <summary>
        /// Gets the work sleep value.
        /// </summary>
        /// <returns></returns>
        protected override int GetWorkSleepValue()
        {
            return _workSleep;
        }

        /// <summary>
        /// Gets the wait on terminate thread value.
        /// </summary>
        /// <returns></returns>
        protected override int GetWaitOnTerminateThreadValue()
        {
            return 5000;
        }

        /// <summary>
        /// Called when [work].
        /// </summary>
        protected override void OnWork()
        {
            try
            {
                var threadArray = (ThreadWatcherInfo[])null;
                lock (_threads)
                {
                    threadArray = _threads.ToArray();
                }

                if (threadArray.Length == 0)
                {
                    return;
                }

                for (var i = 0; i < threadArray.Length; i++)
                {
                    if (DateTime.Now.Subtract(threadArray[i].LastResponseTime) > threadArray[i].MaxAliveWindow)
                    {
                        if (_workManager != null)
                        {
                            _workManager.RemoveNonResponsiveWorker(threadArray[i]);
                        }

                        lock (_threads)
                        {
                            _threads.Remove(threadArray[i]);
                        }
                    }
                }

                PassiveSleep(_workSleep);
            }
            catch (Exception ex)
            {
                Notification.SendException(ex, true);
            }
        }

        /// <summary>
        /// Adds the thread.
        /// </summary>
        /// <param name="tInfo">The t information.</param>
        public void AddThread(ThreadWatcherInfo tInfo)
        {
            if (tInfo.MaxAliveWindow == TimeSpan.Zero || WorkManagerConfig.DisableWatchWhenDebugging)
            {
                return;
            }

            lock (_threads)
            {
                _threads.Add(tInfo);
                tInfo.LastResponseTime = DateTime.Now;
            }
        }

        /// <summary>
        /// Removes the thread.
        /// </summary>
        /// <param name="tInfo">The t information.</param>
        public void RemoveThread(ThreadWatcherInfo tInfo)
        {
            if (tInfo.MaxAliveWindow == TimeSpan.Zero || WorkManagerConfig.DisableWatchWhenDebugging)
            {
                return;
            }

            lock (_threads)
            {
                _threads.Remove(tInfo);
            }
        }

        /// <summary>
        /// Passives the sleep.
        /// </summary>
        /// <param name="tInfo">The t information.</param>
        /// <param name="mSec">The m sec.</param>
        /// <param name="terminate">if set to <c>true</c> [terminate].</param>
        public static void PassiveSleep(ThreadWatcherInfo tInfo, long mSec, ref bool terminate)
        {
            var sleep = mSec;
            while ((sleep > 0) && !terminate)
            {
                tInfo.LastResponseTime = DateTime.Now;

                Thread.Sleep(100);
                sleep -= 100;
            }

            tInfo.LastResponseTime = DateTime.Now;
        }
    }
}
