using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTANonCrown.Converter
{

    public class CollectionCountToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is System.Collections.ICollection collection)
            {
                bool hasItems = collection.Count > 0;

                // By default, show when count > 0
                if (parameter?.ToString() == "Invert")
                    return !hasItems; // Show when empty if inverted

                return hasItems; // Show when not empty
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }

}
