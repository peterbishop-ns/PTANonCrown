using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations.Schema;
using static System.Net.Mime.MediaTypeNames;

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
        public int? Easting { get; set; }
        public int? Northing { get; set; }
        public Plot()
        {
            Blowdown = 0;
            UnderstoryStrata = 0;
            StockingLITSeedTree = 0;
            StockingRegenCommercialSpecies = 0;
            StockingRegenLITSpecies = 0;
            TransectLength = 20; //default

        }
        public override string ToString()
        {
            return $"Plot {PlotNumber}"; // or include more: $"Plot {PlotNumber} - {Location}"
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
                    OnPlantedTypeChanged();
                }
            }
        }

        private EcositeGroup _ecositeGroup;
        public EcositeGroup EcositeGroup
        {
            get => _ecositeGroup;
            set
            {
                if (_ecositeGroup != value)
                {
                    _ecositeGroup = value;
                    OnPropertyChanged();
                }
            }
        }



        private TreeSpecies _ageTreeSpecies;

        public TreeSpecies AgeTreeSpecies
        {
            get => _ageTreeSpecies;
            set
            {
                if (_ageTreeSpecies != value)
                {
                    _ageTreeSpecies = value;
                    OnPropertyChanged();
                }
            }
        }


        
        public void OnPlantedTypeChanged()
        {
            // Keep the two Groups in sync
            if (PlantedType == PlantedType.Acadian)
            {
                EcositeGroup = EcositeGroup.Acadian;
            }

            else if (PlantedType == PlantedType.Coastal ||
                PlantedType == PlantedType.MaritimeBoreal)
            {
                EcositeGroup = EcositeGroup.MaritimeBoreal;
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


       

        private void InitializeLiveTree()
        {
            PlotTreeLive = new ObservableCollection<TreeLive>();

            //PlotTreeLive.Add(new TreeLive() { PlotID = ID });

        }

        private async void OnIsPlantedChanged()
        {
            if (!IsPlanted)
            {
                PlantedType = PlantedType.None;



                    // Reset the planted method for all trees to none
                    foreach (TreeLive tree in PlotTreeLive)
                    {
                        tree.PlantedMethod = PlantedMethod.NotPlanted;

                    }



            }

        }


        // Keeping LookupTrees list on the Plot itself is a workaround. 
        // Was running into issues with the Picker list, where it wouldn't set SelectedItem Correctly
        //  There were different binding contexts; LookupTrees was on the VM, whereas the selected tree species
        // Was on the row of the picker. 
        // Having LookupTrees as a prop of the Plot solved this issue
        // This is used for the AgeTree speceis. 
        [NotMapped]
        public List<TreeSpecies> LookupTrees { get; set; }


        private void OnTreeLiveCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // Update TreeCount when items are added or removed from the collection
            TreeCount = _plotTreeLive?.Count ?? 0;
        }
    }
}