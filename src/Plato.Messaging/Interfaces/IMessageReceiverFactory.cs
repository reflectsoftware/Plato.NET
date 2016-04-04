// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System.Collections.Generic;

namespace Plato.Messaging.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMessageReceiverFactory<TData>
    {
        /// <summary>
        /// Creates the specified settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="routingKeys">The routing keys.</param>
        /// <returns></returns>
        IMessageReceiver<TData> Create(IMessageSettings settings, IEnumerable<string> routingKeys = null);

        /// <summary>
        /// Creates the specified settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="routingKey">The routing key.</param>
        /// <returns></returns>
        IMessageReceiver<TData> Create(IMessageSettings settings, string routingKey = "");
    }
}
