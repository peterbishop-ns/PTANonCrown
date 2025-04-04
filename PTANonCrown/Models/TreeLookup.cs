namespace PTANonCrown.Models
{
    public class TreeLookup : BaseModel
    {
        public virtual string DisplayName => $"{ShortCode} - {Name}";
        public int ID { get; set; }
        public string Name { get; set; }
        public string ShortCode { get; set; }
    }
}