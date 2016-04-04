// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Apache.NMS;
using Plato.Messaging.Interfaces;

namespace Plato.Messaging.Implementations.AMQ.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Messaging.Interfaces.IMessageConnectionFactory{Apache.NMS.IConnection}" />
    public interface IAMQConnectionFactory : IMessageConnectionFactory<IConnection>
    {
    }
}
