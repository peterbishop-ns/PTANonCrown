using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTANonCrown.Models
{
    public class TreeLive : BaseModel
    {
        public int ID { get; set; }
        public int TreeNumber { get; set; }
        public int PlotID { get; set; }
        private int _species;
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

        private TreeLookup _treeLookup;

        public TreeLookup TreeLookup
        {
            get => _treeLookup;
            set
            {
                if (_treeLookup != value)
                {
                    _treeLookup = value;
                    OnPropertyChanged();
                }
            }
        }


        public ObservableCollection<TreeLookup> TreeLookupFilteredList { get; set; } = new ObservableCollection<TreeLookup>();




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
                }
            }
        }

        private int _dbh_cm;
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

        public TreeLive()
        {
            TreeLookupFilteredList = new ObservableCollection<TreeLookup>();
        }
        public decimal Height_m { get; set; }
        public bool AGS { get; set; }
        public bool LIT { get; set; }
        public bool Cavity { get; set; }
        public bool Diversity { get; set; }
        public bool Legacy { get; set; }
        public bool SCanopy { get; set; }
        public bool Mast { get; set; }
        public bool PLInSitu { get; set; }
        public bool PLExSitu { get; set; }


        // Calculated 
        public bool IsMerchantable => DBH_cm > 9 ? true : false;
        public double BasalArea => DBH_cm == 0 ? 0 : (DBH_cm * DBH_cm) * 0.00007854;
        public double TreesPerHectare => BasalArea == 0 ? 0 : 2 / BasalArea; // todo confirm; is the "2" constant? Isn't this plot area 

        public Plot Plot { get; set; }
    
    }
}
