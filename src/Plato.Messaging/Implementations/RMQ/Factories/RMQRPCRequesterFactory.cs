// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Messaging.Interfaces;
using Plato.Messaging.Implementations.RMQ.Settings;

namespace Plato.Messaging.Implementations.RMQ.Factories
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Messaging.Interfaces.IMessageRPCRequesterFactory" />
    public class RMQRPCRequesterFactory : IMessageRPCRequesterFactory<byte[]>
    {
        /// <summary>
        /// Creates the specified settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns></returns>
        public IMessageRPCRequester<byte[]> Create(IMessageSettings settings)
        {
            return new RMQRPCRequester((settings as RMQSettings));
        }
    }
}
