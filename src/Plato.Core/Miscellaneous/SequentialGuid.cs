// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System;

namespace Plato.Core.Miscellaneous
{
    /// <summary>
    /// 
    /// </summary>
    public static class SequentialGuid
    {
        /// <summary>
        /// News the unique identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public static Guid NewGuid(long id)
        {
            var guidArray = Guid.NewGuid().ToByteArray();
            var idArray = BitConverter.GetBytes(id);

            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(idArray);
            }

            guidArray[0] = idArray[0];
            guidArray[1] = idArray[1];
            guidArray[2] = idArray[2];
            guidArray[3] = idArray[3];

            return new Guid(guidArray);
        }

        /// <summary>
        /// News the unique identifier.
        /// </summary>
        /// <returns></returns>
        public static Guid NewGuid()
        {
            return NewGuid(DateTime.UtcNow.Ticks);
        }
    }
}
