// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System.Configuration;
using System.Xml;

namespace Plato.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Configuration.IConfigurationSectionHandler" />
    public class ConfigurationSectionHandler : IConfigurationSectionHandler
    {
        /// <summary>
        /// Creates the specified parent.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="context">The context.</param>
        /// <param name="section">The section.</param>
        /// <returns></returns>
        object IConfigurationSectionHandler.Create(object parent, object context, XmlNode section)
        {
            return section;
        }
    }
}
