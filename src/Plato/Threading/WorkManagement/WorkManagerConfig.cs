// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Configuration;
using Plato.Configuration.Interfaces;
using System;

namespace Plato.Threading.WorkManagement
{
    /// <summary>
    /// 
    /// </summary>
    public static class WorkManagerConfig
    {
        /// <summary>
        /// Gets the container.
        /// </summary>
        /// <value>
        /// The container.
        /// </value>
        public static IConfigContainer Container { get; private set; }

        /// <summary>
        /// Occurs when [on configuration change].
        /// </summary>
        public static event Action OnConfigChange;

        /// <summary>
        /// Initializes the <see cref="WorkManagerConfig"/> class.
        /// </summary>
        static WorkManagerConfig()
        {
            Container = ConfigManager.GetConfigurationBySection("./configuration/platoSettings");
            Container.WatchAllowOnChangeAttribute(".", "reactOnConfigChange", "false");
            Container.OnConfigChange += DoConfigFileChanged;
        }

        /// <summary>
        /// Does the configuration file changed.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="changeType">Type of the change.</param>
        private static void DoConfigFileChanged(IConfigContainer configuration, Plato.Configuration.Enums.OnChangeType changeType)
        {
            if (OnConfigChange != null)
            {
                OnConfigChange();
            }
        }

        /// <summary>
        /// Gets the wait on terminate thread.
        /// </summary>
        /// <value>
        /// The wait on terminate thread.
        /// </value>
        public static int WaitOnTerminateThread
        {
            get
            {
                return int.Parse(Container.Node.GetAttribute("./workManager", "waitOnTerminateThread", "10000"));
            }
        }

        /// <summary>
        /// Gets the event tracker.
        /// </summary>
        /// <value>
        /// The event tracker.
        /// </value>
        public static int EventTracker
        {
            get
            {
                return int.Parse(Container.Node.GetAttribute("./workManager", "eventTracker", "20"));
            }
        }

        /// <summary>
        /// Gets the name of the application.
        /// </summary>
        /// <value>
        /// The name of the application.
        /// </value>
        public static string ApplicationName
        {
            get
            {
                return Container.Node.GetAttribute("./workManager", "applicationName", "Plato.Threading.WorkManagement");
            }
        }

        /// <summary>
        /// Gets a value indicating whether [disable watch when debugging].
        /// </summary>
        /// <value>
        /// <c>true</c> if [disable watch when debugging]; otherwise, <c>false</c>.
        /// </value>
        public static bool DisableWatchWhenDebugging
        {
            get
            {
                return Container.Node.GetAttribute("./workManager", "disableWatchWhenDebugging", "true") == "true";
            }
        }

        /// <summary>
        /// Gets the attribute.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static string GetAttribute(string key, string defaultValue = null)
        {
            return Container.Node.GetAttribute("./workManager", key, defaultValue);
        }
    }
}
