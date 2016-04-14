// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.ExceptionManagement.Interfaces;
using Plato.Utils.Interfaces;
using Plato.Utils.Miscellaneous;
using Plato.Utils.Strings;
using System;
using System.Collections.Specialized;

namespace Plato.ExceptionManagement
{
    /// <summary>
    /// To take advantage of this Exception Publisher, the following attributes
    /// must be added to the app/web config file.
    /// <platoSetting>
    /// <exceptionManagement mode="on">
    /// <publisher mode="on"name="TextFilePublisher"type="Plato.ExceptionManagement.ExceptionLogTextFilePublisher, Plato.ExceptionManagement"recycle="5"filePath="$(workingdir)\Logs\ri_exceptions.txt"/>
    /// </exceptionManagement>	
    /// </platoSetting>
    ///
    /// In addition to this section a configuration sections needs to be defined as well.
    /// <configSections>    
    /// <section name="platoSetting"type="Plato.Configuration.ConfigurationHandler, Plato.Configuration"/>		
    /// </configSections>
    ///
    /// </summary>
    public class ExceptionLogTextFilePublisher : IExceptionPublisher, IDisposable
    {
        private readonly static string _separator;

        private string _filePath;
        private int _recycle;
        private ILogTextFileWriter _logTextFileWriter;

        /// <summary>
        /// Gets a value indicating whether this <see cref="ExceptionLogTextFilePublisher"/> is disposed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if disposed; otherwise, <c>false</c>.
        /// </value>
        public bool Disposed { get; private set; }

        /// <summary>
        /// Initializes the <see cref="ExceptionLogTextFilePublisher"/> class.
        /// </summary>
        static ExceptionLogTextFilePublisher()
        {
            _separator = string.Format("{0,80}", string.Empty).Replace(" ", "_");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionLogTextFilePublisher"/> class.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <param name="args">The arguments.</param>
        public ExceptionLogTextFilePublisher(NameValueCollection parameters, params object[] args)
        {
            Disposed = false;

            _recycle = 0;
            int.TryParse(parameters["recycle"], out _recycle);
            if (_recycle < 0)
            {
                _recycle = 0;
            }

            _filePath = parameters["filePath"];

            CreateTextFileWriter(_filePath, _recycle);
        }

        /// <summary>
        /// Creates the text file writer.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="recycle">The recycle.</param>
        protected void CreateTextFileWriter(string filePath, int recycle)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                return;
            }

            if (_logTextFileWriter == null)
            {
                _logTextFileWriter = new LogTextFileWriter(filePath, recycle, true);
            }
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

                    _logTextFileWriter?.Dispose();
                    _logTextFileWriter = null;
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
                    if (_logTextFileWriter != null)
                    {
                        var entry = ExceptionFormatter.ConstructMessage(exception, additionalParameters);
                        _logTextFileWriter.Write("{1}{0}{2}{0}", Environment.NewLine, entry, _separator);
                    }
                }
            }
            catch (Exception)
            {
            }
        }
    }
}


