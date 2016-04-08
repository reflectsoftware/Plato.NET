// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.ClassFactory;
using Plato.Configuration;
using Plato.Configuration.Interfaces;
using Plato.ExceptionManagement.Enums;
using Plato.ExceptionManagement.Interfaces;
using Plato.Utils.Miscellaneous;
using Plato.Utils.Strings;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;

namespace Plato.ExceptionManagement
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.ExceptionManagement.Interfaces.IExceptionManager" />
    /// <seealso cref="Plato.ExceptionManagement.Interfaces.IExceptionManagerExtension" />
    public class ExceptionManager : IExceptionManager, IExceptionManagerExtension
    {
        protected class UnhandledExceptionInfo
        {
            public NodeAttributes PublisherAttributes { get; internal set; }
            public Exception Exception { get; internal set; }
        }

        protected string _eventLogSource;
        protected List<PublisherInfo> _publishers;
        protected List<UnhandledExceptionInfo> _unhandledExceptions;
        protected IConfigContainer _configContainer;
        protected string _configSection;
        protected TimeSpan _eventTracking;

        /// <summary>
        /// Gets a value indicating whether this <see cref="ExceptionManager"/> is disposed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if disposed; otherwise, <c>false</c>.
        /// </value>
        public bool Disposed { get; private set; }

        /// <summary>
        /// Gets the mode.
        /// </summary>
        /// <value>
        /// The mode.
        /// </value>
        public PublisherManagerMode Mode { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionManager"/> class.
        /// </summary>
        /// <param name="configContainer">The configuration container.</param>
        /// <param name="configSection">The configuration section.</param>
        public ExceptionManager(IConfigContainer configContainer, string configSection)
        {
            Disposed = false;
            _unhandledExceptions = new List<UnhandledExceptionInfo>();
            _publishers = new List<PublisherInfo>();
            _eventTracking = new TimeSpan();
            _configSection = configSection;
            _configContainer = configContainer;
            _configContainer.OnConfigChange += OnConfigChange;

            LoadPublishers();
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="ExceptionManager"/> class.
        /// </summary>
        ~ExceptionManager()
        {
            Dispose(false);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="bDisposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool bDisposing)
        {
            lock (this)
            {
                if (!Disposed)
                {
                    Disposed = true;
                    if (bDisposing)
                    {
                        GC.SuppressFinalize(this);
                    }

                    FreePublishers();
                    _configContainer.OnConfigChange -= OnConfigChange;
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
        /// Loads the publishers.
        /// </summary>
        public void LoadPublishers()
        {
            lock (_publishers)
            {
                var nodeChildAttributes = ConfigHelper.GetNodeChildAttributes(_configContainer, _configSection);

                _eventLogSource = StringHelper.IfNullOrEmptyUseDefault(nodeChildAttributes.ParentAttributes.Attributes["eventSource"], "Application");

                var similarTimeStr = StringHelper.IfNullOrEmptyUseDefault(nodeChildAttributes.ParentAttributes.Attributes["eventTracking"], "0");
                var similarTime = 0;
                int.TryParse(similarTimeStr, out similarTime);

                _eventTracking = new TimeSpan(0, similarTime, 0);

                var mode = StringHelper.IfNullOrEmptyUseDefault(nodeChildAttributes.ParentAttributes.Attributes["mode"], "on");
                Mode = mode == "off" ? PublisherManagerMode.Off : PublisherManagerMode.On;

                ReloadPublishers(nodeChildAttributes);
            }
        }

        /// <summary>
        /// Frees the publishers.
        /// </summary>
        public void FreePublishers()
        {
            lock (_publishers)
            {
                foreach (PublisherInfo publisher in _publishers)
                {
                    MiscHelper.DisposeObject(publisher.Publisher);
                }
                _publishers.Clear();
                _publishers.Capacity = 0;
            }
        }

        /// <summary>
        /// Reloads the publishers.
        /// </summary>
        /// <param name="nodeChildAttributes">The node child attributes.</param>
        protected void ReloadPublishers(NodeChildAttributes nodeChildAttributes)
        {
            lock (_publishers)
            {
                FreePublishers();

                if (Mode == PublisherManagerMode.Off)
                {
                    return;
                }

                var factory = new ProviderClassFactory(nodeChildAttributes, "publisher");

                foreach (NodeAttributes attribute in nodeChildAttributes.ChildAttributes)
                {
                    if (StringHelper.IfNullOrEmptyUseDefault(attribute.Attributes["mode"], "on") != "on")
                    {
                        continue;
                    }

                    var publisher = factory.CreateInstance<IExceptionPublisher>(attribute.Attributes["name"]);
                    if (publisher == null)
                    {
                        continue;
                    }

                    var newPublisher = new PublisherInfo()
                    {
                        PublisherAttributes = attribute,
                        Publisher = publisher
                    };

                    _publishers.Add(newPublisher);
                }
            }
        }

        /// <summary>
        /// Called when [configuration change].
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="cType">Type of the c.</param>
        protected void OnConfigChange(IConfigContainer container, Plato.Configuration.Enums.OnChangeType cType)
        {
            LoadPublishers();
        }

        /// <summary>
        /// Publishes the specified ex.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="additionalParameters">The additional parameters.</param>
        public void Publish(Exception ex, NameValueCollection additionalParameters)
        {
            int occurrences;
            if (!TimeEventTracker.CanEvent(ex.Message, _eventTracking, out occurrences))
            {
                return;
            }

            if (additionalParameters == null)
            {
                additionalParameters = new NameValueCollection();
            }

            additionalParameters["TrackingId"] = Guid.NewGuid().ToString();

            if (occurrences != 0)
            {
                additionalParameters["Occurrences"] = occurrences.ToString("N0");
            }

            lock (_publishers)
            {
                try
                {
                    try
                    {
                        _unhandledExceptions.Clear();
                        _unhandledExceptions.Capacity = 0;

                        foreach (PublisherInfo publisher in _publishers.ToArray())
                        {
                            try
                            {
                                publisher.Publisher.Publish(ex, additionalParameters);
                            }
                            catch (Exception unhandledEx)
                            {
                                MiscHelper.DisposeObject(publisher);
                                _publishers.Remove(publisher);

                                var unEx = new UnhandledExceptionInfo()
                                {
                                    PublisherAttributes = publisher.PublisherAttributes,
                                    Exception = unhandledEx
                                };

                                _unhandledExceptions.Add(unEx);
                            }
                        }
                        
                        foreach (UnhandledExceptionInfo eInfo in _unhandledExceptions)
                        {
                            var eMsg = string.Format("The following Publisher: '{0}' caused an Unhandled exception", eInfo.PublisherAttributes.Attributes["name"]);
                            var internalEx = new Exception(eMsg, eInfo.Exception);

                            var exMessage = ExceptionFormatter.ConstructMessage(internalEx);
                            MiscHelper.WriteToEventLog(_eventLogSource, exMessage, EventLogEntryType.Error);
                        }
                    }
                    catch (Exception excep)
                    {
                        var exMessage = ExceptionFormatter.ConstructMessage(excep);
                        MiscHelper.WriteToEventLog(_eventLogSource, exMessage, EventLogEntryType.Error);
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        /// <summary>
        /// Publishes the specified ex.
        /// </summary>
        /// <param name="ex">The ex.</param>
        public void Publish(Exception ex)
        {
            Publish(ex, new NameValueCollection());
        }

        /// <summary>
        /// Removes the type of the publisher by.
        /// </summary>
        /// <param name="pType">Type of the p.</param>
        public void RemovePublisherByType(Type pType)
        {
            lock (_publishers)
            {
                foreach (PublisherInfo pInfo in _publishers)
                {
                    if (pInfo.Publisher.GetType() == pType)
                    {
                        _publishers.Remove(pInfo);
                        MiscHelper.DisposeObject(pInfo.Publisher);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Removes the name of the publisher by.
        /// </summary>
        /// <param name="name">The name.</param>
        public void RemovePublisherByName(string name)
        {
            lock (_publishers)
            {
                foreach (PublisherInfo pInfo in _publishers)
                {
                    if (pInfo.PublisherAttributes.Attributes["name"] == name)
                    {
                        _publishers.Remove(pInfo);
                        MiscHelper.DisposeObject(pInfo.Publisher);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Adds the publisher.
        /// </summary>
        /// <param name="publisher">The publisher.</param>
        /// <param name="parameters">The parameters.</param>
        public void AddPublisher(IExceptionPublisher publisher, NameValueCollection parameters)
        {
            lock (_publishers)
            {
                var attributes = new NodeAttributes() { NodeName = "publisher" };
                attributes.Attributes.Add(parameters);

                _publishers.Add(new PublisherInfo()
                {
                    PublisherAttributes = attributes,
                    Publisher = publisher
                });
            }
        }

        /// <summary>
        /// Adds the publisher.
        /// </summary>
        /// <param name="publisher">The publisher.</param>
        public void AddPublisher(IExceptionPublisher publisher)
        {
            AddPublisher(publisher, new NameValueCollection());
        }

        /// <summary>
        /// Gets the publisher count.
        /// </summary>
        /// <value>
        /// The publisher count.
        /// </value>
        public int PublisherCount
        {
            get
            {
                lock (_publishers)
                {
                    return _publishers.Count;
                }
            }
        }

        /// <summary>
        /// Gets the publisher infos.
        /// </summary>
        /// <value>
        /// The publisher infos.
        /// </value>
        public PublisherInfo[] PublisherInfos
        {
            get
            {
                lock (_publishers)
                {
                    return _publishers.ToArray();
                }
            }
        }

        /// <summary>
        /// Gets the publishers.
        /// </summary>
        /// <value>
        /// The publishers.
        /// </value>
        public IExceptionPublisher[] Publishers
        {
            get
            {
                lock (_publishers)
                {
                    return _publishers.Cast<IExceptionPublisher>().ToArray();
                }
            }
        }
    }
}
