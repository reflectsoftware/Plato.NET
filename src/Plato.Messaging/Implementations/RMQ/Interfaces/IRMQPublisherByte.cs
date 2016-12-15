// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Messaging.Interfaces;

namespace Plato.Messaging.Implementations.RMQ.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Messaging.Interfaces.IMessageSender{System.Byte[]}" />
    /// <seealso cref="Plato.Messaging.Implementations.RMQ.Interfaces.IRMQPublisher" />
    public interface IRMQPublisherByte : IMessageSender<byte[]>, IRMQPublisher
    {
    }
}
