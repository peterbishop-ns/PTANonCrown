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
        public SoilLookup Soil { get; set; }
        public int Count { get; set; }

        public double Percentage { get; set; }
    }
    public class SummaryVegetationResult
    {
        public VegLookup Vegetation { get; set; }
        public int Count { get; set; }

        public double Percentage { get; set; }
    }

}