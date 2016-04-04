// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System;

namespace Plato.Messaging.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public interface IMessageReceiverSender : IDisposable
    {
        /// <summary>
        /// Gets a value indicating whether this <see cref="IMessageReceiverSender" /> is disposed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if disposed; otherwise, <c>false</c>.
        /// </value>
        bool Disposed { get; }

        /// <summary>
        /// Determines whether this instance is open.
        /// </summary>
        /// <returns></returns>
        bool IsOpen();

        /// <summary>
        /// Closes this instance.
        /// </summary>
        void Close();

        /// <summary>
        /// Opens this instance.
        /// </summary>
        void Open();
    }
}
