// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System;

namespace Plato.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IWorkManagerDependencyResolver
    {
        /// <summary>
        /// Resolves the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
        /// <param name="package">The package.</param>
        /// <returns></returns>
        T Resolve<T>(string name, Type type, IWorkPackage package);
    }
}
