// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System.Collections.Specialized;

namespace Plato.Core.Strings
{
    /// <summary>
    /// 
    /// </summary>
    public class ExceptionFormatterExtension
    {
        /// <summary>
        /// Gets or sets the caption.
        /// </summary>
        /// <value>
        /// The caption.
        /// </value>
        public string Caption { get; set; }

        /// <summary>
        /// Gets or sets the properties.
        /// </summary>
        /// <value>
        /// The properties.
        /// </value>
        public NameValueCollection Properties { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionFormatterExtension" /> class.
        /// </summary>
        /// <param name="caption">The caption.</param>
        public ExceptionFormatterExtension(string caption)
        {
            Caption = caption;
            Properties = new NameValueCollection();
        }
    }
}
