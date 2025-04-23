using PTANonCrown.Data.Models;
using System.Collections.ObjectModel;
using System.Globalization;

namespace PTANonCrown.Converter
{
    public class SpeciesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var shortCode = value as string;
            if (string.IsNullOrEmpty(shortCode))
                return null;

            // Assuming LookupTrees is available in your ViewModel and contains all TreeSpecies
            var treeSpeciesCollection = parameter as ObservableCollection<TreeLookup>;

            return treeSpeciesCollection?.FirstOrDefault(t => t.ShortCode.Equals(shortCode, StringComparison.OrdinalIgnoreCase));
            //

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var treeSpecies = value as TreeLookup;
            return treeSpecies?.ShortCode; // Return the ShortCode of the selected TreeSpecies
                                           //

        }
    }

}