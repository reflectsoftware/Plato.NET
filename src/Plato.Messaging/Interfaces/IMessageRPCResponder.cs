// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

namespace Plato.Messaging.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Messaging.Interfaces.IMessageReceiverSender" />
    public interface IMessageRPCResponder<TData> : IMessageReceiverSender
    {
        /// <summary>
        /// Responds the specified result.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <param name="data">The data.</param>
        void Respond(IMessageReceiveResult<TData> result, TData data);
    }
}
