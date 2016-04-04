// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System;
using System.Threading;

namespace Plato.Utils.Miscellaneous
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="System.IDisposable"/>
    public class DelayEvent : IDisposable
    {
        private readonly Action _onDelayEventBeginHandler;
        private Action _onDelayEventEndHandler;
        private AutoResetEvent _autoEventBegin;
        private AutoResetEvent _autoEventEnd;

        private bool _terminated;
        private Thread _delayEventThread;
        private bool _hasBegun;

        /// <summary>
        /// Gets a value indicating whether this <see cref="DelayEvent"/> is disposed.
        /// </summary>
        /// <value>
        /// <c>true</c> if disposed; otherwise, <c>false</c>.
        /// </value>
        public bool Disposed { get; private set; }

        /// <summary>
        /// Gets or sets the delay.
        /// </summary>
        /// <value>
        /// The delay.
        /// </value>
        public int Delay { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DelayEvent"/> class.
        /// </summary>
        /// <param name="delay">The delay.</param>
        /// <param name="startHandler">The start handler.</param>
        /// <param name="endHandler">The end handler.</param>
        public DelayEvent(int delay, Action startHandler, Action endHandler)
        {
            Disposed = false;
            Delay = delay;
            _terminated = true;            
            _delayEventThread = null;
            _hasBegun = false;
            _onDelayEventBeginHandler = startHandler;
            _onDelayEventEndHandler = endHandler;
            _autoEventBegin = new AutoResetEvent(false);
            _autoEventEnd = new AutoResetEvent(false);
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

                    _onDelayEventEndHandler = null;
                    Stop();

                    _autoEventBegin?.Close();
                    _autoEventBegin?.Dispose();
                    _autoEventBegin = null;

                    _autoEventEnd?.Close();
                    _autoEventEnd?.Dispose();
                    _autoEventEnd = null;
                }
            }
        }

        /// <summary>
        /// Delays the event thread.
        /// </summary>
        private void DelayEventThread()
        {
            while (!_terminated)
            {
                if (!_autoEventEnd.WaitOne(100))
                {
                    continue;
                }

                while (_autoEventBegin.WaitOne(Delay))
                {
                    Thread.Sleep(100);
                }

                DoOnDelayEventEndHandler();
            }
        }

        /// <summary>
        /// Does the on delay event begin handler.
        /// </summary>
        private void DoOnDelayEventBeginHandler()
        {
            lock (this)
            {
                if (_onDelayEventBeginHandler != null)
                {
                    _onDelayEventBeginHandler();
                }
            }
        }

        /// <summary>
        /// Does the on delay event end handler.
        /// </summary>
        private void DoOnDelayEventEndHandler()
        {
            lock (this)
            {
                if (_onDelayEventEndHandler != null)
                {
                    _onDelayEventEndHandler();
                }

                _autoEventEnd.Reset();
                _hasBegun = false;
            }
        }

        /// <summary>
        /// Begins the event.
        /// </summary>
        public void BeginEvent()
        {
            lock (this)
            {
                if (!_hasBegun)
                {
                    DoOnDelayEventBeginHandler();
                    _hasBegun = true;
                }

                _autoEventBegin?.Set();
            }
        }

        /// <summary>
        /// Ends the event.
        /// </summary>
        public void EndEvent()
        {
            lock (this)
            {
                _autoEventEnd?.Set();
            }
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Start()
        {
            if (_terminated)
            {
                _terminated = false;
                _hasBegun = false;
                _delayEventThread = new Thread(DelayEventThread) { IsBackground = true };
                _delayEventThread.Start();
            }
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void Stop()
        {
            _terminated = true;
            _delayEventThread?.Join();
            _delayEventThread = null;
        }
    }
}
