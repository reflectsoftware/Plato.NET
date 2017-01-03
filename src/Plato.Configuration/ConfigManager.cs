// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Configuration.Enums;
using Plato.Configuration.Interfaces;
using Plato.Core.Strings;
using System;
using System.Collections.Generic;

namespace Plato.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    public static class ConfigManager
    {
        /// <summary>
        /// 
        /// </summary>
        private class ConfigContainerNode
        {
            public int FileKey;
            public int ReferenceCount;
            public IConfigContainer ConfigContainer;
        }

        private readonly static Dictionary<int, ConfigContainerNode> _congurations;

        /// <summary>
        /// Gets the value intercept.
        /// </summary>
        /// <value>
        /// The value intercept.
        /// </value>
        public static IConfigValueIntercept ValueIntercept { get; private set; }

        /// <summary>
        /// Occurs when [on configuration container change].
        /// </summary>
        public static event Action<IConfigContainer, OnChangeType> OnConfigContainerChange;

        /// <summary>
        /// Occurs when [on configuration container loaded].
        /// </summary>
        public static event Action<IConfigContainer> OnConfigContainerLoaded;

        /// <summary>
        /// Occurs when [on configuration container unloaded].
        /// </summary>
        public static event Action<IConfigContainer> OnConfigContainerUnloaded;


        /// <summary>
        /// Initializes the <see cref="ConfigManager"/> class.
        /// </summary>
        static ConfigManager()
        {
            _congurations = new Dictionary<int, ConfigContainerNode>();
            ValueIntercept = new ConfigVariables();
        }

        /// <summary>
        /// Files the key.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="sectionName">Name of the section.</param>
        /// <param name="settingsSectionName">Name of the settings section.</param>
        /// <returns></returns>
        private static int FileKey(string fileName, string sectionName, string settingsSectionName)
        {
            return (int)StringHash.BKDRHash(string.Format("{0}.{1}.{2}", fileName, sectionName, settingsSectionName).ToLower());
        }

        /// <summary>
        /// Does the on configuration container change.
        /// </summary>
        /// <param name="configContainer">The configuration container.</param>
        /// <param name="cType">Type of the c.</param>
        private static void DoOnConfigContainerChange(IConfigContainer configContainer, OnChangeType cType)
        {
            if (OnConfigContainerChange != null)
            {
                OnConfigContainerChange(configContainer, cType);
            }
        }

        /// <summary>
        /// Does the on configuration container loaded.
        /// </summary>
        /// <param name="configContainer">The configuration container.</param>
        private static void DoOnConfigContainerLoaded(IConfigContainer configContainer)
        {
            if (OnConfigContainerLoaded != null)
            {
                OnConfigContainerLoaded(configContainer);
            }
        }

        /// <summary>
        /// Does the on configuration container unloaded.
        /// </summary>
        /// <param name="configContainer">The configuration container.</param>
        private static void DoOnConfigContainerUnloaded(IConfigContainer configContainer)
        {
            if (OnConfigContainerUnloaded != null)
            {
                OnConfigContainerUnloaded(configContainer);
            }
        }

        /// <summary>
        /// Removes the specified container.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="bDispose">if set to <c>true</c> [b dispose].</param>
        internal static void Remove(IConfigContainer container, bool bDispose)
        {
            lock (_congurations)
            {
                foreach (var node in _congurations.Values)
                {
                    if (ReferenceEquals(node.ConfigContainer, container))
                    {
                        node.ReferenceCount--;
                        if (node.ReferenceCount == 0)
                        {
                            DoOnConfigContainerUnloaded(container);

                            _congurations.Remove(node.FileKey);
                            container.OnConfigChange -= DoOnConfigContainerChange;

                            if (bDispose)
                            {
                                (container as IDisposable).Dispose();
                            }
                        }

                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Removes the specified container.
        /// </summary>
        /// <param name="container">The container.</param>
        public static void Remove(IConfigContainer container)
        {
            Remove(container, true);
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="sectionName">Name of the section.</param>
        /// <param name="settingsSectionName">Name of the settings section.</param>
        /// <returns></returns>
        public static IConfigContainer GetConfiguration(string fileName, string sectionName, string settingsSectionName)
        {
            lock (_congurations)
            {
                var key = FileKey(fileName, sectionName, settingsSectionName);

                ConfigContainerNode node;
                if (_congurations.ContainsKey(key))
                {
                    node = _congurations[key];
                }
                else
                {
                    var container = new ConfigContainer(fileName, sectionName, settingsSectionName);
                    container.OnConfigChange += DoOnConfigContainerChange;

                    node = new ConfigContainerNode() { FileKey = key, ReferenceCount = 0, ConfigContainer = container };
                    _congurations.Add(key, node);

                    DoOnConfigContainerLoaded(container);
                }

                node.ReferenceCount++;

                return node.ConfigContainer;
            }
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="sectionName">Name of the section.</param>
        /// <returns></returns>
        public static IConfigContainer GetConfiguration(string fileName, string sectionName)
        {
            return GetConfiguration(fileName, sectionName, "appSettings");
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public static IConfigContainer GetConfiguration(string fileName)
        {
            return GetConfiguration(fileName, "./configuration");
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <returns></returns>
        public static IConfigContainer GetConfiguration()
        {
            return GetConfiguration(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
        }

        /// <summary>
        /// Gets the configuration by section.
        /// </summary>
        /// <param name="sectionName">Name of the section.</param>
        /// <param name="settingsSectionName">Name of the settings section.</param>
        /// <returns></returns>
        public static IConfigContainer GetConfigurationBySection(string sectionName, string settingsSectionName)
        {
            var configFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
            return GetConfiguration(configFile, sectionName, settingsSectionName);
        }

        /// <summary>
        /// Gets the configuration by section.
        /// </summary>
        /// <param name="sectionName">Name of the section.</param>
        /// <returns></returns>
        public static IConfigContainer GetConfigurationBySection(string sectionName)
        {
            return GetConfigurationBySection(sectionName, "appSettings");
        }

        /// <summary>
        /// Refreshes this instance.
        /// </summary>
        public static void Refresh()
        {
            lock (_congurations)
            {
                foreach (var node in _congurations.Values)
                {
                    node.ConfigContainer.Refresh();
                }
            }
        }

        /// <summary>
        /// Intercepts the value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string InterceptValue(string value)
        {
            return ValueIntercept.ValueIntercept(value);
        }
    }
}
