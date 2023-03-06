﻿//
// TnefPropertyReader.cs
//
// Author: Jeffrey Stedfast <jestedfa@microsoft.com>
//
// Copyright (c) 2013-2022 .NET Foundation and Contributors
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NON INFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//

using MsgReader.Exceptions;
using MsgReader.Tnef.Enums;
using System;
using System.IO;
using System.Text;

namespace MsgReader.Tnef
{
    /// <summary>
    /// A TNEF property reader.
    /// </summary>
    /// <remarks>
    /// A TNEF property reader.
    /// </remarks>
    internal class PropertyReader
    {
        private static readonly Encoding DefaultEncoding = Encoding.GetEncoding(1252);

        #region Consts
        // Note: these constants taken from Microsoft's Reference Source in DateTime.cs
        private const long TicksPerMillisecond = 10000;
        private const long TicksPerSecond = TicksPerMillisecond * 1000;
        private const long TicksPerMinute = TicksPerSecond * 60;
        private const long TicksPerHour = TicksPerMinute * 60;
        private const long TicksPerDay = TicksPerHour * 24;

        private const int MillisPerSecond = 1000;
        private const int MillisPerMinute = MillisPerSecond * 60;
        private const int MillisPerHour = MillisPerMinute * 60;
        private const int MillisPerDay = MillisPerHour * 24;

        private const int DaysPerYear = 365;
        private const int DaysPer4Years = DaysPerYear * 4 + 1;
        private const int DaysPer100Years = DaysPer4Years * 25 - 1;
        private const int DaysPer400Years = DaysPer100Years * 4 + 1;
        private const int DaysTo1899 = DaysPer400Years * 4 + DaysPer100Years * 3 - 367;

        private const int DaysTo10000 = DaysPer400Years * 25 - 366;

        private const long MaxMillis = (long)DaysTo10000 * MillisPerDay;

        private const long DoubleDateOffset = DaysTo1899 * TicksPerDay;
        private const long OADateMinAsTicks = (DaysPer100Years - DaysPerYear) * TicksPerDay;
        private const double OADateMinAsDouble = -657435.0;
        private const double OADateMaxAsDouble = 2958466.0;
        #endregion

        #region Fields
        private PropertyTag _propertyTag;
        private readonly TnefReader _reader;
        private NameId _propertyName;
        private int _rawValueOffset;
        private int _rawValueLength;
        private int _propertyIndex;
        private int _propertyCount;
        private Decoder _decoder;
        private int _valueIndex;
        private int _valueCount;
        private int _rowIndex;
        private int _rowCount;
        #endregion

        #region Properties
        internal AttachMethod AttachMethod
        {
            get; set;
        }

#if false
		/// <summary>
		/// Get a value indicating whether the current property is a computed property.
		/// </summary>
		/// <remarks>
		/// Gets a value indicating whether the current property is a computed property.
		/// </remarks>
		/// <value><c>true</c> if the current property is a computed property; otherwise, <c>false</c>.</value>
		public bool IsComputedProperty {
			get { throw new NotImplementedException (); }
		}
#endif

        /// <summary>
        /// Get a value indicating whether the current property is an embedded TNEF message.
        /// </summary>
        /// <remarks>
        /// Gets a value indicating whether the current property is an embedded TNEF message.
        /// </remarks>
        /// <value><c>true</c> if the current property is an embedded TNEF message; otherwise, <c>false</c>.</value>
        public bool IsEmbeddedMessage
        {
            get { return _propertyTag.Id == PropertyId.AttachData && AttachMethod == AttachMethod.EmbeddedMessage; }
        }

#if false
		/// <summary>
		/// Get a value indicating whether the current property has a large value.
		/// </summary>
		/// <remarks>
		/// Gets a value indicating whether the current property has a large value.
		/// </remarks>
		/// <value><c>true</c> if the current property has a large value; otherwise, <c>false</c>.</value>
		public bool IsLargeValue {
			get { throw new NotImplementedException (); }
		}
#endif

        /// <summary>
        /// Get a value indicating whether or not the current property has multiple values.
        /// </summary>
        /// <remarks>
        /// Gets a value indicating whether or not the current property has multiple values.
        /// </remarks>
        /// <value><c>true</c> if the current property has multiple values; otherwise, <c>false</c>.</value>
        public bool IsMultiValuedProperty
        {
            get { return _propertyTag.IsMultiValued; }
        }

        /// <summary>
        /// Get a value indicating whether or not the current property is a named property.
        /// </summary>
        /// <remarks>
        /// Gets a value indicating whether or not the current property is a named property.
        /// </remarks>
        /// <value><c>true</c> if the current property is a named property; otherwise, <c>false</c>.</value>
        public bool IsNamedProperty
        {
            get { return _propertyTag.IsNamed; }
        }

        /// <summary>
        /// Get a value indicating whether the current property contains object values.
        /// </summary>
        /// <remarks>
        /// Gets a value indicating whether the current property contains object values.
        /// </remarks>
        /// <value><c>true</c> if the current property contains object values; otherwise, <c>false</c>.</value>
        public bool IsObjectProperty
        {
            get { return _propertyTag.ValueTnefType == PropertyType.Object; }
        }

#if false
		/// <summary>
		/// Get the object iid.
		/// </summary>
		/// <remarks>
		/// Gets the object iid.
		/// </remarks>
		/// <value>The object iid.</value>
		public Guid ObjectIid {
			get { throw new NotImplementedException (); }
		}
#endif

        /// <summary>
        /// Get the number of properties available.
        /// </summary>
        /// <remarks>
        /// Gets the number of properties available.
        /// </remarks>
        /// <value>The property count.</value>
        public int PropertyCount
        {
            get { return _propertyCount; }
        }

        /// <summary>
        /// Get the property name identifier.
        /// </summary>
        /// <remarks>
        /// Gets the property name identifier.
        /// </remarks>
        /// <value>The property name identifier.</value>
        public NameId PropertyNameId
        {
            get { return _propertyName; }
        }

        /// <summary>
        /// Get the property tag.
        /// </summary>
        /// <remarks>
        /// Gets the property tag.
        /// </remarks>
        /// <value>The property tag.</value>
        public PropertyTag PropertyTag
        {
            get { return _propertyTag; }
        }

        /// <summary>
        /// Get the length of the raw value.
        /// </summary>
        /// <remarks>
        /// Gets the length of the raw value.
        /// </remarks>
        /// <value>The length of the raw value.</value>
        public int RawValueLength
        {
            get { return _rawValueLength; }
        }

        /// <summary>
        /// Get the raw value stream offset.
        /// </summary>
        /// <remarks>
        /// Gets the raw value stream offset.
        /// </remarks>
        /// <value>The raw value stream offset.</value>
        public int RawValueStreamOffset
        {
            get { return _rawValueOffset; }
        }

        /// <summary>
        /// Get the number of table rows available.
        /// </summary>
        /// <remarks>
        /// Gets the number of table rows available.
        /// </remarks>
        /// <value>The row count.</value>
        public int RowCount
        {
            get { return _rowCount; }
        }

        /// <summary>
        /// Get the number of values available.
        /// </summary>
        /// <remarks>
        /// Gets the number of values available.
        /// </remarks>
        /// <value>The value count.</value>
        public int ValueCount
        {
            get { return _valueCount; }
        }

        /// <summary>
        /// Get the type of the value.
        /// </summary>
        /// <remarks>
        /// Gets the type of the value.
        /// </remarks>
        /// <value>The type of the value.</value>
        public Type ValueType
        {
            get
            {
                if (_propertyCount > 0)
                    return GetPropertyValueType();

                return GetAttributeValueType();
            }
        }
        #endregion

        #region Constructor
        internal PropertyReader(TnefReader tnef)
        {
            _propertyTag = PropertyTag.Null;
            _propertyName = new NameId();
            _rawValueOffset = 0;
            _rawValueLength = 0;
            _propertyIndex = 0;
            _propertyCount = 0;
            _valueIndex = 0;
            _valueCount = 0;
            _rowIndex = 0;
            _rowCount = 0;

            _reader = tnef;
        }
        #endregion

        /// <summary>
        /// Get the embedded TNEF message reader.
        /// </summary>
        /// <remarks>
        /// Gets the embedded TNEF message reader.
        /// </remarks>
        /// <returns>The embedded TNEF message reader.</returns>
        /// <exception cref="InvalidOperationException">
        /// <para>The property does not contain any more values.</para>
        /// <para>-or-</para>
        /// <para>The property value is not an embedded message.</para>
        /// </exception>
        public TnefReader GetEmbeddedMessageReader()
        {
            if (!IsEmbeddedMessage)
                throw new InvalidOperationException();

            var stream = GetRawValueReadStream();
            var guid = new byte[16];

            stream.Read(guid, 0, 16);

            return new TnefReader(stream, _reader.MessageCodepage, _reader.ComplianceMode);
        }

        /// <summary>
        /// Get the raw value of the attribute or property as a stream.
        /// </summary>
        /// <remarks>
        /// Gets the raw value of the attribute or property as a stream.
        /// </remarks>
        /// <returns>The raw value stream.</returns>
        /// <exception cref="InvalidOperationException">
        /// The property does not contain any more values.
        /// </exception>
        public Stream GetRawValueReadStream()
        {
            if (_valueIndex >= _valueCount)
                throw new InvalidOperationException();

            int startOffset = RawValueStreamOffset;
            int length = RawValueLength;

            if (_propertyCount > 0 && _reader.StreamOffset == RawValueStreamOffset)
            {
                switch (_propertyTag.ValueTnefType)
                {
                    case PropertyType.Unicode:
                    case PropertyType.String8:
                    case PropertyType.Binary:
                    case PropertyType.Object:
                        int n = ReadInt32();
                        if (n >= 0 && n + 4 < length)
                            length = n + 4;
                        break;
                }
            }

            _valueIndex++;

            int valueEndOffset = startOffset + RawValueLength;
            int dataEndOffset = startOffset + length;

            return new ReaderStream(_reader, dataEndOffset, valueEndOffset);
        }

        bool CheckRawValueLength()
        {
            // Check that the property value does not go beyond the end of the end of the attribute
            int attrEndOffset = _reader.AttributeRawValueStreamOffset + _reader.AttributeRawValueLength;
            int valueEndOffset = RawValueStreamOffset + RawValueLength;

            if (valueEndOffset > attrEndOffset)
            {
                _reader.SetComplianceError(ComplianceStatus.InvalidAttributeValue);
                return false;
            }

            return true;
        }

        byte ReadByte()
        {
            return _reader.ReadByte();
        }

        byte[] ReadBytes(int count)
        {
            var bytes = new byte[count];
            int offset = 0;
            int nread;

            while (offset < count && (nread = _reader.ReadAttributeRawValue(bytes, offset, count - offset)) > 0)
                offset += nread;

            return bytes;
        }

        short ReadInt16()
        {
            return _reader.ReadInt16();
        }

        int ReadInt32()
        {
            return _reader.ReadInt32();
        }

        int PeekInt32()
        {
            return _reader.PeekInt32();
        }

        long ReadInt64()
        {
            return _reader.ReadInt64();
        }

        float ReadSingle()
        {
            return _reader.ReadSingle();
        }

        double ReadDouble()
        {
            return _reader.ReadDouble();
        }

        // Note: this method taken from Microsoft's Reference Source in DateTime.cs
        static long DoubleDateToTicks(double value)
        {
            // The check done this way will take care of NaN
            if (!(value < OADateMaxAsDouble) || !(value > OADateMinAsDouble))
                throw new ArgumentException(@"Invalid OLE Automation Date.", nameof(value));

            long millis = (long)(value * MillisPerDay + (value >= 0 ? 0.5 : -0.5));

            if (millis < 0)
                millis -= millis % MillisPerDay * 2;

            millis += DoubleDateOffset / TicksPerMillisecond;

            if (millis < 0 || millis >= MaxMillis)
                throw new ArgumentException(@"Invalid OLE Automation Date.", nameof(value));

            return millis * TicksPerMillisecond;
        }

        DateTime ReadAppTime()
        {
            var appTime = ReadDouble();

            // Note: equivalent to DateTime.FromOADate(). Unfortunately, FromOADate() is
            // not available in some PCL profiles.
            return new DateTime(DoubleDateToTicks(appTime), DateTimeKind.Unspecified);
        }

        DateTime ReadSysTime()
        {
            var fileTime = ReadInt64();

            return DateTime.FromFileTime(fileTime);
        }

        static int GetPaddedLength(int length)
        {
            return length + 3 & ~3;
        }

        byte[] ReadByteArray()
        {
            int length = ReadInt32();
            var bytes = ReadBytes(length);

            if (length % 4 != 0)
            {
                // remaining bytes are padding
                int padding = 4 - length % 4;

                _reader.Seek(_reader.StreamOffset + padding);
            }

            return bytes;
        }

        string ReadUnicodeString()
        {
            var bytes = ReadByteArray();
            int length = bytes.Length;

            // force length to a multiple of 2 bytes
            length &= ~1;

            while (length > 1 && bytes[length - 1] == 0 && bytes[length - 2] == 0)
                length -= 2;

            if (length < 2)
                return string.Empty;

            return Encoding.Unicode.GetString(bytes, 0, length);
        }

        Encoding GetMessageEncoding()
        {
            int codepage = _reader.MessageCodepage;

            if (codepage != 0 && codepage != 1252)
            {
                try
                {
                    return Encoding.GetEncoding(codepage);
                }
                catch
                {
                    return DefaultEncoding;
                }
            }

            return DefaultEncoding;
        }

        string DecodeAnsiString(byte[] bytes)
        {
            int length = bytes.Length;

            while (length > 0 && bytes[length - 1] == 0)
                length--;

            if (length == 0)
                return string.Empty;

            try
            {
                return GetMessageEncoding().GetString(bytes, 0, length);
            }
            catch
            {
                return DefaultEncoding.GetString(bytes, 0, length);
            }
        }

        string ReadString()
        {
            var bytes = ReadByteArray();

            return DecodeAnsiString(bytes);
        }

        byte[] ReadAttrBytes()
        {
            return ReadBytes(RawValueLength);
        }

        string ReadAttrString()
        {
            var bytes = ReadBytes(RawValueLength);

            // attribute strings are null-terminated
            return DecodeAnsiString(bytes);
        }

        DateTime ReadAttrDateTime()
        {
            int year = ReadInt16();
            int month = ReadInt16();
            int day = ReadInt16();
            int hour = ReadInt16();
            int minute = ReadInt16();
            int second = ReadInt16();
#pragma warning disable 219
            int dow = ReadInt16();
#pragma warning restore 219

            try
            {
                return new DateTime(year, month, day, hour, minute, second);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                _reader.SetComplianceError(ComplianceStatus.InvalidDate, ex);
                return default;
            }
        }

        void LoadPropertyName()
        {
            var guid = new Guid(ReadBytes(16));
            var kind = (NameIdKind)ReadInt32();

            if (kind == NameIdKind.Name)
            {
                var name = ReadUnicodeString();

                _propertyName = new NameId(guid, name);
            }
            else if (kind == NameIdKind.Id)
            {
                int id = ReadInt32();

                _propertyName = new NameId(guid, id);
            }
            else
            {
                _reader.SetComplianceError(ComplianceStatus.InvalidAttributeValue);
                _propertyName = new NameId(guid, 0);
            }
        }

        /// <summary>
        /// Advance to the next MAPI property.
        /// </summary>
        /// <remarks>
        /// Advances to the next MAPI property.
        /// </remarks>
        /// <returns><c>true</c> if there is another property available to be read; otherwise <c>false</c>.</returns>
        /// <exception cref="MRTnefException">
        /// The TNEF data is corrupt or invalid.
        /// </exception>
        public bool ReadNextProperty()
        {
            while (ReadNextValue())
            {
                // skip over the remaining value(s) for the current property...
            }

            if (_propertyIndex >= _propertyCount)
                return false;

            try
            {
                var type = (PropertyType)ReadInt16();
                var id = (PropertyId)ReadInt16();

                _propertyTag = new PropertyTag(id, type);

                if (_propertyTag.IsNamed)
                    LoadPropertyName();

                LoadValueCount();
                _propertyIndex++;

                if (!TryGetPropertyValueLength(out _rawValueLength))
                    return false;

                _rawValueOffset = _reader.StreamOffset;

                switch (id)
                {
                    case PropertyId.AttachMethod:
                        AttachMethod = (AttachMethod)PeekInt32();
                        break;
                }
            }
            catch (EndOfStreamException)
            {
                return false;
            }

            return CheckRawValueLength();
        }

        /// <summary>
        /// Advance to the next table row of properties.
        /// </summary>
        /// <remarks>
        /// Advances to the next table row of properties.
        /// </remarks>
        /// <returns><c>true</c> if there is another row available to be read; otherwise <c>false</c>.</returns>
        /// <exception cref="MRTnefException">
        /// The TNEF data is corrupt or invalid.
        /// </exception>
        public bool ReadNextRow()
        {
            while (ReadNextProperty())
            {
                // skip over the remaining property/properties in the current row...
            }

            if (_rowIndex >= _rowCount)
                return false;

            try
            {
                LoadPropertyCount();
                _rowIndex++;
            }
            catch (EndOfStreamException)
            {
                _reader.SetComplianceError(ComplianceStatus.StreamTruncated);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Advance to the next value in the TNEF stream.
        /// </summary>
        /// <remarks>
        /// Advances to the next value in the TNEF stream.
        /// </remarks>
        /// <returns><c>true</c> if there is another value available to be read; otherwise <c>false</c>.</returns>
        /// <exception cref="MRTnefException">
        /// The TNEF data is corrupt or invalid.
        /// </exception>
        public bool ReadNextValue()
        {
            if (_valueIndex >= _valueCount || _propertyCount == 0)
                return false;

            int offset = RawValueStreamOffset + RawValueLength;

            if (_reader.StreamOffset < offset && !_reader.Seek(offset))
                return false;

            try
            {
                if (!TryGetPropertyValueLength(out _rawValueLength))
                    return false;

                _rawValueOffset = _reader.StreamOffset;
                _valueIndex++;
            }
            catch (EndOfStreamException)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Read the raw attribute or property value as a sequence of bytes.
        /// </summary>
        /// <remarks>
        /// Reads the raw attribute or property value as a sequence of bytes.
        /// </remarks>
        /// <returns>The total number of bytes read into the buffer. This can be less than the number of bytes requested if that many
        /// bytes are not currently available, or zero (0) if the end of the stream has been reached.</returns>
        /// <param name="buffer">The buffer to read data into.</param>
        /// <param name="offset">The offset into the buffer to start reading data.</param>
        /// <param name="count">The number of bytes to read.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="buffer"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <para><paramref name="offset"/> is less than zero or greater than the length of <paramref name="buffer"/>.</para>
        /// <para>-or-</para>
        /// <para>The <paramref name="buffer"/> is not large enough to contain <paramref name="count"/> bytes starting
        /// at the specified <paramref name="offset"/>.</para>
        /// </exception>
        /// <exception cref="IOException">
        /// An I/O error occurred.
        /// </exception>
        public int ReadRawValue(byte[] buffer, int offset, int count)
        {
            if (buffer is null)
                throw new ArgumentNullException(nameof(buffer));

            if (offset < 0 || offset >= buffer.Length)
                throw new ArgumentOutOfRangeException(nameof(offset));

            if (count < 0 || count > buffer.Length - offset)
                throw new ArgumentOutOfRangeException(nameof(count));

            if (_propertyCount > 0 && _reader.StreamOffset == RawValueStreamOffset)
            {
                switch (_propertyTag.ValueTnefType)
                {
                    case PropertyType.Unicode:
                    case PropertyType.String8:
                    case PropertyType.Binary:
                    case PropertyType.Object:
                        ReadInt32();
                        break;
                }
            }

            int valueEndOffset = RawValueStreamOffset + RawValueLength;
            int valueLeft = valueEndOffset - _reader.StreamOffset;
            int n = Math.Min(valueLeft, count);

            return n > 0 ? _reader.ReadAttributeRawValue(buffer, offset, n) : 0;
        }

        /// <summary>
        /// Read the raw attribute or property value as a sequence of unicode characters.
        /// </summary>
        /// <remarks>
        /// Reads the raw attribute or property value as a sequence of unicode characters.
        /// </remarks>
        /// <returns>The total number of characters read into the buffer. This can be less than the number of characters
        /// requested if that many bytes are not currently available, or zero (0) if the end of the stream has been
        /// reached.</returns>
        /// <param name="buffer">The buffer to read data into.</param>
        /// <param name="offset">The offset into the buffer to start reading data.</param>
        /// <param name="count">The number of characters to read.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="buffer"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <para><paramref name="offset"/> is less than zero or greater than the length of <paramref name="buffer"/>.</para>
        /// <para>-or-</para>
        /// <para>The <paramref name="buffer"/> is not large enough to contain <paramref name="count"/> characters starting
        /// at the specified <paramref name="offset"/>.</para>
        /// </exception>
        /// <exception cref="IOException">
        /// An I/O error occurred.
        /// </exception>
        public int ReadTextValue(char[] buffer, int offset, int count)
        {
            if (buffer is null)
                throw new ArgumentNullException(nameof(buffer));

            if (offset < 0 || offset >= buffer.Length)
                throw new ArgumentOutOfRangeException(nameof(offset));

            if (count < 0 || count > buffer.Length - offset)
                throw new ArgumentOutOfRangeException(nameof(count));

            if (_reader.StreamOffset == RawValueStreamOffset && _decoder is null)
                throw new InvalidOperationException();

            if (_propertyCount > 0 && _reader.StreamOffset == RawValueStreamOffset)
            {
                switch (_propertyTag.ValueTnefType)
                {
                    case PropertyType.Unicode:
                        ReadInt32();
                        _decoder = Encoding.Unicode.GetDecoder();
                        break;
                    case PropertyType.String8:
                    case PropertyType.Binary:
                    case PropertyType.Object:
                        ReadInt32();
                        _decoder = GetMessageEncoding().GetDecoder();
                        break;
                }
            }

            int valueEndOffset = RawValueStreamOffset + RawValueLength;
            int valueLeft = valueEndOffset - _reader.StreamOffset;
            int n = Math.Min(valueLeft, count);

            if (n <= 0)
                return 0;

            var bytes = new byte[n];

            n = _reader.ReadAttributeRawValue(bytes, 0, n);

            var flush = _reader.StreamOffset >= valueEndOffset;

            return _decoder.GetChars(bytes, 0, n, buffer, offset, flush);
        }

        bool TryGetPropertyValueLength(out int length)
        {
            switch (_propertyTag.ValueTnefType)
            {
                case PropertyType.Unspecified:
                case PropertyType.Null:
                    length = 0;
                    break;
                case PropertyType.Boolean:
                case PropertyType.Error:
                case PropertyType.Long:
                case PropertyType.R4:
                case PropertyType.I2:
                    length = 4;
                    break;
                case PropertyType.Currency:
                case PropertyType.Double:
                case PropertyType.I8:
                    length = 8;
                    break;
                case PropertyType.ClassId:
                    length = 16;
                    break;
                case PropertyType.Unicode:
                case PropertyType.String8:
                case PropertyType.Binary:
                case PropertyType.Object:
                    length = 4 + GetPaddedLength(PeekInt32());
                    break;
                case PropertyType.AppTime:
                case PropertyType.SysTime:
                    length = 8;
                    break;
                default:
                    _reader.SetComplianceError(ComplianceStatus.UnsupportedPropertyType);
                    length = 0;

                    return false;
            }

            return true;
        }

        Type GetPropertyValueType()
        {
            switch (_propertyTag.ValueTnefType)
            {
                case PropertyType.I2: return typeof(short);
                case PropertyType.Boolean: return typeof(bool);
                case PropertyType.Currency: return typeof(long);
                case PropertyType.I8: return typeof(long);
                case PropertyType.Error: return typeof(int);
                case PropertyType.Long: return typeof(int);
                case PropertyType.Double: return typeof(double);
                case PropertyType.R4: return typeof(float);
                case PropertyType.AppTime: return typeof(DateTime);
                case PropertyType.SysTime: return typeof(DateTime);
                case PropertyType.Unicode: return typeof(string);
                case PropertyType.String8: return typeof(string);
                case PropertyType.Binary: return typeof(byte[]);
                case PropertyType.ClassId: return typeof(Guid);
                case PropertyType.Object: return typeof(byte[]);
                default: return typeof(object);
            }
        }

        Type GetAttributeValueType()
        {
            switch (_reader.AttributeType)
            {
                case AttributeType.Triples: return typeof(byte[]);
                case AttributeType.String: return typeof(string);
                case AttributeType.Text: return typeof(string);
                case AttributeType.Date: return typeof(DateTime);
                case AttributeType.Short: return typeof(short);
                case AttributeType.Long: return typeof(int);
                case AttributeType.Byte: return typeof(byte[]);
                case AttributeType.Word: return typeof(short);
                case AttributeType.DWord: return typeof(int);
                default: return typeof(object);
            }
        }

        object ReadPropertyValue()
        {
            object value;

            switch (_propertyTag.ValueTnefType)
            {
                case PropertyType.Null:
                    value = null;
                    break;
                case PropertyType.I2:
                    // 2 bytes for the short followed by 2 bytes of padding
                    value = (short)(ReadInt32() & 0xFFFF);
                    break;
                case PropertyType.Boolean:
                    value = (ReadInt32() & 0xFF) != 0;
                    break;
                case PropertyType.Currency:
                case PropertyType.I8:
                    value = ReadInt64();
                    break;
                case PropertyType.Error:
                case PropertyType.Long:
                    value = ReadInt32();
                    break;
                case PropertyType.Double:
                    value = ReadDouble();
                    break;
                case PropertyType.R4:
                    value = ReadSingle();
                    break;
                case PropertyType.AppTime:
                    value = ReadAppTime();
                    break;
                case PropertyType.SysTime:
                    value = ReadSysTime();
                    break;
                case PropertyType.Unicode:
                    value = ReadUnicodeString();
                    break;
                case PropertyType.String8:
                    value = ReadString();
                    break;
                case PropertyType.Binary:
                    value = ReadByteArray();
                    break;
                case PropertyType.ClassId:
                    value = new Guid(ReadBytes(16));
                    break;
                case PropertyType.Object:
                    value = ReadByteArray();
                    break;
                default:
                    _reader.SetComplianceError(ComplianceStatus.UnsupportedPropertyType);
                    value = null;
                    break;
            }

            _valueIndex++;

            return value;
        }

        /// <summary>
        /// Read the value.
        /// </summary>
        /// <remarks>
        /// Reads an attribute or property value as its native type.
        /// </remarks>
        /// <returns>The value.</returns>
        /// <exception cref="InvalidOperationException">
        /// There are no more values to read or the value could not be read.
        /// </exception>
        /// <exception cref="EndOfStreamException">
        /// The TNEF stream is truncated and the value could not be read.
        /// </exception>
        public object ReadValue()
        {
            if (_valueIndex >= _valueCount || _reader.StreamOffset > RawValueStreamOffset)
                throw new InvalidOperationException();

            if (_propertyCount > 0)
                return ReadPropertyValue();

            object value = null;

            switch (_reader.AttributeType)
            {
                case AttributeType.Triples: value = ReadAttrBytes(); break;
                case AttributeType.String: value = ReadAttrString(); break;
                case AttributeType.Text: value = ReadAttrString(); break;
                case AttributeType.Date: value = ReadAttrDateTime(); break;
                case AttributeType.Short: value = ReadInt16(); break;
                case AttributeType.Long: value = ReadInt32(); break;
                case AttributeType.Byte: value = ReadAttrBytes(); break;
                case AttributeType.Word: value = ReadInt16(); break;
                case AttributeType.DWord: value = ReadInt32(); break;
            }

            _valueIndex++;

            return value;
        }

        /// <summary>
        /// Read the value as a boolean.
        /// </summary>
        /// <remarks>
        /// Reads any integer-based attribute or property value as a boolean.
        /// </remarks>
        /// <returns>The value as a boolean.</returns>
        /// <exception cref="InvalidOperationException">
        /// There are no more values to read or the value could not be read as a boolean.
        /// </exception>
        /// <exception cref="EndOfStreamException">
        /// The TNEF stream is truncated and the value could not be read.
        /// </exception>
        public bool ReadValueAsBoolean()
        {
            if (_valueIndex >= _valueCount || _reader.StreamOffset > RawValueStreamOffset)
                throw new InvalidOperationException();

            bool value;

            if (_propertyCount > 0)
            {
                switch (_propertyTag.ValueTnefType)
                {
                    case PropertyType.Boolean:
                        value = (ReadInt32() & 0xFF) != 0;
                        break;
                    case PropertyType.I2:
                        value = (ReadInt32() & 0xFFFF) != 0;
                        break;
                    case PropertyType.Error:
                    case PropertyType.Long:
                        value = ReadInt32() != 0;
                        break;
                    case PropertyType.Currency:
                    case PropertyType.I8:
                        value = ReadInt64() != 0;
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }
            else
            {
                switch (_reader.AttributeType)
                {
                    case AttributeType.Short: value = ReadInt16() != 0; break;
                    case AttributeType.Long: value = ReadInt32() != 0; break;
                    case AttributeType.Word: value = ReadInt16() != 0; break;
                    case AttributeType.DWord: value = ReadInt32() != 0; break;
                    case AttributeType.Byte: value = ReadByte() != 0; break;
                    default: throw new InvalidOperationException();
                }
            }

            _valueIndex++;

            return value;
        }

        /// <summary>
        /// Read the value as a byte array.
        /// </summary>
        /// <remarks>
        /// Reads any string, binary blob, Class ID, or Object attribute or property value as a byte array.
        /// </remarks>
        /// <returns>The value as a byte array.</returns>
        /// <exception cref="InvalidOperationException">
        /// There are no more values to read or the value could not be read as a byte array.
        /// </exception>
        /// <exception cref="EndOfStreamException">
        /// The TNEF stream is truncated and the value could not be read.
        /// </exception>
        public byte[] ReadValueAsBytes()
        {
            if (_valueIndex >= _valueCount || _reader.StreamOffset > RawValueStreamOffset)
                throw new InvalidOperationException();

            byte[] bytes;

            if (_propertyCount > 0)
            {
                switch (_propertyTag.ValueTnefType)
                {
                    case PropertyType.Unicode:
                    case PropertyType.String8:
                    case PropertyType.Binary:
                    case PropertyType.Object:
                        bytes = ReadByteArray();
                        break;
                    case PropertyType.ClassId:
                        bytes = ReadBytes(16);
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }
            else
            {
                switch (_reader.AttributeType)
                {
                    case AttributeType.Triples:
                    case AttributeType.String:
                    case AttributeType.Text:
                    case AttributeType.Byte:
                        bytes = ReadAttrBytes();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            _valueIndex++;

            return bytes;
        }

        /// <summary>
        /// Read the value as a date and time.
        /// </summary>
        /// <remarks>
        /// Reads any date and time attribute or property value as a <see cref="DateTime"/>.
        /// </remarks>
        /// <returns>The value as a date and time.</returns>
        /// <exception cref="InvalidOperationException">
        /// There are no more values to read or the value could not be read as a date and time.
        /// </exception>
        /// <exception cref="EndOfStreamException">
        /// The TNEF stream is truncated and the value could not be read.
        /// </exception>
        public DateTime ReadValueAsDateTime()
        {
            if (_valueIndex >= _valueCount || _reader.StreamOffset > RawValueStreamOffset)
                throw new InvalidOperationException();

            DateTime value;

            if (_propertyCount > 0)
            {
                switch (_propertyTag.ValueTnefType)
                {
                    case PropertyType.AppTime:
                        value = ReadAppTime();
                        break;
                    case PropertyType.SysTime:
                        value = ReadSysTime();
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }
            else if (_reader.AttributeType == AttributeType.Date)
            {
                value = ReadAttrDateTime();
            }
            else
            {
                throw new InvalidOperationException();
            }

            _valueIndex++;

            return value;
        }

        /// <summary>
        /// Read the value as a double.
        /// </summary>
        /// <remarks>
        /// Reads any numeric attribute or property value as a double.
        /// </remarks>
        /// <returns>The value as a double.</returns>
        /// <exception cref="InvalidOperationException">
        /// There are no more values to read or the value could not be read as a double.
        /// </exception>
        /// <exception cref="EndOfStreamException">
        /// The TNEF stream is truncated and the value could not be read.
        /// </exception>
        public double ReadValueAsDouble()
        {
            if (_valueIndex >= _valueCount || _reader.StreamOffset > RawValueStreamOffset)
                throw new InvalidOperationException();

            double value;

            if (_propertyCount > 0)
            {
                switch (_propertyTag.ValueTnefType)
                {
                    case PropertyType.Boolean:
                        value = ReadInt32() & 0xFF;
                        break;
                    case PropertyType.I2:
                        value = ReadInt32() & 0xFFFF;
                        break;
                    case PropertyType.Error:
                    case PropertyType.Long:
                        value = ReadInt32();
                        break;
                    case PropertyType.Currency:
                    case PropertyType.I8:
                        value = ReadInt64();
                        break;
                    case PropertyType.Double:
                        value = ReadDouble();
                        break;
                    case PropertyType.R4:
                        value = ReadSingle();
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }
            else
            {
                switch (_reader.AttributeType)
                {
                    case AttributeType.Short: value = ReadInt16(); break;
                    case AttributeType.Long: value = ReadInt32(); break;
                    case AttributeType.Word: value = ReadInt16(); break;
                    case AttributeType.DWord: value = ReadInt32(); break;
                    case AttributeType.Byte: value = ReadDouble(); break;
                    default: throw new InvalidOperationException();
                }
            }

            _valueIndex++;

            return value;
        }

        /// <summary>
        /// Read the value as a float.
        /// </summary>
        /// <remarks>
        /// Reads any numeric attribute or property value as a float.
        /// </remarks>
        /// <returns>The value as a float.</returns>
        /// <exception cref="InvalidOperationException">
        /// There are no more values to read or the value could not be read as a float.
        /// </exception>
        /// <exception cref="EndOfStreamException">
        /// The TNEF stream is truncated and the value could not be read.
        /// </exception>
        public float ReadValueAsFloat()
        {
            if (_valueIndex >= _valueCount || _reader.StreamOffset > RawValueStreamOffset)
                throw new InvalidOperationException();

            float value;

            if (_propertyCount > 0)
            {
                switch (_propertyTag.ValueTnefType)
                {
                    case PropertyType.Boolean:
                        value = ReadInt32() & 0xFF;
                        break;
                    case PropertyType.I2:
                        value = ReadInt32() & 0xFFFF;
                        break;
                    case PropertyType.Error:
                    case PropertyType.Long:
                        value = ReadInt32();
                        break;
                    case PropertyType.Currency:
                    case PropertyType.I8:
                        value = ReadInt64();
                        break;
                    case PropertyType.Double:
                        value = (float)ReadDouble();
                        break;
                    case PropertyType.R4:
                        value = ReadSingle();
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }
            else
            {
                switch (_reader.AttributeType)
                {
                    case AttributeType.Short: value = ReadInt16(); break;
                    case AttributeType.Long: value = ReadInt32(); break;
                    case AttributeType.Word: value = ReadInt16(); break;
                    case AttributeType.DWord: value = ReadInt32(); break;
                    case AttributeType.Byte: value = ReadSingle(); break;
                    default: throw new InvalidOperationException();
                }
            }

            _valueIndex++;

            return value;
        }

        /// <summary>
        /// Read the value as a GUID.
        /// </summary>
        /// <remarks>
        /// Reads any Class ID value as a GUID.
        /// </remarks>
        /// <returns>The value as a GUID.</returns>
        /// <exception cref="InvalidOperationException">
        /// There are no more values to read or the value could not be read as a GUID.
        /// </exception>
        /// <exception cref="EndOfStreamException">
        /// The TNEF stream is truncated and the value could not be read.
        /// </exception>
        public Guid ReadValueAsGuid()
        {
            if (_valueIndex >= _valueCount || _reader.StreamOffset > RawValueStreamOffset)
                throw new InvalidOperationException();

            Guid guid;

            if (_propertyCount > 0)
            {
                switch (_propertyTag.ValueTnefType)
                {
                    case PropertyType.ClassId:
                        guid = new Guid(ReadBytes(16));
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }
            else
            {
                throw new InvalidOperationException();
            }

            _valueIndex++;

            return guid;
        }

        /// <summary>
        /// Read the value as a 16-bit integer.
        /// </summary>
        /// <remarks>
        /// Reads any integer-based attribute or property value as a 16-bit integer.
        /// </remarks>
        /// <returns>The value as a 16-bit integer.</returns>
        /// <exception cref="InvalidOperationException">
        /// There are no more values to read or the value could not be read as a 16-bit integer.
        /// </exception>
        /// <exception cref="EndOfStreamException">
        /// The TNEF stream is truncated and the value could not be read.
        /// </exception>
        public short ReadValueAsInt16()
        {
            if (_valueIndex >= _valueCount || _reader.StreamOffset > RawValueStreamOffset)
                throw new InvalidOperationException();

            short value;

            if (_propertyCount > 0)
            {
                switch (_propertyTag.ValueTnefType)
                {
                    case PropertyType.Boolean:
                        value = (short)(ReadInt32() & 0xFF);
                        break;
                    case PropertyType.I2:
                        value = (short)(ReadInt32() & 0xFFFF);
                        break;
                    case PropertyType.Error:
                    case PropertyType.Long:
                        value = (short)ReadInt32();
                        break;
                    case PropertyType.Currency:
                    case PropertyType.I8:
                        value = (short)ReadInt64();
                        break;
                    case PropertyType.Double:
                        value = (short)ReadDouble();
                        break;
                    case PropertyType.R4:
                        value = (short)ReadSingle();
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }
            else
            {
                switch (_reader.AttributeType)
                {
                    case AttributeType.Short: value = ReadInt16(); break;
                    case AttributeType.Long: value = (short)ReadInt32(); break;
                    case AttributeType.Word: value = ReadInt16(); break;
                    case AttributeType.DWord: value = (short)ReadInt32(); break;
                    case AttributeType.Byte: value = ReadInt16(); break;
                    default: throw new InvalidOperationException();
                }
            }

            _valueIndex++;

            return value;
        }

        /// <summary>
        /// Read the value as a 32-bit integer.
        /// </summary>
        /// <remarks>
        /// Reads any integer-based attribute or property value as a 32-bit integer.
        /// </remarks>
        /// <returns>The value as a 32-bit integer.</returns>
        /// <exception cref="InvalidOperationException">
        /// There are no more values to read or the value could not be read as a 32-bit integer.
        /// </exception>
        /// <exception cref="EndOfStreamException">
        /// The TNEF stream is truncated and the value could not be read.
        /// </exception>
        public int ReadValueAsInt32()
        {
            if (_valueIndex >= _valueCount || _reader.StreamOffset > RawValueStreamOffset)
                throw new InvalidOperationException();

            int value;

            if (_propertyCount > 0)
            {
                switch (_propertyTag.ValueTnefType)
                {
                    case PropertyType.Boolean:
                        value = ReadInt32() & 0xFF;
                        break;
                    case PropertyType.I2:
                        value = ReadInt32() & 0xFFFF;
                        break;
                    case PropertyType.Error:
                    case PropertyType.Long:
                        value = ReadInt32();
                        break;
                    case PropertyType.Currency:
                    case PropertyType.I8:
                        value = (int)ReadInt64();
                        break;
                    case PropertyType.Double:
                        value = (int)ReadDouble();
                        break;
                    case PropertyType.R4:
                        value = (int)ReadSingle();
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }
            else
            {
                switch (_reader.AttributeType)
                {
                    case AttributeType.Short: value = ReadInt16(); break;
                    case AttributeType.Long: value = ReadInt32(); break;
                    case AttributeType.Word: value = ReadInt16(); break;
                    case AttributeType.DWord: value = ReadInt32(); break;
                    case AttributeType.Byte: value = ReadInt32(); break;
                    default: throw new InvalidOperationException();
                }
            }

            _valueIndex++;

            return value;
        }

        /// <summary>
        /// Read the value as a 64-bit integer.
        /// </summary>
        /// <remarks>
        /// Reads any integer-based attribute or property value as a 64-bit integer.
        /// </remarks>
        /// <returns>The value as a 64-bit integer.</returns>
        /// <exception cref="InvalidOperationException">
        /// There are no more values to read or the value could not be read as a 64-bit integer.
        /// </exception>
        /// <exception cref="EndOfStreamException">
        /// The TNEF stream is truncated and the value could not be read.
        /// </exception>
        public long ReadValueAsInt64()
        {
            if (_valueIndex >= _valueCount || _reader.StreamOffset > RawValueStreamOffset)
                throw new InvalidOperationException();

            long value;

            if (_propertyCount > 0)
            {
                switch (_propertyTag.ValueTnefType)
                {
                    case PropertyType.Boolean:
                        value = ReadInt32() & 0xFF;
                        break;
                    case PropertyType.I2:
                        value = ReadInt32() & 0xFFFF;
                        break;
                    case PropertyType.Error:
                    case PropertyType.Long:
                        value = ReadInt32();
                        break;
                    case PropertyType.Currency:
                    case PropertyType.I8:
                        value = ReadInt64();
                        break;
                    case PropertyType.Double:
                        value = (long)ReadDouble();
                        break;
                    case PropertyType.R4:
                        value = (long)ReadSingle();
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }
            else
            {
                switch (_reader.AttributeType)
                {
                    case AttributeType.Short: value = ReadInt16(); break;
                    case AttributeType.Long: value = ReadInt32(); break;
                    case AttributeType.Word: value = ReadInt16(); break;
                    case AttributeType.DWord: value = ReadInt32(); break;
                    case AttributeType.Byte: value = ReadInt64(); break;
                    default: throw new InvalidOperationException();
                }
            }

            _valueIndex++;

            return value;
        }

        /// <summary>
        /// Read the value as a string.
        /// </summary>
        /// <remarks>
        /// Reads any string or binary blob values as a string.
        /// </remarks>
        /// <returns>The value as a string.</returns>
        /// <exception cref="InvalidOperationException">
        /// There are no more values to read or the value could not be read as a string.
        /// </exception>
        /// <exception cref="EndOfStreamException">
        /// The TNEF stream is truncated and the value could not be read.
        /// </exception>
        public string ReadValueAsString()
        {
            if (_valueIndex >= _valueCount || _reader.StreamOffset > RawValueStreamOffset)
                throw new InvalidOperationException();

            string value;

            if (_propertyCount > 0)
            {
                switch (_propertyTag.ValueTnefType)
                {
                    case PropertyType.Unicode: value = ReadUnicodeString(); break;
                    case PropertyType.String8: value = ReadString(); break;
                    case PropertyType.Binary: value = ReadString(); break;
                    default: throw new InvalidOperationException();
                }
            }
            else
            {
                switch (_reader.AttributeType)
                {
                    case AttributeType.String: value = ReadAttrString(); break;
                    case AttributeType.Text: value = ReadAttrString(); break;
                    case AttributeType.Byte: value = ReadAttrString(); break;
                    default: throw new InvalidOperationException();
                }
            }

            _valueIndex++;

            return value;
        }

        /// <summary>
        /// Read the value as a Uri.
        /// </summary>
        /// <remarks>
        /// Reads any string or binary blob values as a Uri.
        /// </remarks>
        /// <returns>The value as a Uri.</returns>
        /// <exception cref="InvalidOperationException">
        /// There are no more values to read or the value could not be read as a string.
        /// </exception>
        /// <exception cref="EndOfStreamException">
        /// The TNEF stream is truncated and the value could not be read.
        /// </exception>
        internal Uri ReadValueAsUri()
        {
            var value = ReadValueAsString();

            if (Uri.IsWellFormedUriString(value, UriKind.Absolute))
                return new Uri(value, UriKind.Absolute);

            if (Uri.IsWellFormedUriString(value, UriKind.Relative))
                return new Uri(value, UriKind.Relative);

            return null;
        }

        /// <summary>
        /// Serves as a hash function for a <see cref="PropertyReader"/> object.
        /// </summary>
        /// <remarks>
        /// Serves as a hash function for a <see cref="PropertyReader"/> object.
        /// </remarks>
        /// <returns>A hash code for this instance that is suitable for use in hashing algorithms
        /// and data structures such as a hash table.</returns>
        public override int GetHashCode()
        {
            return _reader.GetHashCode();
        }

        /// <summary>
        /// Determine whether the specified <see cref="object"/> is equal to the current <see cref="PropertyReader"/>.
        /// </summary>
        /// <remarks>
        /// Determines whether the specified <see cref="object"/> is equal to the current <see cref="PropertyReader"/>.
        /// </remarks>
        /// <param name="obj">The <see cref="object"/> to compare with the current <see cref="PropertyReader"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="object"/> is equal to the current
        /// <see cref="PropertyReader"/>; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return obj is PropertyReader prop && prop._reader == _reader;
        }

        void LoadPropertyCount()
        {
            if ((_propertyCount = ReadInt32()) < 0)
            {
                _reader.SetComplianceError(ComplianceStatus.InvalidPropertyLength);
                _propertyCount = 0;
            }

            _propertyIndex = 0;
            _valueCount = 0;
            _valueIndex = 0;
            _decoder = null;
        }

        int ReadValueCount()
        {
            int count;

            if ((count = ReadInt32()) < 0)
            {
                _reader.SetComplianceError(ComplianceStatus.InvalidAttributeValue);
                return 0;
            }

            return count;
        }

        void LoadValueCount()
        {
            if (_propertyTag.IsMultiValued)
            {
                _valueCount = ReadValueCount();
            }
            else
            {
                switch (_propertyTag.ValueTnefType)
                {
                    case PropertyType.Unicode:
                    case PropertyType.String8:
                    case PropertyType.Binary:
                    case PropertyType.Object:
                        _valueCount = ReadValueCount();
                        break;
                    default:
                        _valueCount = 1;
                        break;
                }
            }

            _valueIndex = 0;
            _decoder = null;
        }

        void LoadRowCount()
        {
            if ((_rowCount = ReadInt32()) < 0)
            {
                _reader.SetComplianceError(ComplianceStatus.InvalidRowCount);
                _rowCount = 0;
            }

            _propertyCount = 0;
            _propertyIndex = 0;
            _valueCount = 0;
            _valueIndex = 0;
            _decoder = null;
            _rowIndex = 0;
        }

        internal void Load()
        {
            _propertyTag = PropertyTag.Null;
            _rawValueOffset = 0;
            _rawValueLength = 0;
            _propertyCount = 0;
            _propertyIndex = 0;
            _valueCount = 0;
            _valueIndex = 0;
            _decoder = null;
            _rowCount = 0;
            _rowIndex = 0;

            switch (_reader.AttributeTag)
            {
                case AttributeTag.MapiProperties:
                case AttributeTag.Attachment:
                    LoadPropertyCount();
                    break;
                case AttributeTag.RecipientTable:
                    LoadRowCount();
                    break;
                default:
                    _rawValueLength = _reader.AttributeRawValueLength;
                    _rawValueOffset = _reader.StreamOffset;
                    _valueCount = 1;
                    break;
            }
        }
    }
}