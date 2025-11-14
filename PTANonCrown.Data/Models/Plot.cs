using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

namespace PTANonCrown.Data.Models
{
    public class Plot : BaseModel
    {
        public bool _isPlanted;
        public ObservableCollection<CoarseWoody> _plotCoarseWoody;
        private int _ageTreeAge;
        private int _ageTreeDBH;

        private int _ageTreeSpeciesID;
        private Ecosite _ecositeLookup;
        private EcositeGroup _ecositeGroup;
       // private ForestGroup _forestGroup;
        private int _oldGrowthAge;
        private int _oldGrowthDBH;

        private int _oldGrowthSpeciesID;
        private bool _oneCohortSenescent;
        private PlantedType _plantedType;
        private int _plotNumber;

        private ObservableCollection<PlotTreatment> _plotTreatments;
        private ObservableCollection<TreeDead> _plotTreeDead;
        private ObservableCollection<TreeLive> _plotTreeLive = new ObservableCollection<TreeLive>();
        private CardinalDirections _transectDirection;
        private decimal _transectLength;
        private int _treeCount;

        private UnderstoryDominated _understoryDominated;

        public Plot()
        {
            Blowdown = 0;
            UnderstoryStrata = 0;
            StockingLITSeedTree = 0;
            StockingRegenCommercialSpecies = 0;
            StockingRegenLITSpecies = 0;
            TransectLength = 20; //default

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

        public int AgeTreeSpeciesID
        {
            get => _ageTreeSpeciesID;
            set
            {
                if (_ageTreeSpeciesID != value)
                {
                    _ageTreeSpeciesID = value;
                    OnPropertyChanged();
                }
            }
        }

        public int AGSPatches
        {
            get => _agsPatches;
            set
            {
                if (_agsPatches != value)
                {
                    _agsPatches = value;
                    OnPropertyChanged();
                }
            }
        }

        public int AverageSampleTreeAge { get; set; }
        public int AverageSampleTreeDBH_cm { get; set; }
        public int AverageSampleTreeSpecies { get; set; }
        public int Blowdown { get; set; }
        public int? Easting { get; set; }

     

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



        public bool HasOldGrowth
        {
            get => _hasOldGrowth;
            set
            {
                if (_hasOldGrowth != value)
                {
                    _hasOldGrowth = value;
                    OnPropertyChanged();
                    OnHasOldGrowthChanged(value);
                }
            }
        }

        public int HorizontalStructure { get; set; }

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

        // Keeping LookupTrees list on the Plot itself is a workaround.
        // Was running into issues with the Picker list, where it wouldn't set SelectedItem Correctly
        //  There were different binding contexts; LookupTrees was on the VM, whereas the selected tree species
        // Was on the row of the picker.
        // Having LookupTrees as a prop of the Plot solved this issue
        // This is used for the AgeTree speceis.
        [NotMapped]
        public List<TreeSpecies> LookupTrees { get; set; }

        public int? Northing { get; set; }
        public int OGFSampleTreeAge { get; set; }

        public int OGFSampleTreeDBH_cm { get; set; }

        public int OGFSampleTreeSpecies { get; set; }

        public int OldGrowthAge
        {
            get => _oldGrowthAge;
            set
            {
                if (_oldGrowthAge != value)
                {
                    _oldGrowthAge = value;
                    OnPropertyChanged();
                }
            }
        }

        public int OldGrowthDBH
        {
            get => _oldGrowthDBH;
            set
            {
                if (_oldGrowthDBH != value)
                {
                    _oldGrowthDBH = value;
                    OnPropertyChanged();
                }
            }
        }

        public int OldGrowthSpeciesID
        {
            get => _oldGrowthSpeciesID;
            set
            {
                if (_oldGrowthSpeciesID != value)
                {
                    _oldGrowthSpeciesID = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool OneCohortSenescent
        {
            get => _oneCohortSenescent;
            set
            {
                if (_oneCohortSenescent != value)
                {
                    _oneCohortSenescent = value;
                    OnPropertyChanged();
                }
            }
        }

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

        public ObservableCollection<PlotTreatment> PlotTreatments
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

        public Stand Stand { get; set; }

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

        public bool UnevenAged
        {
            get => _unevenAged;
            set
            {
                if (_unevenAged != value)
                {
                    _unevenAged = value;
                    OnPropertyChanged();
                    OnUnevenAgedChanged(value);
                }
            }
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// Lookup Tables
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////


        public string? SoilCode { get; set; }

        [Required]
        private Soil? _soil = null!;

        public Soil? Soil
        {
            get => _soil;
            set
            {
                _soil = value;
                OnPropertyChanged();
                OnSoilChanged(value);
            }
        }

        private void OnSoilChanged(Soil soil)
        {
            SoilCode = soil?.ShortCode ?? string.Empty;
        }


        public string? VegCode { get; set; }



        [Required]
        private Vegetation? _vegetation = null!;
        public Vegetation? Vegetation
        {
            get => _vegetation;
            set
            {
                _vegetation = value;
                OnPropertyChanged();
                OnVegChanged();
            }
        }

        public string? EcositeCode { get; set; }


        private Ecosite? _ecosite = null!;
        public Ecosite? Ecosite
        {
            get => _ecosite;
            set
            {
                _ecosite = value;
                OnPropertyChanged();

            }
        }



        private int _ecodistrict;
        public int Ecodistrict
        {
            get => _ecodistrict;
            set
            {
                _ecodistrict = value;
                OnPropertyChanged();

            }
        }




        public string? Exposure { get; set; } = null!;


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////



        private int _agsPatches { get; set; }

        private bool _hasOldGrowth { get; set; }

        private bool _unevenAged { get; set; }

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

        public override string ToString()
        {
            return $"Plot {PlotNumber}"; // or include more: $"Plot {PlotNumber} - {Location}"
        }




        public string ForestGroup
        {
            get => GetForestGroup(Vegetation?.ShortCode);
            
        }


        


        public void UpdatePlotTreeLIT()
        {
            if (PlotTreeLive?.Count() == 0)
            {
                return;
            }

            foreach (var tree in PlotTreeLive)
            {
                tree.UpdateTreeLIT();
            }
        
        }

        private string GetForestGroup(string? vegType)
        {
            var pattern = new Regex(@"^([A-Z]+)"); // capture letters at the start

            if (vegType is null)
            {
                return "n/a";
            }

            var match = pattern.Match(vegType);
            if (!match.Success)
                return vegType; // fallback if regex doesn't match

            string forestGroup = match.Groups[1].Value;  // "MW"

            return forestGroup;

        }

        private void InitializeLiveTree()
        {
            PlotTreeLive = new ObservableCollection<TreeLive>();

            //PlotTreeLive.Add(new TreeLive() { PlotID = ID });

        }

        private void OnHasOldGrowthChanged(bool hasOldGrowth)
        {
            if (!hasOldGrowth)
            { //reset things
                OldGrowthAge = 0;
                OldGrowthDBH = 0;
                OldGrowthSpeciesID = 1;
            }

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

        private void OnTreeLiveCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // Update TreeCount when items are added or removed from the collection
            TreeCount = _plotTreeLive?.Count ?? 0;
        }

        private void OnUnevenAgedChanged(bool unevenAged)
        {
            if (!unevenAged)
            {
                // clear the child props
                AGSPatches = 0;
                OneCohortSenescent = false;
            }
        }

        private void OnVegChanged()
        {
            OnPropertyChanged(nameof(ForestGroup));
            // whenever the Veg is changed, need to refresh the LIT status of all the trees
            this.UpdatePlotTreeLIT();

        }
    }
}