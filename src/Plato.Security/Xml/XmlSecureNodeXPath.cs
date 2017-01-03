// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Security.Cryptography;

namespace Plato.Security.Xml
{
    /// <summary>
    /// xPath examples
    /// http://www.w3schools.com/XPath/xpath_axes.asp
    /// </summary>    
    internal class XmlSecureNodeXPath
    {
        /// <summary>
        /// Gets or sets the x path.
        /// </summary>
        /// <value>
        /// The x path.
        /// </value>
        public string XPath { get; set; }

        /// <summary>
        /// Gets or sets the hash string.
        /// </summary>
        /// <value>
        /// The hash string.
        /// </value>
        public string HashString { get; set; }

        /// <summary>
        /// Gets or sets the applied x paths.
        /// </summary>
        /// <value>
        /// The applied x paths.
        /// </value>
        public string[] AppliedXPaths { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlSecureNodeXPath"/> class.
        /// </summary>
        /// <param name="xPath">The x path.</param>
        public XmlSecureNodeXPath(string xPath)
        {
            XPath = xPath;
            HashString = CryptoServices.ComputeHashBase64String(xPath, "SHA256");
            AppliedXPaths = new string[] { xPath };
        }
    }
}
