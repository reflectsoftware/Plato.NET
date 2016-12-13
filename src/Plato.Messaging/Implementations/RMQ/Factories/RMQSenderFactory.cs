// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Messaging.Interfaces;
using Plato.Messaging.Implementations.RMQ.Settings;
using Plato.Messaging.Implementations.RMQ.Interfaces;

namespace Plato.Messaging.Implementations.RMQ.Factories
{
    public class RMQSenderFactory : IRMQSenderFactory
    {
        /// <summary>
        /// Creates the specified settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns></returns>
        public IMessageSender<byte[]> Create(IMessageSettings settings)
        {
            return new RMQSender((settings as RMQSettings));
        }

        /// <summary>
        /// Creates the text.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns></returns>
        public IMessageSender<string> CreateText(IMessageSettings settings)
        {
            return new RMQSenderText((settings as RMQSettings));
        }
    }
}
