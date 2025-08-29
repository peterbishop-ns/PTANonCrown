using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using static System.Net.Mime.MediaTypeNames;

namespace PTANonCrown.Data.Models

{
    public class TreeLive : BaseModel
    {
        private static readonly Dictionary<int, int> _dbhHeightLookupHardwood = new Dictionary<int, int>
    {
        {2, 2 },
        {4, 3 },
        {6, 5 },
        {8,  6 },
        {10, 7 },
        {12, 8 },
        {14, 8 },
        {16, 9 },
        {18, 10 },
        {20, 10 },
        {22, 11 },
        {24, 12 },
        {26,  12 },
        {28,  13},
        {30, 14 },
        {32, 14 },
        {34, 14 },
        {36, 15 },
        {38, 16 },
        {40, 16 },
        {42, 16 },
        {44, 16 },
        {46, 17 },
        {98, 17 }
};

        private static readonly Dictionary<int, int> _dbhHeightLookupSoftwood = new Dictionary<int, int>
    {{10,   2},
        {12,    3},
        {14,    5},
        {16,    7},
        {18,    9},
        {20,    10},
        {22,    10},
        {24,    11},
        {26,    12},
        {28,    13},
        {30,    14},
        {32,    15},
        {34,    16},
        {36,    17},
        {38,    17},
        {40,    17},
        {42,    18},
        {44,    18},
        {46,    19},
        {48,    19},
        {50,    20},
        { 90,  20}};

        private int _dbh_cm;
        private int _heightPredicted_m;
        private PlantedMethod _plantedMethod;
        private bool _pLInSitu;
        private TreeSpecies _treeSpecies;

        public TreeLive()
        {
            //TreeSpecies = new TreeSpecies();
            //TreeSpeciesFilteredList = new ObservableCollection<TreeSpecies>();
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
        public int DBH_cm
        {
            get => _dbh_cm;
            set
            {
                if (_dbh_cm != value)
                {
                    _dbh_cm = value;
                    OnPropertyChanged(nameof(DBH_cm));
                    OnDBHChanged();
                    OnPropertyChanged(nameof(HeightPredicted_m)); // Notify UI that BasalArea changed

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
        public int Height_m
        {
            get => _height_m;
            set
            {
                if (_height_m != value)
                {
                    _height_m = value;
                    OnPropertyChanged();
                }
            }
        }

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
        }

        public int ID { get; set; }

        // Calculated
        public bool IsMerchantable => DBH_cm > 9 ? true : false;

        public bool Legacy { get; set; }

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

        public int PlotID { get; set; }

        public bool SCanopy { get; set; }

        private string _searchSpecies;


        public string SearchSpecies
        {
            get => _searchSpecies;
            set
            {
                if (_searchSpecies != value)
                {
                    _searchSpecies = value;
                    OnPropertyChanged();
                    OnSearchSpeciesChanged(value);
                }
            }

        }

        private async void OnSearchSpeciesChanged(string searchString)
        {
            var result = LookupTrees
                .FirstOrDefault(t => string.Equals(t.ShortCode, searchString, StringComparison.OrdinalIgnoreCase));
                TreeSpecies = result;

        }


        public int TreeSpeciesID { get; set; }


        public virtual TreeSpecies TreeSpecies
        {
            get => _treeSpecies;
            set
            {
                if (_treeSpecies != value)
                {
                    _treeSpecies = value;
                    OnPropertyChanged();
                    OnTreeSpeciesChanged();
                }
                    
            }
        }
        private void OnTreeSpeciesChanged()
        {
            SearchSpecies = TreeSpecies?.ShortCode;
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
        public int GetHeightPredictedFromDBH(Dictionary<int, int> lookup, int DBH_cm)
        {
            var height = Interpolate(lookup, DBH_cm);

            return (int)Math.Round(height);
        }

        public void OnDBHChanged()
        {
            PredictHeight();
        }

        public void PredictHeight()
        {
            
            if(TreeSpecies is null) { return; }
            switch (TreeSpecies.HardwoodSoftwood)
            {
                case HardwoodSoftwood.Softwood: 
                    HeightPredicted_m = GetHeightPredictedFromDBH(_dbhHeightLookupSoftwood, DBH_cm);
                    break;

                case HardwoodSoftwood.Hardwood:
                    HeightPredicted_m = GetHeightPredictedFromDBH(_dbhHeightLookupHardwood, DBH_cm);
                    break;

                default:
                    break;
                    // Handle unexpected values
                    //throw new InvalidOperationException($"Unknown HardwoodSoftwood value: {TreeSpecies.HardwoodSoftwood}. Expected 1 (Softwood) or 2 (Hardwood).");
            }
        }


    }
}