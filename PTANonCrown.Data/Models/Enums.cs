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


    public enum ForestGroup
    {
        [Description("Coastal Acadian")]
        CoastalAcadian = 0,

        [Description("Flood Plain Forest")]
        FloodPlainForest = 1,

        [Description("Intolerant Hardwood")]
        IntolerantHardwood = 2,

        [Description("Karst Forest")]
        KarstForest = 3,

        [Description("Mixedwood")]
        Mixedwood = 4,

        [Description("Old Field")]
        OldField = 5,

        [Description("Open Woodlands")]
        OpenWoodlands = 6,

        [Description("Spruce Hemlock")]
        SpruceHemlock = 7,

        [Description("Spruce Pine")]
        SprucePine = 8,

        [Description("Tolerant Hardwood")]
        TolerantHardwood = 9,

        [Description("Wet Coniferous")]
        WetConiferous = 10,

        [Description("Wet Deciduous")]
        WetDeciduous = 11,

        [Description("Wet Mixed Wood")]
        WetMixedWood = 12,

        [Description("Coastal Boreal")]
        CoastalBoreal = 13,

        [Description("Highland")]
        Highland = 14,

        [Description("Wet Boreal")]
        WetBoreal = 15,

        [Description("Planted Forest")]
        PlantedForest = 16
    }
}
