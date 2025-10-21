using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace PTANonCrown.Data.Models
{
    public class BaseLookup : BaseModel
    {
      //  public virtual string DisplayName => $"{ShortCode} - {Name}";
      //  public int ID { get; set; }
        
        
        private string _name;

        public string Name {
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
            return Name;
        }

    }

    public class Soil : BaseLookup
    {
    
    }
    public class Exposure : BaseLookup
    {
        public int ID { get; set; }
    }

    public class Vegetation : BaseLookup
    {

    }

    public class Ecodistrict : BaseLookup
    {

        /*private string _ecositeGroup { get; set; }
        
        public string EcositeGroup
        {
            get => _ecositeGroup;
            set
            {
                if (_ecositeGroup != value)
                {
                    _ecositeGroup = value;
                    OnPropertyChanged();
                }
            }
        }*/
    }

    // Junction table
    public class EcodistrictSoilVeg
    {

        public string SoilCode { get; set; } = null!;
       // public virtual Soil Soil { get; set; } = null!;

        public string VegCode { get; set; } = null!;
      //  public virtual Vegetation Veg { get; set; } = null!;

        public string EcodistrictCode { get; set; } = null!;
       // public virtual Ecodistrict Ecodistrict { get; set; } = null!;

        // Optional: a friendly composite name
        //public string DisplayName => $"{SoilCode} + {VegCode} → {EcodistrictCode}";
    }





}