// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

namespace Plato.Messaging.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IMessageConnectionFactory<T> where T : class
    {
        /// <summary>
        /// Declares the connection.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        T CreateConnection(string name);
    }
}
