// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Threading.Enums;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Plato.Threading.WorkManagement
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Threading.BaseThread" />
    internal class MessageManager : BaseThread
    {
        private readonly WorkManager _workManager;
        private readonly List<MessageManagerInfo> _messageQueue;
        
        /// <summary>
        /// The _message states
        /// </summary>
        public MessageState[] _messageStates;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageManager"/> class.
        /// </summary>
        /// <param name="manager">The manager.</param>
        public MessageManager(WorkManager manager) : base("Plato.Threading.WorkManagement.MessageManager")
        {
            _workManager = manager;
            _messageQueue = new List<MessageManagerInfo>();
            _messageStates = new MessageState[Enum.GetNames(typeof(MessageManagerId)).Length];
        }

        /// <summary>
        /// Called when [initialize thread].
        /// </summary>
        protected override void OnInitializeThread()
        {
            base.OnInitializeThread();
            SetAllMessagesToState(MessageState.Allow);
        }

        /// <summary>
        /// Gets the work sleep value.
        /// </summary>
        /// <returns></returns>
        protected override int GetWorkSleepValue()
        {
            return 250;
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
        /// Called when [dispose].
        /// </summary>
        protected override void OnDispose()
        {
            lock (_messageQueue)
            {
                foreach (var msg in _messageQueue)
                {
                    msg.Dispose();
                }
                _messageQueue.Clear();
                _messageQueue.Capacity = 0;
            }
        }

        /// <summary>
        /// Called when [work].
        /// </summary>
        protected override void OnWork()
        {
            var messages = (MessageManagerInfo[])null;
            lock (_messageQueue)
            {
                messages = _messageQueue.ToArray();
            }

            if (_messageQueue.Count == 0)
            {
                return;
            }

            foreach (var msg in messages)
            {
                if (_messageStates[msg.MessageId.GetHashCode()] == MessageState.Allow)
                {
                    _workManager.ReceivedMQMessage(msg);
                }

                lock (_messageQueue)
                {
                    _messageQueue.Remove(msg);
                }
            }
        }

        /// <summary>
        /// Sends the messages.
        /// </summary>
        /// <param name="messages">The messages.</param>
        public void SendMessages(List<MessageManagerInfo> messages)
        {
            lock (_messageQueue)
            {
                _messageQueue.AddRange(messages);
            }
        }

        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="mInfo">The m information.</param>
        public void SendMessage(MessageManagerInfo mInfo)
        {
            lock (_messageQueue)
            {
                _messageQueue.Add(mInfo);
            }
        }

        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="mid">The mid.</param>
        /// <param name="mData">The m data.</param>
        public void SendMessage(MessageManagerId mid, object mData)
        {
            SendMessage(new MessageManagerInfo(mid, mData));
        }

        /// <summary>
        /// Sets the state of the message.
        /// </summary>
        /// <param name="mid">The mid.</param>
        /// <param name="state">The state.</param>
        public void SetMessageState(MessageManagerId mid, MessageState state)
        {
            lock (_messageQueue)
            {
                _messageStates[mid.GetHashCode()] = state;
            }
        }

        /// <summary>
        /// Sets the state of all messages to.
        /// </summary>
        /// <param name="state">The state.</param>
        public void SetAllMessagesToState(MessageState state)
        {
            lock (_messageQueue)
            {
                for (var i = 0; i < _messageStates.Length; i++)
                {
                    _messageStates[i] = state;
                }
            }
        }

        /// <summary>
        /// Gets the message count.
        /// </summary>
        /// <value>
        /// The message count.
        /// </value>
        public int MessageCount
        {
            get
            {
                lock (_messageQueue)
                {
                    return _messageQueue.Count;
                }
            }
        }

        /// <summary>
        /// Waits the on empty queue.
        /// </summary>
        public void WaitOnEmptyQueue()
        {
            while (true)
            {
                if (MessageCount == 0)
                {
                    break;
                }

                Thread.Sleep(100);
            }
        }
    }
}
