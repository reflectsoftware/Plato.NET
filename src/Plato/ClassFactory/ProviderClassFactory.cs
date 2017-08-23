// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.ClassFactory.Interfaces;
using Plato.Configuration;
using Plato.Configuration.Enums;
using Plato.Configuration.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Plato.ClassFactory
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.ClassFactory.Interfaces.IProviderClassFactory" />
    /// <seealso cref="Plato.ClassFactory.Interfaces.IProviderClassFactoryExtension" />
    /// <seealso cref="System.IDisposable" />
    public class ProviderClassFactory : IProviderClassFactory, IProviderClassFactoryExtension, IDisposable
    {
        private IConfigContainer _configContainer { get; set; }
        private string _configSection { get; set; }
        private string _attributeNodeName { get; set; }

        /// <summary>
        /// Gets the attributes.
        /// </summary>
        /// <value>
        /// The attributes.
        /// </value>
        public NodeChildAttributes Attributes { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ProviderClassFactory"/> is disposed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if disposed; otherwise, <c>false</c>.
        /// </value>
        public bool Disposed { get; private set; }

        /// <summary>
        /// Occurs when [on change].
        /// </summary>
        public event Action OnChange;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProviderClassFactory"/> class.
        /// </summary>
        /// <param name="configContainer">The configuration container.</param>
        /// <param name="configSection">The configuration section.</param>
        /// <param name="attributeNodeName">Name of the attribute node.</param>
        public ProviderClassFactory(IConfigContainer configContainer, string configSection, string attributeNodeName = "provider")
        {
            Disposed = false;
            _configContainer = configContainer;
            _configSection = configSection;
            _attributeNodeName = attributeNodeName;
            _configContainer.OnConfigChange += OnConfigChange;

            (this as IProviderClassFactoryExtension).RefreshAttributes();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProviderClassFactory"/> class.
        /// </summary>
        /// <param name="attributes">The attributes.</param>
        /// <param name="attributeNodeName">Name of the attribute node.</param>
        public ProviderClassFactory(NodeChildAttributes attributes, string attributeNodeName = "provider")
        {
            Disposed = false;
            Attributes = attributes;
            _attributeNodeName = attributeNodeName;
            _configContainer = null;
            _configSection = null;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="ProviderClassFactory"/> class.
        /// </summary>
        ~ProviderClassFactory()
        {
            Dispose(false);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="bDisposing">if set to <c>true</c> [b disposing].</param>
        protected virtual void Dispose(bool bDisposing)
        {
            lock (this)
            {
                if (!Disposed)
                {
                    Disposed = true;
                    if (bDisposing)
                    {
                        GC.SuppressFinalize(this);
                    }

                    if (_configContainer != null)
                    {
                        _configContainer.OnConfigChange -= OnConfigChange;
                        _configContainer = null;
                    }
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
        /// Called when [configuration change].
        /// </summary>
        /// <param name="configContainer">The configuration container.</param>
        /// <param name="cType">Type of the c.</param>
        private void OnConfigChange(IConfigContainer configContainer, OnChangeType cType)
        {
            (this as IProviderClassFactoryExtension).RefreshAttributes();
        }

        /// <summary>
        /// Gets the named attribute.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        private NodeAttributes GetNamedAttribute(string name)
        {
            return Attributes.ChildAttributes.Find((x) => x.NodeName == _attributeNodeName && x.Attributes["name"] == name);
        }

        /// <summary>
        /// Creates the instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">The name.</param>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">
        /// name
        /// or
        /// name
        /// </exception>
        /// <exception cref="TypeAccessException"></exception>
        public T CreateInstance<T>(string name, params object[] args)
        {
            var namedAttributes = GetNamedAttribute(name);
            if (namedAttributes == null)
            {
                throw new ArgumentException(string.Format("ProviderClassFactory: cannot find node attributes for the following named instance: '{0}'", name), "name");
            }

            var type = namedAttributes.Attributes["type"];
            if (string.IsNullOrWhiteSpace(type))
            {
                throw new ArgumentException(string.Format("ProviderClassFactory: cannot find type attribute for the following named instance: '{0}'", name), "name");
            }

            var objectType = Type.GetType(type);
            if (objectType == null)
            {
                throw new TypeAccessException(string.Format("ProviderClassFactory: unable to locate class type '{0}' for the following named instance: '{1}'", type, name));
            }

            var pargs = new List<object>();
            pargs.Add(namedAttributes.Attributes);
            pargs.Add(args);

            return ClassFactoryActivator.CreateInstance<T>(objectType, pargs.ToArray());
        }

        /// <summary>
        /// Creates the instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        public T CreateInstance<T>(params object[] args)
        {
            return CreateInstance<T>(GetDefaultName(), args);
        }

        /// <summary>
        /// Gets the default name.
        /// </summary>
        /// <returns></returns>
        public string GetDefaultName()
        {
            var defaultInstance = Attributes.ParentAttributes.Attributes["default"];
            return defaultInstance ?? "default";
        }

        /// <summary>
        /// Determines whether [has named instance defined] [the specified name].
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public bool HasNamedInstanceDefined(string name)
        {
            return GetNamedAttribute(name) != null;
        }

        /// <summary>
        /// Determines whether [has default named instance defined].
        /// </summary>
        /// <returns></returns>
        public bool HasDefaultNamedInstanceDefined()
        {
            return GetNamedAttribute(GetDefaultName()) != null;
        }

        /// <summary>
        /// Any providers.
        /// </summary>
        /// <returns></returns>
        public bool AnyProviders()
        {
            return Attributes.ChildAttributes.Any(x => x.NodeName == _attributeNodeName);
        }

        /// <summary>
        /// Providers the count.
        /// </summary>
        /// <returns></returns>
        public int ProviderCount()
        {
            return Attributes.ChildAttributes.Count(x => x.NodeName == _attributeNodeName);
        }

        /// <summary>
        /// Refreshes the attributes.
        /// </summary>
        void IProviderClassFactoryExtension.RefreshAttributes()
        {
            if (_configContainer == null)
            {
                return;
            }

            Attributes = ConfigHelper.GetNodeChildAttributes(_configContainer, _configSection);

            if (OnChange != null)
            {
                OnChange();
            }
        }
    }
}
