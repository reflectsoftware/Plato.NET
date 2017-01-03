// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Configuration.Enums;
using Plato.Configuration.Interfaces;
using Plato.Security.Interfaces;
using Plato.Core.Miscellaneous;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Threading;
using System.Xml;

namespace Plato.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Configuration.Interfaces.IConfigValueIntercept" />
    /// <seealso cref="System.IDisposable" />
    public class ConfigVariables : IConfigValueIntercept, IDisposable
    {
        private class VariableContainer
        {
            public int ReferenceCount;
            public NameValueCollection Variables;
        }

        private string _sectionXPath;
        private ReaderWriterLockSlim _variablesLock;
        private Dictionary<IConfigContainer, VariableContainer> _variables;

        /// <summary>
        /// Gets or sets the encryption provider.
        /// </summary>
        /// <value>
        /// The encryption provider.
        /// </value>
        public IEncryptionProvider EncryptionProvider { get; set; }

        public bool Disposed { get; private set; }

        /// <summary>
        /// Gets or sets the encryption provider.
        /// </summary>
        /// <value>
        /// The encryption provider.
        /// </value>
        /// <exception cref="System.NotImplementedException">
        /// </exception>
        IEncryptionProvider IConfigValueIntercept.EncryptionProvider
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigVariables"/> class.
        /// </summary>
        /// <param name="sectionXPath">The section x path.</param>
        internal ConfigVariables(string sectionXPath = "./variables/variable")
        {
            Disposed = false;
            EncryptionProvider = null;

            _sectionXPath = sectionXPath;
            _variables = new Dictionary<IConfigContainer, VariableContainer>();
            _variablesLock = new ReaderWriterLockSlim();
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="ConfigVariables"/> class.
        /// </summary>
        ~ConfigVariables()
        {
            Dispose(false);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="bDisposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool bDisposing)
        {
            lock (this)
            {
                if (!Disposed)
                {
                    Disposed = true;
                    GC.SuppressFinalize(this);

                    Clear();

                    _variablesLock?.Dispose();
                }
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }
        
        /// <summary>
        /// Clears this instance.
        /// </summary>
        private void Clear()
        {
            _variablesLock.EnterWriteLock();
            try
            {
                foreach (var configContainer in _variables.Keys)
                {
                    configContainer.OnConfigChange -= DoOnConfigContainerChange;
                    MiscHelper.DisposeObject(configContainer);
                }

                _variables.Clear();
            }
            finally
            {
                _variablesLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Finds the container.
        /// </summary>
        /// <param name="configContainer">The configuration container.</param>
        /// <returns></returns>
        private VariableContainer FindContainer(IConfigContainer configContainer)
        {
            return _variables.ContainsKey(configContainer) ? _variables[configContainer] : null;
        }

        /// <summary>
        /// Does the on configuration container change.
        /// </summary>
        /// <param name="configContainer">The configuration container.</param>
        /// <param name="cType">Type of the c.</param>
        private void DoOnConfigContainerChange(IConfigContainer configContainer, OnChangeType cType)
        {
            if (cType == OnChangeType.Refresh)
            {
                return;
            }

            _variablesLock.EnterReadLock();
            try
            {
                var container = _variables[configContainer];
                container.Variables = ConstructValues(configContainer);
            }
            finally
            {
                _variablesLock.ExitReadLock();
            }

            ConfigManager.Refresh();
        }

        /// <summary>
        /// Constructs the values.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <returns></returns>
        private NameValueCollection ConstructValues(IConfigContainer container)
        {
            var values = new NameValueCollection();
            var xNodes = container.Node.GetNodes(_sectionXPath);

            if (xNodes == null)
            {
                return values;
            }

            foreach (XmlNode xNode in xNodes)
            {
                var xKey = xNode.Attributes["key"];
                var xValue = xNode.Attributes["value"];
                if (xKey == null || xValue == null)
                {
                    continue;
                }

                var xSecure = xNode.Attributes["secure"];
                var secure = EncryptionProvider != null && xSecure != null && xSecure.Value.ToLower() == "true";

                values[xKey.Value] = secure ? EncryptionProvider.DecryptString(xValue.Value, Encoding.UTF8) : xValue.Value;
            }

            return values;
        }

        /// <summary>
        /// Adds the variables.
        /// </summary>
        /// <param name="configContainer">The configuration container.</param>
        /// <param name="bForceRefresh">if set to <c>true</c> [b force refresh].</param>
        public void AddVariables(IConfigContainer configContainer, bool bForceRefresh = true)
        {
            _variablesLock.EnterWriteLock();
            try
            {
                var container = FindContainer(configContainer);
                if (container == null)
                {
                    var variables = ConstructValues(configContainer);

                    container = new VariableContainer() { ReferenceCount = 0, Variables = variables };
                    _variables.Add(configContainer, container);

                    configContainer.OnConfigChange += DoOnConfigContainerChange;
                }

                container.ReferenceCount++;

                if (bForceRefresh)
                {
                    ConfigManager.Refresh();
                }
            }
            finally
            {
                _variablesLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Removes the variables.
        /// </summary>
        /// <param name="configContainer">The configuration container.</param>
        /// <param name="bForceRefresh">if set to <c>true</c> [b force refresh].</param>
        public void RemoveVariables(IConfigContainer configContainer, bool bForceRefresh = true)
        {
            _variablesLock.EnterWriteLock();
            try
            {
                var container = FindContainer(configContainer);
                if (container == null)
                {
                    return;
                }

                container.ReferenceCount--;
                if (container.ReferenceCount > 0)
                {
                    return;
                }

                configContainer.OnConfigChange -= DoOnConfigContainerChange;
                _variables.Remove(configContainer);
            }
            finally
            {
                _variablesLock.ExitWriteLock();
            }

            if (bForceRefresh)
            {
                ConfigManager.Refresh();
            }
        }

        /// <summary>
        /// Values the intercept.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public string ValueIntercept(string value)
        {
            _variablesLock.EnterReadLock();
            try
            {
                var sb = new StringBuilder(value);

                foreach (var container in _variables.Values)
                {
                    foreach (string key in container.Variables.Keys)
                    {
                        sb.Replace(key, container.Variables[key]);
                    }
                }

                return sb.ToString();
            }
            finally
            {
                _variablesLock.ExitReadLock();
            }
        }
    }
}
