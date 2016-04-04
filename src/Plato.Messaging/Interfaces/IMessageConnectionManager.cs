// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System;

namespace Plato.Messaging.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="System.IDisposable" />
    public interface IMessageConnectionManager<T> : IDisposable where T : class
    {
        /// <summary>
        /// Declares the connection.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        T DeclareConnection(string name);

        /// <summary>
        /// Removes the connection.
        /// </summary>
        /// <param name="name">The name.</param>
        void RemoveConnection(string name);
    }
}
