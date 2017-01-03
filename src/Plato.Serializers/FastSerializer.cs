// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Serializers.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Plato.Serializers
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="System.IO.BinaryWriter"/>
    public class FastBinaryWriter : BinaryWriter
    {
        private readonly byte[] FLengthBuffer;

        /// <summary>
        /// Gets or sets the tag.
        /// </summary>
        /// <value>
        /// The tag.
        /// </value>
        public object Tag { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FastBinaryWriter"/> class.
        /// </summary>
        /// <param name="output">The output.</param>
        public FastBinaryWriter(Stream output) : base(output)
        {
            Tag = null;
            FLengthBuffer = new byte[Marshal.SizeOf(typeof(int))];
        }

        /// <summary>
        /// Writes the data.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="additionalInfo">The additional information.</param>
        internal void WriteData(IFastBinarySerializable graph, object additionalInfo)
        {
            var startOfGraphPos = (int)BaseStream.Position;
            
            Write(FLengthBuffer, 0, FLengthBuffer.Length);
            graph.WriteData(this, additionalInfo);
            var graphSize = (int)BaseStream.Position - startOfGraphPos - FLengthBuffer.Length;

            BaseStream.Position = startOfGraphPos;
            var bGraphSize = BitConverter.GetBytes(graphSize);
            Write(bGraphSize, 0, bGraphSize.Length);
            Seek(0, SeekOrigin.End);
        }

        /// <summary>
        /// Writes the state of the null.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public bool WriteNullState(object obj)
        {
            Write(obj != null);
            return obj != null;
        }

        /// <summary>
        /// Writes the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="additionalInfo">The additional information.</param>
        public void Write(IFastBinarySerializable obj, object additionalInfo)
        {
            if (WriteNullState(obj))
            {
                WriteData(obj, additionalInfo);
            }
        }

        /// <summary>
        /// Writes the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        public void Write(IFastBinarySerializable obj)
        {
            Write(obj, null);
        }

        /// <summary>
        /// Writes the specified list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="additionalInfo">The additional information.</param>
        public void Write(IEnumerable<IFastBinarySerializable> list, object additionalInfo)
        {
            Write(BitConverter.GetBytes(list.Count()));
            foreach (var obj in list)
            {
                Write(obj, additionalInfo);
            }
        }

        /// <summary>
        /// Writes the specified list.
        /// </summary>
        /// <param name="list">The list.</param>
        public void Write(IEnumerable<IFastBinarySerializable> list)
        {
            Write(list, null);
        }

        /// <summary>
        /// Writes the byte array.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        public void WriteByteArray(byte[] bytes)
        {
            if (WriteNullState(bytes))
            {
                Write(bytes.Length);
                Write(bytes);
            }
        }

        /// <summary>
        /// Writes the safe string.
        /// </summary>
        /// <param name="value">The value.</param>
        public void WriteSafeString(string value)
        {
            if (WriteNullState(value))
            {
                Write(value);
            }
        }

        /// <summary>
        /// Writes the specified list.
        /// </summary>
        /// <param name="list">The list.</param>
        public void Write(string[] list)
        {
            if (WriteNullState(list))
            {
                Write(list.Length);
                foreach (var str in list)
                {
                    WriteSafeString(str);
                }
            }
        }

        /// <summary>
        /// Writes the specified list.
        /// </summary>
        /// <param name="list">The list.</param>
        public void Write(int[] list)
        {
            if (WriteNullState(list))
            {
                Write(list.Length);
                foreach (var val in list)
                {
                    Write(val);
                }
            }
        }

        /// <summary>
        /// Writes the specified list.
        /// </summary>
        /// <param name="list">The list.</param>
        public void Write(uint[] list)
        {
            if (WriteNullState(list))
            {
                Write(list.Length);
                foreach (var val in list)
                {
                    Write(val);
                }
            }
        }

        /// <summary>
        /// Writes the specified DataSet.
        /// </summary>
        /// <param name="ds">The DataSet.</param>
        public void Write(DataSet ds)
        {
            if (WriteNullState(ds))
            {
                using (var ms = new MemoryStream())
                {
                    ds.WriteXml(ms, XmlWriteMode.WriteSchema);
                    WriteByteArray(ms.ToArray());
                }
            }
        }

        /// <summary>
        /// Writes the specified DataTable.
        /// </summary>
        /// <param name="dt">The DataTable.</param>
        public void Write(DataTable dt)
        {
            if (WriteNullState(dt))
            {
                using (var ms = new MemoryStream())
                {
                    dt.WriteXml(ms, XmlWriteMode.WriteSchema);
                    WriteByteArray(ms.ToArray());
                }
            }
        }

        /// <summary>
        /// Writes the specified NameValueCollection.
        /// </summary>
        /// <param name="nvcol">The NameValueCollection.</param>
        public void Write(NameValueCollection nvcol)
        {
            if (WriteNullState(nvcol))
            {
                Write(nvcol.Count);
                foreach (string key in nvcol.Keys)
                {
                    WriteSafeString(key);
                    WriteSafeString(nvcol[key]);
                }
            }
        }

        /// <summary>
        /// Writes the named directory.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dictionary">The dictionary.</param>
        public void WriteNamedDirectory<T>(Dictionary<string, T> dictionary) where T: IFastBinarySerializable
        {
            if (WriteNullState(dictionary))
            {
                var keys = new string[dictionary.Count];
                var values = new IFastBinarySerializable[dictionary.Count];
                
                dictionary.Keys.CopyTo(keys, 0);

                for (var i = 0; i < keys.Length; i++)
                {
                    values[i] = dictionary[keys[i]];
                }

                Write(keys);
                Write(values);
            }
        }

        /// <summary>
        /// Writes the hashed directory.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dictionary">The dictionary.</param>
        public void WriteHashedDirectory<T>(Dictionary<int, T> dictionary) where T: IFastBinarySerializable
        {
            if (WriteNullState(dictionary))
            {
                var keys = new int[dictionary.Count];
                var values = new IFastBinarySerializable[dictionary.Count];
                
                dictionary.Keys.CopyTo(keys, 0);

                for (var i = 0; i < keys.Length; i++)
                {
                    values[i] = dictionary[keys[i]];
                }

                Write(keys);
                Write(values);
            }
        }

        /// <summary>
        /// Writes the type of the value.
        /// </summary>
        /// <param name="vType">Type of the v.</param>
        public void WriteValueType(ValueType vType)
        {
            var tmpBytes = new byte[Marshal.SizeOf(vType)];
            var mBuffer = Marshal.AllocCoTaskMem(tmpBytes.Length);
            try
            {
                Marshal.StructureToPtr(vType, mBuffer, true);
                Marshal.Copy(mBuffer, tmpBytes, 0, tmpBytes.Length);
                WriteByteArray(tmpBytes);
            }
            finally
            {
                Marshal.FreeCoTaskMem(mBuffer);
            }
        }

        /// <summary>
        /// Writes the specified DateTime.
        /// </summary>
        /// <param name="dt">The DateTime.</param>
        public void Write(DateTime dt)
        {
            Write(dt.ToBinary());
        }

        /// <summary>
        /// Writes the nullable.
        /// </summary>
        /// <param name="dt">The DateTime.</param>
        public void WriteNullable(DateTime? dt)
        {
            if (WriteNullState(dt))
            {
                Write(dt.Value);
            }
        }

        /// <summary>
        /// Writes the nullable.
        /// </summary>
        /// <param name="value">The value.</param>
        public void WriteNullable(short? value)
        {
            if (WriteNullState(value))
            {
                Write(value.Value);
            }
        }

        /// <summary>
        /// Writes the nullable.
        /// </summary>
        /// <param name="value">The value.</param>
        public void WriteNullable(int? value)
        {
            if (WriteNullState(value))
            {
                Write(value.Value);
            }
        }

        /// <summary>
        /// Writes the write nullable.
        /// </summary>
        /// <param name="value">The value.</param>
        public void WriteNullable(long? value)
        {
            if (WriteNullState(value))
            {
                Write(value.Value);
            }
        }

        /// <summary>
        /// Writes the nullable.
        /// </summary>
        /// <param name="value">The value.</param>
        public void WriteNullable(decimal? value)
        {
            if (WriteNullState(value))
            {
                Write(value.Value);
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="System.IO.BinaryReader"/>
    public class FastBinaryReader : BinaryReader
    {
        private class ActiveObjectStates
        {
            public int GraphSize { get; set; }
            public int StartOfGraphPos { get; set; }
        }

        private ActiveObjectStates CurrentActiveObjectStates;
        private readonly byte[] FLengthBuffer;

        /// <summary>
        /// Gets or sets the tag.
        /// </summary>
        /// <value>
        /// The tag.
        /// </value>
        public object Tag { get; set; }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="FastBinaryReader"/> class.
        /// </summary>
        /// <param name="input">The input.</param>
        public FastBinaryReader(Stream input) : base(input)
        {
            Tag = null;
            CurrentActiveObjectStates = new ActiveObjectStates();
            FLengthBuffer = new byte[Marshal.SizeOf(typeof(int))];
        }

        /// <summary>
        /// Reads the data.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="additionalInfo">The additional information.</param>
        internal void ReadData(IFastBinarySerializable graph, object additionalInfo)
        {
            var lastActiveObject = CurrentActiveObjectStates;

            CurrentActiveObjectStates = new ActiveObjectStates();
            
            Read(FLengthBuffer, 0, FLengthBuffer.Length);
            CurrentActiveObjectStates.GraphSize = BitConverter.ToInt32(FLengthBuffer, 0);
            CurrentActiveObjectStates.StartOfGraphPos = (int)BaseStream.Position;

            graph.ReadData(this, additionalInfo);

            var remainingSize = GetActiveObjectRemainingSize();
            if (remainingSize > 0)
            {
                BaseStream.Seek(remainingSize, SeekOrigin.Current);
            }

            CurrentActiveObjectStates = lastActiveObject;
        }

        /// <summary>
        /// Gets the size of the active object remaining.
        /// </summary>
        /// <returns></returns>
        public int GetActiveObjectRemainingSize()
        {
            var currentPos = (int)BaseStream.Position;
            var readSize = currentPos - CurrentActiveObjectStates.StartOfGraphPos;

            return CurrentActiveObjectStates.GraphSize - readSize;
        }

        /// <summary>
        /// Any objects remaining.
        /// </summary>
        /// <returns></returns>
        public bool AnyObjectDataRemaining()
        {
            return GetActiveObjectRemainingSize() > 0;
        }

        /// <summary>
        /// Determines whether this instance is null.
        /// </summary>
        /// <returns></returns>
        public bool IsNull()
        {
            return !ReadBoolean();
        }

        /// <summary>
        /// Reads the object.
        /// </summary>
        /// <param name="oType">Type of the o.</param>
        /// <param name="additionalInfo">The additional information.</param>
        /// <returns></returns>
        public IFastBinarySerializable ReadObject(Type oType, object additionalInfo)
        {
            if (IsNull())
            {
                return null;
            }

            var obj = (IFastBinarySerializable)FormatterServices.GetUninitializedObject(oType);
            ReadData(obj, additionalInfo);

            return obj;
        }

        /// <summary>
        /// Reads the object.
        /// </summary>
        /// <param name="oType">Type of the o.</param>
        /// <returns></returns>
        public IFastBinarySerializable ReadObject(Type oType)
        {
            return ReadObject(oType, null);
        }

        /// <summary>
        /// Reads the object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="additionalInfo">The additional information.</param>
        /// <returns></returns>
        public T ReadObject<T>(object additionalInfo) where T: IFastBinarySerializable
        {
            return (T)ReadObject(typeof(T), additionalInfo);
        }

        /// <summary>
        /// Reads the object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T ReadObject<T>() where T: IFastBinarySerializable
        {
            return ReadObject<T>(null);
        }

        /// <summary>
        /// Reads the fast serializer object data.
        /// </summary>
        /// <param name="additionalInfo">The additional information.</param>
        /// <returns></returns>
        public FastSerializerObjectData ReadFastSerializerObjectData(object additionalInfo)
        {
            return ReadObject<FastSerializerObjectData>(additionalInfo);
        }

        /// <summary>
        /// Reads the fast serializer object data.
        /// </summary>
        /// <returns></returns>
        public FastSerializerObjectData ReadFastSerializerObjectData()
        {
            return ReadFastSerializerObjectData(null);
        }

        /// <summary>
        /// Reads the enumerable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="additionalInfo">The additional information.</param>
        /// <returns></returns>
        public IEnumerable<T> ReadEnumerable<T>(object additionalInfo) where T: IFastBinarySerializable
        {
            var items = new T[ReadInt32()];
            for (var i = 0; i < items.Length; i++)
            {
                items[i] = ReadObject<T>(additionalInfo);
            }

            return items;
        }

        /// <summary>
        /// Reads the enumerable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IEnumerable<T> ReadEnumerable<T>() where T: IFastBinarySerializable
        {
            return ReadEnumerable<T>(null);
        }

        /// <summary>
        /// Reads the named dictionary.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public Dictionary<string, T> ReadNamedDictionary<T>() where T: IFastBinarySerializable
        {
            if (IsNull())
            {
                return null;
            }

            var dictionary = new Dictionary<string, T>();
            var keys = ReadStringArray();
            var values = ReadEnumerable<T>().ToArray();

            for (var i = 0; i < keys.Length; i++)
            {
                dictionary[keys[i]] = values[i];
            }

            return dictionary;
        }

        /// <summary>
        /// Reads the hashed dictionary.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public Dictionary<int, T> ReadHashedDictionary<T>() where T: IFastBinarySerializable
        {
            if (IsNull())
            {
                return null;
            }

            var dictionary = new Dictionary<int, T>();
            var keys = ReadInt32Array();
            var values = ReadEnumerable<T>().ToArray();

            for (var i = 0; i < keys.Length; i++)
            {
                dictionary[keys[i]] = values[i];
            }

            return dictionary;
        }

        /// <summary>
        /// Reads the byte array.
        /// </summary>
        /// <returns></returns>
        public byte[] ReadByteArray()
        {
            if (IsNull())
            {
                return null;
            }

            return ReadBytes(ReadInt32());
        }

        /// <summary>
        /// Reads the safe string.
        /// </summary>
        /// <returns></returns>
        public string ReadSafeString()
        {
            if (IsNull())
            {
                return null;
            }

            return ReadString();
        }

        /// <summary>
        /// Reads the string array.
        /// </summary>
        /// <returns></returns>
        public string[] ReadStringArray()
        {
            if (IsNull())
            {
                return null;
            }

            var list = new string[ReadInt32()];
            for (var i = 0; i < list.Length; i++)
            {
                list[i] = ReadSafeString();
            }

            return list;
        }

        /// <summary>
        /// Reads the name value collection.
        /// </summary>
        /// <returns></returns>
        public NameValueCollection ReadNameValueCollection()
        {
            if (IsNull())
            {
                return null;
            }

            var count = ReadInt32();
            var nvCol = new NameValueCollection();

            for (var i = 0; i < count; i++)
            {
                var key = ReadSafeString();
                var val = ReadSafeString();

                nvCol[key] = val;
            }

            return nvCol;
        }

        /// <summary>
        /// Reads the int32 array.
        /// </summary>
        /// <returns></returns>
        public int[] ReadInt32Array()
        {
            if (IsNull())
            {
                return null;
            }

            var list = new int[ReadInt32()];
            for (var i = 0; i < list.Length; i++)
            {
                list[i] = ReadInt32();
            }

            return list;
        }

        /// <summary>
        /// Reads the u int32 array.
        /// </summary>
        /// <returns></returns>
        public uint[] ReadUInt32Array()
        {
            if (IsNull())
            {
                return null;
            }

            var list = new uint[ReadInt32()];
            for (var i = 0; i < list.Length; i++)
            {
                list[i] = ReadUInt32();
            }

            return list;
        }

        /// <summary>
        /// Reads the data set.
        /// </summary>
        /// <returns></returns>
        public DataSet ReadDataSet()
        {
            if (IsNull())
            {
                return null;
            }

            using (var ms = new MemoryStream(ReadByteArray()))
            {
                ms.Seek(0, SeekOrigin.Begin);

                var ds = new DataSet();
                ds.ReadXml(ms, XmlReadMode.Auto);

                return ds;
            }
        }

        /// <summary>
        /// Reads the data table.
        /// </summary>
        /// <returns></returns>
        public DataTable ReadDataTable()
        {
            if (IsNull())
            {
                return null;
            }

            using (var ms = new MemoryStream(ReadByteArray()))
            {
                ms.Seek(0, SeekOrigin.Begin);

                var dt = new DataTable();
                dt.ReadXml(ms);

                return dt;
            }
        }

        /// <summary>
        /// Reads the type of the value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T ReadValueType<T>()
        {
            var data = ReadByteArray();
            var mBuffer = Marshal.AllocCoTaskMem(data.Length);
            try
            {
                Marshal.Copy(data, 0, mBuffer, data.Length);
                return (T)Marshal.PtrToStructure(mBuffer, typeof(T));
            }
            finally
            {
                Marshal.FreeCoTaskMem(mBuffer);
            }
        }

        /// <summary>
        /// Reads the date time.
        /// </summary>
        /// <returns></returns>
        public DateTime ReadDateTime()
        {
            return DateTime.FromBinary(ReadInt64());
        }

        /// <summary>
        /// Reads the nullable date time.
        /// </summary>
        /// <returns></returns>
        public DateTime? ReadNullableDateTime()
        {
            if (IsNull())
            {
                return null;
            }

            return ReadDateTime();
        }

        /// <summary>
        /// Reads the nullable int16.
        /// </summary>
        /// <returns></returns>
        public short? ReadNullableInt16()
        {
            if (IsNull())
            {
                return null;
            }

            return ReadInt16();
        }

        /// <summary>
        /// Reads the nullable int32.
        /// </summary>
        /// <returns></returns>
        public int? ReadNullableInt32()
        {
            if (IsNull())
            {
                return null;
            }

            return ReadInt32();
        }

        /// <summary>
        /// Reads the nullable int64.
        /// </summary>
        /// <returns></returns>
        public long? ReadNullableInt64()
        {
            if (IsNull())
            {
                return null;
            }

            return ReadInt64();
        }

        /// <summary>
        /// Reads the nullable decimal.
        /// </summary>
        /// <returns></returns>
        public decimal? ReadNullableDecimal()
        {
            if (IsNull())
            {
                return null;
            }

            return ReadDecimal();
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="Plato.Serializers.Interfaces.IFastBinarySerializable"/>
    public class FastSerializerObjectData : IFastBinarySerializable
    {
        public byte[] ObjectData { get; private set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="FastSerializerObjectData"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        public FastSerializerObjectData(byte[] data)
        {
            ObjectData = data;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FastSerializerObjectData"/> class.
        /// </summary>
        public FastSerializerObjectData() : this((byte[])null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FastSerializerObjectData"/> class.
        /// </summary>
        /// <param name="ff">The ff.</param>
        /// <param name="obj">The object.</param>
        public FastSerializerObjectData(FastBinaryFormatter ff, IFastBinarySerializable obj)
        {
            SetObject(ff, obj);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FastSerializerObjectData"/> class.
        /// </summary>
        /// <param name="obj">The object.</param>
        public FastSerializerObjectData(IFastBinarySerializable obj)
        {
            SetObject(obj);
        }

        public void SetObject(FastBinaryFormatter ff, IFastBinarySerializable obj)
        {
            ObjectData = null;
            if (obj == null)
            {
                return;
            }

            ObjectData = ff.Serialize(obj);
        }

        /// <summary>
        /// Sets the object.
        /// </summary>
        /// <param name="obj">The object.</param>
        public void SetObject(IFastBinarySerializable obj)
        {
            ObjectData = null;
            if (obj == null)
            {
                return;
            }

            using (var ff = new FastBinaryFormatter())
            {
                SetObject(ff, obj);
            }
        }

        /// <summary>
        /// Gets the object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ff">The ff.</param>
        /// <param name="additionalInfo">The additional information.</param>
        /// <returns></returns>
        public T GetObject<T>(FastBinaryFormatter ff, object additionalInfo) where T: IFastBinarySerializable
        {
            T value;
            if (ObjectData != null)
            {
                value = ff.Deserialize<T>(ObjectData, additionalInfo);
            }
            else
            {
                value = default(T);
            }

            return value;
        }

        /// <summary>
        /// Gets the object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ff">The ff.</param>
        /// <returns></returns>
        public T GetObject<T>(FastBinaryFormatter ff) where T: IFastBinarySerializable
        {
            return GetObject<T>(ff, null);
        }

        /// <summary>
        /// Gets the object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="additionalInfo">The additional information.</param>
        /// <returns></returns>
        public T GetObject<T>(object additionalInfo) where T: IFastBinarySerializable
        {
            var value = default(T);
            if (ObjectData != null)
            {
                using (var ff = new FastBinaryFormatter())
                {
                    value = GetObject<T>(ff, additionalInfo);
                }
            }

            return value;
        }

        /// <summary>
        /// Gets the object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetObject<T>() where T: IFastBinarySerializable
        {
            return GetObject<T>((object)null);
        }

        /// <summary>
        /// Reads the data.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="additionalInfo">The additional information.</param>
        public virtual void ReadData(FastBinaryReader reader, object additionalInfo)
        {
            ObjectData = reader.ReadByteArray();
        }

        /// <summary>
        /// Writes the data.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="additionalInfo">The additional information.</param>
        public virtual void WriteData(FastBinaryWriter writer, object additionalInfo)
        {
            writer.WriteByteArray(ObjectData);
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="Plato.Serializers.Interfaces.IFastBinarySerializable"/>
    public class FastSerializerEnumerable<T> : IFastBinarySerializable where T: IFastBinarySerializable
    {
        private IEnumerable<T> Items;

        /// <summary>
        /// Initializes a new instance of the <see cref="FastSerializerEnumerable{T}"/> class.
        /// </summary>
        /// <param name="items">The items.</param>
        private FastSerializerEnumerable(IEnumerable<T> items)
        {
            Items = items;
        }

        /// <summary>
        /// Reads the data.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="additonalInfo">The additional information.</param>
        void IFastBinarySerializable.ReadData(FastBinaryReader reader, object additonalInfo)
        {
            Items = reader.ReadEnumerable<T>();
        }

        void IFastBinarySerializable.WriteData(FastBinaryWriter writer, object additonalInfo)
        {
            writer.Write(Items.Cast<IFastBinarySerializable>().ToArray());
        }

        /// <summary>
        /// Serializes the specified formatter.
        /// </summary>
        /// <param name="formatter">The formatter.</param>
        /// <param name="items">The items.</param>
        /// <param name="additionalInfo">The additional information.</param>
        /// <returns></returns>
        public static byte[] Serialize(FastBinaryFormatter formatter, IEnumerable<T> items, object additionalInfo)
        {
            var serializer = new FastSerializerEnumerable<T>(items);
            return formatter.Serialize(serializer, additionalInfo);
        }

        /// <summary>
        /// Serializes the specified formatter.
        /// </summary>
        /// <param name="formatter">The formatter.</param>
        /// <param name="items">The items.</param>
        /// <returns></returns>
        public static byte[] Serialize(FastBinaryFormatter formatter, IEnumerable<T> items)
        {
            return Serialize(formatter, items, null);
        }

        /// <summary>
        /// Serializes the specified formatter.
        /// </summary>
        /// <param name="formatter">The formatter.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="items">The items.</param>
        /// <param name="additionalInfo">The additional information.</param>
        public static void Serialize(FastBinaryFormatter formatter, Stream stream, IEnumerable<T> items, object additionalInfo)
        {
            var serializer = new FastSerializerEnumerable<T>(items);
            formatter.Serialize(stream, serializer, additionalInfo);
        }

        /// <summary>
        /// Serializes the specified formatter.
        /// </summary>
        /// <param name="formatter">The formatter.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="items">The items.</param>
        public static void Serialize(FastBinaryFormatter formatter, Stream stream, IEnumerable<T> items)
        {
            Serialize(formatter, stream, items, null);
        }

        /// <summary>
        /// Deserializes the specified formatter.
        /// </summary>
        /// <param name="formatter">The formatter.</param>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        public static IEnumerable<T> Deserialize(FastBinaryFormatter formatter, Stream stream)
        {
            var serializer = formatter.Deserialize<FastSerializerEnumerable<T>>(stream);
            return serializer.Items;
        }

        /// <summary>
        /// Deserializes the specified formatter.
        /// </summary>
        /// <param name="formatter">The formatter.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static IEnumerable<T> Deserialize(FastBinaryFormatter formatter, byte[] data)
        {
            var serializer = formatter.Deserialize<FastSerializerEnumerable<T>>(data);
            return serializer.Items;
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="System.IDisposable"/>
    public class FastBinaryFormatter : IDisposable
    {
        public const int STREAM_MAX_CAPACITY = 65536;

        private MemoryStream FMemoryStream;
        private FastBinaryWriter FWriter;
        private FastBinaryReader FReader;
        private readonly byte[] FLengthBuffer;
        private readonly byte[] FCopyBuffer;

        /// <summary>
        /// Gets a value indicating whether this <see cref="FastBinaryFormatter"/> is disposed.
        /// </summary>
        /// <value>
        /// <c>true</c> if disposed; otherwise, <c>false</c>.
        /// </value>
        public bool Disposed { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FastBinaryFormatter"/> class.
        /// </summary>
        public FastBinaryFormatter()
        {
            Disposed = false;
            FLengthBuffer = new byte[Marshal.SizeOf(typeof(int))];
            FCopyBuffer = new byte[STREAM_MAX_CAPACITY];
            FMemoryStream = new MemoryStream(STREAM_MAX_CAPACITY);
            FWriter = new FastBinaryWriter(FMemoryStream);
            FReader = new FastBinaryReader(FMemoryStream);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            lock (this)
            {
                if (!Disposed)
                {
                    Disposed = true;
                    GC.SuppressFinalize(this);

                    if (FMemoryStream != null)
                    {
                        FMemoryStream.Close();
                        FMemoryStream.Dispose();
                        FMemoryStream = null;
                    }

                    if (FWriter != null)
                    {
                        FWriter.Close();
                        FWriter.Dispose();
                        FWriter = null;
                    }

                    if (FReader != null)
                    {
                        FReader.Close();
                        FReader.Dispose();
                        FReader = null;
                    }
                }
            }
        }

        /// <summary>
        /// Serializes the specified serialization stream.
        /// </summary>
        /// <param name="serializationStream">The serialization stream.</param>
        /// <param name="graph">The graph.</param>
        /// <param name="additionalInfo">The additional information.</param>
        public void Serialize(Stream serializationStream, IFastBinarySerializable graph, object additionalInfo)
        {
            try
            {
                FMemoryStream.Seek(0, SeekOrigin.Begin);
                FWriter.WriteData(graph, additionalInfo);

                var bRootGraphSize = BitConverter.GetBytes((int)FMemoryStream.Position);
                serializationStream.Write(bRootGraphSize, 0, bRootGraphSize.Length);
                serializationStream.Write(FMemoryStream.GetBuffer(), 0, (int)FMemoryStream.Position);
            }
            finally
            {
                FMemoryStream.SetLength(0);
                FMemoryStream.Capacity = STREAM_MAX_CAPACITY;
            }
        }

        /// <summary>
        /// Serializes the specified graph.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="additionalInfo">The additional information.</param>
        /// <returns></returns>
        public byte[] Serialize(IFastBinarySerializable graph, object additionalInfo)
        {
            using (var ms = new MemoryStream())
            {
                Serialize(ms, graph, additionalInfo);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Serializes the specified serialization stream.
        /// </summary>
        /// <param name="serializationStream">The serialization stream.</param>
        /// <param name="graph">The graph.</param>
        public void Serialize(Stream serializationStream, IFastBinarySerializable graph)
        {
            Serialize(serializationStream, graph, null);
        }

        /// <summary>
        /// Serializes the specified graph.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <returns></returns>
        public byte[] Serialize(IFastBinarySerializable graph)
        {
            return Serialize(graph, null);
        }

        /// <summary>
        /// Deserializes the specified serialization stream.
        /// </summary>
        /// <param name="serializationStream">The serialization stream.</param>
        /// <param name="oType">Type of the o.</param>
        /// <param name="additionalInfo">The additional information.</param>
        /// <returns></returns>
        /// <exception cref="System.Runtime.Serialization.SerializationException">
        /// FastBinaryFormatter.Deserialize(): Can only de-serialize IFastBinarySerializable types.
        /// or
        /// FastBinaryFormatter.Deserialize(): Could not read object length from the stream.
        /// or
        /// FastBinaryFormatter.Deserialize(): The root graph size is bigger than the actual stream physical size.
        /// or
        /// </exception>
        public IFastBinarySerializable Deserialize(Stream serializationStream, Type oType, object additionalInfo)
        {
            try
            {
                var obj = FormatterServices.GetUninitializedObject(oType);
                var deserialize = (obj as IFastBinarySerializable);
                if (deserialize == null)
                {
                    throw new SerializationException("FastBinaryFormatter.Deserialize(): Can only de-serialize IFastBinarySerializable types.");
                }

                if (serializationStream.Read(FLengthBuffer, 0, FLengthBuffer.Length) != FLengthBuffer.Length)
                {
                    throw new SerializationException("FastBinaryFormatter.Deserialize(): Could not read object length from the stream.");
                }

                var rootGraphSize = BitConverter.ToInt32(FLengthBuffer, 0);
                if (rootGraphSize > serializationStream.Length)
                {
                    throw new SerializationException("FastBinaryFormatter.Deserialize(): The root graph size is bigger than the actual stream physical size.");
                }

                FMemoryStream.Seek(0, SeekOrigin.Begin);
                var total = 0;
                while (total != rootGraphSize)
                {
                    var left = rootGraphSize - total;
                    var toRead = left > FCopyBuffer.Length ? FCopyBuffer.Length : left;
                    var read = serializationStream.Read(FCopyBuffer, 0, toRead);
                    total += read;

                    FMemoryStream.Write(FCopyBuffer, 0, read);
                }

                FMemoryStream.Seek(0, SeekOrigin.Begin);
                FReader.ReadData(deserialize, additionalInfo);
                if (FMemoryStream.Position != rootGraphSize)
                {
                    var eMsg = string.Format("FastBinaryFormatter.Deserialize(): Object of type {0} did not read its entire buffer during deserialization. This is most likely an imbalance between the writes and the reads of the Object.", oType.FullName);
                    throw new SerializationException(eMsg);
                }

                return deserialize;
            }
            finally
            {
                FMemoryStream.SetLength(0);
                FMemoryStream.Capacity = STREAM_MAX_CAPACITY;
            }
        }

        /// <summary>
        /// Deserializes the specified serialization stream.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializationStream">The serialization stream.</param>
        /// <param name="additionalInfo">The additional information.</param>
        /// <returns></returns>
        public T Deserialize<T>(Stream serializationStream, object additionalInfo)
        {
            return (T)Deserialize(serializationStream, typeof(T), additionalInfo);
        }

        /// <summary>
        /// Deserializes the specified graph.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="graph">The graph.</param>
        /// <param name="additionalInfo">The additional information.</param>
        /// <returns></returns>
        public T Deserialize<T>(byte[] graph, object additionalInfo)
        {
            using (var ms = new MemoryStream(graph))
            {
                ms.Position = 0;
                return (T)Deserialize(ms, typeof(T), additionalInfo);
            }
        }

        /// <summary>
        /// Deserializes the specified serialization stream.
        /// </summary>
        /// <param name="serializationStream">The serialization stream.</param>
        /// <param name="oType">Type of the o.</param>
        /// <returns></returns>
        public IFastBinarySerializable Deserialize(Stream serializationStream, Type oType)
        {
            return Deserialize(serializationStream, oType, null);
        }

        /// <summary>
        /// Deserializes the specified graph.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="oType">Type of the o.</param>
        /// <returns></returns>
        public IFastBinarySerializable Deserialize(byte[] graph, Type oType)
        {
            using (var ms = new MemoryStream(graph))
            {
                return Deserialize(ms, oType);
            }
        }

        /// <summary>
        /// Deserializes the specified serialization stream.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializationStream">The serialization stream.</param>
        /// <returns></returns>
        public T Deserialize<T>(Stream serializationStream)
        {
            return Deserialize<T>(serializationStream, null);
        }

        /// <summary>
        /// Deserializes the specified graph.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="graph">The graph.</param>
        /// <returns></returns>
        public T Deserialize<T>(byte[] graph)
        {
            return Deserialize<T>(graph, null);
        }
    }
}
