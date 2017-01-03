// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Core.Logging.Enums;
using System;

namespace Plato.Core.Logging.Interfaces
{
    /// <summary>
    ///
    /// </summary>
    public interface ILogNotification
    {
        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <param name="nType">Type of the n.</param>
        void SendMessage(string msg, NotificationType nType);

        /// <summary>
        /// Sends the exception.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="bIgnoreTracker">if set to <c>true</c> [b ignore tracker].</param>
        void SendException(Exception ex, bool bIgnoreTracker = false);

        /// <summary>
        /// Sends the debug.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <param name="args">The arguments.</param>
        void SendDebug(string msg, params object[] args);

        /// <summary>
        /// Sends the debug.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <param name="ex">The ex.</param>
        /// <param name="args">The arguments.</param>
        void SendDebug(string msg, Exception ex, params object[] args);

        /// <summary>
        /// Sends the information.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <param name="args">The arguments.</param>
        void SendInformation(string msg, params object[] args);

        /// <summary>
        /// Sends the information.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <param name="ex">The ex.</param>
        /// <param name="args">The arguments.</param>
        void SendInformation(string msg, Exception ex, params object[] args);

        /// <summary>
        /// Sends the warning.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <param name="args">The arguments.</param>
        void SendWarning(string msg, params object[] args);

        /// <summary>
        /// Sends the warning.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <param name="ex">The ex.</param>
        /// <param name="args">The arguments.</param>
        void SendWarning(string msg, Exception ex, params object[] args);

        /// <summary>
        /// Sends the error.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <param name="args">The arguments.</param>
        void SendError(string msg, params object[] args);

        /// <summary>
        /// Sends the error.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <param name="ex">The ex.</param>
        /// <param name="args">The arguments.</param>
        void SendError(string msg, Exception ex, params object[] args);

        /// <summary>
        /// Sends the fatal.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <param name="args">The arguments.</param>
        void SendFatal(string msg, params object[] args);

        /// <summary>
        /// Sends the fatal.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <param name="ex">The ex.</param>
        /// <param name="args">The arguments.</param>
        void SendFatal(string msg, Exception ex, params object[] args);
    }
}
