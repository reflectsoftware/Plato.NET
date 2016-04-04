// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System;
using System.Collections;
using System.Collections.Generic;

namespace Plato.Utils.Miscellaneous
{
    /// <summary>
    ///
    /// </summary>
    public static class TimeEventTracker
    {
        private class EventInfo
        {
            public DateTime LastDateTime { get; set; }
            public TimeSpan LastEventDuration { get; set; }
            public int Occurrences { get; set; }
        }

        private readonly static Hashtable _tracker;
        private static DateTime _lastExpiredCheckDateTime;


        /// <summary>
        /// Initializes the <see cref="TimeEventTracker"/> class.
        /// </summary>
        static TimeEventTracker()
        {
            _tracker = new Hashtable();
            _lastExpiredCheckDateTime = DateTime.Now;
        }

        /// <summary>
        /// Removes the expired events.
        /// </summary>
        private static void RemoveExpiredEvents()
        {
            lock (_tracker)
            {
                if (DateTime.Now.Subtract(_lastExpiredCheckDateTime).TotalMinutes >= 2)
                {
                    var removeEventIds = new List<object>();
                    foreach (var eventId in _tracker.Keys)
                    {
                        var eInfo = (EventInfo)_tracker[eventId];
                        var lastCheckedTimeSpan = LastCheckedTimeSpan(eventId);

                        if (lastCheckedTimeSpan >= eInfo.LastEventDuration.Add(TimeSpan.FromMinutes(5)))
                        {
                            removeEventIds.Add(eventId);
                        }
                    }

                    foreach (var eventId in removeEventIds)
                    {
                        _tracker.Remove(eventId);
                    }

                    _lastExpiredCheckDateTime = DateTime.Now;
                }
            }
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public static void Clear()
        {
            lock (_tracker)
            {
                _tracker.Clear();
            }
        }

        /// <summary>
        /// Lasts the checked time span.
        /// </summary>
        /// <param name="eventID">The event identifier.</param>
        /// <returns></returns>
        public static TimeSpan LastCheckedTimeSpan(object eventID)
        {
            lock (_tracker)
            {
                var lastDateTime = DateTime.Now;
                var eInfo = (EventInfo)_tracker[eventID];

                if (eInfo != null)
                {
                    lastDateTime = eInfo.LastDateTime;
                }

                return DateTime.Now.Subtract(lastDateTime);
            }
        }

        /// <summary>
        /// Determines whether this instance can event the specified event identifier.
        /// </summary>
        /// <param name="eventID">The event identifier.</param>
        /// <param name="eventDuration">Duration of the event.</param>
        /// <param name="occurrences">The occurrences.</param>
        /// <returns></returns>
        public static bool CanEvent(object eventID, TimeSpan eventDuration, out int occurrences)
        {
            if (eventDuration.TotalMinutes == 0)
            {
                occurrences = 0;
                return true;
            }

            lock (_tracker)
            {
                RemoveExpiredEvents();

                var eInfo = (EventInfo)_tracker[eventID];
                if (eInfo == null)
                {
                    eInfo = new EventInfo() { LastDateTime = DateTime.Now, LastEventDuration = eventDuration, Occurrences = 1 };                    
                    occurrences = eInfo.Occurrences;
                    _tracker.Add(eventID, eInfo);
                    return true;
                }

                eInfo.Occurrences++;
                occurrences = eInfo.Occurrences;

                var  lastCheckedTimeSpan = LastCheckedTimeSpan(eventID);
                if (lastCheckedTimeSpan >= eventDuration)
                {
                    eInfo.LastEventDuration = eventDuration;
                    eInfo.LastDateTime = DateTime.Now;
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Determines whether this instance can event the specified event identifier.
        /// </summary>
        /// <param name="eventID">The event identifier.</param>
        /// <param name="eventDuration">Duration of the event.</param>
        /// <returns></returns>
        public static bool CanEvent(object eventID, TimeSpan eventDuration)
        {
            int occurrences;
            return CanEvent(eventID, eventDuration, out occurrences);
        }
    }
}
