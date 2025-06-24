using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using System;

using System.Globalization;
using Microsoft.Maui.Controls;

namespace PTANonCrown.Converter
{

    public class ZeroToEmptyStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || (value is int intValue && intValue == 0))
                return string.Empty;

            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var str = value as string;
            if (string.IsNullOrWhiteSpace(str))
                return null;

            if (int.TryParse(str, out int result))
                return result;

            return null;
        }
    }


}