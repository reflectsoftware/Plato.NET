// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace Plato.Core.Strings
{
    /// <summary>
    /// 
    /// </summary>
    public static class ExceptionFormatter
    {
        private readonly static string Line;

        /// <summary>
        /// Initializes the <see cref="ExceptionFormatter"/> class.
        /// </summary>
        static ExceptionFormatter()
        {
            Line = string.Format("{0,40}", string.Empty).Replace(" ", "-");
        }

        /// <summary>
        /// Adds the exception.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="agexCount">The agex count.</param>
        /// <param name="tabs">The tabs.</param>
        /// <param name="extension">The extension.</param>
        private static void AddException(StringBuilder body, Exception exception, int agexCount, string tabs, Func<Exception, IEnumerable<ExceptionFormatterExtension>> extension)
        {
            var count = 1;
            var currentException = exception;
            do
            {
                var countString = count.ToString();
                if (agexCount > 0)
                {
                    countString = string.Format("{0}.{1}", agexCount, count);
                }

                var currentExceptionType = currentException.GetType();
                body.AppendFormat("{0}{0}{1}{2}) Exception Information{0}", Environment.NewLine, tabs, countString);
                body.AppendFormat("{0}{1}", tabs, Line);
                body.AppendFormat("{0}{1}Exception Type: {2}", Environment.NewLine, tabs, currentExceptionType.FullName);

                var aryPublicProperties = currentExceptionType.GetProperties();
                foreach (var p in aryPublicProperties)
                {
                    if (p.Name != "InnerException" && p.Name != "StackTrace")
                    {
                        var exceptionValue = p.GetValue(currentException, null);
                        if (exceptionValue == null)
                        {
                            body.AppendFormat("{0}{1}{2}: NULL", Environment.NewLine, tabs, p.Name);
                        }
                        else
                        {
                            body.AppendFormat("{0}{1}{2}: {3}", Environment.NewLine, tabs, p.Name, exceptionValue.ToString().Replace(Environment.NewLine, " "));
                        }
                    }
                }

                // allow the application to extend the message for exceptions they wish
                // to drill down for further errors/information to be displayed
                if (extension != null)
                {
                    var extendedProperties = extension(currentException);
                    foreach (var extended in extendedProperties)
                    {
                        body.AppendFormat("{0}{0}{1}{2}{0}", Environment.NewLine, tabs, extended.Caption);
                        body.AppendFormat("{0}{1}{2}", tabs, Line, Environment.NewLine);

                        foreach (var key in extended.Properties.AllKeys)
                        {
                            body.AppendFormat("{0}{1}: {2}", tabs, key, extended.Properties[key]);
                        }
                    }
                }

                var stackTrace = currentException.StackTrace;
                if (stackTrace != null)
                {
                    body.AppendFormat("{0}{0}{1}StackTrace Information{0}", Environment.NewLine, tabs);
                    body.AppendFormat("{0}{1}", tabs, Line);
                    body.AppendFormat("{0}{1}{2}", Environment.NewLine, tabs, stackTrace);
                }

                currentException = currentException.InnerException;
                count++;
            }
            while (currentException != null);
        }

        /// <summary>
        /// Constructs the indented message.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="additionalInfo">The additional information.</param>
        /// <returns></returns>
        public static string ConstructMessage(Exception exception, NameValueCollection additionalInfo = null, Func<Exception, IEnumerable<ExceptionFormatterExtension>> extension = null)
        {
            additionalInfo = additionalInfo ?? new NameValueCollection();
            additionalInfo["TrackingId"] = Guid.NewGuid().ToString();
            additionalInfo["MachineName"] = Environment.MachineName;
            additionalInfo["UTC"] = DateTime.UtcNow.ToString();

            var body = new StringBuilder();
            body.AppendFormat("The following exception occurred:{0}", Environment.NewLine);

            if (additionalInfo != null && additionalInfo.Count > 0)
            {
                body.AppendFormat("{0}General Information", Environment.NewLine);
                body.AppendFormat("{0}{1}", Environment.NewLine, Line);
                body.AppendFormat("{0}Additional Info:", Environment.NewLine);

                foreach (string key in additionalInfo)
                {
                    var sb = new StringBuilder(additionalInfo.Get(key));
                    sb.Replace("\n", string.Empty);

                    body.AppendFormat("{0}\t{1}: {2}", Environment.NewLine, key, sb);
                }
            }

            if (exception == null)
            {
                body.AppendFormat("{0}{0}No Exception object has been provided.{0}", Environment.NewLine);
            }
            else
            {
                if (exception is AggregateException)
                {
                    var agexCount = 1;
                    foreach (var agex in (exception as AggregateException).InnerExceptions)
                    {
                        body.AppendFormat("{0}{0}{1}) AggregateException Item {0}", Environment.NewLine, agexCount);
                        AddException(body, agex, agexCount, "\t", extension);
                        agexCount++;
                    }
                }
                else
                {
                    AddException(body, exception, -1, string.Empty, extension);
                }
            }

            return body.ToString();
        }

        /// <summary>
        /// Constructs the message.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="additionalInfo">The additional information.</param>
        /// <returns></returns>
        public static string ConstructIndentedMessage(Exception exception, NameValueCollection additionalInfo = null, Func<Exception, IEnumerable<ExceptionFormatterExtension>> extension = null)
        {
            var nonIdentedMessage = ConstructMessage(exception, additionalInfo, extension);
            var lines = nonIdentedMessage.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            var message = new StringBuilder();
            foreach (var line in lines)
            {
                message.AppendFormat("\t{0}{1}", line, Environment.NewLine);
            }

            return message.ToString();
        }
    }
}
