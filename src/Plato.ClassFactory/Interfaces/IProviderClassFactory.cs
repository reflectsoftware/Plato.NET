// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System;

namespace Plato.ClassFactory.Interfaces
{
    /// <summary>
    ///
    /// </summary>
    public interface IProviderClassFactory
    {
        /// <summary>
        /// Occurs when [on change].
        /// </summary>
        event Action OnChange;

        /// <summary>
        /// Gets the default name.
        /// </summary>
        /// <returns></returns>
        string GetDefaultName();

        /// <summary>
        /// Determines whether [has named instance defined] [the specified name].
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        bool HasNamedInstanceDefined(string name);

        /// <summary>
        /// Determines whether [has default named instance defined].
        /// </summary>
        /// <returns></returns>
        bool HasDefaultNamedInstanceDefined();

        /// <summary>
        /// Any providers.
        /// </summary>
        /// <returns></returns>
        bool AnyProviders();

        /// <summary>
        /// Creates the instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">The name.</param>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        T CreateInstance<T>(string name, params object[] args);

        /// <summary>
        /// Creates the instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        T CreateInstance<T>(params object[] args);
    }
}
