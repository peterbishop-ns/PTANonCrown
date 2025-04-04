using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations.Schema;

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

        public ObservableCollection<CoarseWoody> _plotCoarseWoody;

        public ObservableCollection<CoarseWoody> PlotCoarseWoody
        {
            get => _plotCoarseWoody;
            set
            {
                if (_plotCoarseWoody != value)
                {
                    _plotCoarseWoody = value;
                    OnPropertyChanged();
                }
            }
        }


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




        private ObservableCollection<TreeDead> _plotTreeDead = new ObservableCollection<TreeDead>();

        public virtual ObservableCollection<TreeDead> PlotTreeDead
        {
            get => _plotTreeDead;
            set
            {
                if (_plotTreeDead != value)
                {

                    _plotTreeDead = value;
                    OnPropertyChanged();

                }
            }
        }



        private ObservableCollection<TreeLive> _plotTreeLive = new ObservableCollection<TreeLive>();

        public virtual ObservableCollection<TreeLive> PlotTreeLive
        {
            get => _plotTreeLive;
            set
            {
                if (_plotTreeLive != value)
                {
                    if (_plotTreeLive != null)
                        _plotTreeLive.CollectionChanged -= OnTreeLiveCollectionChanged;

                    _plotTreeLive = value;

                    if (_plotTreeLive != null)
                        _plotTreeLive.CollectionChanged += OnTreeLiveCollectionChanged;

                    OnPropertyChanged(nameof(PlotTreeLive));

                }
            }
        }

        private void OnTreeLiveCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // Update TreeCount when items are added or removed from the collection
            TreeCount = _plotTreeLive?.Count ?? 0;
        }

        private void InitializeLiveTree()
        {
            PlotTreeLive = new ObservableCollection<TreeLive>();
            PlotTreeLive.Add(new TreeLive() { PlotID = ID });


        }

        private int _treeCount;

        [NotMapped]
        public int TreeCount
        {
            get => _treeCount;
            set {
                SetProperty(ref _treeCount, value);
            } 
        }
        private void InitializeDeadTreeDefaults()
        {
            
            if (PlotTreeDead is null)
            {
                PlotTreeDead = new ObservableCollection<TreeDead>();
                PlotTreeDead.Add(new TreeDead() { PlotID = ID, DBH_start = 21, DBH_end = 30 });
                PlotTreeDead.Add(new TreeDead() { PlotID = ID, DBH_start = 31, DBH_end = 40 });
                PlotTreeDead.Add(new TreeDead() { PlotID = ID, DBH_start = 41, DBH_end = 50 });
                PlotTreeDead.Add(new TreeDead() { PlotID = ID, DBH_start = 51, DBH_end = 60 });
                PlotTreeDead.Add(new TreeDead() { PlotID = ID, DBH_start = 60, DBH_end = 1000 });
            }
         
        }
        private void InitializeCoarseWoodyDefaults()
        {            // todo this should first check DB; if none exist, THEN initialize

            
            if (PlotCoarseWoody is null)
            {
                PlotCoarseWoody = new ObservableCollection<CoarseWoody>();
                PlotCoarseWoody.Add(new CoarseWoody() { PlotID = ID, DBH_start = 21, DBH_end = 30 });
                PlotCoarseWoody.Add(new CoarseWoody() { PlotID = ID, DBH_start = 31, DBH_end = 40 });
                PlotCoarseWoody.Add(new CoarseWoody() { PlotID = ID, DBH_start = 41, DBH_end = 50 });
                PlotCoarseWoody.Add(new CoarseWoody() { PlotID = ID, DBH_start = 51, DBH_end = 60 });
                PlotCoarseWoody.Add(new CoarseWoody() { PlotID = ID, DBH_start = 60, DBH_end = 1000 });

            }
            
        }
        public int UnderstoryDominated { get; set; }
        public int UnderstoryStrata { get; set; }
        public bool UnevenAged { get; set; }

    }
}