// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Messaging.Interfaces;
using Plato.Messaging.Implementations.RMQ.Settings;
using System;
using System.Text;
using Plato.Messaging.Implementations.RMQ.Interfaces;

namespace Plato.Messaging.Implementations.RMQ
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Messaging.Implementations.RMQ.RMQSenderBase" />
    /// <seealso cref="Plato.Messaging.Implementations.RMQ.Interfaces.IRMQSenderText" />
    public class RMQSenderText : RMQSenderBase, IRMQSenderText
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RMQSenderText"/> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        public RMQSenderText(RMQSettings settings) : base(settings)
        {
        }

        /// <summary>
        /// Sends the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="action">The action.</param>
        public void Send(string data, Action<ISenderProperties> action = null)
        {
            var bdata = Encoding.UTF8.GetBytes(data);
            _Send(bdata, action);
        }
    }
}
