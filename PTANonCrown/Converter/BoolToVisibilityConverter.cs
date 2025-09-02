using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
    using System;
    using System.Globalization;
    using Microsoft.Maui.Controls;


namespace PTANonCrown.Converter
{

    public class BoolToVisibilityConverter : IValueConverter
    {
        // If ConverterParameter is "Collapsed", false => Collapsed, true => Visible
        // If ConverterParameter is "Hidden", false => Hidden, true => Visible
        // If parameter is null or "Default", false => NotVisible (Collapsed)
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool boolValue = value is bool b && b;

            string param = parameter?.ToString()?.ToLower() ?? "collapsed";

            return boolValue
                ? true // Visible (in MAUI, usually use bool for IsVisible)
                : param switch
                {
                    "collapsed" => false, // IsVisible false
                    "hidden" => false,    // MAUI doesn't have separate Hidden, so use false
                    _ => false
                };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b)
                return b;

            return false;
        }
    }
}