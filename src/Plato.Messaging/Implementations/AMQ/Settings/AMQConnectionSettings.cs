// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

namespace Plato.Messaging.Implementations.AMQ.Settings
{
    /// <summary>
    /// 
    /// </summary>
    public class AMQConnectionSettings
    {
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the name of the host.
        /// </summary>
        /// <value>
        /// The name of the host.
        /// </value>
        public string Uri { get; set; }

        /// <summary>
        /// Gets or sets the asynchronous send.
        /// </summary>
        /// <value>
        /// The asynchronous send.
        /// </value>
        public bool AsyncSend { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        /// <value>
        /// The username.
        /// </value>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the delay on reconnect.
        /// </summary>
        /// <value>
        /// The delay on reconnect.
        /// </value>
        public int DelayOnReconnect { get; set; }
    } 
}
