// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System.Collections.Specialized;

namespace Plato.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    public class NodeAttributes
    {
        /// <summary>
        /// Gets or sets the name of the node.
        /// </summary>
        /// <value>
        /// The name of the node.
        /// </value>
        public string NodeName { get; set; }

        /// <summary>
        /// Gets the attributes.
        /// </summary>
        /// <value>
        /// The attributes.
        /// </value>
        public NameValueCollection Attributes { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="NodeAttributes"/> class.
        /// </summary>
        public NodeAttributes()
        {
            NodeName = string.Empty;
            Attributes = new NameValueCollection();
        }
    }
}
