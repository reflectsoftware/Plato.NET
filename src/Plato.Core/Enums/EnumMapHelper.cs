// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System;
using System.Reflection;

namespace Plato.Core.Enums
{
    /// <summary>
    ///
    /// </summary>
    public static class EnumMapHelper
    {
        /// <summary>
        /// Gets the raw value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string GetRawValue(Enum value)
        {
            var mi = value.GetType().GetMember(value.ToString());
            if (mi != null && mi.Length > 0)
            {
                var attributes = mi[0].GetCustomAttributes(typeof(EnumMapAttribute), false);
                if (attributes != null && attributes.Length > 0)
                {
                    var attribute = attributes[0] as EnumMapAttribute;
                    if (attribute != null)
                    {
                        return attribute.RawValue;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the enum value by raw.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rawValue">The raw value.</param>
        /// <returns></returns>
        public static T GetEnumValueByRaw<T>(string rawValue) where T: struct, IConvertible
        {
            var defVal = default(T);
            foreach (var mi in typeof(T).GetMembers(BindingFlags.Public | BindingFlags.Static))
            {
                var attributes = mi.GetCustomAttributes(typeof(EnumMapAttribute), false);
                if (attributes != null)
                {
                    var attribute = attributes[0] as EnumMapAttribute;
                    if (attribute != null && attributes.Length > 0)
                    {
                        if (attribute.IsDefault)
                        {
                            defVal = (T)typeof(T).GetField(mi.Name).GetValue(typeof(T));
                        }

                        if (attribute.RawValue == rawValue)
                        {
                            defVal = (T)typeof(T).GetField(mi.Name).GetValue(typeof(T));
                            break;
                        }
                    }
                }
            }

            return defVal;
        }

        /// <summary>
        /// Gets the name of the enum value by.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static T GetEnumValueByName<T>(string name) where T: struct, IConvertible
        {
            var defVal = default(T);
            foreach (var mi in typeof(T).GetMembers(BindingFlags.Public | BindingFlags.Static))
            {
                if (mi.Name == name)
                {
                    defVal = (T)typeof(T).GetField(mi.Name).GetValue(typeof(T));
                    break;
                }
            }

            return defVal;
        }
    }
}
