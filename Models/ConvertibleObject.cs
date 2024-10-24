/*namespace swap_service.Models
{
    using System;

    class ConvertibleObject : Object, IConvertible
    {
        private object value;

        public ConvertibleObject(object value)
        {
            this.value = value;
        }

        public TypeCode GetTypeCode()
        {
            return Type.GetTypeCode(value.GetType());
        }

        public List<double>ToDoubleList()
        {
            return (List<double>)Convert.ChangeType(value, typeof(List<double>));
        }
        public bool ToBoolean(IFormatProvider provider)
        {
            return Convert.ToBoolean(value);
        }

        public byte ToByte(IFormatProvider provider)
        {
            return Convert.ToByte(value);
        }

        public char ToChar(IFormatProvider? provider)
        {
            throw new NotImplementedException();
        }

        public DateTime ToDateTime(IFormatProvider? provider)
        {
            throw new NotImplementedException();
        }

        public decimal ToDecimal(IFormatProvider? provider)
        {
            throw new NotImplementedException();
        }

        // Implement other IConvertible methods...

        public double ToDouble(IFormatProvider provider)
        {
            if (value is double)
                return (double)value;
            else if (value is string)
                return Convert.ToDouble(value);
            else
                throw new InvalidCastException();
        }

        public short ToInt16(IFormatProvider? provider)
        {
            throw new NotImplementedException();
        }

        public int ToInt32(IFormatProvider? provider)
        {
            throw new NotImplementedException();
        }

        public long ToInt64(IFormatProvider? provider)
        {
            throw new NotImplementedException();
        }

        public sbyte ToSByte(IFormatProvider? provider)
        {
            throw new NotImplementedException();
        }

        public float ToSingle(IFormatProvider? provider)
        {
            throw new NotImplementedException();
        }

        public string ToString(IFormatProvider? provider)
        {
            throw new NotImplementedException();
        }

        public object ToType(Type conversionType, IFormatProvider? provider)
        {
            throw new NotImplementedException();
        }

        public ushort ToUInt16(IFormatProvider? provider)
        {
            throw new NotImplementedException();
        }

        public uint ToUInt32(IFormatProvider? provider)
        {
            throw new NotImplementedException();
        }

        public ulong ToUInt64(IFormatProvider? provider)
        {
            throw new NotImplementedException();
        }

        // Implement other IConvertible methods...
    }
}
*/