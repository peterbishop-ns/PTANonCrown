using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace PTANonCrown.Models
{
    public class TreeLive : BaseModel
    {
        private int _dbh_cm;
        private string _searchSpecies;
        private int _species;
        private TreeLookup _treeLookup;

        public TreeLive()
        {
            TreeLookupFilteredList = new ObservableCollection<TreeLookup>();
        }

        public bool AGS { get; set; }
        public double BasalArea => DBH_cm == 0 ? 0 : (DBH_cm * DBH_cm) * 0.00007854;
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
                    OnPropertyChanged(nameof(BasalArea)); // Notify UI that BasalArea changed
                    OnPropertyChanged(nameof(TreesPerHectare)); // Notify UI that BasalArea changed
                }
            }
        }

        public bool Diversity { get; set; }
        public decimal Height_m { get; set; }
        public int ID { get; set; }

        // Calculated
        public bool IsMerchantable => DBH_cm > 9 ? true : false;

        public bool Legacy { get; set; }
        public bool LIT { get; set; }
        public bool Mast { get; set; }
        public bool PLExSitu { get; set; }
        public bool PLInSitu { get; set; }
        public Plot Plot { get; set; }
        public int PlotID { get; set; }
        public bool SCanopy { get; set; }

        [NotMapped]
        public string SearchSpecies
        {
            get => _searchSpecies;
            set => SetProperty(ref _searchSpecies, value);

                 }

        public int Species
        {
            get => _species;
            set
            {
                if (_species != value)
                {
                    _species = value;
                    OnPropertyChanged();
                }
            }
        }

        [NotMapped]
        public TreeLookup TreeLookup
        {
            get => _treeLookup;
            set
            {
                if (_treeLookup != value)
                {
                    _treeLookup = value;
                    OnPropertyChanged();
                    Species = _treeLookup.ID;
                }
            }
        }

        [NotMapped]
        public ObservableCollection<TreeLookup> TreeLookupFilteredList { get; set; } = new ObservableCollection<TreeLookup>();
        public int TreeNumber { get; set; }
        public double TreesPerHectare => BasalArea == 0 ? 0 : 2 / BasalArea; // todo confirm; is the "2" constant? Isn't this plot area
    }
}