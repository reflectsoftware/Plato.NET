// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Configuration.Enums;
using Plato.Configuration.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Xml;

namespace Plato.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Configuration.Interfaces.IConfigContainer" />
    public class ConfigContainer : IConfigContainer
    {
        /// <summary>
        /// 
        /// </summary>
        private class AllowOnChangeAttribute
        {
            public string XPath;
            public string AttrName;
            public string Value;

            /// <summary>
            /// Initializes a new instance of the <see cref="AllowOnChangeAttribute"/> class.
            /// </summary>
            /// <param name="xPath">The x path.</param>
            /// <param name="attrName">Name of the attribute.</param>
            /// <param name="value">The value.</param>
            public AllowOnChangeAttribute(string xPath, string attrName, string value)
            {
                XPath = xPath;
                AttrName = attrName;
                Value = value;
            }
        }

        private static readonly string _xmlDefault = "<?xml version='1.0' encoding='utf-8'?><configuration></configuration>";

        private List<AllowOnChangeAttribute> _allowOnChangeAttribute;
        private object _onConfigChangeLock;
        private IConfigNode _configNode;
        private IConfigSettings _settings;
        private IConfigSettings _connectionStrings;
        private Dictionary<string, string> _sectionNames;
        private FileSystemWatcher _fileWatcher;
        private DateTime _lastFileChangeTimestamp;

        /// <summary>
        /// Occurs when [on configuration change].
        /// </summary>
        public event Action<IConfigContainer, OnChangeType> OnConfigChange;

        /// <summary>
        /// Gets a value indicating whether this <see cref="ConfigContainer"/> is disposed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if disposed; otherwise, <c>false</c>.
        /// </value>
        public bool Disposed { get; private set; }

        /// <summary>
        /// Gets the name of the file.
        /// </summary>
        /// <value>
        /// The name of the file.
        /// </value>
        public string FileName { get; private set; }

        /// <summary>
        /// Gets the name of the section.
        /// </summary>
        /// <value>
        /// The name of the section.
        /// </value>
        public string SectionName { get; private set; }

        /// <summary>
        /// Gets the name of the settings section.
        /// </summary>
        /// <value>
        /// The name of the settings section.
        /// </value>
        public string SettingsSectionName { get; private set; }

        /// <summary>
        /// Gets the name of the connection strings section.
        /// </summary>
        /// <value>
        /// The name of the connection strings section.
        /// </value>
        public string ConnectionStringsSectionName { get; private set; }

        /// <summary>
        /// Bases the initialize.
        /// </summary>
        private void BaseInit()
        {
            Disposed = false;
            _onConfigChangeLock = new object();
            _fileWatcher = null;
            _lastFileChangeTimestamp = DateTime.MinValue;
            _allowOnChangeAttribute = new List<AllowOnChangeAttribute>();
            _sectionNames = new Dictionary<string, string>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigContainer"/> class.
        /// </summary>
        internal ConfigContainer()
        {
            BaseInit();

            SectionName = "./configuration";
            SettingsSectionName = "appSettings";
            ConnectionStringsSectionName = "connectionStrings";

            var fileName = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
            if (File.Exists(fileName))
            {
                FileName = fileName;
                LoadFile();
            }
            else
            {
                FileName = string.Empty;
                _configNode = new ConfigNode(null);
                RefreshBaseSections();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigContainer"/> class.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="sectionName">Name of the section.</param>
        /// <param name="settingsSectionName">Name of the settings section.</param>
        /// <param name="connectionStringsSectionName">Name of the connection strings section.</param>
        public ConfigContainer(string fileName, string sectionName = "./configuration", string settingsSectionName = "appSettings", string connectionStringsSectionName = "connectionStrings")
        {
            BaseInit();

            FileName = fileName;
            SectionName = sectionName;
            SettingsSectionName = settingsSectionName;
            ConnectionStringsSectionName = connectionStringsSectionName;

            LoadFile();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        void IDisposable.Dispose()
        {
            lock (this)
            {
                if (!Disposed)
                {
                    Disposed = true;
                    GC.SuppressFinalize(this);

                    if (_fileWatcher != null)
                    {
                        _fileWatcher.Changed -= OnConfigFileChanged;
                        _fileWatcher.Created -= OnConfigFileChanged;
                        _fileWatcher.Deleted -= OnConfigFileChanged;
                        _fileWatcher.Dispose();
                        _fileWatcher = null;
                    }
                }
            }
        }

        /// <summary>
        /// Refreshes the base sections.
        /// </summary>
        private void RefreshBaseSections()
        {
            _settings = new ConfigSettings(_configNode.Section, SettingsSectionName, "key", "value");
            _connectionStrings = new ConfigSettings(_configNode.Section, ConnectionStringsSectionName, "name", "connectionString");
        }

        /// <summary>
        /// Logs the exception.
        /// </summary>
        /// <param name="ex">The ex.</param>
        private static void LogException(Exception ex)
        {
            try
            {
                EventLog.WriteEntry("Plato.NET", ex.ToString(), EventLogEntryType.Error);
            }
            catch (Exception)
            {
                // swallow, just in case event logs are full or access denied. 
            }
        }

        /// <summary>
        /// Loads the section.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="sectionName">Name of the section.</param>
        /// <returns></returns>
        private static XmlNode LoadSection(string fileName, string sectionName)
        {
            if (fileName == null)
            {
                return null;
            }

            var xmlData = _xmlDefault;
            var attemps = 5;

            while (true)
            {
                try
                {
                    using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        using (var tr = new StreamReader(fs))
                        {
                            Thread.Sleep(10);

                            xmlData = tr.ReadToEnd();

                            if (string.IsNullOrWhiteSpace(xmlData))
                            {
                                xmlData = _xmlDefault;
                            }

                            break;
                        }
                    }
                }
                catch (FileNotFoundException)
                {
                    xmlData = _xmlDefault;
                    break;
                }
                catch (IOException ex)
                {
                    attemps--;
                    if (attemps < 0)
                    {
                        xmlData = _xmlDefault;
                        LogException(ex);
                        break;
                    }

                    Thread.Sleep(100);
                }
                catch (Exception ex)
                {
                    xmlData = _xmlDefault;
                    LogException(ex);
                    break;
                }
            }

            try
            {
                var xDoc = new XmlDocument() { PreserveWhitespace = true };
                xDoc.LoadXml(xmlData);

                return xDoc.SelectSingleNode(sectionName);
            }
            catch (Exception ex)
            {
                LogException(ex);
            }

            return null;
        }

        /// <summary>
        /// Loads the file.
        /// </summary>
        /// <param name="node">The node.</param>
        protected void LoadFile(ConfigNode node = null)
        {
            lock (this)
            {
                _configNode = node ?? new ConfigNode(LoadSection(FileName, SectionName));
                RefreshBaseSections();

                var fileOnly = Path.GetFileName(FileName);
                var pathOnly = FileName.Replace(fileOnly, string.Empty);


                if (_fileWatcher != null)
                {
                    _fileWatcher.Dispose();
                }

                _fileWatcher = new FileSystemWatcher()
                {
                    Path = pathOnly,
                    Filter = fileOnly,
                    NotifyFilter = NotifyFilters.LastWrite,
                    EnableRaisingEvents = true
                };

                _fileWatcher.Changed += OnConfigFileChanged;
                _fileWatcher.Created += OnConfigFileChanged;
                _fileWatcher.Deleted += OnConfigFileChanged;
            }
        }

        /// <summary>
        /// Allows the on change.
        /// </summary>
        /// <param name="cNode">The c node.</param>
        /// <returns></returns>
        protected bool AllowOnChange(IConfigNode cNode)
        {
            var rValue = true;
            foreach (var cAttr in _allowOnChangeAttribute)
            {
                if (cNode.GetAttribute(cAttr.XPath, cAttr.AttrName, cAttr.Value) != cAttr.Value)
                {
                    rValue = false;
                    break;
                }
            }

            return rValue;
        }

        /// <summary>
        /// Configurations the file change.
        /// </summary>
        protected void ConfigFileChange()
        {
            var configNode = new ConfigNode(LoadSection(FileName, SectionName));
            if (!AllowOnChange(configNode))
            {
                return;
            }

            LoadFile(configNode);

            if (OnConfigChange != null)
            {
                OnConfigChange(this, OnChangeType.File);
            }
        }

        /// <summary>
        /// Called when [configuration file changed].
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="e">The <see cref="FileSystemEventArgs"/> instance containing the event data.</param>
        protected void OnConfigFileChanged(object source, FileSystemEventArgs e)
        {
            lock (_onConfigChangeLock)
            {
                switch (e.ChangeType)
                {
                    case WatcherChangeTypes.Created:
                    case WatcherChangeTypes.Changed:
                    case WatcherChangeTypes.Deleted:
                        {
                            var newFileTimestamp = File.GetLastWriteTime(e.FullPath);
                            if (newFileTimestamp != _lastFileChangeTimestamp)
                            {
                                _lastFileChangeTimestamp = newFileTimestamp;
                                ConfigFileChange();
                            }
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Refreshes this instance.
        /// </summary>
        public void Refresh()
        {
            RefreshBaseSections();

            if (OnConfigChange != null)
            {
                OnConfigChange(this, OnChangeType.Refresh);
            }
        }

        /// <summary>
        /// Watches the allow on change attribute.
        /// </summary>
        /// <param name="xPath">The x path.</param>
        /// <param name="attrName">Name of the attribute.</param>
        /// <param name="value">The value.</param>
        public void WatchAllowOnChangeAttribute(string xPath, string attrName, string value)
        {
            lock (_allowOnChangeAttribute)
            {
                _allowOnChangeAttribute.Add(new AllowOnChangeAttribute(xPath, attrName, value));
            }
        }

        /// <summary>
        /// Gets the node.
        /// </summary>
        /// <value>
        /// The node.
        /// </value>
        public IConfigNode Node
        {
            get
            {
                lock (this)
                {
                    return _configNode;
                }
            }
        }

        /// <summary>
        /// Gets the settings.
        /// </summary>
        /// <value>
        /// The settings.
        /// </value>
        public IConfigSettings Settings
        {
            get
            {
                lock (this)
                {
                    return _settings;
                }
            }
        }

        /// <summary>
        /// Gets the connection strings.
        /// </summary>
        /// <value>
        /// The connection strings.
        /// </value>
        public IConfigSettings ConnectionStrings
        {
            get
            {
                lock (this)
                {
                    return _connectionStrings;
                }
            }
        }
    }
}
