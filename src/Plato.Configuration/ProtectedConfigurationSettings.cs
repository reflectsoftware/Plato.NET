// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Configuration.Interfaces;
using Plato.Core.Miscellaneous;
using Plato.Core.Strings;
using System.Configuration;
using System.Web.Configuration;
using System.Xml;

namespace Plato.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    public class ProtectedConfigurationSettings
    {
        private IConfigSettings _configSettings;

        /// <summary>
        /// Gets the name of the section.
        /// </summary>
        /// <value>
        /// The name of the section.
        /// </value>
        public string SectionName { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProtectedConfigurationSettings"/> class.
        /// </summary>
        /// <param name="sectionName">Name of the section.</param>
        public ProtectedConfigurationSettings(string sectionName)
        {
            SectionName = sectionName;
            _configSettings = new ConfigSettings(null);
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <returns></returns>
        public static System.Configuration.Configuration GetConfiguration()
        {
            if (MiscHelper.IsCurrentDomainWebApplication())
            {
                return WebConfigurationManager.OpenWebConfiguration("/web.config");
            }

            return ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        }

        /// <summary>
        /// Prepares the configuration object.
        /// </summary>
        protected void PrepareConfigObject()
        {
            var config = GetConfiguration();
            var section = config.GetSection(SectionName);

            if (section != null)
            {
                var xDoc = new XmlDocument();
                xDoc.LoadXml(section.SectionInformation.GetRawXml());
                _configSettings = new ConfigSettings(xDoc.SelectSingleNode("."), SectionName);
            }
        }

        /// <summary>
        /// Protects the specified provider.
        /// </summary>
        /// <param name="provider">The provider.</param>
        public void Protect(string provider)
        {
            lock (this)
            {
                var config = GetConfiguration();
                var section = config.GetSection(SectionName);

                if (section != null && !section.SectionInformation.IsProtected)
                {
                    section.SectionInformation.ForceSave = true;
                    section.SectionInformation.ProtectSection(provider);
                    config.Save();
                }
            }
        }

        /// <summary>
        /// Unprotects the specified save.
        /// </summary>
        /// <param name="save">if set to <c>true</c> [save].</param>
        public void Unprotect(bool save)
        {
            lock (this)
            {
                var config = GetConfiguration();
                var section = config.GetSection(SectionName);

                if (section != null && section.SectionInformation.IsProtected)
                {
                    section.SectionInformation.ForceSave = true;
                    section.SectionInformation.UnprotectSection();

                    if (save)
                    {
                        config.Save();
                    }
                }
            }
        }

        /// <summary>
        /// Unprotects this instance.
        /// </summary>
        public void Unprotect()
        {
            Unprotect(false);
        }

        public string this[string key]
        {
            get
            {
                lock (this)
                {
                    PrepareConfigObject();
                    return _configSettings[key];
                }
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
