﻿// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Messaging.Interfaces;
using Plato.Messaging.Implementations.RMQ.Settings;
using System.Collections.Generic;
using Plato.Messaging.Implementations.RMQ.Interfaces;

namespace Plato.Messaging.Implementations.RMQ.Factories
{    
    public class RMQReceiverFactory : IRMQReceiverFactory
    {
        /// <summary>
        /// Creates the specified settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="routingKeys">The routing keys.</param>
        /// <returns></returns>
        public IMessageReceiver<byte[]> Create(IMessageSettings settings, IEnumerable<string> routingKeys = null)
        {
            return new RMQReceiver((settings as RMQSettings), routingKeys);
        }

        /// <summary>
        /// Creates the specified settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="routingKey">The routing key.</param>
        /// <returns></returns>
        public IMessageReceiver<byte[]> Create(IMessageSettings settings, string routingKey = "")
        {
            return new RMQReceiver((settings as RMQSettings), routingKey);
        }

        /// <summary>
        /// Creates the text.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="routingKeys">The routing keys.</param>
        /// <returns></returns>
        public IMessageReceiver<string> CreateText(IMessageSettings settings, IEnumerable<string> routingKeys = null)
        {
            return new RMQReceiverText((settings as RMQSettings), routingKeys);
        }

        /// <summary>
        /// Creates the text.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="routingKey">The routing key.</param>
        /// <returns></returns>
        public IMessageReceiver<string> CreateText(IMessageSettings settings, string routingKey = "")
        {
            return new RMQReceiverText((settings as RMQSettings), routingKey);
        }
    }
}
