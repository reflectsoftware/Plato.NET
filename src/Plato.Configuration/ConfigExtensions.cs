// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Configuration.Interfaces;

namespace Plato.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    public static class ConfigExtensions
    {
        /// <summary>
        /// Detaches from manager.
        /// </summary>
        /// <param name="container">The container.</param>
        public static void DetatchFromManager(this IConfigContainer container)
        {
            ConfigManager.Remove(container, false);
        }
    }
}
