// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Apache.NMS;
using Plato.Messaging.Implementations.AMQ.Settings;

namespace Plato.Messaging.Implementations.AMQ.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Messaging.Interfaces.IMessageConnectionFactory{Apache.NMS.IConnection}" />
    public interface IAMQConnectionFactory 
    {
        IConnection CreateConnection(AMQConnectionSettings settings);
    }
}
