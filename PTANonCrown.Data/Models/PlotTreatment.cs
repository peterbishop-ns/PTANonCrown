using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PTANonCrown.Data.Models;

namespace PTANonCrown.Data.Models
{
    public class PlotTreatment : BaseModel
    {
        public int PlotId { get; set; }
        
        
        public Plot Plot { get; set; }

        public int TreatmentId { get; set; }

        private Treatment _treatment;
        [NotMapped]
        public Treatment Treatment
        {
            get => _treatment;
            set
            {
                if (_treatment != value)
                {
                    _treatment = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(TreatmentName)); // optional, for safe binding
                }
            }
        }

        [NotMapped]
        public string TreatmentName => Treatment?.Name ?? "";

        private bool _isActive;
        public bool IsActive
        {
            get => _isActive;
            set
            {
                if (_isActive != value)
                {
                    _isActive = value;
                    OnPropertyChanged(); // assuming you're using INotifyPropertyChanged
                }
            }
        }
    }
}
