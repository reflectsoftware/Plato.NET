// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Apache.NMS;
using Plato.Messaging.Interfaces;
using System;
using System.Collections.Generic;

namespace Plato.Messaging.Implementations.AMQ
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Messaging.Interfaces.IMessageReceiveResult" />
    public class AMQReceiverResult : IMessageReceiveResult<string>
    {
        private readonly ITextMessage _message;

        /// <summary>
        /// Gets the headers.
        /// </summary>
        /// <value>
        /// The headers.
        /// </value>
        public IDictionary<string, object> Headers { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance has acknowledged.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has acknowledged; otherwise, <c>false</c>.
        /// </value>
        public bool HasAcknowledged { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AMQReceiverResult"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        internal AMQReceiverResult(ITextMessage message)
        {
            _message = message;

            HasAcknowledged = false;            
            Headers = new Dictionary<string, object>();

            foreach(string key in _message.Properties.Keys)
            {
                Headers[key] = _message.Properties[key];
            }
        }
             
        /// <summary>
        /// Gets the header.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public T GetHeader<T>(string key, T defaultValue = default(T))
        {
            return Headers != null && Headers.ContainsKey(key) ? (T)Headers[key] : defaultValue;
        }

        /// <summary>
        /// Gets the message identifier.
        /// </summary>
        /// <value>
        /// The message identifier.
        /// </value>
        public string MessageId
        {
            get
            {
                return _message.NMSMessageId;
            }
        }

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        public string Data
        {
            get
            {
                return _message.Text;
            }
        }

        /// <summary>
        /// Acknowledges this instance.
        /// </summary>
        public void Acknowledge()
        {
            if (!HasAcknowledged)
            {
                _message.Acknowledge();
                HasAcknowledged = true;
            }
        }

        /// <summary>
        /// Rejects the specified requeue.
        /// </summary>
        /// <param name="requeue">if set to <c>true</c> [requeue].</param>
        public void Reject(bool requeue = false)
        {
            throw new NotSupportedException();
        }
    }
}
