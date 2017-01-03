// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System;

namespace Plato.Core.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public interface IDisposables : IDisposable
    {
        /// <summary>
        /// Gets a value indicating whether this <see cref="IDisposables"/> is disposed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if disposed; otherwise, <c>false</c>.
        /// </value>
        bool Disposed { get; }

        /// <summary>
        /// Adds the specified disposable.
        /// </summary>
        /// <param name="disposable">The disposable.</param>
        void Add(object disposable);

        /// <summary>
        /// Disposes all.
        /// </summary>
        void DisposeAll();
    }
}
