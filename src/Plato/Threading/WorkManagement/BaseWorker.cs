// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Threading.Enums;
using Plato.Threading.Exceptions;
using Plato.Interfaces;
using System;
using System.Threading;

namespace Plato.Threading.WorkManagement
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Threading.BaseThread" />
    /// <seealso cref="Plato.Interfaces.IBaseWorker" />
    public abstract class BaseWorker : BaseThread, IBaseWorker
    {
        private readonly ThreadWatcherInfo _threadWatcherInfo;        
        private readonly bool _abortThreadOnUnhandledException;
        private readonly bool _restartNonResponsive;
        private readonly int _workSleep;
        private bool _restartAbortedThread;

        /// <summary>
        /// Gets the work package.
        /// </summary>
        /// <value>
        /// The work package.
        /// </value>
        public IWorkPackage WorkPackage { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseWorker"/> class.
        /// </summary>
        public BaseWorker() : base(string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseWorker"/> class.
        /// </summary>
        /// <param name="workPackage">The work package.</param>
        /// <exception cref="WorkManagerException"></exception>
        public BaseWorker(IWorkPackage workPackage) : base(workPackage.NameInstance)
        {
            try
            {
                WorkPackage = workPackage;
                Name = WorkPackage.NameInstance;
                Notification = WorkPackage.WorkManager.Notification;
                _restartAbortedThread = false;
                _abortThreadOnUnhandledException = WorkPackage.Parameters("abortThreadOnUnhandledException", "false") == "true";
                _restartNonResponsive = WorkPackage.Parameters("restartNonResponsive", "false") == "true";
                _workSleep = int.Parse(WorkPackage.Parameters("workSleep", "60000"));
                _threadWatcherInfo = new ThreadWatcherInfo(this, int.Parse(WorkPackage.Parameters("aliveResponseWindow", "0")));

                OnInitialize();
            }
            catch (Exception ex)
            {
                var eMsg = string.Format("Initialize failed: An unhandled exception was detected in Thread '{0} -> {1}'.", Name, ex.Message);
                throw new WorkManagerException(eMsg, ex);
            }
        }

        /// <summary>
        /// Called when [initialize].
        /// </summary>
        protected virtual void OnInitialize()
        {
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
        /// Passives the sleep.
        /// </summary>
        /// <param name="sleep">The sleep.</param>
        protected override void PassiveSleep(int sleep)
        {
            ThreadWatcher.PassiveSleep(_threadWatcherInfo, sleep, ref _terminated);
        }

        /// <summary>
        /// Gets the wait on terminate thread value.
        /// </summary>
        /// <returns></returns>
        protected override int GetWaitOnTerminateThreadValue()
        {
            return WorkManagerConfig.WaitOnTerminateThread;
        }

        /// <summary>
        /// Called when [started thread].
        /// </summary>
        protected override void OnStartedThread()
        {
            (WorkPackage as WorkPackage).Watcher.AddThread(_threadWatcherInfo);
        }

        /// <summary>
        /// Called when [stopped thread].
        /// </summary>
        protected override void OnStoppedThread()
        {
            (WorkPackage as WorkPackage).Watcher.RemoveThread(_threadWatcherInfo);

            if (_restartAbortedThread)
            {
                WorkPackage.WorkManager.RemoveNonResponsiveWorker(_threadWatcherInfo);
            }
        }

        /// <summary>
        /// Called when [handle exception].
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns></returns>
        protected override HandleExceptionType OnHandleException(Exception ex)
        {
            var option = _abortThreadOnUnhandledException ? HandleExceptionType.DefaultAndAbort : HandleExceptionType.Default;

            if (option == HandleExceptionType.DefaultAndAbort && _restartNonResponsive)
            {
                _restartAbortedThread = true;
            }

            return option;
        }

        /// <summary>
        /// Waits the state of for worker on.
        /// </summary>
        /// <param name="worker">The worker.</param>
        /// <param name="state">The state.</param>
        /// <param name="waitmsec">The waitmsec.</param>
        /// <returns></returns>
        protected bool WaitForWorkerOnState(IBaseWorker worker, BaseThreadState state, int waitmsec)
        {
            if (waitmsec != 0)
            {
                long sleep = waitmsec;
                while ((sleep > 0 || waitmsec == Timeout.Infinite) && !_terminated)
                {
                    if (worker.ThreadState == state)
                    {
                        break;
                    }

                    PassiveSleep(100);
                    sleep -= 100;
                }
            }

            return ThreadState == state;
        }

        /// <summary>
        /// Creates the worker.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        protected IBaseWorker CreateWorker(string name, object data = null)
        {
            return WorkPackage.WorkManager.CreateWorker(name, name, data);
        }

        /// <summary>
        /// Spawns the worker.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="instanceName">Name of the instance.</param>
        /// <param name="data">The data.</param>
        /// <param name="waitmsec">The waitmsec.</param>
        /// <returns></returns>
        protected IBaseWorker SpawnWorker(string name, string instanceName, object data = null, int waitmsec = 0)
        {
            var worker = WorkPackage.WorkManager.SpawnWorker(name, instanceName, data);
            WaitForWorkerOnState(worker, BaseThreadState.Started, waitmsec);

            return worker;
        }

        /// <summary>
        /// Spawns the worker.
        /// </summary>
        /// <param name="worker">The worker.</param>
        /// <param name="waitmsec">The waitmsec.</param>
        /// <returns></returns>
        protected IBaseWorker SpawnWorker(IBaseWorker worker, int waitmsec = 0)
        {
            WorkPackage.WorkManager.SpawnWorker(worker);
            WaitForWorkerOnState(worker, BaseThreadState.Started, waitmsec);

            return worker;
        }

        /// <summary>
        /// Terminates the worker.
        /// </summary>
        /// <param name="worker">The worker.</param>
        protected void TerminateWorker(IBaseWorker worker)
        {
            WorkPackage.WorkManager.TerminateWorker(worker);
        }

        /// <summary>
        /// Terminates the worker.
        /// </summary>
        /// <param name="name">The name.</param>
        protected void TerminateWorker(string name)
        {
            WorkPackage.WorkManager.TerminateWorker(name);
        }

        /// <summary>
        /// Joins the specified worker.
        /// </summary>
        /// <param name="worker">The worker.</param>
        /// <param name="waitmsec">The waitmsec.</param>
        public void Join(IBaseWorker worker, int waitmsec)
        {
            if (waitmsec != 0)
            {
                long sleep = waitmsec;
                while ((sleep > 0 || waitmsec == Timeout.Infinite) && !_terminated && worker.ActiveThread != null)
                {
                    PassiveSleep(100);
                    sleep -= 100;
                }
            }
        }

        /// <summary>
        /// Joins the specified worker.
        /// </summary>
        /// <param name="worker">The worker.</param>
        public void Join(IBaseWorker worker)
        {
            Join(worker, Timeout.Infinite);
        }

        /// <summary>
        /// Terminates this instance.
        /// </summary>
        public new void Terminate()
        {
            base.Terminate();
            TerminateWorker(this);
        }

        /// <summary>
        /// Gets the worker.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        protected IBaseWorker GetWorker(string name)
        {
            return WorkPackage.WorkManager.GetWorker(name);
        }
    }
}
