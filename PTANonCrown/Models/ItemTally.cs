using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTANonCrown.Models
{
    public class ItemTally
    {
        public int ID { get; set; }
        public int PlotID { get; set; }
        public int DBH_start { get; set; }
        public int DBH_end { get; set; }

        public string DBH_range { get
            {
                return $"{DBH_start} - {DBH_end}";
            } 
        }
        public int Tally_Softwood { get; set; }
        public int Tally_Hardwood { get; set; }
        public int Tally_Unknown { get; set; }
        public int Tally_Cavity { get; set; }
    }
}
