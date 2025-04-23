namespace PTANonCrown.Data.Models
{
    public class SummaryItem
    {

        public string DisplayName { get; set; }
        public string DisplayValue => $"{Value} {Units}";
        public string Units { get; set; }
        public object Value { get; set; }
    }
}