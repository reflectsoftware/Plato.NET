// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Configuration.Enums;
using Plato.Configuration.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Plato.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Configuration.Interfaces.ISectionContentContainer" />
    /// <seealso cref="System.IDisposable" />
    public class SectionContentContainer : ISectionContentContainer, IDisposable
    {
        private IConfigContainer _config;
        private readonly Dictionary<string, Dictionary<string, string>> _sections;
        private ReaderWriterLockSlim _sectionsLock;

        /// <summary>
        /// Gets a value indicating whether this <see cref="SectionContentContainer"/> is disposed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if disposed; otherwise, <c>false</c>.
        /// </value>
        public bool Disposed { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SectionContentContainer"/> class.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="bDetectChange">if set to <c>true</c> [b detect change].</param>
        public SectionContentContainer(string path, bool bDetectChange = false)
        {
            Disposed = false;
            _sections = new Dictionary<string, Dictionary<string, string>>();
            _sectionsLock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

            _config = ConfigManager.GetConfiguration(path, "./configuration");

            if (bDetectChange)
            {
                _config.WatchAllowOnChangeAttribute(".", "reactOnConfigChange", "true");
                _config.OnConfigChange += OnConfigChange;
            }

            ReadSections();
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="SectionContentContainer"/> class.
        /// </summary>
        ~SectionContentContainer()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool bDisposing)
        {
            lock (this)
            {
                if (!Disposed)
                {
                    Disposed = true;
                    GC.SuppressFinalize(this);

                    if (bDisposing)
                    {
                        if (_config != null)
                        {
                            _config.OnConfigChange -= OnConfigChange;
                            ConfigManager.Remove(_config);
                            _config = null;
                        }

                        _sectionsLock?.Dispose();
                    }
                }
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Called when [configuration change].
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="cType">Type of the c.</param>
        private void OnConfigChange(IConfigContainer container, OnChangeType cType)
        {
            ReadSections();
        }

        /// <summary>
        /// Reads the sections.
        /// </summary>
        private void ReadSections()
        {
            _sectionsLock.EnterWriteLock();
            try
            {
                _sections.Clear();

                var sectionNodes =  _config.Node.GetConfigNodes("./section");
                foreach (var sNode in sectionNodes)
                {
                    var sectionId = sNode.GetAttribute(".", "id", null);
                    if (string.IsNullOrWhiteSpace(sectionId))
                    {
                        continue;
                    }

                    if (_sections.ContainsKey(sectionId))
                    {
                        _sections.Remove(sectionId);
                    }

                    var contents = new Dictionary<string, string>();
                    _sections.Add(sectionId, contents);

                    var contentNodes = sNode.GetConfigNodes("./content");
                    foreach (var stNode in contentNodes)
                    {
                        var contentId = stNode.GetAttribute(".", "id", null);
                        if (string.IsNullOrWhiteSpace(contentId))
                        {
                            continue;
                        }

                        var content = stNode.GetNodeInnerText(".");
                        if (string.IsNullOrWhiteSpace(content))
                        {
                            continue;
                        }

                        if (contents.ContainsKey(contentId))
                        {
                            contents.Remove(contentId);
                        }

                        contents.Add(contentId, content);
                    }
                }
            }
            finally
            {
                _sectionsLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Filters the specified content.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        private static string Filter(string content)
        {
            var sbContent = new StringBuilder();
            var contentParts = content.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var contentPart in contentParts)
            {
                var part = contentPart.Replace('\r', ' ');
                if (string.IsNullOrWhiteSpace(part))
                {
                    continue;
                }

                sbContent.AppendLine(part.Trim());
            }

            return sbContent.ToString();
        }

        /// <summary>
        /// Gets the content.
        /// </summary>
        /// <param name="sectionId">The section identifier.</param>
        /// <param name="contentId">The content identifier.</param>
        /// <param name="bFilter">if set to <c>true</c> [b filter].</param>
        /// <param name="bThrowOnMissing">if set to <c>true</c> [b throw on missing].</param>
        /// <returns></returns>
        /// <exception cref="System.ApplicationException"></exception>
        public string GetContent(string sectionId, string contentId, bool bFilter, bool bThrowOnMissing)
        {
            _sectionsLock.EnterReadLock();
            try
            {
                string rContent = null;

                if (_sections.ContainsKey(sectionId))
                {
                    var contents = _sections[sectionId];
                    if (contents.ContainsKey(contentId))
                    {
                        rContent = contents[contentId];
                    }
                }

                if (string.IsNullOrWhiteSpace(rContent))
                {
                    if (bThrowOnMissing)
                    {
                        throw new ApplicationException(string.Format("SectionContentContainer: Missing Content for section id: '{0}', for content id: '{1}'", sectionId, contentId));
                    }
                }
                else
                {
                    if (bFilter)
                    {
                        rContent = Filter(rContent);
                    }
                }

                return rContent;
            }
            finally
            {
                _sectionsLock.ExitReadLock();
            }
        }

        /// <summary>
        /// Gets the content.
        /// </summary>
        /// <param name="sectionId">The section identifier.</param>
        /// <param name="contentId">The content identifier.</param>
        /// <param name="bFilter">if set to <c>true</c> [b filter].</param>
        /// <returns></returns>
        public string GetContent(string sectionId, string contentId, bool bFilter)
        {
            return GetContent(sectionId, contentId, bFilter, false);
        }

        /// <summary>
        /// Gets the content.
        /// </summary>
        /// <param name="sectionId">The section identifier.</param>
        /// <param name="contentId">The content identifier.</param>
        /// <returns></returns>
        public string GetContent(string sectionId, string contentId)
        {
            return GetContent(sectionId, contentId, true, false);
        }
    }
}
