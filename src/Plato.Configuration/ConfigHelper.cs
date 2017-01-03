// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Configuration.Interfaces;

namespace Plato.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    public static class ConfigHelper
    {
        /// <summary>
        /// Gets the node child attributes.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="parentXPath">The parent x path.</param>
        /// <returns></returns>
        public static NodeChildAttributes GetNodeChildAttributes(IConfigNode node, string parentXPath)
        {
            var nca = new NodeChildAttributes();

            var pnode = node.GetConfigNode(parentXPath);
            if (pnode != null)
            {
                nca.ParentAttributes.NodeName = pnode.Name;
                nca.ParentAttributes.Attributes.Add(pnode.GetAttributes());

                var children = pnode.GetConfigNodes("./*");
                foreach (var cnode in children)
                {
                    var na = new NodeAttributes() { NodeName = cnode.Name };
                    na.Attributes.Add(cnode.GetAttributes());
                    nca.ChildAttributes.Add(na);
                }
            }

            return nca;
        }

        /// <summary>
        /// Gets the node child attributes.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="parentXPath">The parent x path.</param>
        /// <returns></returns>
        public static NodeChildAttributes GetNodeChildAttributes(IConfigContainer container, string parentXPath)
        {
            return GetNodeChildAttributes(container.Node, parentXPath);
        }
    }
}
