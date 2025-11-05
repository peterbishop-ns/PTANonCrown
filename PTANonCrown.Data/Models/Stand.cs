using System.Collections.ObjectModel;

namespace PTANonCrown.Data.Models
{
    public class Stand : BaseModel
    {

        private int _standNumber;

        public Stand()
        {
            Plots = new ObservableCollection<Plot>();
        }

        public string CruiseID { get; set; }

        public int Ecodistrict { get; set; }

        public string? Location { get; set; }

        public float? Area_ha { get; set; } 

        public string? Organization { get; set; }
        public string? Comment { get; set; }

        public string PlannerID { get; set; }

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