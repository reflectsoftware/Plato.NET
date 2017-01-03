// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Dapper;

namespace Plato.Dapper.CommandBuilder
{
    /// <summary>
    ///
    /// </summary>
    public class StatementBuilderResult
    {
        /// <summary>
        /// Gets or sets the statement.
        /// </summary>
        /// <value>
        /// The statement.
        /// </value>
        public string Statement { get; set; }

        /// <summary>
        /// Gets or sets the parameter list.
        /// </summary>
        /// <value>
        /// The parameter list.
        /// </value>
        public DynamicParameters ParameterList { get; set; }
    }
}
