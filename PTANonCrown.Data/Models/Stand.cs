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




        [Required]
        public string CruiseID
        {
            get => _cruiseID;
            set => SetProperty(ref _cruiseID, value, true);

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
            set => SetProperty(ref _location, value, true);

        }

        public float? Area_ha
        {
            get => _area_ha;
            set => SetProperty(ref _area_ha, value, false);

        }

        public string? Organization
        {
            get => _organization;
            set => SetProperty(ref _organization, value, false);

        }

        public string? Comment
        {
            get => _comment;
            set => SetProperty(ref _comment, value, false);
        }

        [Required]
        public string PlannerID
        {
            get => _plannerID;
            set => SetProperty(ref _plannerID, value, true);

        }

        public virtual ObservableCollection<Plot> Plots { get; set; }


        [Required(ErrorMessage = "Stand Number is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Stand Number must be a positive number")]
        public int StandNumber
        {
            get => _standNumber;
            set => SetProperty(ref _standNumber, value, true);

        }
    }
}
