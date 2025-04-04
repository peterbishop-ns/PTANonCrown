using System.Collections.ObjectModel;

namespace PTANonCrown.Models
{
    public class Plot : BaseModel
    {
        private int _plotNumber;

        public Plot()
        {
            InitializeLiveTree();
            InitializeDeadTreeDefaults();
            InitializeCoarseWoodyDefaults();
        }

        public int AverageSampleTreeAge { get; set; }
        public int AverageSampleTreeDBH_cm { get; set; }
        public int AverageSampleTreeSpecies { get; set; }
        public int Blowdown { get; set; }

        public ObservableCollection<CoarseWoody> PlotCoarseWoody { get; set; }

        public int HorizontalStructure { get; set; }
        public int ID { get; set; }
        public int OGFSampleTreeAge { get; set; }

        public int OGFSampleTreeDBH_cm { get; set; }

        public int OGFSampleTreeSpecies { get; set; }

        public bool OneCohortSenescent { get; set; }

        public int PlotNumber
        {
            get => _plotNumber;
            set => SetProperty(ref _plotNumber, value);

        }

        public bool RegenHeightHWLIT { get; set; }
        public bool RegenHeightSWLIT { get; set; }
        public Stand Stand { get; set; }
        public int StandID { get; set; }

        public int StockingBeechRegeneration { get; set; }
        public int StockingLITSeedTree { get; set; }
        public int StockingRegenCommercialSpecies { get; set; }
        public int StockingRegenLITSpecies { get; set; }

        public ObservableCollection<TreeDead> PlotTreeDead { get; set; }

        public ObservableCollection<TreeLive> PlotTreeLive { get; set; }

        private void InitializeLiveTree()
        {
            PlotTreeLive = new ObservableCollection<TreeLive>();
            PlotTreeLive.Add(new TreeLive() { PlotID = ID });


        }

        private void InitializeDeadTreeDefaults()
        {
            PlotTreeDead = new ObservableCollection<TreeDead>();
            PlotTreeDead.Add(new TreeDead() { PlotID = ID, DBH_start = 21, DBH_end = 30 });
            PlotTreeDead.Add(new TreeDead() { PlotID = ID, DBH_start = 31, DBH_end = 40 });
            PlotTreeDead.Add(new TreeDead() { PlotID = ID, DBH_start = 41, DBH_end = 50 });
            PlotTreeDead.Add(new TreeDead() { PlotID = ID, DBH_start = 51, DBH_end = 60 });
            PlotTreeDead.Add(new TreeDead() { PlotID = ID, DBH_start = 60, DBH_end = 1000 });
        }
        private void InitializeCoarseWoodyDefaults()
        {            // todo this should first check DB; if none exist, THEN initialize

            PlotCoarseWoody = new ObservableCollection<CoarseWoody>();
            PlotCoarseWoody.Add(new CoarseWoody() { PlotID = ID, DBH_start = 21, DBH_end = 30 });
            PlotCoarseWoody.Add(new CoarseWoody() { PlotID = ID, DBH_start = 31, DBH_end = 40 });
            PlotCoarseWoody.Add(new CoarseWoody() { PlotID = ID, DBH_start = 41, DBH_end = 50 });
            PlotCoarseWoody.Add(new CoarseWoody() { PlotID = ID, DBH_start = 51, DBH_end = 60 });
            PlotCoarseWoody.Add(new CoarseWoody() { PlotID = ID, DBH_start = 60, DBH_end = 1000 });
        }
        public int UnderstoryDominated { get; set; }
        public int UnderstoryStrata { get; set; }
        public bool UnevenAged { get; set; }

    }
}