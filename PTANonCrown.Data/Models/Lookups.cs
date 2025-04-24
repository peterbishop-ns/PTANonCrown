namespace PTANonCrown.Data.Models
{
    public class BaseLookup : BaseModel
    {
        public virtual string DisplayName => $"{ShortCode} - {Name}";
        public int ID { get; set; }
        public string Name { get; set; }
        public string? ShortCode { get; set; }
    }

    public class SoilLookup : BaseLookup
    { }

    public class TreatmentLookup : BaseLookup
    {
        // public bool IsActive { get; set; }
    }

    public class TreeLookup : BaseLookup
    { 
    public int HardwoodSoftwood { get; set; }
    public bool LIT { get; set; }
        public bool LIT_planted { get; set; }
        public bool LT { get; set; }

    }

    public class VegLookup : BaseLookup
    { }
}