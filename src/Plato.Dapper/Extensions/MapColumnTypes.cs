// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Dapper;
using System;
using System.Linq;
using System.Reflection;

namespace Plato.Dapper.Extensions
{
    /// <summary>
    ///
    /// </summary>
    public class MapColumnTypes
    {
        /// <summary>
        /// Registers the specified assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        public static void Register(Assembly assembly)
        {
            var mappedTypes = assembly.GetTypes().Where(f => f.GetProperties().Any(p => p.GetCustomAttributes(false).Any(a => a.GetType().Name == "ColumnAttribute")));

            var mapper = typeof(ColumnAttributeTypeMapper<>);
            foreach (var mappedType in mappedTypes)
            {
                var genericType = mapper.MakeGenericType(new[] { mappedType });
                SqlMapper.SetTypeMap(mappedType, Activator.CreateInstance(genericType) as SqlMapper.ITypeMap);
            }
        }
    }
}
