using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.EntityFrameworkCore.Storage.Json;
using PTANonCrown.Data.Services;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static System.Net.Mime.MediaTypeNames;

namespace PTANonCrown.Data.Models

{
    public partial class TreeLive : BaseModel
    {
        private static readonly Dictionary<int, int> _dbhHeightLookupHardwood = new Dictionary<int, int>
   {{10, 7},
{12, 8},
{14, 8},
{16, 9},
{18, 10},
{20, 10},
{22, 11},
{24, 12},
{26, 12},
{28, 13},
{30, 14},
{32, 14},
{34, 14},
{36, 15},
{38, 16},
{40, 16},
{42, 16},
{44, 16},
{46, 17},
{48, 17},
{50, 17},
{52, 17},
{54, 17},
{56, 17},
{58, 17},
{60, 17},
{62, 17},
{64, 17},
{66, 17},
{68, 17},
{70, 17},
{72, 17},
{74, 17},
{76, 17},
{78, 17},
{80, 17},
{82, 17},
{84, 17},
{86, 17},
{88, 17},
{90, 17},
{92, 17},
{94, 17},
{96, 17},
{98, 17},
};

        private static readonly Dictionary<int, int> _dbhHeightLookupSoftwood = new Dictionary<int, int>
    {{2, 2},
{4, 3},
{6, 5},
{8, 7},
{10, 9},
{12, 10},
{14, 10},
{16, 11},
{18, 12},
{20, 13},
{22, 14},
{24, 15},
{26, 16},
{28, 17},
{30, 17},
{32, 17},
{34, 18},
{36, 18},
{38, 19},
{40, 19},
{42, 20},
{44, 20},
{46, 20},
{48, 20},
{50, 20},
{52, 20},
{54, 20},
{56, 20},
{58, 20},
{60, 20},
{62, 20},
{64, 20},
{66, 20},
{68, 20},
{70, 20},
{72, 20},
{74, 20},
{76, 20},
{78, 20},
{80, 20},
{82, 20},
{84, 20},
{86, 20},
{88, 20},
{90, 20},
{92, 20},
{94, 20},
{96, 20},
{98, 20}};

        private int _dbh_cm;
        private int _heightPredicted_m = 0;
        private PlantedMethod _plantedMethod;
        private bool _pLInSitu;

        public TreeLive()
        {

        }


        // Keeping LookupTrees list on the TreeLive itself is a workaround. 
        // Was running into issues with the Picker list, where it wouldn't set SelectedItem Correctly
        //  There were different binding contexts; LookupTrees was on the VM, whereas the selected tree species
        // Was on the row of the picker. 
        // Having LookupTrees as a prop of the TreeLive solved this issue. 
        [NotMapped]
        public List<TreeSpecies> LookupTrees { get; set; }
        public bool AGS { get; set; }

        private bool _cavity;
        public bool Cavity
        {
            get => _cavity;
            set
            {
                if (_cavity != value)
                {
                    _cavity = value;
                    OnPropertyChanged();
                }
            }
        }

        [Range(1, int.MaxValue, ErrorMessage = "DBH is required.")]
        public int DBH_cm
        {
            get => _dbh_cm;
            set
            {
                if (_dbh_cm != value)
                {
                    SetProperty(ref _dbh_cm, value, true);
                    OnPropertyChanged(nameof(IsMerchantable));
                    OnPropertyChanged(nameof(HeightToDiameter));
                    OnDBHChanged();
                }
            }
        }

      


        private bool _diversity;
        public bool Diversity
        {
            get => _diversity;
            set
            {
                if (_diversity != value)
                {
                    _diversity = value;
                    OnPropertyChanged();
                }
            }
        }


        private int _height_m;
        [Range(1, int.MaxValue, ErrorMessage = "Height is required.")]

        public int Height_m
        {
            get => _height_m;
            set
            {
                SetProperty(ref _height_m, value, true);
                OnPropertyChanged();
                OnPropertyChanged(nameof(HeightToDiameter));

            }
        }


        /*
        public int HeightPredicted_m
        {
            get => _heightPredicted_m;
            set
            {
                if (_heightPredicted_m != value)
                {
                    _heightPredicted_m = value;
                    OnPropertyChanged();
                }
            }
        }*/

        public int ID { get; set; }

        // Calculated
        public bool IsMerchantable => DBH_cm > 9 ? true : false;


        public bool LIT { get; set; }

        public bool Mast { get; set; }

        public PlantedMethod PlantedMethod
        {
            get => _plantedMethod;
            set
            {
                if (_plantedMethod != value)
                {
                    _plantedMethod = value;
                    OnPropertyChanged();
                }
            }
        }

        public Plot Plot { get; set; }

        public bool SCanopy { get; set; }

        [Required(ErrorMessage = "Tree Species is required.")]
        public string TreeSpeciesShortCode { get; set; }

        // -----------------------------
        // Observable SearchSpecies
        // -----------------------------

        private string _searchSpecies;
        public string SearchSpecies
        {
            get => _searchSpecies;
            set
            {
                if (_searchSpecies != value)
                {
                    SetProperty(ref _searchSpecies, value, true);
                    OnPropertyChanged();

                    // Run any dependent logic
                    OnSearchSpeciesChanged(_searchSpecies);
                }
            }
        }
        // Called automatically when SearchSpecies changes
        private void OnSearchSpeciesChanged(string value)
        {
            // Only update TreeSpecies if different to avoid recursion
            if (TreeSpecies?.ShortCode != value)
            {
                var result = LookupTrees
                    .FirstOrDefault(t => string.Equals(t.ShortCode, value, StringComparison.OrdinalIgnoreCase));
                TreeSpecies = result;
            }
        }

        // -----------------------------
        // Observable TreeSpecies
        // -----------------------------
        private TreeSpecies _treeSpecies;
        [NotMapped]
        [ForeignKey(nameof(TreeSpeciesShortCode))]
        public TreeSpecies TreeSpecies
        {
            get => _treeSpecies;
            set
            {
                if (_treeSpecies != value)
                {
                    _treeSpecies = value;
                    OnPropertyChanged();

                    // Only update SearchSpecies if different to avoid recursion
                    if (SearchSpecies != _treeSpecies?.ShortCode)
                        SearchSpecies = _treeSpecies?.ShortCode;
                    TreeSpeciesShortCode = _treeSpecies?.ShortCode;
                    OnPropertyChanged(nameof(SearchSpecies));
                    // Any additional dependent logic
                    Height_m = PredictHeight() ?? 0;

                    UpdateTreeLIT();
                    PredictHeight();
                }
            }
        }



        public void UpdateTreeLIT()
        {
            var forestGroup = this.Plot.ForestGroup;

            if (this.TreeSpecies == null)
                return;

            var species = this.TreeSpecies;
            var name = species.Name.ToLowerInvariant();

            if (name.Contains("red maple"))
            {
                // Red maple is LIT only in tolerant hardwood
                species.LIT = forestGroup == "TH";
                Console.WriteLine($"LIT for Red Maple is {species.LIT}. FG is {forestGroup}");
            }
            else if (name.Contains("white spruce"))
            {
                // White spruce is not LIT in these groups
                var notLitGroups = new[] { "CB", "OF", "HL", "PF" };
                species.LIT = !notLitGroups.Contains(forestGroup);
                Console.WriteLine($"LIT for White Spruce is {species.LIT}. FG is {forestGroup}");
            }
        }





        [NotMapped]
        public ObservableCollection<TreeSpecies> TreeSpeciesFilteredList { get; set; } = new ObservableCollection<TreeSpecies>();

        private int _treeNumber;
        public int TreeNumber
        {
            get => _treeNumber;
            set
            {
                if (_treeNumber != value)
                {
                    _treeNumber = value;
                    OnPropertyChanged();
                }
            }

        }

        [NotMapped]
        public float HeightToDiameter => DBH_cm > 0
                    ? (float)Height_m / DBH_cm
                    : 0f;


        public int GetHeightPredictedFromDBH(Dictionary<int, int> lookup, int DBH_cm)
        {
            if (DBH_cm == 0)
                return 0;
            
            var height = Interpolate(lookup, DBH_cm);

            return (int)Math.Round(height);
        }

        public void OnDBHChanged()
        {
            Height_m = PredictHeight() ?? 0;
        }

        public int? PredictHeight()
        {
            int? result = null;
            if(TreeSpecies is null || DBH_cm == 0) { return null; }

            try
            {
                switch (TreeSpecies.HardwoodSoftwood)
                {
                    case HardwoodSoftwood.Softwood:
                        //case HardwoodSoftwood.Softwood: 
                        result = GetHeightPredictedFromDBH(_dbhHeightLookupSoftwood, DBH_cm);
                        break;

                    //case HardwoodSoftwood.Hardwood:
                    case HardwoodSoftwood.Hardwood:
                        result = GetHeightPredictedFromDBH(_dbhHeightLookupHardwood, DBH_cm);
                        break;

                    default:
                        break;
                        // Handle unexpected values
                        //throw new InvalidOperationException($"Unknown HardwoodSoftwood value: {TreeSpecies.HardwoodSoftwood}. Expected 1 (Softwood) or 2 (Hardwood).");
                }
            }
            catch (Exception ex)
            {
                AppLoggerData.Log($"{ex.InnerException.ToString()}", "PredictHeight");
            }
            return result;

        }


    }
}