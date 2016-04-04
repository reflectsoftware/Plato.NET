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
        /// <param name="settings">The settings.</param>
        /// <returns></returns>
        IAMQSender Create(AMQDestinationSettings settings);
    }
}
