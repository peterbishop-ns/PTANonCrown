using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTANonCrown.Models
{
    public class Stand : BaseModel
    {
        
        public Stand()
        {
            Plots = new ObservableCollection<Plot>();
        }
        public int ID { get; set; }
        private int _standNumber;

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


        public int CruiseID { get; set; }
        public int PlannerID { get; set; }
        public string? Organization { get; set; }
        public float? Easting { get; set; }
        public float? Northing { get; set; }
        public int Ecodistrict { get; set; }
        public string? Location { get; set; }

        private ObservableCollection<Plot> _plots;

        public ObservableCollection<Plot> Plots
        {
            get => _plots;
            set
            {
                if (_plots != value)
                {
                    _plots = value;
                    OnPropertyChanged();
                }

            }
        } 
    }
}
