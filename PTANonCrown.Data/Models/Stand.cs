using System.Collections.ObjectModel;

namespace PTANonCrown.Data.Models
{
    public class Stand : BaseModel
    {
        private int _standNumber;
        private string _cruiseID = string.Empty;
        private int _ecodistrict;
        private string? _location;
        private float? _area_ha;
        private string? _organization;
        private string? _comment;
        private string _plannerID = string.Empty;

        public Stand()
        {
            Plots = new ObservableCollection<Plot>();
        }

        public string CruiseID
        {
            get => _cruiseID;
            set
            {
                if (_cruiseID != value)
                {
                    _cruiseID = value;
                    OnPropertyChanged();
                }
            }
        }

        public int Ecodistrict
        {
            get => _ecodistrict;
            set
            {
                if (_ecodistrict != value)
                {
                    _ecodistrict = value;
                    OnPropertyChanged();
                }
            }
        }

        public string? Location
        {
            get => _location;
            set
            {
                if (_location != value)
                {
                    _location = value;
                    OnPropertyChanged();
                }
            }
        }

        public float? Area_ha
        {
            get => _area_ha;
            set
            {
                if (_area_ha != value)
                {
                    _area_ha = value;
                    OnPropertyChanged();
                }
            }
        }

        public string? Organization
        {
            get => _organization;
            set
            {
                if (_organization != value)
                {
                    _organization = value;
                    OnPropertyChanged();
                }
            }
        }

        public string? Comment
        {
            get => _comment;
            set
            {
                if (_comment != value)
                {
                    _comment = value;
                    OnPropertyChanged();
                }
            }
        }

        public string PlannerID
        {
            get => _plannerID;
            set
            {
                if (_plannerID != value)
                {
                    _plannerID = value;
                    OnPropertyChanged();
                }
            }
        }

        public virtual ObservableCollection<Plot> Plots { get; set; }

        public int StandNumber
        {
            get => _standNumber;
            set
            {
                if (_standNumber != value)
                {
                    _standNumber = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
