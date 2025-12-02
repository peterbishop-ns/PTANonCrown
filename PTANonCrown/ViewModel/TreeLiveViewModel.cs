using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PTANonCrown.Data.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Windows.Media.AppBroadcasting;

namespace PTANonCrown.ViewModel
{
    public class TreeLiveViewModel : BaseViewModel
    {
        public TreeLive Model { get; }
        public List<TreeSpecies> LookupTrees { get; }

        public TreeLiveViewModel(TreeLive model, List<TreeSpecies> lookupTrees)
        {
            Model = model;
            LookupTrees = lookupTrees;
            SearchText = model.TreeSpeciesShortCode;

        }

        // UI state for the row
        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                SetProperty(ref _searchText, value);

                UpdateDropDown(value);
            }
        }

        public void UpdateDropDown(string value)
        {
            var match = LookupTrees
                .FirstOrDefault(t => !string.IsNullOrEmpty(t.ShortCode) &&
                                     string.Equals(t.ShortCode, value ?? string.Empty, StringComparison.OrdinalIgnoreCase));

            if (match == null)
            {
                // No species matches typed value → set TreeSpecies to null
                TreeSpecies = null;
                return;
            }

            // Only update if different
            if (TreeSpecies == null || TreeSpecies.ShortCode != match.ShortCode)
            {
                TreeSpecies = match;
            }
        }



        public TreeSpecies? TreeSpecies
        {
            get => Model.TreeSpecies;
            set
            {
                if (Model.TreeSpecies == value)
                    return;

                Model.TreeSpecies = value;

                // Prevent SearchText update from re-triggering UpdateDropDown
                SearchText = value?.ShortCode ?? string.Empty;

                OnPropertyChanged();
                OnPropertyChanged(nameof(SearchText));
                OnPropertyChanged(nameof(TreeSpeciesName));
            }
        }

        public ICommand SelectTreeCommand => new Command<TreeSpecies>(tree =>
        {
            TreeSpecies = tree;
        });

  
        // Expose the Name of the species directly
        public string TreeSpeciesName => TreeSpecies?.Name ?? "test";

        // Example for TreeNumber
        public int TreeNumber => Model.TreeNumber;

        // Example for DBH
        public float DBH => Model.DBH_cm;

        // Example for Height
        public float Height => Model.Height_m;
    }

}
