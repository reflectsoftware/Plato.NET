// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System.Linq;

namespace Plato.SqlServer
{
    /// <summary>
    /// SQL Server Errors
    /// </summary>
    public class SQLErrors
    {
        // error types                
        public const int DEADLOCK = 1205;
        public const int UNIQUE_KEY_CONSTRAINT = 2627;
        public const int DUPLICATE_KEY_CONSTRAINT = 2601;
        public const int FOREIGN_KEY_CONSTRAINT = 547;

        // connection or severe failure codes
        public const int UNKNOWN_ERROR = -1;
        public const int FAILED_TO_CONTACT_SERVER = 53;
        public const int CANNOT_OPEN_DATABASE = 4060;
        public const int LOGIN_FAILED = 18456;
        public const int SYSTEM_ERROR = 50000;

        /// <summary>
        /// Determines whether [is severe error code] [the specified code].
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        public static bool IsSevereErrorCode(int code)
        {
            var severeCodes = new int[] {
                UNKNOWN_ERROR,
                FAILED_TO_CONTACT_SERVER,
                CANNOT_OPEN_DATABASE,
                LOGIN_FAILED,
                SYSTEM_ERROR,
            };

            return severeCodes.Contains(code);
        }

        /// <summary>
        /// Determines whether the specified code is duplicate.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        public static bool IsDuplicate(int code)
        {
            var dubCodes = new int[] {
                UNIQUE_KEY_CONSTRAINT,
                DUPLICATE_KEY_CONSTRAINT,
            };

            return dubCodes.Contains(code);
        }

        /// <summary>
        /// Determines whether [is referential integrity] [the specified code].
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns>
        ///   <c>true</c> if [is referential integrity] [the specified code]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsReferentialIntegrity(int code)
        {
            var refCodes = new int[] {
                UNIQUE_KEY_CONSTRAINT,
                DUPLICATE_KEY_CONSTRAINT,
                FOREIGN_KEY_CONSTRAINT,
            };

            return refCodes.Contains(code);
        }
    }
}
