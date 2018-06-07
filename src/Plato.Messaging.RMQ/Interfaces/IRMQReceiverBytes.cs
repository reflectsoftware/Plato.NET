// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System.Threading;

namespace Plato.Messaging.RMQ.Interfaces
{
    public interface IRMQReceiverBytes
    {
        RMQReceiverResultByte Receive(int msecTimeout = Timeout.Infinite);
    }
}
