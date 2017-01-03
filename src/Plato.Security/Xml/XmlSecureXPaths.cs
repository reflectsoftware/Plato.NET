// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System;
using System.Collections;

namespace Plato.Security.Xml
{
    /// <summary>
    /// 
    /// </summary>
    public class XmlSecureXPaths
    {
        /// <summary>
        /// Gets or sets the x paths.
        /// </summary>
        /// <value>
        /// The x paths.
        /// </value>
        internal Hashtable XPaths { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlSecureXPaths"/> class.
        /// </summary>
        /// <param name="xPaths">The x paths.</param>
        internal XmlSecureXPaths(Hashtable xPaths)
        {
            XPaths = (Hashtable)xPaths.Clone();
        }

        /// <summary>
        /// Applies the parameters by arguments.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        public string ApplyParametersByArguments(string name, params object[] args)
        {
            var xPath = (XmlSecureNodeXPath)XPaths[name];
            if (xPath == null)
            {
                return null;
            }

            xPath.AppliedXPaths = new string[] { string.Format(xPath.XPath, args) };
            return xPath.AppliedXPaths[0];
        }

        /// <summary>
        /// Applies the parameters by delegate.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="method">The method.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public string[] ApplyParametersByDelegate(string name, Func<string, string[]> method)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            var xPath = (XmlSecureNodeXPath)XPaths[name];
            if (xPath == null)
            {
                return null;
            }

            xPath.AppliedXPaths = method(xPath.XPath);
            return xPath.AppliedXPaths;
        }

        /// <summary>
        /// Gets the applied.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public string[] GetApplied(string name)
        {
            var xPath = (XmlSecureNodeXPath)XPaths[name];
            if (xPath == null)
            {
                return null;
            }

            return xPath.AppliedXPaths;
        }
    }
}
