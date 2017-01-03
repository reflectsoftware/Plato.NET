// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Dapper;
using System.Collections.Generic;
using System.Data;

namespace Plato.Dapper.CommandBuilder
{
    /// <summary>
    ///
    /// </summary>
    public class StatementParameterList
    {
        private readonly Dictionary<string, StatementParameter> Parameters;

        /// <summary>
        /// Initializes a new instance of the <see cref="StatementParameterList"/> class.
        /// </summary>
        public StatementParameterList()
        {
            Parameters = new Dictionary<string, StatementParameter>();
        }

        /// <summary>
        /// Adds the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <param name="dbType">Type of the database.</param>
        /// <param name="direction">The direction.</param>
        /// <param name="size">The size.</param>
        public void Add(string name, object value = null, DbType? dbType = null, ParameterDirection? direction = null, int? size = null)
        {
            if (Parameters.ContainsKey(name))
            {
                Parameters.Remove(name);
            }

            var parameter = new StatementParameter()
            {
                Name = name,
                Value = value,
                DbType = dbType,
                Direction = direction,
                Size = size
            };

            Parameters.Add(name, parameter);
        }

        /// <summary>
        /// Removes the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        public void Remove(string name)
        {
            Parameters.Remove(name);
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            Parameters.Clear();
        }

        /// <summary>
        /// Gets the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public StatementParameter Get(string name)
        {
            if (!Parameters.ContainsKey(name))
            {
                return null;
            }

            return Parameters[name];
        }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        public int Count
        {
            get
            {
                return Parameters.Count;
            }
        }

        /// <summary>
        /// Adds the dynamic parameters by identifier.
        /// </summary>
        /// <param name="dParamters">The parameters.</param>
        /// <param name="id">The identifier.</param>
        internal void AddDynamicParametersById(DynamicParameters dParamters, int id)
        {
            foreach (string pname in Parameters.Keys)
            {
                var idParam = string.Format("{0}_{1}", id, pname.Replace("@", string.Empty));
                var sqlParam = Parameters[pname];

                dParamters.Add(idParam, sqlParam.Value, sqlParam.DbType, sqlParam.Direction, sqlParam.Size);
            }
        }
    }
}
