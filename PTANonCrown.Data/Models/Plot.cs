using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations.Schema;

namespace PTANonCrown.Data.Models
{
    public class Plot : BaseModel
    {
        public bool _isPlanted;
        private int _ageTreeAge;
        private int _ageTreeDBH;
        private PlantedType _plantedType;
        private int _plotNumber;

        private ICollection<PlotTreatment> _plotTreatments;

        public ObservableCollection<CoarseWoody> _plotCoarseWoody;

        private ObservableCollection<TreeDead> _plotTreeDead;
        private ObservableCollection<TreeLive> _plotTreeLive = new ObservableCollection<TreeLive>();

        private EcodistrictLookup _ecodistrictLookup;
        private SoilLookup _soil;
        private CardinalDirections _transectDirection;
        private decimal _transectLength;
        private int _treeCount;

        private UnderstoryDominated _understoryDominated;

        private VegLookup _vegetation;
        public float? Easting { get; set; }
        public float? Northing { get; set; }
        public Plot()
        {
            Blowdown = 0;
            UnderstoryStrata = 0;
            StockingLITSeedTree = 0;
            StockingRegenCommercialSpecies = 0;
            StockingRegenLITSpecies = 0;
            TransectLength = 20; //default
            InitializeDeadTreeDefaults();
            InitializeCoarseWoodyDefaults();
        }

        public int AgeTreeAge
        {
            get => _ageTreeAge;
            set
            {
                if (_ageTreeAge != value)
                {
                    _ageTreeAge = value;
                    OnPropertyChanged();
                }
            }
        }

        public int AgeTreeDBH
        {
            get => _ageTreeDBH;
            set
            {
                if (_ageTreeDBH != value)
                {
                    _ageTreeDBH = value;
                    OnPropertyChanged();
                }
            }
        }

        public int AverageSampleTreeAge { get; set; }

        public int AverageSampleTreeDBH_cm { get; set; }

        public int AverageSampleTreeSpecies { get; set; }

        public int Blowdown { get; set; }

        
       public EcodistrictLookup EcodistrictLookup
        {
            get => _ecodistrictLookup;
            set
            {
                if (_ecodistrictLookup != value)
                {
                    _ecodistrictLookup = value;
                    OnPropertyChanged();
                }
            }
        }

        private ExposureLookup _exposure;
        public ExposureLookup Exposure
        {
            get => _exposure;
            set
            {
                if (_exposure != value)
                {
                    _exposure = value;
                    OnPropertyChanged();
                }
            }
        }
        public int HorizontalStructure { get; set; }

        public int ID { get; set; }

        public bool IsPlanted
        {
            get => _isPlanted;
            set
            {
                if (_isPlanted != value)
                {
                    _isPlanted = value;
                    OnPropertyChanged();
                    OnIsPlantedChanged();
                }
            }
        }

        public int OGFSampleTreeAge { get; set; }

        public int OGFSampleTreeDBH_cm { get; set; }

        public int OGFSampleTreeSpecies { get; set; }

        public bool OneCohortSenescent { get; set; }

        public PlantedType PlantedType
        {
            get => _plantedType;
            set
            {
                if (_plantedType != value)
                {
                    _plantedType = value;
                    OnPropertyChanged();
                }
            }
        }

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

        public int PlotNumber
        {
            get => _plotNumber;
            set
            {
                if (_plotNumber != value)
                {
                    _plotNumber = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICollection<PlotTreatment> PlotTreatments
        {
            get => _plotTreatments;
            set
            {
                if (_plotTreatments != value)
                {
                    _plotTreatments = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(PlotTreatmentsDisplayString));
                }
            }
        }

        public string PlotTreatmentsDisplayString
        {
            get
            {
                return string.Join(", ", PlotTreatments
                        .Where(pt => (pt.IsActive == true) & (pt.Treatment != null))
                        .Select(pt => pt.Treatment.Name));
            }

        }

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

                    OnPropertyChanged();

                }
            }
        }

        public bool RegenHeightHWLIT { get; set; }

        public bool RegenHeightSWLIT { get; set; }

        public SoilLookup Soil
        {
            get => _soil;
            set
            {
                if (_soil != value)
                {
                    _soil = value;
                    OnPropertyChanged();
                }
            }
        }

        public Stand Stand { get; set; }

        public int StandID { get; set; }

        public int StockingBeechRegeneration { get; set; }

        public int StockingLITSeedTree { get; set; }

        public int StockingRegenCommercialSpecies { get; set; }

        public int StockingRegenLITSpecies { get; set; }

        public CardinalDirections TransectDirection
        {
            get => _transectDirection;
            set
            {
                if (_transectDirection != value)
                {
                    _transectDirection = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal TransectLength
        {
            get => _transectLength;
            set
            {
                if (_transectLength != value)
                {
                    _transectLength = value;
                    OnPropertyChanged();
                }
            }
        }

        [NotMapped]
        public int TreeCount
        {
            get => _treeCount;
            set
            {
                SetProperty(ref _treeCount, value);
            }
        }

        public UnderstoryDominated UnderstoryDominated
        {
            get => _understoryDominated;
            set
            {
                if (_understoryDominated != value)
                {
                    _understoryDominated = value;
                    OnPropertyChanged();
                }
            }
        }

        public int UnderstoryStrata { get; set; }

        public bool UnevenAged { get; set; }

        public VegLookup Vegetation
        {
            get => _vegetation;
            set
            {
                if (_vegetation != value)
                {
                    _vegetation = value;
                    OnPropertyChanged();
                }
            }
        }

        private void InitializeCoarseWoodyDefaults()
        {            // todo test; will this load existing values?

            if (PlotCoarseWoody is null)
            {
                PlotCoarseWoody = new ObservableCollection<CoarseWoody>();
                PlotCoarseWoody.Add(new CoarseWoody() { Plot = this, DBH_start = 21, DBH_end = 30 });
                PlotCoarseWoody.Add(new CoarseWoody() { Plot = this, DBH_start = 31, DBH_end = 40 });
                PlotCoarseWoody.Add(new CoarseWoody() { Plot = this, DBH_start = 41, DBH_end = 50 });
                PlotCoarseWoody.Add(new CoarseWoody() { Plot = this, DBH_start = 51, DBH_end = 60 });
                PlotCoarseWoody.Add(new CoarseWoody() { Plot = this, DBH_start = 60, DBH_end = -1 });
            }
        }

        private void InitializeDeadTreeDefaults()
        {
            // todo test; will this load existing values?

            if (PlotTreeDead is null)
            {
                PlotTreeDead = new ObservableCollection<TreeDead>();
                PlotTreeDead.Add(new TreeDead() { Plot = this, DBH_start = 21, DBH_end = 30 });
                PlotTreeDead.Add(new TreeDead() { Plot = this, DBH_start = 31, DBH_end = 40 });
                PlotTreeDead.Add(new TreeDead() { Plot = this, DBH_start = 41, DBH_end = 50 });
                PlotTreeDead.Add(new TreeDead() { Plot = this, DBH_start = 51, DBH_end = 60 });
                PlotTreeDead.Add(new TreeDead() { Plot = this, DBH_start = 60, DBH_end = -1 });
            }

        }

        private void InitializeLiveTree()
        {
            PlotTreeLive = new ObservableCollection<TreeLive>();

            //PlotTreeLive.Add(new TreeLive() { PlotID = ID });

        }

        private void OnIsPlantedChanged()
        {
            if (!IsPlanted)
            {
                PlantedType = PlantedType.None;
            }
        }

        private void OnTreeLiveCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // Update TreeCount when items are added or removed from the collection
            TreeCount = _plotTreeLive?.Count ?? 0;
        }
    }
}