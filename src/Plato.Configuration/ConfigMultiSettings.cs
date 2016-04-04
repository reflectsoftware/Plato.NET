// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Configuration.Interfaces;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Plato.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Configuration.Interfaces.IConfigMultiSettings" />
    public class ConfigMultiSettings : IConfigMultiSettings
    {
        private NameValueCollection _emptySettings;
        private Dictionary<string, NameValueCollection> _settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigMultiSettings"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="parentXPath">The parent x path.</param>
        public ConfigMultiSettings(IConfigContainer container, string parentXPath)
        {
            _emptySettings = new NameValueCollection();
            _settings = new Dictionary<string, NameValueCollection>();

            NodeChildAttributes nca = ConfigHelper.GetNodeChildAttributes(container, parentXPath);
            foreach (var attributes in nca.ChildAttributes)
            {
                if (attributes.Attributes.Count > 0)
                {
                    string key = attributes.Attributes[0];
                    _settings[key] = attributes.Attributes;
                }
            }
        }

        /// <summary>
        /// Gets the keys.
        /// </summary>
        /// <value>
        /// The keys.
        /// </value>
        public string[] Keys
        {
            get
            {
                return _settings.Keys.ToArray();
            }
        }

        /// <summary>
        /// Gets the values.
        /// </summary>
        /// <value>
        /// The values.
        /// </value>
        public NameValueCollection[] Values
        {
            get
            {
                return _settings.Values.ToArray();
            }
        }

        /// <summary>
        /// Gets the <see cref="NameValueCollection"/> with the specified key.
        /// </summary>
        /// <value>
        /// The <see cref="NameValueCollection"/>.
        /// </value>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public NameValueCollection this[string key]
        {
            get
            {
                return _settings.ContainsKey(key) ? _settings[key] : _emptySettings;
            }
        }
    }
}
