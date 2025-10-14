namespace PTANonCrown.Data.Models
{
    public class SummaryItem
    {

        public string DisplayName { get; set; }
        public string DisplayValue => $"{Value} {Units}";
        public string Units { get; set; }
        public object Value { get; set; }
    }

    public class SummaryResultTreeSpecies
    {
        public int Count { get; set; }
        public int PlotNumber { get; set; }
        public string Species { get; set; }
        public double Percentage { get; set; }
    }

    public class SummaryTreatmentResult
    {
        public int PlotNumber { get; set; }
        public string Treatments { get; set; }

    }

    public class SummarySoilResult
    {
        public string SoilCode { get; set; }
        public int Count { get; set; }

        public double Percentage { get; set; }
    }
    public class SummaryVegetationResult
    {
        public string VegCode { get; set; }
        public int Count { get; set; }

        public double Percentage { get; set; }
    }

}