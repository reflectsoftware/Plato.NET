// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System;

namespace Plato.Utils.Dates
{
    /// <summary>
    /// Date functions
    /// </summary>
    public static class DateHelper
    {
        /// <summary>
        /// Starts the of day.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        public static DateTime StartOfDay(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
        }

        /// <summary>
        /// Ends the of day.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        public static DateTime EndOfDay(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
        }
    }
}
