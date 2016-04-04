// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Messaging.Interfaces;
using Plato.Messaging.Implementations.RMQ.Settings;

/// <summary>
/// 
/// </summary>
namespace Plato.Messaging.Implementations.RMQ.Factories
{
    public class RMQRPCResponderFactory : IMessageRPCResponderFactory<byte[]>
    {
        /// <summary>
        /// Creates the specified settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns></returns>
        public IMessageRPCResponder<byte[]> Create(IMessageSettings settings)
        {
            return new RMQRPCResponder((settings as RMQSettings));
        }
    }
}
