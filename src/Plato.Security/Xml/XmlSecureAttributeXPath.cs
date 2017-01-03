// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

namespace Plato.Security.Xml
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Security.Xml.XmlSecureNodeXPath" />
    internal class XmlSecureAttributeXPath : XmlSecureNodeXPath
    {
        /// <summary>
        /// Gets or sets the attributes.
        /// </summary>
        /// <value>
        /// The attributes.
        /// </value>
        public string[] Attributes { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlSecureAttributeXPath"/> class.
        /// </summary>
        /// <param name="xPath">The x path.</param>
        /// <param name="attribite">The attribute.</param>
        public XmlSecureAttributeXPath(string xPath, string[] attribite) : base(xPath)
        {
            Attributes = attribite;
        }
    }
}
