using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace PTANonCrown.Data.Models
{
    public class BaseLookup : BaseModel
    {
      //  public virtual string DisplayName => $"{ShortCode} - {Name}";
      //  public int ID { get; set; }
        
        
        private string? _name;

        public string? Name {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }
        private string? _shortCode { get; set; }
        public string? ShortCode
        {
            get => _shortCode;
            set
            {
                if (_shortCode != value)
                {
                    _shortCode = value;
                    OnPropertyChanged();
                }
            }
        }
        public override string ToString()
        {
            return ShortCode ?? "n/a";
        }

    }

    public class Soil : BaseLookup
    {
        private static readonly Dictionary<string, string> SoilPhaseLookup = new()
            {
                {"B", "Boulder"},
                {"S", "Stony"},
                {"SB", "Stony-Boulder"},
                {"C", "Coarse"},
                {"CB", "Coarse-Boulder"},
                {"CS", "Coarse-Stony"},
                {"CSB", "Coarse-Stony-Boulder"},
                {"L", "Loamy"},
                {"LB", "Loamy-Boulder"},
                {"LS", "Loamy-Stony"},
                {"LSB", "Loamy-Stony-Boulder"},
                {"U", "Upland"},
                {"UB", "Upland-Boulder"},
                {"US", "Upland-Stony"},
                {"USB", "Upland-Stony-Boulder"}
            };

        private string? _soilPhaseLong;

        private int _soilType;
        public int SoilType
        {
            get => _soilType;
            set
            {
                if (_soilType != value)
                {
                    _soilType = value;
                    OnPropertyChanged();
                }
            }
        }
        private string? _soilPhaseShort;
        public string? SoilPhaseShort
        {
            get => _soilPhaseShort;
            set
            {
                if (_soilPhaseShort != value)
                {
                    _soilPhaseShort = value;
                    OnPropertyChanged();
                }
            }
        }



        [NotMapped]
        public string? SoilPhaseLong =>
            _soilPhaseShort != null && SoilPhaseLookup.TryGetValue(_soilPhaseShort, out var longVal)
                ? longVal
                : null;

        public override string ToString()
        {
            if (ID == -1)
            {
                return "unknown";
            }
            if (SoilPhaseShort is null || SoilPhaseShort == string.Empty)
            {
                return $"ST{SoilType}";

            }
            return $"ST{SoilType}-{SoilPhaseShort} ({SoilPhaseLong})";
        }


    }
    public class Exposure : BaseLookup
    {
        public int ID { get; set; }
    }

    public class TreeSpecies : BaseLookup
    {
        public int HardwoodSoftwood { get; set; }
        private bool _lit;
        public bool LIT
        {
            get => _lit;
            set
            {
                if (_lit != value)
                {
                    _lit = value;
                    OnPropertyChanged();
                }
            }
        }


        private string? _name;

        public string? Name
        {
            get => _name ?? string.Empty;  // for pick lists
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }


        public bool LT { get; set; }
        public int CustomOrder { get; set; }


        public override string ToString()
        {

            if (ID == -1)
            {
                return string.Empty;
            }

            return $"{ShortCode} - {Name}";
        }
    }


    public class Vegetation : BaseLookup
    {
        public override string ToString()
        {

            if (ID == -1)
            {
                return "unknown";
            }

            return ShortCode;
        }
    }

    public class Ecosite : BaseLookup
    {

        
    }

    // Junction table
    public class EcositeSoilVeg
    {

        public string SoilCode { get; set; } = null!;
        public string VegCode { get; set; } = null!;

        public string EcositeCode { get; set; } = null!;

        public string EcositeGroup { get; set; } = null!;

    }
}