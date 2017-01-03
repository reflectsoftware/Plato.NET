// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System.Threading.Tasks;

namespace Plato.Cache.Interfaces
{
    /// <summary>
    ///
    /// </summary>
    public interface IMemoryCacheExtension
    {
        /// <summary>
        /// Clears this instance.
        /// </summary>
        void Clear();

        /// <summary>
        /// Clears the asynchronous.
        /// </summary>
        /// <returns></returns>
        Task ClearAsync();
    }
}
