using System.ComponentModel.DataAnnotations.Schema;

namespace PTANonCrown.Data.Models
{
    public class BaseLookup : BaseModel
    {
        public virtual string DisplayName => $"{ShortCode} - {Name}";
        public int ID { get; set; }
        
        
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
    }

    public class SoilLookup : BaseLookup
    { }
    public class ExposureLookup : BaseLookup
    { }    
    
    

    public class VegLookup : BaseLookup
    {
        private ForestGroup _forestGroup { get; set; }

        public ForestGroup ForestGroup
        {
            get => _forestGroup;
            set
            {
                if (_forestGroup != value)
                {
                    _forestGroup = value;
                    OnPropertyChanged();
                }
            }
        }
    }

    public class EcodistrictLookup : BaseLookup
    {
        [NotMapped]
        public string DisplayName { get => $"{ShortCode} - {Name}"; }
        private string _ecositeGroup { get; set; }
        
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
        }
    }




}