using System;
using System.Collections.Generic;
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
        public Treatment Treatment { get; set; }

        public bool IsActive { get; set; } 
    }
}
