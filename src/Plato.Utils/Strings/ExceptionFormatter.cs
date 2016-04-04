// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System;
using System.Collections.Specialized;
using System.Text;

namespace Plato.Utils.Strings
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
        /// Constructs the indented message.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="additionalInfo">The additional information.</param>
        /// <returns></returns>
        public static string ConstructIndentedMessage(Exception exception, NameValueCollection additionalInfo = null)
        {
            var body = new StringBuilder();
            body.AppendFormat("\tThe following exception occurred:{0}", Environment.NewLine);
            
            if (additionalInfo != null && additionalInfo.Count > 0)
            {
                body.AppendFormat("{0}\tGeneral Information", Environment.NewLine);
                body.AppendFormat("{0}\t{1}", Environment.NewLine, Line);
                body.AppendFormat("{0}\tAdditional Info:", Environment.NewLine);

                foreach (string key in additionalInfo)
                {
                    var sb = new StringBuilder(additionalInfo.Get(key));
                    sb.Replace("\n", string.Empty);

                    body.AppendFormat("{0}\t\t{1}: {2}", Environment.NewLine, key, sb);
                }
            }

            if ( exception == null )
            {
                body.AppendFormat("{0}{0}\tNo Exception object has been provided.{0}", Environment.NewLine);
            }
            else
            {
                var currentException = exception;
                var intExceptionCount = 1;
                do
                {
                    var currentExceptionType = currentException.GetType();
                    body.AppendFormat("{0}{0}\t{1}) Exception Information{0}\t{2}", Environment.NewLine, intExceptionCount, Line);
                    body.AppendFormat("{0}\tException Type: {1}", Environment.NewLine, currentExceptionType.FullName);
                    
                    var aryPublicProperties = currentExceptionType.GetProperties();
                    foreach (var p in aryPublicProperties)
                    {
                        if (p.Name != "InnerException" && p.Name != "StackTrace")
                        {
                            var exceptionValue = p.GetValue(currentException, null);
                            if (exceptionValue == null)
                            {
                                body.AppendFormat("{0}\t{1}: NULL", Environment.NewLine, p.Name);
                            }
                            else
                            {
                                body.AppendFormat("{0}\t{1}: {2}", Environment.NewLine, p.Name, exceptionValue.ToString().Replace(Environment.NewLine, " "));
                            }
                        }
                    }

                    var stackTrace = currentException.StackTrace;
                    if (stackTrace != null )
                    {
                        body.AppendFormat("{0}{0}\tStackTrace Information{0}\t{1}", Environment.NewLine, Line);
                        body.AppendFormat("{0}\t{1}", Environment.NewLine, stackTrace);
                    }

                    currentException = currentException.InnerException;
                    intExceptionCount++;
                }
                while (currentException != null);
            }

            return body.ToString();
        }

        /// <summary>
        /// Constructs the message.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="additionalInfo">The additional information.</param>
        /// <returns></returns>
        public static string ConstructMessage(Exception exception, NameValueCollection additionalInfo = null)
        {
            return ConstructIndentedMessage(exception, additionalInfo).Replace("\t", string.Empty);
        }
    }
}




