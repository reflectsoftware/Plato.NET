// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

namespace Plato.Dapper.CommandBuilder
{
    /// <summary>
    ///
    /// </summary>
    public class StatementBuilderInject
    {
        /// <summary>
        /// Gets or sets the inject identifier.
        /// </summary>
        /// <value>
        /// The inject identifier.
        /// </value>
        public string InjectId { get; set; }

        /// <summary>
        /// Gets or sets the builder.
        /// </summary>
        /// <value>
        /// The builder.
        /// </value>
        public StatementBuilder Builder { get; set; }
    }
}
