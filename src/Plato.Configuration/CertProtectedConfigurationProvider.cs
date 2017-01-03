// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Security.Cryptography;
using Plato.Security.Xml;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Security.Cryptography.X509Certificates;
using System.Xml;

namespace Plato.Configuration
{
    /// <summary>
    /// Configuration usage:
    ///
    /// <configProtectedData defaultProvider="CertProtectedConfigurationProvider">
    /// <providers>
    /// <add name="CertProtectedConfigurationProvider"type="Plato.Configuration.CertProtectedConfigurationProvider, Plato.Configuration"keyName="RSA Key"certThumbprint="e323c6cac2526c328cd5e2ee10e41b2874911a9d"currentUser="false"/>
    /// </providers>
    /// </configProtectedData>
    ///
    /// </summary>
    public class CertProtectedConfigurationProvider : ProtectedConfigurationProvider, IDisposable
    {
        private X509Certificate2 _certificate;
        private readonly bool _isCurrentUser;
        private string _name;
        private string _keyName;
        private string _certThumbprint;

        /// <summary>
        /// Gets a value indicating whether this <see cref="CertProtectedConfigurationProvider"/> is disposed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if disposed; otherwise, <c>false</c>.
        /// </value>
        public bool Disposed { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CertProtectedConfigurationProvider"/> class.
        /// </summary>
        public CertProtectedConfigurationProvider()
        {
            Disposed = false;
            _isCurrentUser = false;
            _certificate = null;
            _name = "CertProtectedConfigurationProvider";
            _keyName = "RSA Key";
            _certThumbprint = string.Empty;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="CertProtectedConfigurationProvider"/> class.
        /// </summary>
        ~CertProtectedConfigurationProvider()
        {
            Dispose(false);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            lock(this)
            {
                if(!Disposed)
                {
                    Disposed = true;
                    GC.SuppressFinalize(this);

                    _certificate?.Dispose();
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
        /// Initializes the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="configurationValues">The configuration values.</param>
        /// <exception cref="ConfigurationErrorsException">
        /// Missing or invalid 'currentUser' value. Must be either 'true' or 'false'.
        /// or
        /// Missing or empty Certificate Thumbprint value.
        /// or
        /// Unable to located certificate.
        /// </exception>
        public override void Initialize(string name, NameValueCollection configurationValues)
        {
            base.Initialize(name, configurationValues);

            _name = name;

            var sUseCurrentUser = configurationValues["currentUser"];
            if ( sUseCurrentUser == null || (sUseCurrentUser != "true" && sUseCurrentUser != "false"))
            {
                throw new ConfigurationErrorsException("Missing or invalid 'currentUser' value. Must be either 'true' or 'false'.");
            }

            _certThumbprint = configurationValues["certThumbprint"];
            if (string.IsNullOrWhiteSpace(_certThumbprint) )
            {
                throw new ConfigurationErrorsException("Missing or empty Certificate Thumbprint value.");
            }

            var loc = configurationValues["currentUser"] == "true" ? StoreLocation.CurrentUser : StoreLocation.LocalMachine;
            _certificate = Certificate.GetCert(loc, X509FindType.FindByThumbprint, _certThumbprint, false);

            if ( _certificate == null )
            {
                throw new ConfigurationErrorsException("Unable to located certificate.");
            }

            _keyName = configurationValues["keyName"] ?? _keyName;
        }

        /// <summary>
        /// Encrypts the specified node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns></returns>
        public override XmlNode Encrypt(XmlNode node)
        {
            return XmlHelper.Encrypt(node, _certificate, _keyName);
        }

        public override XmlNode Decrypt(XmlNode encryptedNode)
        {
            if (!_certificate.HasPrivateKey)
            {
                throw new ConfigurationErrorsException("Configuration Provider certificate does not have a private key associated to it.");
            }

            return XmlHelper.Decrypt(encryptedNode, _certificate, _keyName);
        }

        /// <summary>
        /// Gets a brief, friendly description suitable for display in administrative tools or other user interfaces (UIs).
        /// </summary>
        public override string Description
        {
            get
            {
                return "Provides configuration protection using X509 certificates";
            }
        }

        /// <summary>
        /// Gets the friendly name used to refer to the provider during configuration.
        /// </summary>
        public override string Name
        {
            get
            {
                return _name;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is current user.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is current user; otherwise, <c>false</c>.
        /// </value>
        public bool IsCurrentUser
        {
            get
            {
                return _isCurrentUser;
            }
        }

        /// <summary>
        /// Gets the cert thumbprint.
        /// </summary>
        /// <value>
        /// The cert thumbprint.
        /// </value>
        public string CertThumbprint
        {
            get
            {
                return _certThumbprint;
            }
        }

        /// <summary>
        /// Gets the name of the key.
        /// </summary>
        /// <value>
        /// The name of the key.
        /// </value>
        public string KeyName
        {
            get
            {
                return _keyName;
            }
        }
    }
}
