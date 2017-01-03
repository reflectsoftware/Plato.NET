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
        /// <param name="connectionSettings">The connection settings.</param>
        /// <param name="destinationSettings">The destination settings.</param>
        /// <returns></returns>
        IAMQSender Create(AMQConnectionSettings connectionSettings, AMQDestinationSettings destinationSettings);

        /// <summary>
        /// Creates the text.
        /// </summary>
        /// <param name="connectionSettings">The connection settings.</param>
        /// <param name="destinationSettings">The destination settings.</param>
        /// <returns></returns>
        IAMQSenderText CreateText(AMQConnectionSettings connectionSettings, AMQDestinationSettings destinationSettings);

        /// <summary>
        /// Creates the bytes.
        /// </summary>
        /// <param name="connectionSettings">The connection settings.</param>
        /// <param name="destinationSettings">The destination settings.</param>
        /// <returns></returns>
        IAMQSenderBytes CreateBytes(AMQConnectionSettings connectionSettings, AMQDestinationSettings destinationSettings);
    }
}
