// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System;

namespace Plato.ClassFactory
{
    /// <summary>
    ///
    /// </summary>
    public class ClassFactoryActivator
    {
        /// <summary>
        /// Creates the instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type">The type.</param>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static T CreateInstance<T>(Type type, params object[] args)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return (T)Activator.CreateInstance(type, args);
        }
    }
}
