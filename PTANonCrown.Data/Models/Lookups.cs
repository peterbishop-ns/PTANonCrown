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

    public class VegLookup : BaseLookup
    { }






}