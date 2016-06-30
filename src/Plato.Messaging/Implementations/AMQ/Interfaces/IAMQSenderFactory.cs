// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Messaging.Implementations.AMQ.Settings;

namespace Plato.Messaging.Implementations.AMQ.Interfaces
{    
    /// <summary>
    /// 
    /// </summary>
    public interface IAMQSenderFactory
    {
        /// <summary>
        /// Creates the specified settings.
        /// </summary>
        /// <typeparam name="TData">The type of the data.</typeparam>
        /// <param name="settings">The settings.</param>
        /// <param name="connectionName">Name of the connection.</param>
        /// <returns></returns>
        IAMQSender<TData> Create<TData>(AMQDestinationSettings settings, string connectionName);
    }
}
