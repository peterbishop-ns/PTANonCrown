using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

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
        private decimal _heightPredicted_m;
        private bool _pLExSitu;
        private bool _pLInSitu;
        private string _searchSpecies;
        private TreeSpecies _treeLookup;

        public TreeLive()
        {
            TreeLookup = new TreeSpecies();
            TreeLookupFilteredList = new ObservableCollection<TreeSpecies>();
        }

        public bool AGS { get; set; }
        public bool Cavity { get; set; }

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

        public bool Diversity { get; set; }

        public decimal Height_m { get; set; }

        public decimal HeightPredicted_m
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

        public bool PLExSitu
        {
            get => _pLExSitu;
            set
            {
                if (_pLExSitu != value)
                {
                    _pLExSitu = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool PLInSitu
        {
            get => _pLInSitu;
            set
            {
                if (_pLInSitu != value)
                {
                    _pLInSitu = value;
                    OnPropertyChanged();
                }
            }
        }

        public Plot Plot { get; set; }

        public int PlotID { get; set; }

        public bool SCanopy { get; set; }

        [NotMapped]
        public string SearchSpecies
        {
            get => _searchSpecies;
            set => SetProperty(ref _searchSpecies, value);

        }


        public TreeSpecies TreeLookup
        {
            get => _treeLookup;
            set
            {
                if (_treeLookup != value)
                {
                    _treeLookup = value;
                    OnPropertyChanged();
                    OnTreeLookupChanged();
                }
            }
        }

        [NotMapped]
        public ObservableCollection<TreeSpecies> TreeLookupFilteredList { get; set; } = new ObservableCollection<TreeSpecies>();

        public int TreeNumber { get; set; }

        public decimal GetHeightPredictedFromDBH(Dictionary<int, int> lookup, int DBH_cm)
        {

            return Interpolate(lookup, DBH_cm);
        }

        public void OnDBHChanged()
        {
            PredictHeight();
        }

        public void PredictHeight()
        {
            switch (TreeLookup.HardwoodSoftwood)
            {
                case 1: // Softwood
                    HeightPredicted_m = GetHeightPredictedFromDBH(_dbhHeightLookupSoftwood, DBH_cm);
                    break;

                case 2: // Hardwood
                    HeightPredicted_m = GetHeightPredictedFromDBH(_dbhHeightLookupHardwood, DBH_cm);
                    break;

                default:
                    break;
                    // Handle unexpected values
                    //throw new InvalidOperationException($"Unknown HardwoodSoftwood value: {TreeSpecies.HardwoodSoftwood}. Expected 1 (Softwood) or 2 (Hardwood).");
            }
        }

        private void OnTreeLookupChanged()
        {
            SearchSpecies = TreeLookup?.ShortCode;
        }
    }
}