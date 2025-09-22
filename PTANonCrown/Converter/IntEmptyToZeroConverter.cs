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

    public class IntEmptyToZeroConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // When displaying in the UI
            if (value == null)
                return 0;

            if (value is int intVal)
                return intVal;

            if (int.TryParse(value.ToString(), out int parsed))
                return parsed;

            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // When saving back from the UI
            if (value == null)
                return 0;

            if (value is int intVal)
                return intVal;

            if (int.TryParse(value.ToString(), out int parsed))
                return parsed;

            return 0;
        }
    }


}