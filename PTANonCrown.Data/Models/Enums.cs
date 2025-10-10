using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace PTANonCrown.Data.Models
{

    // allows us to use a "Description" tag on the Enum. 
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = field.GetCustomAttribute<DescriptionAttribute>();
            return attribute != null ? attribute.Description : value.ToString();
        }
    }
    
    
    public enum PlantedType
    {
        None = 0,
        Acadian = 1,
        MaritimeBoreal = 2,
        Coastal = 3
    }

    public enum PlantedMethod
    {
        NotPlanted = 0,
        InSitu = 1,
        ExSitu = 2
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
