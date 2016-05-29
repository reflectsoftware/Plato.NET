// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Threading.Enums;
using Plato.Threading.Exceptions;
using Plato.Threading.Interfaces;
using Plato.Utils.Logging.Enums;
using Plato.Utils.Miscellaneous;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Plato.Threading.WorkManagement
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public class WorkManager : IDisposable
    {
        private readonly List<IBaseWorker> _workers;
        private readonly WorkManagerRegistry _workManagerRegistry;
        private ThreadWatcher _threadWatcher;
        private MessageManager _messageManager;
        private Notification _notification;

        /// <summary>
        /// Gets a value indicating whether this <see cref="WorkManager"/> is disposed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if disposed; otherwise, <c>false</c>.
        /// </value>
        public bool Disposed { get; private set; }

        /// <summary>
        /// Gets the state of the manager runtime.
        /// </summary>
        /// <value>
        /// The state of the manager runtime.
        /// </value>
        public ManagerRuntimeStates ManagerRuntimeState { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkManager"/> class.
        /// </summary>
        /// <param name="registry">The registry.</param>
        /// <param name="resolver">The resolver.</param>
        /// <param name="notifier">The notifier.</param>
        public WorkManager(WorkManagerRegistry registry, IWorkManagerDependencyResolver resolver, Notification notifier)
        {
            _threadWatcher = new ThreadWatcher(this);
            _messageManager = new MessageManager(this);
            _workers = new List<IBaseWorker>();

            _workManagerRegistry = registry;
            _workManagerRegistry.DependencyResolver = resolver;

            Disposed = false;
            ManagerRuntimeState = ManagerRuntimeStates.Stopped;
            Notification = notifier ?? new WorkManagerNotification();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkManager"/> class.
        /// </summary>
        /// <param name="registry">The registry.</param>
        /// <param name="resolver">The resolver.</param>
        public WorkManager(WorkManagerRegistry registry, IWorkManagerDependencyResolver resolver) : this(registry, resolver, null)
        {
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

                    _threadWatcher?.Dispose();
                    _messageManager?.Dispose();

                    if (_notification != null)
                    {
                        MiscHelper.DisposeObject(_notification);
                        _notification = null;
                    }
                }
            }
        }

        /// <summary>
        /// Adds the worker.
        /// </summary>
        /// <param name="worker">The worker.</param>
        private void AddWorker(IBaseWorker worker)
        {
            lock (_workers)
            {
                _workers.Add(worker);
            }
        }

        /// <summary>
        /// Removes the worker.
        /// </summary>
        /// <param name="worker">The worker.</param>
        private void RemoveWorker(IBaseWorker worker)
        {
            lock (_workers)
            {
                _workers.Remove(worker);
            }
        }

        /// <summary>
        /// Receiveds the mq message.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        internal void ReceivedMQMessage(MessageManagerInfo msg)
        {
            try
            {
                if (msg.BeginEvent != null)
                {
                    msg.BeginEvent();
                }

                switch (msg.MessageId)
                {
                    case MessageManagerId.SpawnWorker:
                        MQSpawnWorker(msg);
                        break;

                    case MessageManagerId.TerminateWorker:
                        MQTerminateWorker(msg);
                        break;

                    case MessageManagerId.WorkerFailedToRespond:
                        MQRemoveNonResponsiveWorker(msg);
                        break;
                }

                if (msg.EndEvent != null)
                {
                    msg.EndEvent();
                }
            }
            catch (Exception ex)
            {
                Notification.SendException(ex, true);
            }
        }

        /// <summary>
        /// Mqs the remove non responsive worker.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        private void MQRemoveNonResponsiveWorker(MessageManagerInfo msg)
        {
            var threadInfo = (ThreadWatcherInfo)msg.MessageData;
            var worker = threadInfo.Worker;
            if (worker == null)
            {
                return;
            }

            var restart = worker.WorkPackage.Parameters("restartNonResponsive") == "true";
            var workerName = worker.WorkPackage.Name;
            var instanceName = worker.WorkPackage.NameInstance;

            Notification.SendException(new Exception(string.Format("The following thread: '{0}' failed to respond.", worker.WorkPackage.NameInstance)), true);

            var mInfoTerminate = new MessageManagerInfo(MessageManagerId.TerminateWorker, worker)
            {
                EndEvent = () =>
                {
                    if (restart && ManagerRuntimeState == ManagerRuntimeStates.Started)
                    {
                        worker = CreateWorker(workerName, instanceName, worker.WorkPackage.GetData<object>());
                        var mInfo = new MessageManagerInfo(MessageManagerId.SpawnWorker, worker)
                        {
                            BeginEvent = () =>
                            {
                                Notification.SendMessage(string.Format("Restarting worker: {0}", worker.WorkPackage.NameInstance), NotificationType.Information);
                            }
                        };

                        _messageManager.SendMessage(mInfo);
                    }
                }
            };

            _messageManager.SendMessage(mInfoTerminate);
        }

        /// <summary>
        /// Mqs the spawn worker.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        private void MQSpawnWorker(MessageManagerInfo msg)
        {
            var worker = (IBaseWorker)msg.MessageData;
            AddWorker(worker);
            worker.Start(true);
        }

        private void MQTerminateWorker(MessageManagerInfo msg)
        {
            var worker = (IBaseWorker)msg.MessageData;
            try
            {
                if (worker != null)
                {
                    worker.Stop();
                    DisposeWorker(worker);
                    RemoveWorker(worker);
                }
            }
            catch (WorkManagerException ex)
            {
                Notification.SendException(ex);
            }
            catch (Exception ex)
            {
                var eMsg = string.Format("An unhandled exception was detected while Terminating Worker '{0}'", worker.Name);
                Notification.SendException(new WorkManagerException(eMsg, ex), true);
            }
        }

        /// <summary>
        /// Creates the worker.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="nameInstance">The name instance.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        internal IBaseWorker CreateWorker(string name, string nameInstance, object data = null)
        {
            var package = _workManagerRegistry.GetWorker(name);
            if (package == null)
            {
                return null;
            }

            return _workManagerRegistry.Resolve(name, new WorkPackage(name, nameInstance, this, _threadWatcher, package.GetParameters(), data));
        }

        /// <summary>
        /// Spawns the worker.
        /// </summary>
        /// <param name="worker">The worker.</param>
        /// <returns></returns>
        internal IBaseWorker SpawnWorker(IBaseWorker worker)
        {
            _messageManager.SendMessage(MessageManagerId.SpawnWorker, worker);
            return worker;
        }

        /// <summary>
        /// Spawns the worker.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="nameInstance">The name instance.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        internal IBaseWorker SpawnWorker(string name, string nameInstance, object data)
        {
            var worker = CreateWorker(name, nameInstance, data);
            return SpawnWorker(worker);
        }

        /// <summary>
        /// Terminates the worker.
        /// </summary>
        /// <param name="worker">The worker.</param>
        internal void TerminateWorker(IBaseWorker worker)
        {
            _messageManager.SendMessage(MessageManagerId.TerminateWorker, worker);
        }

        /// <summary>
        /// Terminates the worker.
        /// </summary>
        /// <param name="name">The name.</param>
        internal void TerminateWorker(string name)
        {
            var worker = GetWorker(name);
            if (worker != null)
            {
                _messageManager.SendMessage(MessageManagerId.TerminateWorker, worker);
            }
        }

        /// <summary>
        /// Removes the non responsive worker.
        /// </summary>
        /// <param name="tInfo">The t information.</param>
        internal void RemoveNonResponsiveWorker(ThreadWatcherInfo tInfo)
        {
            _messageManager.SendMessage(MessageManagerId.WorkerFailedToRespond, tInfo);
        }

        /// <summary>
        /// Gets the worker.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        internal IBaseWorker GetWorker(string name)
        {
            lock (_workers)
            {
                foreach (IBaseWorker worker in _workers)
                {
                    if (worker.Name == name)
                    {
                        return worker;
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// Loads the and start workers.
        /// </summary>
        /// <exception cref="WorkManagerException">
        /// </exception>
        private void LoadAndStartWorkers()
        {
            try
            {
                var messages = new List<MessageManagerInfo>();

                try
                {
                    foreach (WorkerRegisteryPackage package in _workManagerRegistry.GetAllActiveWorkers())
                    {
                        var instanceName = package.Name;
                        var instances = int.Parse(package.Parameters("instances", "1"));
                        if (instances <= 0)
                        {
                            throw new WorkManagerException(string.Format("Worker: '{0}' 'instances' setting must be greater than 0. Please check the application configuration file.", instanceName));
                        }

                        for (var i = 0; i < instances; i++)
                        {
                            var nameCount = instanceName;
                            if (instances > 1)
                            {
                                nameCount = string.Format("{0}.{1}", instanceName, i + 1);
                            }

                            var worker = CreateWorker(instanceName, nameCount, null);
                            if (worker == null)
                            {
                                throw new WorkManagerException(string.Format("Unable to load worker: '{0}'. Please check the application configuration file.", nameCount));
                            }

                            messages.Add(new MessageManagerInfo(MessageManagerId.SpawnWorker, worker));
                        }
                    }

                    if (messages.Count == 0)
                    {
                        Notification.SendMessage("No Workers have been loaded.", NotificationType.Warning);
                    }
                }
                catch (Exception)
                {
                    foreach (MessageManagerInfo message in messages)
                    {
                        message.Dispose();
                    }

                    throw;
                }

                _messageManager.SendMessages(messages);
            }
            catch (Exception ex)
            {
                Notification.SendException(ex, true);
            }
        }

        /// <summary>
        /// Disposes the worker.
        /// </summary>
        /// <param name="worker">The worker.</param>
        private void DisposeWorker(IBaseWorker worker)
        {
            try
            {
                if (worker != null)
                {
                    worker.Dispose();
                }
            }
            catch (Exception ex)
            {
                var eMsg = string.Format("An unhandled exception was detected disposing Worker '{0}'", worker.Name);
                Notification.SendException(new WorkManagerException(eMsg, ex), true);
            }
        }

        /// <summary>
        /// Terminates the workers.
        /// </summary>
        private void TerminateWorkers()
        {
            var workers = (IBaseWorker[] )null;
            lock (_workers)
            {
                workers = new IBaseWorker[_workers.Count];
                _workers.CopyTo(workers);
            }
            
            foreach (var worker in workers)
            {
                (worker as BaseWorker).Terminate();
            }

            foreach (var worker in workers)
            {
                TerminateWorker(worker);
            }

            while (true)
            {
                lock (_workers)
                {
                    if (_workers.Count == 0)
                    {
                        break;
                    }
                }

                Thread.Sleep(100);
            }
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Start()
        {
            lock (this)
            {
                Notification.SendMessage("WorkingManager initiated startup sequence...", NotificationType.Information);
                try
                {
                    ManagerRuntimeState = ManagerRuntimeStates.Starting;
                    _messageManager.Start(true);
                    _threadWatcher.Start(true);
                    LoadAndStartWorkers();
                    ManagerRuntimeState = ManagerRuntimeStates.Started;
                }
                catch (Exception ex)
                {
                    try
                    {
                        Stop();
                        Notification.SendException(ex, true);
                    }
                    catch (Exception)
                    {
                    }

                    throw;
                }
                finally
                {
                    Notification.SendMessage("WorkingManager startup sequence completed.", NotificationType.Information);
                }
            }
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void Stop()
        {
            lock (this)
            {
                Notification.SendMessage("WorkingManager initiated shutdown sequence...", NotificationType.Information);
                try
                {
                    ManagerRuntimeState = ManagerRuntimeStates.Stopping;
                    _messageManager.SetAllMessagesToState(MessageState.Ignore);
                    _messageManager.SetMessageState(MessageManagerId.TerminateWorker, MessageState.Allow);

                    TerminateWorkers();
                    _threadWatcher.Stop();
                    _messageManager.Stop();
                }
                catch (Exception ex)
                {
                    try
                    {
                        Notification.SendException(ex, true );
                    }
                    catch (Exception)
                    {                        
                    }

                    throw;
                }
                finally
                {
                    ManagerRuntimeState = ManagerRuntimeStates.Stopped;
                    Notification.SendMessage("WorkingManager shutdown sequence completed.", NotificationType.Information);
                }
            }
        }

        /// <summary>
        /// Gets or sets the notification.
        /// </summary>
        /// <value>
        /// The notification.
        /// </value>
        public Notification Notification
        {
            get
            {
                lock (this)
                {
                    return _notification;
                }
            }
            set
            {
                lock (this)
                {
                    _notification = value;
                    _messageManager.Notification = value;
                    _threadWatcher.Notification = value;
                }
            }
        }
    }
}
