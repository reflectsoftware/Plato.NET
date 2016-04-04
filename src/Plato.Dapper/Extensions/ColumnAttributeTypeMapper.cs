// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Plato.Dapper.Extensions
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="Plato.Dapper.Extensions.FallbackTypeMapper"/>
    public class ColumnAttributeTypeMapper<T> : FallbackTypeMapper
    {
        public ColumnAttributeTypeMapper() : base(new SqlMapper.ITypeMap[] { new CustomPropertyTypeMap(
            typeof(T),
            (type, columnName) =>
            type.GetProperties().FirstOrDefault(prop =>
            prop.GetCustomAttributes(false)
            .OfType<ColumnAttribute>()
            .Any(attr => attr.Name == columnName)
            )
            ),
            new DefaultTypeMap(typeof(T)) })
        {
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="System.Attribute"/>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class ColumnAttribute : Attribute
    {
        public string Name { get; set; }
    }

    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="Dapper.SqlMapper.ITypeMap"/>
    public class FallbackTypeMapper : SqlMapper.ITypeMap
    {
        private readonly IEnumerable<SqlMapper.ITypeMap> _mappers;

        /// <summary>
        /// Initializes a new instance of the <see cref="FallbackTypeMapper"/> class.
        /// </summary>
        /// <param name="mappers">The mappers.</param>
        public FallbackTypeMapper(IEnumerable<SqlMapper.ITypeMap> mappers)
        {
            _mappers = mappers;
        }

        /// <summary>
        /// Finds the constructor.
        /// </summary>
        /// <param name="names">The names.</param>
        /// <param name="types">The types.</param>
        /// <returns></returns>
        public ConstructorInfo FindConstructor(string[] names, Type[] types)
        {
            foreach (var mapper in _mappers)
            {
                try
                {
                    var result = mapper.FindConstructor(names, types);
                    if (result != null)
                    {
                        return result;
                    }
                }
                catch (NotImplementedException)
                {
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the constructor parameter.
        /// </summary>
        /// <param name="constructor">The constructor.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        public SqlMapper.IMemberMap GetConstructorParameter(ConstructorInfo constructor, string columnName)
        {
            foreach (var mapper in _mappers)
            {
                try
                {
                    var result = mapper.GetConstructorParameter(constructor, columnName);
                    if (result != null)
                    {
                        return result;
                    }
                }
                catch (NotImplementedException)
                {
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the member.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        public SqlMapper.IMemberMap GetMember(string columnName)
        {
            foreach (var mapper in _mappers)
            {
                try
                {
                    var result = mapper.GetMember(columnName);
                    if (result != null)
                    {
                        return result;
                    }
                }
                catch (NotImplementedException)
                {
                }
            }

            return null;
        }


        /// <summary>
        /// Returns a constructor which should *always* be used.
        /// Parameters will be default values, nulls for reference types and zero'd for value types.
        /// Use this class to force object creation away from parameterless constructors you don't control.
        /// </summary>
        /// <returns></returns>
        public ConstructorInfo FindExplicitConstructor()
        {
            return _mappers.Select(mapper => mapper.FindExplicitConstructor()).FirstOrDefault(result => result != null);
        }
    }
}
