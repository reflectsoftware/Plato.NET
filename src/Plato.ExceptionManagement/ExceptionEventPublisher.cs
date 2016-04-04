// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.ExceptionManagement.Interfaces;
using Plato.Utils.Miscellaneous;
using Plato.Utils.Strings;
using System;
using System.Collections.Specialized;
using System.Diagnostics;

namespace Plato.ExceptionManagement
{
    /// <summary>
    /// Note: It is assumed that the Microsoft.ApplicationBlocks.ExceptionManagement will 
    /// be used.
    /// To take advantage of this Exception Publisher, the following attributes
    /// must be added to the app/web config file.
    /// <platoSetting>
    /// <exceptionManagement mode="on">
    /// <publisher mode="on"name="EventPublisher"type="Plato.Utils.ExceptionManagement.ExceptionEventPublisher, Plato.Utils.ExceptionManagement"eventSource="Your Event Source"/>
    /// </exceptionManagement>	
    /// </platoSetting>
    ///
    /// In addition to this section a configuration sections needs to be defined as well.
    /// <configSections>    
    /// <section name="platoSettings"type="Plato.System.Configuration.ConfigurationHandler, Plato.System.Configuration"/>		
    /// </configSections>
    ///
    /// When working with IIS, it's best to create the actual event source. See CMD below, to do this:
    /// eventcreate /ID 1 /L APPLICATION /T ERROR  /SO MYEVENTSOURCE /D "My first log"
    /// </summary>
    public class ExceptionEventPublisher : IExceptionPublisher
    {
        private readonly string _eventSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionEventPublisher"/> class.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <param name="args">The arguments.</param>
        public ExceptionEventPublisher(NameValueCollection parameters, params object[] args)
        {
            _eventSource = "Application";
            if (!string.IsNullOrWhiteSpace(parameters["eventSource"]) && parameters["eventSource"].Length > 0)
            {
                _eventSource = parameters["eventSource"];
            }
        }
        /// <summary>
        /// Publishes the specified exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="additionalParameters">The additional parameters.</param>
        public void Publish(Exception exception, NameValueCollection additionalParameters)
        {
            var entry = ExceptionFormatter.ConstructMessage(exception, additionalParameters);
            MiscHelper.WriteToEventLog(_eventSource, entry, EventLogEntryType.Error);
        }
    }
}


