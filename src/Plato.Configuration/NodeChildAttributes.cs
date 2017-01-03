// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System.Collections.Generic;

namespace Plato.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    public class NodeChildAttributes
    {
        /// <summary>
        /// Gets the parent attributes.
        /// </summary>
        /// <value>
        /// The parent attributes.
        /// </value>
        public NodeAttributes ParentAttributes { get; internal set; }

        /// <summary>
        /// Gets the child attributes.
        /// </summary>
        /// <value>
        /// The child attributes.
        /// </value>
        public List<NodeAttributes> ChildAttributes { get; internal set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeChildAttributes"/> class.
        /// </summary>
        public NodeChildAttributes()
        {
            ParentAttributes = new NodeAttributes();
            ChildAttributes = new List<NodeAttributes>();
        }
    }
}
