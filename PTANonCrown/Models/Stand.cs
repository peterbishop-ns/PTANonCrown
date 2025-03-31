using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTANonCrown.Models
{
    public class Stand
    {
        public int StandID { get; set; }
        public int CruiseID { get; set; }
        public int PlannerID { get; set; }
        public string Organization { get; set; }
        public float Easting { get; set; }
        public float Northing { get; set; }
        public int Ecodistrict { get; set; }
        public string Location { get; set; }

    }
}
