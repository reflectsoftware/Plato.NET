// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Configuration.Interfaces;
using Plato.Utils.Strings;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Xml;

namespace Plato.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Configuration.Interfaces.IConfigSettings" />
    public class ConfigSettings : IConfigSettings
    {
        protected NameValueCollection _settings;
        protected XmlNode _rootSection;
        protected string _keyName;
        protected string _valueName;
        protected string _sectionName;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigSettings"/> class.
        /// </summary>
        /// <param name="rootSection">The root section.</param>
        /// <param name="sectionName">Name of the section.</param>
        /// <param name="keyName">Name of the key.</param>
        /// <param name="valueName">Name of the value.</param>
        public ConfigSettings(XmlNode rootSection, string sectionName = null, string keyName = null, string valueName = null)
        {
            _settings = new NameValueCollection();
            _rootSection = rootSection;
            _sectionName = sectionName;
            _keyName = keyName ?? "key";
            _valueName = valueName ?? "value";

            LoadSettings();
        }

        /// <summary>
        /// Loads the settings.
        /// </summary>
        protected void LoadSettings()
        {
            _settings.Clear();

            if (_rootSection == null)
            {
                return;
            }

            var namedSection = _rootSection;

            // try to load the key/value settings for this section if any
            if (_sectionName != null)
            {
                namedSection = _rootSection.SelectSingleNode(_sectionName);
                if (namedSection == null)
                {
                    return;
                }
            }

            foreach (XmlNode setting in namedSection.ChildNodes)
            {
                if (setting.Attributes == null
                || setting.Attributes[_keyName] == null || setting.Attributes[_valueName] == null
                || setting.Name != "add")
                {
                    continue;
                }

                _settings[setting.Attributes[_keyName].Value] = ConfigManager.InterceptValue(setting.Attributes[_valueName].Value);
            }
        }

        /// <summary>
        /// Gets the values.
        /// </summary>
        /// <value>
        /// The values.
        /// </value>
        public string[] Values
        {
            get
            {
                var value = new List<string>();
                foreach (string key in _settings.Keys)
                {
                    value.Add(_settings[key]);
                }

                return value.ToArray();
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
                return _settings.AllKeys;
            }
        }

        /// <summary>
        /// Gets the <see cref="System.String"/> with the specified key.
        /// </summary>
        /// <value>
        /// The <see cref="System.String"/>.
        /// </value>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public string this[string key]
        {
            get
            {
                return _settings[key];
            }
        }

        /// <summary>
        /// Gets the <see cref="System.String"/> with the specified key.
        /// </summary>
        /// <value>
        /// The <see cref="System.String"/>.
        /// </value>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public string this[string key, string defaultValue]
        {
            get
            {
                return StringHelper.IfNullOrEmptyUseDefault(this[key], defaultValue);
            }
        }
    }
}
