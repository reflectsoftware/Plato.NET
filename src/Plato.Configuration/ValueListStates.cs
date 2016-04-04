// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System.Collections.Generic;

namespace Plato.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    public class ValueListStates
    {
        private LocalStates _states;
        private string _section;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueListStates"/> class.
        /// </summary>
        /// <param name="states">The states.</param>
        /// <param name="section">The section.</param>
        public ValueListStates(LocalStates states, string section)
        {
            _states = states;
            _section = section;
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            _states.Clear(_section);
        }

        /// <summary>
        /// Gets the value keys.
        /// </summary>
        /// <returns></returns>
        public string[] GetValueKeys()
        {
            return _states.GetKeyNames(_section);
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public string GetValue(string key)
        {
            return _states.Get(_section, key);
        }

        /// <summary>
        /// Gets the values.
        /// </summary>
        /// <returns></returns>
        public List<string> GetValues()
        {
            return new List<string>(_states.GetValues(_section));
        }

        /// <summary>
        /// Removes the value.
        /// </summary>
        /// <param name="key">The key.</param>
        public void RemoveValue(string key)
        {
            _states.Remove(_section, key);
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void SetValue(string key, string value)
        {
            _states.Set(_section, key, value);
        }

        /// <summary>
        /// Saves the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        public void Save(List<string> values)
        {
            _states.ForceSaveOnSet = false;
            try
            {
                Clear();
                for (var i = 0; i < values.Count; i++)
                {
                    _states.Set(_section, i.ToString(), values[i]);
                }
            }
            finally
            {
                _states.ForceSaveOnSet = true;
            }
        }
    }
}
