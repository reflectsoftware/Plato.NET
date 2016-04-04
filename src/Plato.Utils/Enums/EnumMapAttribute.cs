// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System;

namespace Plato.Utils.Enums
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="System.Attribute"/>
    [AttributeUsage(AttributeTargets.Field)]
    public class EnumMapAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the raw value.
        /// </summary>
        /// <value>
        /// The raw value.
        /// </value>
        public string RawValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is default.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is default; otherwise, <c>false</c>.
        /// </value>
        public bool IsDefault { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumMapAttribute"/> class.
        /// </summary>
        /// <param name="rawValue">The raw value.</param>
        public EnumMapAttribute(string rawValue)
        {
            RawValue = rawValue;
        }
    }
}
