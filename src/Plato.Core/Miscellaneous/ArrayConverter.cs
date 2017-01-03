// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Plato.Core.Miscellaneous
{
    /// <summary>
    /// 
    /// </summary>
    public static class ArrayConverter
    {
        /// <summary>
        /// Hexadecimals the encode little endian.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns></returns>
        public static string HexEncodeLittleEndian(byte[] bytes)
        {
            var str = new StringBuilder();
            for (var i = 0; i < bytes.Length; i++)
            {
                str.Append(bytes[bytes.Length - 1 - i].ToString("X2"));
            }

            return str.ToString();
        }

        /// <summary>
        /// Hexadecimals the decode little endian.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        public static byte[] HexDecodeLittleEndian(string str)
        {
            var bytes = new byte[str.Length / 2];
            for (var i = 0; i < bytes.Length; i++)
            {
                bytes[bytes.Length - 1 - i] = byte.Parse(str.Substring(i * 2, 2), NumberStyles.HexNumber);
            }

            return bytes;
        }

        /// <summary>
        /// Hexadecimals the encode big endian.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns></returns>
        public static string HexEncodeBigEndian(byte[] bytes)
        {
            var str = new StringBuilder();
            for (var i = 0; i < bytes.Length; i++)
            {
                str.Append(bytes[i].ToString("X2"));
            }

            return str.ToString();
        }

        /// <summary>
        /// Hexadecimals the decode big endian.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        public static byte[] HexDecodeBigEndian(string str)
        {
            var bytes = new byte[str.Length / 2];
            for (var i = 0; i < bytes.Length; i++)
            {
                bytes[i] = byte.Parse(str.Substring(i * 2, 2), NumberStyles.HexNumber);
            }

            return bytes;
        }

        /// <summary>
        /// To the little endian byte array.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static byte[] ToLittleEndianByteArray(short value)
        {
            return BitConverter.GetBytes(value);
        }

        /// <summary>
        /// To the little endian byte array.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static byte[] ToLittleEndianByteArray(int value)
        {
            return BitConverter.GetBytes(value);
        }

        /// <summary>
        /// To the little endian byte array.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static byte[] ToLittleEndianByteArray(long value)
        {
            return BitConverter.GetBytes(value);
        }

        /// <summary>
        /// To the little endian byte array.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static byte[] ToLittleEndianByteArray(ushort value)
        {
            return BitConverter.GetBytes(value);
        }

        /// <summary>
        /// To the little endian byte array.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static byte[] ToLittleEndianByteArray(uint value)
        {
            return BitConverter.GetBytes(value);
        }

        /// <summary>
        /// To the little endian byte array.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static byte[] ToLittleEndianByteArray(ulong value)
        {
            return BitConverter.GetBytes(value);
        }

        /// <summary>
        /// To the big endian byte array.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static byte[] ToBigEndianByteArray(short value)
        {
            var bValue = BitConverter.GetBytes(value);
            Array.Reverse(bValue);

            return bValue;
        }

        /// <summary>
        /// To the big endian byte array.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static byte[] ToBigEndianByteArray(int value)
        {
            var bValue = BitConverter.GetBytes(value);
            Array.Reverse(bValue);

            return bValue;
        }

        /// <summary>
        /// To the big endian byte array.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static byte[] ToBigEndianByteArray(long value)
        {
            var bValue = BitConverter.GetBytes(value);
            Array.Reverse(bValue);

            return bValue;
        }

        /// <summary>
        /// To the big endian byte array.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static byte[] ToBigEndianByteArray(ushort value)
        {
            var bValue = BitConverter.GetBytes(value);
            Array.Reverse(bValue);

            return bValue;
        }

        /// <summary>
        /// To the big endian byte array.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static byte[] ToBigEndianByteArray(uint value)
        {
            var bValue = BitConverter.GetBytes(value);
            Array.Reverse(bValue);

            return bValue;
        }

        /// <summary>
        /// To the big endian byte array.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static byte[] ToBigEndianByteArray(ulong value)
        {
            var bValue = BitConverter.GetBytes(value);
            Array.Reverse(bValue);

            return bValue;
        }

        /// <summary>
        /// Littles the endain array to int16.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static short LittleEndainArrayToInt16(byte[] value)
        {
            return BitConverter.ToInt16(value, 0);
        }

        /// <summary>
        /// Littles the endain array to int32.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static int LittleEndainArrayToInt32(byte[] value)
        {
            return BitConverter.ToInt32(value, 0);
        }

        /// <summary>
        /// Littles the endain array to int64.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static long LittleEndainArrayToInt64(byte[] value)
        {
            return BitConverter.ToInt64(value, 0);
        }

        /// <summary>
        /// Bigs the endain array to int16.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static short BigEndainArrayToInt16(byte[] value)
        {
            Array.Reverse(value);
            return BitConverter.ToInt16(value, 0);
        }

        /// <summary>
        /// Bigs the endain array to int32.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static int BigEndainArrayToInt32(byte[] value)
        {
            Array.Reverse(value);
            return BitConverter.ToInt32(value, 0);
        }

        /// <summary>
        /// Bigs the endain array to int64.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static long BigEndainArrayToInt64(byte[] value)
        {
            Array.Reverse(value);
            return BitConverter.ToInt64(value, 0);
        }

        /// <summary>
        /// Littles the endain array to u int16.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static ushort LittleEndainArrayToUInt16(byte[] value)
        {
            return BitConverter.ToUInt16(value, 0);
        }

        /// <summary>
        /// Littles the endain array to u int32.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static uint LittleEndainArrayToUInt32(byte[] value)
        {
            return BitConverter.ToUInt32(value, 0);
        }

        /// <summary>
        /// Littles the endain array to u int64.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static ulong LittleEndainArrayToUInt64(byte[] value)
        {
            return BitConverter.ToUInt64(value, 0);
        }

        /// <summary>
        /// Bigs the endain array to u int16.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static ushort BigEndainArrayToUInt16(byte[] value)
        {
            Array.Reverse(value);
            return BitConverter.ToUInt16(value, 0);
        }

        /// <summary>
        /// Bigs the endain array to u int32.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static uint BigEndainArrayToUInt32(byte[] value)
        {
            Array.Reverse(value);
            return BitConverter.ToUInt32(value, 0);
        }

        /// <summary>
        /// Bigs the endain array to u int64.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static ulong BigEndainArrayToUInt64(byte[] value)
        {
            Array.Reverse(value);
            return BitConverter.ToUInt64(value, 0);
        }

        /// <summary>
        /// Rotates the shift right.
        /// </summary>
        /// <param name="src">The source.</param>
        /// <param name="byteCount">The byte count.</param>
        /// <returns></returns>
        public static byte[] RotateShiftRight(byte[] src, int byteCount)
        {
            // nothing to rotate
            if ((byteCount % src.Length) == 0)
            {
                return src;
            }

            var doubleSrc = new byte[src.Length * 2];
            Array.Copy(src, 0, doubleSrc, 0, src.Length);
            Array.Copy(src, 0, doubleSrc, src.Length, src.Length);

            byteCount = byteCount % src.Length;

            var rValue = new byte[src.Length];
            Array.Copy(doubleSrc, byteCount, rValue, 0, src.Length);

            return rValue;
        }

        /// <summary>
        /// Xors the byte arrays.
        /// </summary>
        /// <param name="length">The length.</param>
        /// <param name="xor1">The xor1.</param>
        /// <param name="xor2">The xor2.</param>
        /// <returns></returns>
        public static byte[] XORByteArrays(int length, byte[] xor1, byte[] xor2)
        {
            var rValue = new byte[length];
            for (var i = 0; i < length; i++)
            {
                rValue[i] = (byte)(xor1[i] ^ xor2[i]);
            }

            return rValue;
        }

        /// <summary>
        /// Bytes the arrays equal.
        /// </summary>
        /// <param name="a1">The a1.</param>
        /// <param name="a2">The a2.</param>
        /// <returns></returns>
        public static bool ByteArraysEqual(byte[] a1, byte[] a2)
        {
            if ((a1 == null && a2 != null) || (a1 != null && a2 == null))
            {
                return false;
            }

            if (a1 == null && a2 == null)
            {
                return true;
            }

            if (a1.Length != a2.Length)
            {
                return false;
            }

            for (var i = 0; i < a1.Length; i++)
            {
                if (a1[i] != a2[i])
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Streams the type of the value.
        /// </summary>
        /// <param name="ms">The ms.</param>
        /// <param name="vType">Type of the v.</param>
        /// <returns></returns>
        public static int StreamValueType(Stream ms, ValueType vType)
        {
            var tmpBytes = new byte[Marshal.SizeOf(vType)];
            var mBuffer = Marshal.AllocCoTaskMem(tmpBytes.Length);
            try
            {
                Marshal.StructureToPtr(vType, mBuffer, true);
                Marshal.Copy(mBuffer, tmpBytes, 0, tmpBytes.Length);
                ms.Write(tmpBytes, 0, tmpBytes.Length);

                return tmpBytes.Length;
            }
            finally
            {
                Marshal.FreeCoTaskMem(mBuffer);
            }
        }

        /// <summary>
        /// Values the type to byte array.
        /// </summary>
        /// <param name="vType">Type of the v.</param>
        /// <returns></returns>
        public static byte[] ValueTypeToByteArray(ValueType vType)
        {
            using (var ms = new MemoryStream())
            {
                StreamValueType(ms, vType);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Bytes the type of the array to value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        public static T ByteArrayToValueType<T>(byte[] stream)
        {
            var mBuffer = Marshal.AllocCoTaskMem(stream.Length);
            try
            {
                Marshal.Copy(stream, 0, mBuffer, stream.Length);
                return (T)Marshal.PtrToStructure(mBuffer, typeof(T));
            }
            finally
            {
                Marshal.FreeCoTaskMem(mBuffer);
            }
        }
    }
}
