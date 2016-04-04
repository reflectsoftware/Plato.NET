// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Windows.Forms;

namespace Plato.Utils.Miscellaneous
{
    /// <summary>
    /// Summary description for MiscHelper.
    /// </summary>
    public static class MiscHelper
    {
        /// <summary>
        /// Determines whether [is standard type] [the specified typ].
        /// </summary>
        /// <param name="typ">The typ.</param>
        /// <returns></returns>
        public static bool IsStandardType(Type typ)
        {
            if (typ == typeof(bool))
            {
                return true;
            }
            if (typ == typeof(byte))
            {
                return true;
            }
            if (typ == typeof(sbyte))
            {
                return true;
            }
            if (typ == typeof(char))
            {
                return true;
            }
            if (typ == typeof(decimal))
            {
                return true;
            }
            if (typ == typeof(double))
            {
                return true;
            }
            if (typ == typeof(float))
            {
                return true;
            }
            if (typ == typeof(int))
            {
                return true;
            }
            if (typ == typeof(uint))
            {
                return true;
            }
            if (typ == typeof(long))
            {
                return true;
            }
            if (typ == typeof(ulong))
            {
                return true;
            }
            if (typ == typeof(short))
            {
                return true;
            }
            if (typ == typeof(ushort))
            {
                return true;
            }
            if (typ == typeof(string))
            {
                return true;
            }
            if (typ == typeof(StringBuilder))
            {
                return true;
            }
            if (typ == typeof(DateTime))
            {
                return true;
            }
            if (typ.IsEnum)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Determines whether [is standard type] [the specified object].
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public static bool IsStandardType(object obj)
        {
            return IsStandardType(obj.GetType());
        }

        /// <summary>
        /// Disposes the object.
        /// </summary>
        /// <param name="obj">The object.</param>
        public static void DisposeObject(object obj)
        {
            var dObj = obj as IDisposable;
            dObj?.Dispose();
        }

        /// <summary>
        /// Creates the instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T CreateInstance<T>()
        {
            return (T)Activator.CreateInstance(typeof(T), true);
        }

        /// <summary>
        /// Creates the instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectType">Type of the object.</param>
        /// <returns></returns>
        public static T CreateInstance<T>(string objectType)
        {
            try
            {
                var objType = Type.GetType(objectType, false);
                if (objType == null)
                {
                    return default(T);
                }

                return (T)Activator.CreateInstance(objType, false);
            }
            catch (FileNotFoundException)
            {
                return default(T);
            }
        }

        /// <summary>
        /// Delays the and process events.
        /// </summary>
        /// <param name="mSeconds">The m seconds.</param>
        public static void DelayAndProcessEvents(double mSeconds)
        {
            var start = DateTime.Now;
            while (DateTime.Now.Subtract(start).TotalMilliseconds < mSeconds)
            {
                Application.DoEvents();
                Thread.Sleep(10);
            }
        }

        /// <summary>
        /// Delays the and process events.
        /// </summary>
        /// <param name="mTimeSpan">The m time span.</param>
        public static void DelayAndProcessEvents(TimeSpan mTimeSpan)
        {
            DelayAndProcessEvents(mTimeSpan.TotalMilliseconds);
        }

        /// <summary>
        /// Passives the sleep.
        /// </summary>
        /// <param name="mSec">The m sec.</param>
        /// <param name="terminate">if set to <c>true</c> [terminate].</param>
        public static void PassiveSleep(long mSec, ref bool terminate)
        {
            var sleep = mSec;
            while ((sleep > 0) && !terminate)
            {
                Thread.Sleep(100);
                sleep -= 100;
            }
        }

        /// <summary>
        /// Gets the time in UTC format.
        /// </summary>
        /// <param name="dt">The dt.</param>
        /// <returns></returns>
        public static string GetTimeInUTCFormat(DateTime dt)
        {
            return dt.ToString("yyyy-MM-ddTHH:mm:ss+0000");
        }

        /// <summary>
        /// Gets the current time in UTC format.
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentTimeInUTCFormat()
        {
            return GetTimeInUTCFormat(DateTime.Now.ToUniversalTime());
        }

        /// <summary>
        /// Determines whether [is current domain web application].
        /// </summary>
        /// <returns></returns>
        public static bool IsCurrentDomainWebApplication()
        {
            return HttpContext.Current != null;
        }

        /// <summary>
        /// Shorts the encode.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns></returns>
        public static string ShortEncode(Guid guid)
        {
            var enc = Convert.ToBase64String(guid.ToByteArray());
            enc = enc.Replace("/", "_");
            enc = enc.Replace("+", "-");
            return enc.Substring(0, 22);
        }

        /// <summary>
        /// Determines whether [is full file path name valid] [the specified file path name].
        /// </summary>
        /// <param name="filePathName">Name of the file path.</param>
        /// <returns></returns>
        public static bool IsFullFilePathNameValid(string filePathName)
        {
            if (filePathName == null || filePathName.Trim() == string.Empty)
            {
                return false;
            }

            try
            {
                var fileName = Path.GetFileName(filePathName);
                var filePath = Path.GetDirectoryName(filePathName);

                if (fileName == null || fileName.Trim() == string.Empty)
                {
                    return false;
                }
                
                var r1 = new Regex(@"[:|\\/*?<>""]");
                if (r1.IsMatch(fileName))
                {
                    return false;
                }
                
                if ((filePath == null || filePath.Trim() == string.Empty) && filePathName.Contains(":"))
                {
                    return false;
                }

                if (filePath != null && filePath.Trim() != string.Empty)
                {
                    var r2 = new Regex(@"[|*?<>""]");
                    if (r2.IsMatch(filePath))
                    {
                        return false;
                    }
                }
            }
            catch (ArgumentException)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Determines the parameter path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static string DetermineParameterPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return path;
            }
            
            path = path.Replace("$(workingdir)", AppDomain.CurrentDomain.BaseDirectory);
            path = path.Replace("$(mydocuments)", Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
            path = path.Replace(@"\\", @"\");

            return Path.GetFullPath(Environment.ExpandEnvironmentVariables(path));
        }

        /// <summary>
        /// Gets the enum description.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string GetEnumDescription(Enum value)
        {
            var fi = value.GetType().GetField(value.ToString());
            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return (attributes.Length > 0) ? attributes[0].Description : value.ToString();
        }

        /// <summary>
        /// Writes to event log.
        /// </summary>
        /// <param name="eventSourceName">Name of the event source.</param>
        /// <param name="entry">The entry.</param>
        /// <param name="eType">Type of the e.</param>
        /// <param name="eventId">The event identifier.</param>
        public static void WriteToEventLog(string eventSourceName, string entry, EventLogEntryType eType, int eventId = 1)
        {
            try
            {
                if (entry.Length > 32700)
                {
                    entry = string.Format("{0}...", entry.Substring(0, 32700));
                }

                EventLog.WriteEntry(eventSourceName, entry, eType, eventId);
            }
            catch (Exception)
            {
            }
        }
    }
}
