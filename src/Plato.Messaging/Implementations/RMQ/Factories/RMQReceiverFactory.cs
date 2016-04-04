// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Messaging.Interfaces;
using Plato.Messaging.Implementations.RMQ.Settings;
using System.Collections.Generic;

namespace Plato.Messaging.Implementations.RMQ.Factories
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Messaging.Interfaces.IMessageReceiverFactory" />
    public class RMQReceiverFactory : IMessageReceiverFactory<byte[]>
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
    }
}
