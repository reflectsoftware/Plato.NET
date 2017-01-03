// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.ClassFactory;
using Plato.ExceptionManagement.Interfaces;
using System;
using System.Collections.Specialized;

namespace Plato.ExceptionManagement
{

    /// <summary>
    /// To take advantage of this Exception Publisher, the following attributes
    /// must be added to the app/web.config file.
    /// <platoSettings>
    /// <exceptionManagement mode="on">
    /// <publisher mode="on"name="LogPublisher"type="Plato.ExceptionManagement.ExceptionLogPublisher, Plato.ExceptionManagement"logType="MyCompany.Logging.ExceptionLogPublisher, MyCompany.Logging"/>    
    /// </exceptionManagement>	
    /// </platoSettings>
    ///
    /// In addition to this section a configuration sections needs to be defined as well.
    /// <configSections>    
    /// <section name="platoSettings"type="Plato.Configuration.ConfigurationHandler, Plato.Configuration"/>		
    /// </configSections>
    ///
    /// </summary>
    public class ExceptionLogPublisher : IExceptionPublisher, IDisposable
    {
        private IExceptionLogPublisher _exceptionLogPublisher;
        /// <summary>
        /// Gets a value indicating whether this <see cref="ExceptionLogPublisher"/> is disposed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if disposed; otherwise, <c>false</c>.
        /// </value>
        public bool Disposed { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionLogPublisher"/> class.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <param name="args">The arguments.</param>
        /// <exception cref="System.ArgumentException">logType</exception>
        /// <exception cref="TypeAccessException"></exception>
        public ExceptionLogPublisher(NameValueCollection parameters, params object[] args)
        {
            Disposed = false;
            _exceptionLogPublisher = null;

            var type = parameters["logType"];
            if (string.IsNullOrWhiteSpace(type))
            {
                throw new ArgumentException(string.Format("ExceptionLogPublisher: cannot find logType attribute for the following named instance: '{0}'", parameters["name"]), "logType");
            }

            var objectType = Type.GetType(type);
            if (objectType == null)
            {
                throw new TypeAccessException(string.Format("Unable to locate class type '{0}' for the following named instance: '{1}'", type, parameters["name"]));
            }

            _exceptionLogPublisher = ClassFactoryActivator.CreateInstance<IExceptionLogPublisher>(objectType, parameters);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            lock (this)
            {
                if (!Disposed)
                {
                    Disposed = true;
                    GC.SuppressFinalize(this);

                    _exceptionLogPublisher?.Dispose();
                    _exceptionLogPublisher = null;
                }
            }
        }
        /// <summary>
        /// Publishes the specified exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="additionalParameters">The additional parameters.</param>
        public void Publish(Exception exception, NameValueCollection additionalParameters)
        {
            try
            {
                lock (this)
                {
                    _exceptionLogPublisher?.LogException(exception);
                }
            }
            catch (Exception)
            {
            }
        }
    }
}


