// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System;

namespace Plato.ClassFactory.Interfaces
{
    /// <summary>
    ///
    /// </summary>
    public interface IClassFactoryActivator
    {
        /// <summary>
        /// Creates the instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type">The type.</param>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        T CreateInstance<T>(Type type, params object[] args);
    }
}
