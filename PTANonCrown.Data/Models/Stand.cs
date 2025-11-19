using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

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

        public void ValidateAll()
        {
            this.ValidateAllProperties();
        }


        [Required]
        public string CruiseID
        {
            get => _cruiseID;
            set
            {
                if (_cruiseID != value)
                {
                    _cruiseID = value;
                    ValidateProperty(_cruiseID, nameof(CruiseID));

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

        [Required]
        public string? Location
        {
            get => _location;
            set
            {
                if (_location != value)
                {
                    _location = value;
                    ValidateProperty(_location, nameof(Location)); // triggers validation

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

        [Required]
        public string PlannerID
        {
            get => _plannerID;
            set
            {
                if (_plannerID != value)
                {
                    _plannerID = value;
                    ValidateProperty(_plannerID, nameof(PlannerID)); 

                    OnPropertyChanged();
                }
            }
        }

        public virtual ObservableCollection<Plot> Plots { get; set; }


        [Required(ErrorMessage = "Stand Number is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Stand Number must be a positive integer")]
        public int StandNumber
        {
            get => _standNumber;
            set
            {
                if (_standNumber != value)
                {
                    _standNumber = value;
                    ValidateProperty(_standNumber, nameof(StandNumber)); 
                    OnPropertyChanged();
                }
            }
        }
    }
}
