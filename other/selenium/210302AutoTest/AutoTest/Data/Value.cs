using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTest.Data
{
    [Serializable]
    public class Value
    {
        public Value(object instance)
        {
            this.RawData = instance is Value ? ((Value)instance).RawData : instance;
        }

        public readonly object RawData;
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj is Value)
                obj = ((Value)obj).RawData;

            return object.Equals(this.RawData, obj) ||
                string.Equals((this.RawData ?? "").ToString(), (obj ?? "").ToString());
        }
        public override int GetHashCode()
        {
            return (this.RawData ?? "").ToString().GetHashCode();
        }
        public override string ToString()
        {
            return (RawData ?? "").ToString();
        }

        public static bool operator ==(Value value1, Value value2)
        {
            if (object.Equals(value1, null))
                return object.Equals(value2, null);
            if (object.Equals(value2, null))
                return object.Equals(value1, null);

            return object.Equals(value1, value2) ||
                object.Equals(value1.RawData, value2.RawData) ||
                string.Equals((value1.RawData ?? "").ToString(), (value2.RawData ?? "").ToString());
        }
        public static bool operator !=(Value value1, Value value2)
        {
            return !(value1 == value2);
        }
        public static bool operator >(Value value1, Value value2)
        {
            if (value1 == null)
                return false;
            else if (value2 == null)
                return true;

            float v1;
            float v2;
            if (float.TryParse(value1.RawData.ToString(), out v1) && float.TryParse(value2.RawData.ToString(), out v2))
                return v1 > v2;
            return string.Compare(value1.RawData.ToString(), value2.RawData.ToString()) > 0;
        }
        public static bool operator <(Value value1, Value value2)
        {
            return !(value1 > value2);
        }
        public static bool operator >=(Value value1, Value value2)
        {
            return value1 > value2 || value1 == value2;
        }
        public static bool operator <=(Value value1, Value value2)
        {
            return !(value1 >= value2);
        }

        public static explicit operator float(Value value)
        {
            if (value == null || value.RawData == null || string.IsNullOrWhiteSpace(value.RawData.ToString()))
                return default(float);

            if (value.RawData is float)
                return (float)value.RawData;

            if (value.RawData is string)
                return float.Parse(value.RawData as string);

            throw new Exception("VariableValue contains value of " + value.RawData.GetType().FullName + " , cannot compare with int");
        }
        public static implicit operator Value(float value)
        {
            return new Value(value);
        }
        public static explicit operator string(Value value)
        {
            if (value == null || value.RawData == null)
                return default(string);

            if (value.RawData is string)
                return (string)value.RawData;

            return value.RawData.ToString();
        }
        public static implicit operator Value(string value)
        {
            return new Value(value);
        }
        public static explicit operator bool(Value value)
        {
            if (value == null || value.RawData == null || string.IsNullOrWhiteSpace(value.RawData.ToString()))
                return default(bool);

            if (value.RawData is bool)
                return (bool)value.RawData;

            string st = value.RawData.ToString();
            return string.Equals(st, "true", StringComparison.OrdinalIgnoreCase)
                || string.Equals(st, "1", StringComparison.OrdinalIgnoreCase)
                || string.Equals(st, "on", StringComparison.OrdinalIgnoreCase)
                || string.Equals(st, "yes", StringComparison.OrdinalIgnoreCase);
        }
        public static implicit operator Value(bool value)
        {
            return new Value(value);
        }

    }

    public static class ValueExtension
    {
        public static bool IsEmpty(this Value instance)
        {
            return instance.RawData == null || instance.RawData.ToString() == string.Empty;
        }
    }
}
