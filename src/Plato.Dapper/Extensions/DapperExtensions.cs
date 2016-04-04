// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Plato.Dapper.Extensions
{
    /// <summary>
    ///
    /// </summary>
    public static class DapperExtensions
    {
        /// <summary>
        /// Counts the specified parameters.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public static int Count(this DynamicParameters parameters)
        {
            return parameters.ParameterNames.Count();
        }

        /// <summary>
        /// As table valued parameter.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">The items.</param>
        /// <param name="typeName">Name of the type.</param>
        /// <returns></returns>
        public static SqlMapper.ICustomQueryParameter AsTableValuedParameter<T>(this IEnumerable<T> items, string typeName = null)
        {
            var table = AsDataTable<T>(items);
            return table.AsTableValuedParameter(typeName);
        }

        /// <summary>
        /// As data table.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">The items.</param>
        /// <returns></returns>
        public static DataTable AsDataTable<T>(IEnumerable<T> items)
        {
            var table = new DataTable();
            var type = typeof(T);
            var properties = type.GetProperties().Where(i => i.CanRead && i.GetCustomAttributes(true).FirstOrDefault() == null).ToArray();

            foreach (var p in properties)
            {
                var column = p.GetCustomAttributes(true).FirstOrDefault(i => i.GetType() == typeof(ColumnAttribute)) as ColumnAttribute;
                var columnName = column == null ? p.Name : column.Name;
                var nullableType = Nullable.GetUnderlyingType(p.PropertyType);
                var propType = nullableType ?? p.PropertyType;

                table.Columns.Add(columnName, propType);
            }

            foreach (var item in items)
            {
                var row = table.NewRow();

                for (var i = 0; i < properties.Length; i++)
                {
                    row.SetField(table.Columns[i], properties[i].GetValue(item));
                }

                table.Rows.Add(row);
            }

            return table;
        }
    }
}
