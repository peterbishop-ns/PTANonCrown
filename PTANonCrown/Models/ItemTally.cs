namespace PTANonCrown.Models
{
    public class ItemTally : BaseModel
    {
        private int _tally_Cavity;
        private int _tally_Hardwood;
        private int _tally_Softwood;
        private int _tally_Unknown;
        public int DBH_end { get; set; }

        public string DBH_range
        {
            get
            {
                return $"{DBH_start} - {DBH_end}";
            }
        }

        public int DBH_start { get; set; }
        public int ID { get; set; }
        public Plot Plot { get; set; }
        public int PlotID { get; set; }

        public int Tally_Cavity
        {
            get => _tally_Cavity;
            set => SetProperty(ref _tally_Cavity, value);

        }

        public int Tally_Hardwood
        {
            get => _tally_Hardwood;
            set => SetProperty(ref _tally_Hardwood, value);

        }

        public int Tally_Softwood
        {
            get => _tally_Softwood;
            set => SetProperty(ref _tally_Softwood, value);

        }

        public int Tally_Unknown
        {
            get => _tally_Unknown;
            set => SetProperty(ref _tally_Unknown, value);

        }
    }
}