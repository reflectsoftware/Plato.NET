// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Cache.Interfaces;

namespace Plato.Cache.Extensions
{
    /// <summary>
    ///
    /// </summary>
    public static class MemoryCacheExtension
    {
        /// <summary>
        /// Clears the specified memory cache.
        /// </summary>
        /// <param name="memoryCache">The memory cache.</param>
        public static void Clear(this IMemoryCacheExtension memoryCache)
        {
            memoryCache.Clear();
        }
    }
}
