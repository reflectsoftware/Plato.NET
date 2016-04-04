// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System.Collections;

namespace Plato.Configuration
{
    /// <summary>
    /// LocalStatesManager
    /// </summary>
    public static class LocalStatesManager
    {
        private readonly static Hashtable _LocalStatesCollection;

        /// <summary>
        /// Initializes the <see cref="LocalStatesManager"/> class.
        /// </summary>
        static LocalStatesManager()
        {
            _LocalStatesCollection = new Hashtable();
        }

        /// <summary>
        /// Gets the specified file name.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public static LocalStates Get(string fileName)
        {
            lock (_LocalStatesCollection)
            {
                var rValue = (LocalStates)_LocalStatesCollection[fileName.ToLower()];
                if (rValue == null)
                {
                    rValue = new LocalStates(fileName);
                    _LocalStatesCollection[fileName.ToLower()] = rValue;
                }

                return rValue;
            }
        }
    }
}
