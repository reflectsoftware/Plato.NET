// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Core.Strings;
using System;
using System.Collections;
using System.Xml;
using System.Xml.XPath;

namespace Plato.Security.Xml
{
    /// <summary>
    /// 
    /// </summary>
    public class XmlSecureWriter
    {
        private class NamespaceManagerInfo
        {
            /// <summary>
            /// Gets or sets the prefix.
            /// </summary>
            /// <value>
            /// The prefix.
            /// </value>
            public string Prefix { get; set; }

            /// <summary>
            /// Gets or sets the URI.
            /// </summary>
            /// <value>
            /// The URI.
            /// </value>
            public string Uri { get; set; }

            /// <summary>
            /// Initializes a new instance of the <see cref="NamespaceManagerInfo"/> class.
            /// </summary>
            /// <param name="prefix">The prefix.</param>
            /// <param name="uri">The URI.</param>
            public NamespaceManagerInfo(string prefix, string uri)
            {
                Prefix = prefix;
                Uri = uri;
            }
        }

        private readonly Hashtable _xmlNamespaceManagers;
        private readonly Hashtable _attributeXPaths;
        private readonly Hashtable _nodeXPaths;

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlSecureWriter"/> class.
        /// </summary>
        public XmlSecureWriter()
        {
            _xmlNamespaceManagers = new Hashtable();
            _attributeXPaths = new Hashtable();
            _nodeXPaths = new Hashtable();
        }

        /// <summary>
        /// Namepaces the manager key.
        /// </summary>
        /// <param name="prefix">The prefix.</param>
        /// <param name="uri">The URI.</param>
        /// <returns></returns>
        private static int NamepaceManagerKey(string prefix, string uri)
        {
            return (int)StringHash.BKDRHash(string.Format("{0}:{1}", prefix, uri).ToLower());
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            ClearNamespaces();
            ClearSecureNodeXPaths();
            ClearSecureAttributeXPaths();
        }

        /// <summary>
        /// Clears the namespaces.
        /// </summary>
        public void ClearNamespaces()
        {
            lock (_xmlNamespaceManagers)
            {
                _xmlNamespaceManagers.Clear();
            }
        }

        /// <summary>
        /// Clears the secure node x paths.
        /// </summary>
        public void ClearSecureNodeXPaths()
        {
            lock (_nodeXPaths)
            {
                _nodeXPaths.Clear();
            }
        }

        /// <summary>
        /// Clears the secure attribute x paths.
        /// </summary>
        public void ClearSecureAttributeXPaths()
        {
            lock (_attributeXPaths)
            {
                _attributeXPaths.Clear();
            }
        }

        /// <summary>
        /// Adds the namespace.
        /// </summary>
        /// <param name="prefix">The prefix.</param>
        /// <param name="uri">The URI.</param>
        public void AddNamespace(string prefix, string uri)
        {
            lock (_xmlNamespaceManagers)
            {
                _xmlNamespaceManagers[NamepaceManagerKey(prefix, uri)] = new NamespaceManagerInfo(prefix, uri);
            }
        }

        /// <summary>
        /// Removes the namespace.
        /// </summary>
        /// <param name="prefix">The prefix.</param>
        /// <param name="uri">The URI.</param>
        public void RemoveNamespace(string prefix, string uri)
        {
            lock (_xmlNamespaceManagers)
            {
                _xmlNamespaceManagers.Remove(NamepaceManagerKey(prefix, uri));
            }
        }

        public void AddSecureNodeXPath(string name, string xPath)
        {
            if (xPath == null)
            {
                throw new ArgumentNullException("xPath");
            }

            lock (_nodeXPaths)
            {
                _nodeXPaths[name] = new XmlSecureNodeXPath(xPath);
            }
        }

        /// <summary>
        /// Adds the secure attribute x path.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="xPath">The x path.</param>
        /// <param name="attributes">The attributes.</param>
        /// <exception cref="System.ArgumentNullException">attributes</exception>
        public void AddSecureAttributeXPath(string name, string xPath, string[] attributes)
        {
            if (attributes == null)
            {
                throw new ArgumentNullException("attributes");
            }

            lock (_attributeXPaths)
            {
                _attributeXPaths[name] = new XmlSecureAttributeXPath(xPath, attributes);
            }
        }

        /// <summary>
        /// Removes the secure node x path.
        /// </summary>
        /// <param name="name">The name.</param>
        public void RemoveSecureNodeXPath(string name)
        {
            lock (_nodeXPaths)
            {
                _nodeXPaths.Remove(name);
            }
        }

        /// <summary>
        /// Removes the secure attribute x path.
        /// </summary>
        /// <param name="name">The name.</param>
        public void RemoveSecureAttributeXPath(string name)
        {
            lock (_attributeXPaths)
            {
                _attributeXPaths.Remove(name);
            }
        }

        /// <summary>
        /// Gets the secure node x path.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public string GetSecureNodeXPath(string name)
        {
            XmlSecureNodeXPath nInfo;
            lock (_nodeXPaths)
            {
                nInfo = (XmlSecureNodeXPath)_nodeXPaths[name];
            }

            if (nInfo == null)
            {
                return null;
            }

            return nInfo.XPath;
        }

        /// <summary>
        /// Gets the secure attribute x path.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public string GetSecureAttributeXPath(string name)
        {
            XmlSecureAttributeXPath aInfo;
            lock (_attributeXPaths)
            {
                aInfo = 
                    (XmlSecureAttributeXPath)_attributeXPaths[name];
            }
            if (aInfo == null)
            {
                return null;
            }

            return aInfo.XPath;
        }

        /// <summary>
        /// Gets the node x paths.
        /// </summary>
        /// <returns></returns>
        public XmlSecureXPaths GetNodeXPaths()
        {
            lock (_nodeXPaths)
            {
                return new XmlSecureXPaths(_nodeXPaths);
            }
        }

        /// <summary>
        /// Gets the attribute x paths.
        /// </summary>
        /// <returns></returns>
        public XmlSecureXPaths GetAttributeXPaths()
        {
            lock (_attributeXPaths)
            {
                return new XmlSecureXPaths(_attributeXPaths);
            }
        }

        /// <summary>
        /// Selects the nodes.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="xPath">The x path.</param>
        /// <param name="nsmgr">The NSMGR.</param>
        /// <returns></returns>
        private static XmlNodeList SelectNodes(XmlNode node, string xPath, XmlNamespaceManager nsmgr)
        {
            try
            {
                return node.SelectNodes(xPath, nsmgr);
            }
            catch (XPathException)
            {
            }

            return null;
        }

        /// <summary>
        /// Secures the node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="bReturnCopy">if set to <c>true</c> [b return copy].</param>
        /// <param name="nodes">The nodes.</param>
        /// <param name="attributes">The attributes.</param>
        /// <returns></returns>
        public XmlNode SecureNode(XmlNode node, bool bReturnCopy, XmlSecureXPaths nodes, XmlSecureXPaths attributes)
        {
            if (node == null)
            {
                return null;
            }

            var nodeCopy = bReturnCopy ? node.CloneNode(true) : node;
            var xDoc = nodeCopy.OwnerDocument;
            if (xDoc == null)
            {
                xDoc = (XmlDocument)nodeCopy;
            }

            XmlNamespaceManager nsmgr;
            lock (_xmlNamespaceManagers)
            {
                nsmgr = new XmlNamespaceManager(xDoc.NameTable);
                foreach (NamespaceManagerInfo nsInfo in _xmlNamespaceManagers.Values)
                {
                    nsmgr.AddNamespace(nsInfo.Prefix, nsInfo.Uri);
                }
            }

            if (nodes != null)
            {
                foreach (XmlSecureNodeXPath nInfo in nodes.XPaths.Values)
                {
                    foreach (string xPath in nInfo.AppliedXPaths)
                    {
                        var selectedNodes = SelectNodes(nodeCopy, xPath, nsmgr);
                        if (selectedNodes == null || selectedNodes.Count == 0)
                        {
                            continue;
                        }

                        foreach (XmlNode sNode in selectedNodes)
                        {
                            sNode.InnerText = nInfo.HashString;
                        }
                    }
                }
            }

            if (attributes != null)
            {
                foreach (XmlSecureAttributeXPath aInfo in attributes.XPaths.Values)
                {
                    foreach (string xPath in aInfo.AppliedXPaths)
                    {
                        var selectedNodes = SelectNodes(nodeCopy, xPath, nsmgr);
                        if (selectedNodes == null || selectedNodes.Count == 0)
                        {
                            continue;
                        }
                        foreach (XmlNode sNode in selectedNodes)
                        {
                            if (sNode.Attributes == null || sNode.Attributes.Count == 0)
                            {
                                continue;
                            }
                            foreach (string attrName in aInfo.Attributes)
                            {
                                var attr = sNode.Attributes[attrName];
                                if (attr != null)
                                {
                                    attr.InnerText = aInfo.HashString;
                                }
                            }
                        }
                    }
                }
            }

            return nodeCopy;
        }

        /// <summary>
        /// Secures the node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="bReturnCopy">if set to <c>true</c> [return copy].</param>
        /// <param name="bApplyNodes">if set to <c>true</c> [apply nodes].</param>
        /// <param name="bApplyAttributes">if set to <c>true</c> [apply attributes].</param>
        /// <returns></returns>
        public XmlNode SecureNode(XmlNode node, bool bReturnCopy, bool bApplyNodes, bool bApplyAttributes)
        {
            var nodes = bApplyNodes ? GetNodeXPaths() : null;
            var attributes = bApplyAttributes ? GetAttributeXPaths() : null;

            return SecureNode(node, bReturnCopy, nodes, attributes);
        }

        /// <summary>
        /// Secures the node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="bReturnCopy">if set to <c>true</c> [b return copy].</param>
        /// <returns></returns>
        public XmlNode SecureNode(XmlNode node, bool bReturnCopy)
        {
            return SecureNode(node, bReturnCopy, true, true);
        }

        /// <summary>
        /// Secures the node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns></returns>
        public XmlNode SecureNode(XmlNode node)
        {
            return SecureNode(node, false, true, true);
        }

        /// <summary>
        /// Secures the document.
        /// </summary>
        /// <param name="xDoc">The x document.</param>
        /// <param name="bReturnCopy">if set to <c>true</c> [b return copy].</param>
        /// <param name="nodes">The nodes.</param>
        /// <param name="attributes">The attributes.</param>
        /// <returns></returns>
        public XmlDocument SecureDocument(XmlDocument xDoc, bool bReturnCopy, XmlSecureXPaths nodes, XmlSecureXPaths attributes)
        {
            return (XmlDocument)SecureNode(xDoc, bReturnCopy, nodes, attributes);
        }

        /// <summary>
        /// Secures the document.
        /// </summary>
        /// <param name="xDoc">The document.</param>
        /// <param name="bReturnCopy">if set to <c>true</c> [return copy].</param>
        /// <param name="bApplyNodes">if set to <c>true</c> [apply nodes].</param>
        /// <param name="bApplyAttributes">if set to <c>true</c> [apply attributes].</param>
        /// <returns></returns>
        public XmlDocument SecureDocument(XmlDocument xDoc, bool bReturnCopy, bool bApplyNodes, bool bApplyAttributes)
        {
            return (XmlDocument)SecureNode(xDoc, bReturnCopy, bApplyNodes, bApplyAttributes);
        }

        /// <summary>
        /// Secures the document.
        /// </summary>
        /// <param name="xDoc">The x document.</param>
        /// <param name="bReturnCopy">if set to <c>true</c> [b return copy].</param>
        /// <returns></returns>
        public XmlDocument SecureDocument(XmlDocument xDoc, bool bReturnCopy)
        {
            return SecureDocument(xDoc, bReturnCopy, true, true);
        }

        /// <summary>
        /// Secures the document.
        /// </summary>
        /// <param name="xDoc">The x document.</param>
        /// <returns></returns>
        public XmlDocument SecureDocument(XmlDocument xDoc)
        {
            return SecureDocument(xDoc, false, true, true);
        }
    }
}
