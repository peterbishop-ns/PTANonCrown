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

    public enum EcositeGroup
    {
        None = 0,
        Acadian = 1,
        MaritimeBoreal = 2
    }

    public enum CardinalDirections
    {
        N = 0,
        NE = 1,
        E = 2, 
        SE = 3,
        S = 4, 
        SW = 5, 
        W = 6, 
        NW = 7
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
