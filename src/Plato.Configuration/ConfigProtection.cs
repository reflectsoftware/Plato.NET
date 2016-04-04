// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System.Collections.Specialized;
using System.Configuration;
using System.Text;
using System.Xml;

namespace Plato.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    public class ConfigProtection
    {
        private ProtectedConfigurationProvider _protectionProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigProtection"/> class.
        /// </summary>
        public ConfigProtection()
        {
            var parameters = new NameValueCollection();
            parameters["keyContainerName"] = "CustomContainer";
            parameters["useMachineContainer"] = "true";
            parameters["useOAEP"] = "false";

            _protectionProvider = new RsaProtectedConfigurationProvider();
            _protectionProvider.Initialize("CustomProvider", parameters);
        }

        /// <summary>
        /// Encrypts the specified configuration file.
        /// </summary>
        /// <param name="configFile">The configuration file.</param>
        /// <exception cref="ConfigurationErrorsException">
        /// </exception>
        public void Encrypt(string configFile)
        {
            var origDoc = new XmlDocument();
            origDoc.Load(configFile);
            XmlNode oldRootNode = origDoc.DocumentElement;

            var rootName = oldRootNode.Name;
            if (rootName == "configuration")
            {
                throw new ConfigurationErrorsException(string.Format("The config file '{0}' cannot not have a root starting with 'configuration'.", configFile), oldRootNode);
            }

            if (oldRootNode.Attributes["configProtectionProvider"] != null)
            {
                throw new ConfigurationErrorsException(string.Format("The configuration file '{0}' has already been encrypted. Current operation was aborted.", configFile), oldRootNode);
            }

            var encryptedNode = _protectionProvider.Encrypt(oldRootNode);
            XmlNode newRoot = encryptedNode.OwnerDocument.CreateElement(rootName);
            XmlNode encryptedData = encryptedNode.OwnerDocument.CreateElement("EncryptedData");
            newRoot.AppendChild(encryptedData);

            var aType = encryptedNode.OwnerDocument.CreateAttribute("Type");
            var aNamespace = encryptedNode.OwnerDocument.CreateAttribute("xmlns");

            aType.Value = "http://www.w3.org/2001/04/xmlenc#Element";
            aNamespace.Value = "http://www.w3.org/2001/04/xmlenc#";
            encryptedData.Attributes.Append(aType);
            encryptedData.Attributes.Append(aNamespace);

            encryptedNode.OwnerDocument.RemoveAll();
            encryptedNode.OwnerDocument.AppendChild(newRoot);

            foreach (XmlNode childNode in encryptedNode.ChildNodes)
            {
                encryptedData.AppendChild(childNode.CloneNode(true));
            }

            var provider = newRoot.OwnerDocument.CreateAttribute("configProtectionProvider");
            provider.Value = "CustomProvider";
            encryptedNode.OwnerDocument.DocumentElement.Attributes.Append(provider);

            using (var wr = new XmlTextWriter(configFile, Encoding.UTF8))
            {
                wr.Formatting = Formatting.Indented;
                encryptedNode.OwnerDocument.Save(wr);
            }
        }

        /// <summary>
        /// Decrypts the specified configuration file.
        /// </summary>
        /// <param name="configFile">The configuration file.</param>
        /// <exception cref="ConfigurationErrorsException"></exception>
        public void Decrypt(string configFile)
        {
            var origDoc = new XmlDocument();
            origDoc.Load(configFile);

            XmlNode oldRootNode = origDoc.DocumentElement;
            var encryptedNode = oldRootNode.FirstChild;

            if (encryptedNode != null && encryptedNode.Name != "EncryptedData")
            {
                throw new ConfigurationErrorsException(string.Format("The configuration file '{0}' is already decrypted. Current operation was aborted.", configFile), oldRootNode);
            }

            var decryptedNode = _protectionProvider.Decrypt(encryptedNode);

            using (var wr = new XmlTextWriter(configFile, Encoding.UTF8))
            {
                wr.Formatting = Formatting.Indented;
                decryptedNode.OwnerDocument.Save(wr);
            }
        }
    }
}
