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
        private bool _focusSpecies;
        public bool FocusSpecies
        {
            get => _focusSpecies;
            set {
                SetProperty(ref _focusSpecies, value);
                OnPropertyChanged();
            }
        }
        public TreeLiveViewModel(TreeLive model, IEnumerable<TreeSpecies> allSpecies)
        {
            Model = model;
            _allSpecies = allSpecies.Where(sp=> sp.ShortCode != "n/a").ToList();

            // filtered list is empty until user types
            FilteredSpecies = new TreeSpecies();

            // hydrate the species
            Model.TreeSpecies = _allSpecies.Where(t => t.ShortCode == Model.TreeSpeciesShortCode).FirstOrDefault();

            // Preselect the species if this tree already has one
            if (model.TreeSpeciesShortCode != null)
            {
                SpeciesSearchText = $"{model.TreeSpeciesShortCode} - {model.TreeSpecies.Name}";
            }

            SelectSpeciesCommand = new Command<TreeSpecies>(OnSpeciesSelected);

        }

        public Action? RemoveFocusAction { get; set; }


        public void SelectSpecies(TreeSpecies species)
        {
            Model.TreeSpecies = species;
            OnPropertyChanged(nameof(LT));
            OnPropertyChanged(nameof(LIT));
            Model.TreeSpeciesShortCode = species.ShortCode;
            SpeciesSearchText = $"{species.ShortCode} - {species.Name}";

            FilteredSpecies = null;

        }

        public ICommand SelectSpeciesCommand { get; }

        private void OnSpeciesSelected(TreeSpecies species)
        {
            if (species == null) return;

            Model.TreeSpecies = species;
            Model.TreeSpeciesShortCode = species.ShortCode;


            SpeciesSearchText = $"{species.ShortCode} - {species.Name}";

            // Collapse the dropdown list
            FilteredSpecies = null;
        }


        public string SpeciesSearchText
        {
            get => _speciesSearchText;
            set
            {


                if (SetProperty(ref _speciesSearchText, value))
                {

                        FilterSpecies();
                }
                OnPropertyChanged();
            }
        }

        private void ResetSpecies()
        {
            Model.TreeSpecies = null;
            Model.TreeSpeciesShortCode = string.Empty;
            OnPropertyChanged(nameof(LT));
            OnPropertyChanged(nameof(LIT));

        }
        private void FilterSpecies()
        {
            if (string.IsNullOrWhiteSpace(SpeciesSearchText))
            {
                FilteredSpecies = null;
                ResetSpecies();
                return;
            }

            var query = SpeciesSearchText.ToLowerInvariant();

            // Find all matches first
            var matches = _allSpecies
                .Where(s =>
                    s.ShortCode.ToLower().Contains(query) ||
                    s.Name.ToLower().Contains(query) ||
                    $"{s.ShortCode} - {s.Name}".ToLower().Contains(query)
                )
                .ToList();
            // Clear & take only top 1 for the filtered list
            FilteredSpecies = matches.FirstOrDefault();

            // Reset species if zero or more than one match
            if (matches.Count != 1)
            {
                ResetSpecies();
            }
        }

        private string _speciesSearchText = string.Empty;

        private TreeSpecies? _filteredSpecies;
        public TreeSpecies? FilteredSpecies
        {
            get => _filteredSpecies;
            set
            {
                if (_filteredSpecies != value)
                {
                    _filteredSpecies = value;
                    OnPropertyChanged();
                }
            }
        }



        private readonly List<TreeSpecies> _allSpecies;



        // --- TreeNumber Property ---
        public int TreeNumber
        {
            get => Model.TreeNumber;
            set
            {
                if (Model.TreeNumber != value)
                {
                    Model.TreeNumber = value;
                    OnPropertyChanged(nameof(TreeNumber));
                }
            }
        }

        // --- DBH Property (Diameter at Breast Height) ---
        public int DBH
        {
            get => Model.DBH_cm;
            set
            {
                if (Model.DBH_cm != value)
                {
                    Model.DBH_cm = value;
                    OnPropertyChanged(nameof(DBH));
                }
            }
        }

        // --- Height Property ---
        public int Height
        {
            get => Model.Height_m;
            set
            {
                if (Model.Height_m != value)
                {
                    Model.Height_m = value;
                    OnPropertyChanged(nameof(Height));
                }
            }
        }

        // --- LT ---
        public bool LT => Model.TreeSpecies?.LT ?? false;


        // --- LIT ---
        public bool LIT => Model.TreeSpecies?.LIT ?? false;


        // --- AGS ---
        public bool AGS
        {
            get => Model.AGS;
            set
            {
                if (Model.AGS != value)
                {
                    Model.AGS = value;
                    OnPropertyChanged(nameof(AGS));
                }
            }
        }

        // --- Mast ---
        public bool Mast
        {
            get => Model.Mast;
            set
            {
                if (Model.Mast != value)
                {
                    Model.Mast = value;
                    OnPropertyChanged(nameof(Mast));
                }
            }
        }


                // --- Cavity ---
        public bool Cavity
        {
            get => Model.Cavity;
            set
            {
                if (Model.Cavity != value)
                {
                    Model.Cavity = value;
                    OnPropertyChanged(nameof(Cavity));
                }
            }
        }


        // --- Diversity ---
        public bool Diversity
        {
            get => Model.Diversity;
            set
            {
                if (Model.Diversity != value)
                {
                    Model.Diversity = value;
                    OnPropertyChanged(nameof(Diversity));
                }
            }
        }
        public PlantedMethod PlantedMethod
        {
            get => Model.PlantedMethod;
            set
            {
                if (Model.PlantedMethod != value)
                {
                    Model.PlantedMethod = value;
                    OnPropertyChanged(nameof(PlantedMethod));
                }
            }
        }

    }
}

