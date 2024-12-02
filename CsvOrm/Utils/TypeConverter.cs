using System;
using System.Globalization;

namespace CsvOrm.Utils
{
    public static class TypeConverter
    {
        public static object ConvertToType(string value, Type targetType)
        {
            if (targetType == typeof(string))
                return value;

            if (string.IsNullOrEmpty(value))
                return null;

            if (targetType == typeof(int))
                return int.Parse(value, CultureInfo.InvariantCulture);

            if (targetType == typeof(long))
                return long.Parse(value, CultureInfo.InvariantCulture);

            if (targetType == typeof(decimal))
                return decimal.Parse(value, CultureInfo.InvariantCulture);

            if (targetType == typeof(double))
                return double.Parse(value, CultureInfo.InvariantCulture);

            if (targetType == typeof(float))
                return float.Parse(value, CultureInfo.InvariantCulture);

            if (targetType == typeof(bool))
                return bool.Parse(value);

            if (targetType == typeof(DateTime))
                return DateTime.Parse(value, CultureInfo.InvariantCulture);

            if (targetType.IsEnum)
                return Enum.Parse(targetType, value);

            throw new NotSupportedException($"Type {targetType.Name} non pris en charge.");
        }
    }
}