// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Threading.Enums;
using Plato.Utils.Miscellaneous;
using System;

namespace Plato.Threading.WorkManagement
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    internal class MessageManagerInfo : IDisposable
    {
        /// <summary>
        /// Gets or sets the message identifier.
        /// </summary>
        /// <value>
        /// The message identifier.
        /// </value>
        public MessageManagerId MessageId { get; set; }

        /// <summary>
        /// Gets or sets the message data.
        /// </summary>
        /// <value>
        /// The message data.
        /// </value>
        public object MessageData { get; set; }

        /// <summary>
        /// Gets or sets the begin event.
        /// </summary>
        /// <value>
        /// The begin event.
        /// </value>
        public Action BeginEvent { get; set; }

        /// <summary>
        /// Gets or sets the end event.
        /// </summary>
        /// <value>
        /// The end event.
        /// </value>
        public Action EndEvent { get; set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="MessageManagerInfo"/> is disposed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if disposed; otherwise, <c>false</c>.
        /// </value>
        public bool Disposed { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageManagerInfo"/> class.
        /// </summary>
        public MessageManagerInfo()
        {
            Disposed = false;
            BeginEvent = null;
            EndEvent = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageManagerInfo"/> class.
        /// </summary>
        /// <param name="mid">The mid.</param>
        /// <param name="mData">The m data.</param>
        public MessageManagerInfo(MessageManagerId mid, object mData) : this()
        {
            MessageId = mid;
            MessageData = mData;
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

                    if (MessageData != null)
                    {
                        MiscHelper.DisposeObject(MessageData);
                        MessageData = null;
                    }
                }
            }
        }
    }
}
