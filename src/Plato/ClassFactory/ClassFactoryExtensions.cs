// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.ClassFactory.Interfaces;

namespace Plato.ClassFactory
{
    /// <summary>
    ///
    /// </summary>
    public static class ClassFactoryExtensions
    {
        /// <summary>
        /// Refreshes the attributes.
        /// </summary>
        /// <param name="providerClassFactory">The provider class factory.</param>
        public static void RefreshAttributes(this IProviderClassFactoryExtension providerClassFactory)
        {
            providerClassFactory.RefreshAttributes();
        }
    }
}
