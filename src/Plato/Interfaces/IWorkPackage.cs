// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Threading.WorkManagement;

namespace Plato.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IWorkPackage
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        string Name { get; }

        /// <summary>
        /// Gets the name instance.
        /// </summary>
        /// <value>
        /// The name instance.
        /// </value>
        string NameInstance { get; }

        /// <summary>
        /// Gets the specified parameter by key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        string Parameters(string key, string defaultValue = null);

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        T GetData<T>(T defaultValue = default(T));

        /// <summary>
        /// Gets the work manager.
        /// </summary>
        /// <value>
        /// The work manager.
        /// </value>
        WorkManager WorkManager { get; }
    }
}
