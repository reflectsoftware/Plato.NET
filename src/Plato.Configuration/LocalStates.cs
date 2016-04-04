// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Plato.Configuration
{
    /// <summary>
    /// LocalStates
    /// </summary>
    public class LocalStates
    {
        /// <summary>
        /// 
        /// </summary>
        private class Section
        {
            public string Name;
            public Collection<KeyValuePair<string, string>> Items;

            /// <summary>
            /// Initializes a new instance of the <see cref="Section"/> class.
            /// </summary>
            /// <param name="name">The name.</param>
            /// <param name="nvCol">The nv col.</param>
            public Section(string name, NameValueCollection nvCol)
            {
                Name = name;
                Items = new Collection<KeyValuePair<string, string>>();

                foreach (var key in nvCol.AllKeys)
                {
                    Items.Add(new KeyValuePair<string, string>(key, nvCol[key]));
                }
            }
        }

        private Hashtable _sections;
        private readonly string _fileName;
        private bool _forceSaveOnSet;

        /// <summary>
        /// Prevents a default instance of the <see cref="LocalStates"/> class from being created.
        /// </summary>
        private LocalStates()
        {
            _forceSaveOnSet = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalStates"/> class.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        internal LocalStates(string fileName) : this()
        {
            _fileName = fileName;
            Load();
        }

        /// <summary>
        /// Loads this instance.
        /// </summary>
        protected void Load()
        {
            lock (this)
            {
                _sections = new Hashtable();

                if (!File.Exists(_fileName))
                {
                    return;
                }

                var xDoc = XDocument.Load(_fileName);
                var sections = from section in xDoc.Elements("sections").Elements("section") select section;

                foreach (var secRecord in sections)
                {
                    var nv = new NameValueCollection();
                    _sections[secRecord.Attribute("name").Value] = nv;

                    var items = from item in secRecord.Elements("item") select item;

                    foreach (var itemRec in items)
                    {
                        nv[itemRec.Attribute("key").Value] = itemRec.Attribute("value").Value;
                    }
                }
            }
        }

        /// <summary>
        /// Saves this instance.
        /// </summary>
        public void Save()
        {
            SaveTo(_fileName);
        }

        /// <summary>
        /// Saves to.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        public void SaveTo(string fileName)
        {
            lock (this)
            {
                var sections = new Collection<Section>();
                foreach (string section in _sections.Keys)
                {
                    sections.Add(new Section(section, (NameValueCollection)_sections[section]));
                }

                var doc = new XDocument
                  (
                      new XDeclaration("1.0", null, "yes"),
                      new XElement("sections", from sect in sections
                                               select new XElement("section", new XAttribute("name", sect.Name),
                             from item in sect.Items
                             select new XElement("item", new XAttribute("key", item.Key), new XAttribute("value", item.Value)
                             )))
                     );

                using (var tw = File.CreateText(fileName))
                {
                    tw.WriteLine(doc.Declaration.ToString());
                    tw.WriteLine(doc);
                }
            }
        }

        /// <summary>
        /// Clears the specified section.
        /// </summary>
        /// <param name="section">The section.</param>
        public void Clear(string section)
        {
            lock (this)
            {
                _sections.Remove(section.ToLower());
            }
        }

        /// <summary>
        /// Clears all sections.
        /// </summary>
        public void ClearAllSections()
        {
            lock (this)
            {
                _sections.Clear();
            }
        }

        /// <summary>
        /// Gets the specified section.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public string Get(string section, string key, string defaultValue)
        {
            lock (this)
            {
                var rValue = defaultValue;
                var nvCol = (NameValueCollection)_sections[section.ToLower()];
                if (nvCol != null)
                {
                    rValue = nvCol[key.ToLower()];
                    if (rValue == null)
                    {
                        rValue = defaultValue;
                    }
                }

                return rValue;
            }
        }

        /// <summary>
        /// Gets the specified section.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public string Get(string section, string key)
        {
            return Get(section, key, null);
        }

        /// <summary>
        /// Sets the specified section.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void Set(string section, string key, string value)
        {
            lock (this)
            {
                if (value == null)
                {
                    return;
                }

                var nvCol = (NameValueCollection)_sections[section.ToLower()];
                if (nvCol == null)
                {
                    nvCol = new NameValueCollection();
                    _sections[section.ToLower()] = nvCol;
                }

                nvCol[key.ToLower()] = value;

                if (_forceSaveOnSet)
                {
                    Save();
                }
            }
        }

        /// <summary>
        /// Removes the specified section.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <param name="key">The key.</param>
        public void Remove(string section, string key)
        {
            lock (this)
            {
                var nvCol = (NameValueCollection)_sections[section.ToLower()];
                if (nvCol == null)
                {
                    return;
                }

                nvCol.Remove(key);
            }
        }

        /// <summary>
        /// Gets the section names.
        /// </summary>
        /// <returns></returns>
        public string[] GetSectionNames()
        {
            lock (this)
            {
                var keys = new List<string>();
                foreach (string key in _sections.Keys)
                {
                    keys.Add(key);
                }

                return keys.ToArray();
            }
        }

        /// <summary>
        /// Gets the key names.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <returns></returns>
        public string[] GetKeyNames(string section)
        {
            lock (this)
            {
                var nvCol = (NameValueCollection)_sections[section.ToLower()];

                return nvCol != null ? nvCol.AllKeys : new string[] { };
            }
        }

        /// <summary>
        /// Gets the values.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <returns></returns>
        public string[] GetValues(string section)
        {
            lock (this)
            {
                var rValues = new List<string>();

                var nvCol = (NameValueCollection)_sections[section.ToLower()];
                if (nvCol != null)
                {
                    foreach (string key in nvCol.AllKeys)
                    {
                        rValues.Add(nvCol[key]);
                    }
                }

                return rValues.ToArray();
                ;
            }
        }

        /// <summary>
        /// Gets the section collection.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <returns></returns>
        public NameValueCollection GetSectionCollection(string section)
        {
            lock (this)
            {
                return (NameValueCollection)_sections[section.ToLower()];
            }
        }

        /// <summary>
        /// Sets the section collection.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <param name="nvCol">The nv col.</param>
        public void SetSectionCollection(string section, NameValueCollection nvCol)
        {
            lock (this)
            {
                if (nvCol == null)
                {
                    return;
                }

                var curCol = (NameValueCollection)_sections[section.ToLower()];
                if (curCol == null)
                {
                    curCol = new NameValueCollection();
                    _sections[section.ToLower()] = curCol;
                }

                foreach (string key in nvCol.AllKeys)
                {
                    curCol[key.ToLower()] = nvCol[key];
                }

                if (_forceSaveOnSet)
                {
                    Save();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [force save on set].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [force save on set]; otherwise, <c>false</c>.
        /// </value>
        public bool ForceSaveOnSet
        {
            get
            {
                return _forceSaveOnSet;
            }
            set
            {
                _forceSaveOnSet = value;
                if (_forceSaveOnSet)
                {
                    Save();
                }
            }
        }
    }
}
