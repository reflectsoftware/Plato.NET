// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using RabbitMQ.Client;
using System;
using System.Collections.Generic;

namespace Plato.Messaging.RMQ.Settings
{
    /// <summary>
    /// 
    /// </summary>
    public class RMQConnectionSettings
    {
        /// <summary>
        /// Gets the index of the active endpoint.
        /// </summary>
        /// <value>
        /// The index of the active endpoint.
        /// </value>
        public int ActiveEndpointIndex { get; internal set; }

        /// <summary>
        /// Gets the endpoints.
        /// </summary>
        /// <value>
        /// The endpoints.
        /// </value>
        public List<string> Endpoints { get; private set; }

        public RMQConnectionSettings()
        {            
            Name = "default";            
            ForceReconnectionTime = TimeSpan.Zero;
            DelayOnReconnect = 1000;
            Protocol = Protocols.DefaultProtocol;
            Endpoints = new List<string>();
            ActiveEndpointIndex = 0;
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }


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
        /// Gets or sets the virtual host.
        /// </summary>
        /// <value>
        /// The virtual host.
        /// </value>
        public string VirtualHost { get; set; }

        /// <summary>
        /// Gets or sets the URI.
        /// </summary>
        /// <value>
        /// The URI.
        /// </value>
        public string Uri { get; set; }

        /// <summary>
        /// Gets or sets the protocol.
        /// </summary>
        /// <value>
        /// The protocol.
        /// </value>
        public IProtocol Protocol { get; set; }

        /// <summary>
        /// Gets or sets the delay on reconnect.
        /// </summary>
        /// <value>
        /// The delay on reconnect.
        /// </value>
        public int DelayOnReconnect { get; set; }

        /// <summary>
        /// Gets or sets the reestablish connection.
        /// </summary>
        /// <value>
        /// The reestablish connection.
        /// </value>
        public TimeSpan ForceReconnectionTime { get; set; }
    }
}
