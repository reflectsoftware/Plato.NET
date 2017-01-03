// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System;
using System.Collections.Generic;

namespace Plato.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    public class ValueEnumListStates
    {
        private readonly ValueListStates _valueStates;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueEnumListStates"/> class.
        /// </summary>
        /// <param name="states">The states.</param>
        /// <param name="section">The section.</param>
        public ValueEnumListStates(LocalStates states, string section)
        {
            _valueStates = new ValueListStates(states, section);
        }

        /// <summary>
        /// Saves the specified list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        public void Save<T>(T[] list)
        {
            var sList = new List<string>();
            foreach (T t in list)
            {
                sList.Add(t.ToString());
            }

            _valueStates.Save(sList);
        }

        /// <summary>
        /// Gets the values.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T[] GetValues<T>()
        {
            var tList = new List<T>();

            foreach (string value in _valueStates.GetValues())
            {
                tList.Add((T)Enum.Parse(typeof(T), value, true));
            }

            return tList.ToArray();
        }
    }
}
