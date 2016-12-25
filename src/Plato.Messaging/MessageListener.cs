// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato;
using Plato.Messaging.Interfaces;
using Plato.Core.Logging.Interfaces;
using System;
using Plato.Threading;

namespace Plato.Messaging
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    /// <seealso cref="Plato.Threading.BaseThread" />
    public class MessageListener<TData> : BaseThread
    {
        private readonly IMessageReceiver<TData> _receiver;

        /// <summary>
        /// Gets or sets the on message received.
        /// </summary>
        /// <value>
        /// The on message received.
        /// </value>
        public Action<IMessageReceiveResult<TData>> OnMessageReceived { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageListener{TData}"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="receiver">The receiver.</param>
        /// <param name="notification">The notification.</param>
        public MessageListener(string name, IMessageReceiver<TData> receiver, ILogNotification notification = null) : base(name, notification)
        {
            _receiver = receiver;
        }

        /// <summary>
        /// Gets the work sleep value.
        /// </summary>
        /// <returns></returns>
        protected override int GetWorkSleepValue()
        {
            return 0;
        }

        /// <summary>
        /// Called when [starting].
        /// </summary>
        /// <exception cref="System.MissingMethodException">Must implemented MessageListener.OnMessageReceived action method before starting message listener.</exception>
        protected override void OnStarting() 
        {
            if (OnMessageReceived == null)
            {
                throw new MissingMethodException("Must implemented MessageListener.OnMessageReceived action method before starting message listener.");
            }

            _receiver.Open();
        }

        /// <summary>
        /// Called when [stopped].
        /// </summary>
        protected override void OnStopped()
        {
            _receiver.Close();
        }

        /// <summary>
        /// Called when [work].
        /// </summary>
        protected override void OnWork()
        {
            try
            {
                var result = _receiver.Receive(1000);
                OnMessageReceived(result);
            }
            catch (TimeoutException)
            {
            }
        }
    }
}
