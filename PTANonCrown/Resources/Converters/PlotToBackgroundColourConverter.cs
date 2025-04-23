using PTANonCrown.Data.Models;
using System.Globalization;

namespace PTANonCrown.Resources.Converters
{
    public class PlotToBackgroundColourConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Plot plot)
            {
                //return plot.Equals(currentPlot) ? Color.FromRgb(20,20,20) : Color.FromRgb(100, 0, 0); // Set to green if it's the current plot
                return Color.FromRgb(20, 20, 20); // Set to green if it's the current plot
            }
            return Color.FromRgb(100, 0, 0); // Default color if not
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value; // Not needed in this scenario
        }
    }
}