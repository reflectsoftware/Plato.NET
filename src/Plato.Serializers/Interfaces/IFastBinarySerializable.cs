// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

namespace Plato.Serializers.Interfaces
{
    /// <summary>
    ///
    /// </summary>
    public interface IFastBinarySerializable
    {
        /// <summary>
        /// Writes the data.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="additionalInfo">The additional information.</param>
        void WriteData(FastBinaryWriter writer, object additionalInfo);

        /// <summary>
        /// Reads the data.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="additionalInfo">The additional information.</param>
        void ReadData(FastBinaryReader reader, object additionalInfo);
    }
}
