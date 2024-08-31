using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace komaxApp.Utility.ExtensionMethod
{
    public static class StringExtensions
    {
        public static bool TryConvertToInt(this string str, out int result)
        {
            return int.TryParse(str, out result);
        }


        public static double ToDoble(this string str, double defaultValue = 0)
        {
            if (double.TryParse(str, out double result))
            {
                return result;
            }
            return defaultValue;
        }
        public static int ToInt32(this string str, int defaultValue = 0)
        {
            if (int.TryParse(str, out int result))
            {
                return result;
            }
            return defaultValue;
        }
        public static int? ToNullableInt32(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return null;
            }
            return Convert.ToInt32(str);
        }



        public static DateTime ToDateTime(this string str, DateTime defaultValue, string format = null, CultureInfo cultureInfo = null)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return defaultValue;
            }

            bool success;
            DateTime parsedDate;

            if (string.IsNullOrEmpty(format))
            {
                // Try parsing the date without a specific format
                success = DateTime.TryParse(str, cultureInfo, DateTimeStyles.None, out parsedDate);
            }
            else
            {
                // Try parsing the date with a specific format
                success = DateTime.TryParseExact(str, format, cultureInfo, DateTimeStyles.None, out parsedDate);
            }

            return success ? parsedDate : defaultValue;
        }
        #region Null Not accatable
        //public static double ToDouble(this string str, NumberStyles numberStyles = NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo cultureInfo = null)
        //{
        //    if (string.IsNullOrWhiteSpace(str))
        //    {
        //        throw new ArgumentException("String cannot be null or empty", nameof(str));
        //    }

        //    double result;
        //    bool success = double.TryParse(str, numberStyles, cultureInfo, out result);

        //    if (!success)
        //    {
        //        throw new FormatException($"The string '{str}' is not a valid double.");
        //    }

        //    return result;
        //} 
        #endregion


        public static double? ToDouble(this string str, NumberStyles numberStyles = NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo cultureInfo = null)
        {
            // Return null if the string is null or empty
            if (string.IsNullOrWhiteSpace(str))
            {
                return null;
            }

            // Try to parse the string into a double
            double result;
            bool success = double.TryParse(str, numberStyles, cultureInfo ?? CultureInfo.InvariantCulture, out result);

            // Throw an exception if parsing fails
            if (!success)
            {
                throw new FormatException($"The string '{str}' is not a valid double.");
            }

            return result;
        }



    }
}
