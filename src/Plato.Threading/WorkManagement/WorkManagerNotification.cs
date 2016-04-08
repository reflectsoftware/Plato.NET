// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.ExceptionManagement;
using Plato.Utils.Miscellaneous;
using Plato.Utils.Strings;
using System;
using System.Collections.Specialized;

namespace Plato.Threading.WorkManagement
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Threading.WorkManagement.Notification" />
    /// <seealso cref="System.IDisposable" />
    public class WorkManagerNotification : Notification, IDisposable
    {
        protected ExceptionManager ExceptionManagerContainer  { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="WorkManagerNotification"/> is disposed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if disposed; otherwise, <c>false</c>.
        /// </value>
        public bool Disposed { get; private set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="WorkManagerNotification"/> class.
        /// </summary>
        public WorkManagerNotification()
        {
            Disposed = false;
            ExceptionManagerContainer = new ExceptionManager(WorkManagerConfig.Container, "exception.publishers");
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

                    ExceptionManagerContainer?.Dispose();
                }
            }
        }

        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <param name="nType">Type of the n.</param>
        public override void SendMessage(string msg, Utils.Logging.Enums.NotificationType nType)
        {
            Console.WriteLine("{0}: [{1}]", msg, nType);
        }

        /// <summary>
        /// Sends the exception.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="bIgnoreTracker">if set to <c>true</c> [b ignore tracker].</param>
        public override void SendException(Exception ex, bool bIgnoreTracker = false)
        {
            var eventTracker = new TimeSpan(0, WorkManagerConfig.EventTracker, 0);

            if (bIgnoreTracker || TimeEventTracker.CanEvent((int)StringHash.BKDRHash(ex.Message), eventTracker))
            {
                // add additional information to the Exception Info.
                var additionalInfo = new NameValueCollection();
                additionalInfo.Add("TrackingId", Guid.NewGuid().ToString());
                additionalInfo.Add("Timestamp", DateTimeOffset.Now.ToString());
                additionalInfo.Add("Application Source", WorkManagerConfig.ApplicationName);

                ExceptionManagerContainer.Publish(ex, additionalInfo);
            }
        }
    }
}
