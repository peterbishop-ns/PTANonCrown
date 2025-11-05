using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PTANonCrown.Data.Models
{
    public class BaseModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public int ID { get; set; }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(field, value))
            {
                field = value;
                OnPropertyChanged(propertyName);
                return true;
            }
            return false;
        }
  

        public static decimal Interpolate(Dictionary<int, int> lookup, int input)
        {
            // Method to interpolate a value
            // Compares an input value to the keys of a dict, and interplates the values
            if (lookup.ContainsKey(input))
                return lookup[input]; // Exact match

            var keys = lookup.Keys.OrderBy(k => k).ToList();

            // Edge cases: input lower than lowest key or higher than highest key
            if (input <= keys.First())
                return lookup[keys.First()];
            if (input >= keys.Last())
                return lookup[keys.Last()];

            // Find the two surrounding keys
            int lowerKey = keys.Last(k => k <= input);
            int upperKey = keys.First(k => k >= input);

            int lowerValue = lookup[lowerKey];
            int upperValue = lookup[upperKey];

            // If input is exactly one of the keys, no need to interpolate
            if (lowerKey == upperKey)
                return lowerValue;

            // Linear interpolation formula
            decimal fraction = (input - lowerKey) / (decimal)(upperKey - lowerKey);
            return lowerValue + fraction * (upperValue - lowerValue);
        }
    }
}