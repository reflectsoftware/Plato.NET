// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System.Collections.Generic;

namespace Plato.Messaging.Interfaces
{

    public interface IMessageReceiveResult<TData>
    {
        /// <summary>
        /// Gets the message identifier.
        /// </summary>
        /// <value>
        /// The message identifier.
        /// </value>
        string MessageId { get; }

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        TData Data { get; }

        /// <summary>
        /// Gets the headers.
        /// </summary>
        /// <value>
        /// The headers.
        /// </value>
        IDictionary<string, object> Headers { get; }

        /// <summary>
        /// Gets the header.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        T GetHeader<T>(string key, T defaultValue = default(T));

        /// <summary>
        /// Acknowledges this instance.
        /// </summary>
        void Acknowledge();

        /// <summary>
        /// Rejects the specified requeue.
        /// </summary>
        /// <param name="requeue">if set to <c>true</c> [requeue].</param>
        void Reject(bool requeue = false);
    }
}
