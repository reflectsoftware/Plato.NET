// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System;

namespace Plato.Threading
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Threading.BaseThread" />
    public class DelegateThread : BaseThread
    {
        private readonly object _onThreadHandlerLock;
        private Action _delegateOnWork;
        private Action _delegateOnInitializeThread;
        private Action _delegateOnUninitializeThread;
        private Action _delegateOnStartedThread;
        private Action _delegateOnStoppedThread;
        private Action _delegateOnStarting;
        private Action _delegateOnStarted;
        private Action _delegateOnStopping;
        private Action _delegateOnStopped;
        private Action _delegateOnDisposed;
        private Action<int> _delegateOnPassiveSleep;

        /// <summary>
        /// Gets or sets the work sleep value.
        /// </summary>
        /// <value>
        /// The work sleep value.
        /// </value>
        public int WorkSleepValue { get; set; }

        /// <summary>
        /// Gets or sets the wait on terminate value.
        /// </summary>
        /// <value>
        /// The wait on terminate value.
        /// </value>
        public int WaitOnTerminateValue { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateThread"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public DelegateThread(string name) : base(name)
        {
            _onThreadHandlerLock = new object();
            WorkSleepValue = 5000;
            WaitOnTerminateValue = 10000;

            DelegateOnWork = null;
            DelegateOnDispose = null;
            DelegateOnInitializeThread = null;
            DelegateOnUninitializeThread = null;
            DelegateOnStartedThread = null;
            DelegateOnStoppedThread = null;
            DelegateOnStarting = null;
            DelegateOnStarted = null;
            DelegateOnStopping = null;
            DelegateOnStopped = null;
            DelegateOnPassiveSleep = null;
        }

        /// <summary>
        /// Gets the work sleep value.
        /// </summary>
        /// <returns></returns>
        protected override int GetWorkSleepValue()
        {
            return WorkSleepValue;
        }

        /// <summary>
        /// Gets the wait on terminate thread value.
        /// </summary>
        /// <returns></returns>
        protected override int GetWaitOnTerminateThreadValue()
        {
            return WaitOnTerminateValue;
        }

        /// <summary>
        /// Called when [work].
        /// </summary>
        protected override void OnWork()
        {
            if (DelegateOnWork != null)
            {
                DelegateOnWork();
            }
        }

        /// <summary>
        /// Passives the sleep.
        /// </summary>
        /// <param name="msecSleep">The msec sleep.</param>
        protected override void PassiveSleep(int msecSleep)
        {
            if (DelegateOnPassiveSleep != null)
            {
                DelegateOnPassiveSleep(msecSleep);
            }
            else
            {
                base.PassiveSleep(msecSleep);
            }
        }

        /// <summary>
        /// Called when [initialize thread].
        /// </summary>
        protected override void OnInitializeThread()
        {
            if (DelegateOnInitializeThread != null)
            {
                DelegateOnInitializeThread();
            }
        }

        /// <summary>
        /// Called when [uninitialize thread].
        /// </summary>
        protected override void OnUninitializeThread()
        {
            if (DelegateOnUninitializeThread != null)
            {
                DelegateOnUninitializeThread();
            }
        }

        /// <summary>
        /// Called when [started thread].
        /// </summary>
        protected override void OnStartedThread()
        {
            if (DelegateOnStartedThread != null)
            {
                DelegateOnStartedThread();
            }
        }

        /// <summary>
        /// Called when [stopped thread].
        /// </summary>
        protected override void OnStoppedThread()
        {
            if (DelegateOnStoppedThread != null)
            {
                DelegateOnStoppedThread();
            }
        }

        /// <summary>
        /// Called when [starting].
        /// </summary>
        protected override void OnStarting()
        {
            if (DelegateOnStarting != null)
            {
                DelegateOnStarting();
            }
        }

        /// <summary>
        /// Called when [started].
        /// </summary>
        protected override void OnStarted()
        {
            if (DelegateOnStarted != null)
            {
                DelegateOnStarted();
            }
        }

        /// <summary>
        /// Called when [stopping].
        /// </summary>
        protected override void OnStopping()
        {
            if (DelegateOnStopping != null)
            {
                DelegateOnStopping();
            }
        }

        /// <summary>
        /// Called when [stopped].
        /// </summary>
        protected override void OnStopped()
        {
            if (DelegateOnStopped != null)
            {
                DelegateOnStopped();
            }
        }

        /// <summary>
        /// Called when [dispose].
        /// </summary>
        protected override void OnDispose()
        {
            if (DelegateOnDispose != null)
            {
                DelegateOnDispose();
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is running.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is running; otherwise, <c>false</c>.
        /// </value>
        public bool IsRunning
        {
            get
            {
                return ActiveThread != null;
            }
        }

        /// <summary>
        /// Gets or sets the delegate on work.
        /// </summary>
        /// <value>
        /// The delegate on work.
        /// </value>
        public Action DelegateOnWork
        {
            get
            {
                lock (_onThreadHandlerLock)
                {
                    return _delegateOnWork;
                }
            }
            set
            {
                lock (_onThreadHandlerLock)
                {
                    _delegateOnWork = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the delegate on initialize thread.
        /// </summary>
        /// <value>
        /// The delegate on initialize thread.
        /// </value>
        public Action DelegateOnInitializeThread
        {
            get
            {
                lock (_onThreadHandlerLock)
                {
                    return _delegateOnInitializeThread;
                }
            }
            set
            {
                lock (_onThreadHandlerLock)
                {
                    _delegateOnInitializeThread = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the delegate on uninitialize thread.
        /// </summary>
        /// <value>
        /// The delegate on uninitialize thread.
        /// </value>
        public Action DelegateOnUninitializeThread
        {
            get
            {
                lock (_onThreadHandlerLock)
                {
                    return _delegateOnUninitializeThread;
                }
            }
            set
            {
                lock (_onThreadHandlerLock)
                {
                    _delegateOnUninitializeThread = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the delegate on started thread.
        /// </summary>
        /// <value>
        /// The delegate on started thread.
        /// </value>
        public Action DelegateOnStartedThread
        {
            get
            {
                lock (_onThreadHandlerLock)
                {
                    return _delegateOnStartedThread;
                }
            }
            set
            {
                lock (_onThreadHandlerLock)
                {
                    _delegateOnStartedThread = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the delegate on stopped thread.
        /// </summary>
        /// <value>
        /// The delegate on stopped thread.
        /// </value>
        public Action DelegateOnStoppedThread
        {
            get
            {
                lock (_onThreadHandlerLock)
                {
                    return _delegateOnStoppedThread;
                }
            }
            set
            {
                lock (_onThreadHandlerLock)
                {
                    _delegateOnStoppedThread = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the delegate on starting.
        /// </summary>
        /// <value>
        /// The delegate on starting.
        /// </value>
        public Action DelegateOnStarting
        {
            get
            {
                lock (_onThreadHandlerLock)
                {
                    return _delegateOnStarting;
                }
            }
            set
            {
                lock (_onThreadHandlerLock)
                {
                    _delegateOnStarting = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the delegate on started.
        /// </summary>
        /// <value>
        /// The delegate on started.
        /// </value>
        public Action DelegateOnStarted
        {
            get
            {
                lock (_onThreadHandlerLock)
                {
                    return _delegateOnStarted;
                }
            }
            set
            {
                lock (_onThreadHandlerLock)
                {
                    _delegateOnStarted = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the delegate on stopping.
        /// </summary>
        /// <value>
        /// The delegate on stopping.
        /// </value>
        public Action DelegateOnStopping
        {
            get
            {
                lock (_onThreadHandlerLock)
                {
                    return _delegateOnStopping;
                }
            }
            set
            {
                lock (_onThreadHandlerLock)
                {
                    _delegateOnStopping = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the delegate on stopped.
        /// </summary>
        /// <value>
        /// The delegate on stopped.
        /// </value>
        public Action DelegateOnStopped
        {
            get
            {
                lock (_onThreadHandlerLock)
                {
                    return _delegateOnStopped;
                }
            }
            set
            {
                lock (_onThreadHandlerLock)
                {
                    _delegateOnStopped = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the delegate on dispose.
        /// </summary>
        /// <value>
        /// The delegate on dispose.
        /// </value>
        public Action DelegateOnDispose
        {
            get
            {
                lock (_onThreadHandlerLock)
                {
                    return _delegateOnDisposed;
                }
            }
            set
            {
                lock (_onThreadHandlerLock)
                {
                    _delegateOnDisposed = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the delegate on passive sleep.
        /// </summary>
        /// <value>
        /// The delegate on passive sleep.
        /// </value>
        public Action<int> DelegateOnPassiveSleep
        {
            get
            {
                lock (_onThreadHandlerLock)
                {
                    return _delegateOnPassiveSleep;
                }
            }
            set
            {
                lock (_onThreadHandlerLock)
                {
                    _delegateOnPassiveSleep = value;
                }
            }
        }
    }
}
