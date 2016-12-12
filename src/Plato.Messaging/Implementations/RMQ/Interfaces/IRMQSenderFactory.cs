// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Messaging.Interfaces;

namespace Plato.Messaging.Implementations.RMQ.Interfaces
{
    public interface IRMQSenderFactory
    {
        /// <summary>
        /// Creates the specified settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns></returns>
        IMessageSender<byte[]> Create(IMessageSettings settings);
        /// <summary>
        /// Creates the text.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns></returns>
        IMessageSender<string> CreateText(IMessageSettings settings);
    }
}
