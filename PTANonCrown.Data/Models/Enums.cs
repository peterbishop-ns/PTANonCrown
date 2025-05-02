using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTANonCrown.Data.Models
{
    public enum PlantedType
    {
        None = 0,
        Acadian = 1,
        MaritimeBoreal = 2,
        Coastal = 3
    }

    public enum UnderstoryDominated
    {
        None=0, 
        Trees=1,
        WoodyShrubs = 2
    }

    public enum HardwoodSoftwood
    {
        Unknown = 0, 
        Hardwood = 1, 
        Softwood = 2
    }
}
