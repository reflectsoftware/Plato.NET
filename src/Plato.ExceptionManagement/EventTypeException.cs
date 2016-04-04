// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Plato.ExceptionManagement
{
    /// <summary>
    /// This class will mainly be used by the ExceptiobnEventPublisher when
    /// the caller calls ExceptionManager.Publish( e )
    /// where e = EventTypeException
    /// </summary>
    [Serializable]
    public class EventTypeException : ApplicationException, ISerializable
    {
        protected EventLogEntryType FEventLogType = EventLogEntryType.Error;

        /// <summary>
        /// Gets or sets the incident.
        /// </summary>
        /// <value>
        /// The incident.
        /// </value>
        public int Incident { get; set; }

        /// <summary>
        /// Gets or sets the type of the event log.
        /// </summary>
        /// <value>
        /// The type of the event log.
        /// </value>
        public EventLogEntryType EventLogType { get; set; }

        /// <summary>
        /// Generates the incident.
        /// </summary>
        /// <param name="maxRange">The maximum range.</param>
        /// <returns></returns>
        protected static int GenerateIncident( int maxRange )
        {
            var rNum = new Random( DateTime.Now.Millisecond );
            return rNum.Next(1, maxRange);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="EventTypeException"/> class.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <param name="eType">Type of the e.</param>
        /// <param name="incidentMaxRange">The incident maximum range.</param>
        public EventTypeException( string msg, EventLogEntryType eType, int incidentMaxRange = 65535) : base( msg )
        {
            EventLogType = eType;
            Incident = GenerateIncident(incidentMaxRange);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventTypeException"/> class.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <param name="eType">Type of the e.</param>
        /// <param name="incidentMaxRange">The incident maximum range.</param>
        /// <param name="innerException">The inner exception.</param>
        public EventTypeException( string msg, EventLogEntryType eType, int incidentMaxRange, Exception innerException ) : base( msg, innerException )
        {
            EventLogType = eType;
            Incident = GenerateIncident(incidentMaxRange);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventTypeException"/> class.
        /// </summary>
        /// <param name="info">The information.</param>
        /// <param name="context">The context.</param>
        public EventTypeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            Incident = info.GetInt32("Incident");
            EventLogType = (EventLogEntryType)info.GetInt32("EventLogType");
        }
        /// <summary>
        /// Gets the object data.
        /// </summary>
        /// <param name="info">The information.</param>
        /// <param name="context">The context.</param>
        void ISerializable.GetObjectData( SerializationInfo info, StreamingContext context )
        {
            info.AddValue("Incident", Incident);
            info.AddValue("EventLogType", (int)FEventLogType);
            base.GetObjectData( info, context );
        }
    }
}
