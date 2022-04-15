using System;
using System.Collections.Generic;

namespace ProCare.API.PBM
{
    public static class Extensions
    {
        public static string ConvertToDateString(this DateTime? value, bool emptyStringIfNull = true)
        {
            string output = null;

            if (value.HasValue)
            {
                output = value.Value.ToString("yyyyMMdd");
            }
            else if (emptyStringIfNull)
            {
                output = "";
            }

            return output;
        }

        public static int? ParseNullableInt(this string value)
        {
            int? parsedValue = null;

            if (!string.IsNullOrWhiteSpace(value))
            {
                parsedValue = int.Parse(value);
            }

            return parsedValue;
        }

        public static double? ParseNullableDouble(this string value)
        {
            double? parsedValue = null;

            if (!string.IsNullOrWhiteSpace(value))
            {
                parsedValue = double.Parse(value);
            }

            return parsedValue;
        }

        public static void AddRange<K, T, TI>(this IDictionary<K, T> target, IEnumerable<TI> source, Func<TI, K> key, Func<TI, T> selector, bool set = true)
        {
            source.ForEach(i =>
            {
                var dKey = key(i);
                var dValue = selector(i);
                if (set)
                {
                    target[dKey] = dValue;
                }
                else
                {
                    target.Add(key(i), selector(i));
                }
            });
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
            {
                action(item);
            }
        }
    }
}
