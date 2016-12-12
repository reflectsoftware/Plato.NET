// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Messaging.Interfaces;
using System.Collections.Generic;

namespace Plato.Messaging.Implementations.RMQ.Interfaces
{
    public interface IRMQReceiverFactory
    {
        /// <summary>
        /// Creates the specified settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="routingKeys">The routing keys.</param>
        /// <returns></returns>
        IMessageReceiver<byte[]> Create(IMessageSettings settings, IEnumerable<string> routingKeys = null);

        /// <summary>
        /// Creates the specified settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="routingKey">The routing key.</param>
        /// <returns></returns>
        IMessageReceiver<byte[]> Create(IMessageSettings settings, string routingKey = "");

        /// <summary>
        /// Creates the text.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="routingKeys">The routing keys.</param>
        /// <returns></returns>
        IMessageReceiver<string> CreateText(IMessageSettings settings, IEnumerable<string> routingKeys = null);

        /// <summary>
        /// Creates the text.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="routingKey">The routing key.</param>
        /// <returns></returns>
        IMessageReceiver<string> CreateText(IMessageSettings settings, string routingKey = "");
    }
}
