// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Utils.Logging.Enums;
using Plato.Utils.Logging.Interfaces;
using System;

namespace Plato.Utils.Logging
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="Plato.Utils.Logging.Interfaces.ILogNotification"/>
    public class LogNotification : ILogNotification
    {
        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <param name="nType">Type of the n.</param>
        /// <param name="ex">The ex.</param>
        public virtual void SendMessage(string msg, NotificationType nType, Exception ex)
        {
        }

        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <param name="nType">Type of the n.</param>
        public void SendMessage(string msg, NotificationType nType)
        {
            SendMessage(msg, nType, null);
        }

        /// <summary>
        /// Sends the exception.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="bIgnoreTracker">if set to <c>true</c> [b ignore tracker].</param>
        public virtual void SendException(Exception ex, bool bIgnoreTracker)
        {
        }

        /// <summary>
        /// Sends the exception.
        /// </summary>
        /// <param name="ex">The ex.</param>
        public void SendException(Exception ex)
        {
            SendException(ex, false);
        }

        /// <summary>
        /// Sends the debug.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <param name="args">The arguments.</param>
        public virtual void SendDebug(string msg, params object[] args)
        {
        }

        /// <summary>
        /// Sends the debug.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <param name="ex">The ex.</param>
        /// <param name="args">The arguments.</param>
        public virtual void SendDebug(string msg, Exception ex, params object[] args)
        {
        }

        /// <summary>
        /// Sends the information.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <param name="args">The arguments.</param>
        public virtual void SendInformation(string msg, params object[] args)
        {
        }

        /// <summary>
        /// Sends the information.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <param name="ex">The ex.</param>
        /// <param name="args">The arguments.</param>
        public virtual void SendInformation(string msg, Exception ex, params object[] args)
        {
        }

        /// <summary>
        /// Sends the warning.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <param name="args">The arguments.</param>
        public virtual void SendWarning(string msg, params object[] args)
        {
        }

        /// <summary>
        /// Sends the warning.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <param name="ex">The ex.</param>
        /// <param name="args">The arguments.</param>
        public virtual void SendWarning(string msg, Exception ex, params object[] args)
        {
        }

        /// <summary>
        /// Sends the error.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <param name="args">The arguments.</param>
        public virtual void SendError(string msg, params object[] args)
        {
        }

        /// <summary>
        /// Sends the error.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <param name="ex">The ex.</param>
        /// <param name="args">The arguments.</param>
        public virtual void SendError(string msg, Exception ex, params object[] args)
        {
        }

        /// <summary>
        /// Sends the fatal.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <param name="args">The arguments.</param>
        public virtual void SendFatal(string msg, params object[] args)
        {
        }

        /// <summary>
        /// Sends the fatal.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <param name="ex">The ex.</param>
        /// <param name="args">The arguments.</param>
        public virtual void SendFatal(string msg, Exception ex, params object[] args)
        {
        }
    }
}
