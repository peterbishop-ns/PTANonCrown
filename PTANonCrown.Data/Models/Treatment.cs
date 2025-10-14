using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTANonCrown.Data.Models
{
    public class Treatment : BaseLookup

    {

        public int ID { get; set; }


        private ICollection<PlotTreatment> _plotTreatments;
        public ICollection<PlotTreatment> PlotTreatments
        {
            get => _plotTreatments;
            set
            {
                if (_plotTreatments != value)
                {
                    _plotTreatments = value;
                    OnPropertyChanged();
                }
            }

        }
    }

}
