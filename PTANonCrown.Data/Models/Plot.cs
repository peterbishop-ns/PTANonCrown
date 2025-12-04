using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PTANonCrown.Data.Validation;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;
using PTANonCrown.Data.Validation;
namespace PTANonCrown.Data.Models
{
    public partial class Plot : BaseModel
    {
        public ObservableCollection<CoarseWoody> _plotCoarseWoody;
        private int _ageTreeAge;
        private int _ageTreeDBH;
        private string? _comment;

        private int _ageTreeSpeciesID;
        private Ecosite _ecositeLookup;
        private EcositeGroup _ecositeGroup;
        private int _oldGrowthAge;
        private int _oldGrowthDBH;

        private int _oldGrowthSpeciesID;
        private bool _oneCohortSenescent;
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
            AgeTreeAge = 0;
            AgeTreeDBH = 0;
            Blowdown = 0;
            UnderstoryStrata = 0;
            StockingLITSeedTree = 0;
            StockingRegenCommercialSpecies = 0;
            StockingRegenLITSpecies = 0;
            TransectLength = 20; //default
            PlotTreeLive.CollectionChanged += OnTreeLiveCollectionChanged;
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


        private bool _isCheckedBiodiversity;
        public bool IsCheckedBiodiversity
        {
            get => _isCheckedBiodiversity;
            set
            {
                if (_isCheckedBiodiversity != value)
                {
                    _isCheckedBiodiversity = value;
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

        public string? Comment
        {
            get => _comment;
            set => SetProperty(ref _comment, value, false);

        }


        [CustomValidation(typeof(Plot), nameof(ValidateEasting))]
        public int Easting
        {
            get => _easting;
            set => SetProperty(ref _easting, value, value != 0);  // only validate if it has a value
        }
        private int _easting;


        public EcositeGroup EcositeGroup
        {
            get => _ecositeGroup;
            set => SetProperty(ref _ecositeGroup, value, false);

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

        public HorizontalStructure HorizontalStructure { get; set; }


        [CustomValidation(typeof(Plot), nameof(ConditionalRequired))]
        [ObservableProperty]
        private bool _isPlanted;

        partial void OnIsPlantedChanged(bool value)
        {
            ValidateProperty(value, nameof(IsPlanted));
            
            if (!IsPlanted)
            {
                PlantedType = PlantedTypeEnum.None;

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



        // custom validaiton required because ints are non-nullable, so defaults to 0, but then 
        // if use out of box Range(1000000,999999) validation, it fails.
        [CustomValidation(typeof(Plot), nameof(ValidateNorthing))]
        public int Northing
        {
            get => _northing;
            set => SetProperty(ref _northing, value, value != 0);
        }
        private int _northing;
        
        public int OGFSampleTreeAge { get; set; }

        public int OGFSampleTreeDBH_cm { get; set; }

        public int OGFSampleTreeSpecies { get; set; }

        public int OldGrowthAge
        {
            get => _oldGrowthAge;
            set => SetProperty(ref _oldGrowthAge, value, false);

        }

        public int OldGrowthDBH
        {
            get => _oldGrowthDBH;
            set => SetProperty(ref _oldGrowthDBH, value, false);

        }

        public int OldGrowthSpeciesID
        {
            get => _oldGrowthSpeciesID;
            set => SetProperty(ref _oldGrowthSpeciesID, value, false);

        }

        public bool OneCohortSenescent
        {
            get => _oneCohortSenescent;
            set => SetProperty(ref _oneCohortSenescent, value, false);

        }



        [ObservableProperty] // exposes hook "On___Changed"
        private PlantedTypeEnum _plantedType;
        partial void OnPlantedTypeChanged(PlantedTypeEnum value)
        {
            // Keep the two Groups in sync
            if (value == PlantedTypeEnum.Acadian)
                EcositeGroup = EcositeGroup.Acadian;
            else if (value == PlantedTypeEnum.Coastal || value == PlantedTypeEnum.MaritimeBoreal)
                EcositeGroup = EcositeGroup.MaritimeBoreal;

            ValidateProperty(value, nameof(PlantedType));
        }
        public static ValidationResult ValidateEasting(object value, ValidationContext context)
        {
            // value is expected to be an int
            if (value is int eastingValue)
            {
                // skip validation if it's 0 (Entry is blank or default)
                if (eastingValue == 0)
                    return ValidationResult.Success;

                // validate range
                if (eastingValue >= 100000 && eastingValue <= 999999)
                    return ValidationResult.Success;
            }

            return new ValidationResult("Easting must be a six-digit number.");
        }

        public static ValidationResult ValidateNorthing(object value, ValidationContext context)
        {
            // value is expected to be an int
            if (value is int northingValue)
            {
                // skip validation if it's 0 (Entry is blank or default)
                if (northingValue == 0)
                    return ValidationResult.Success;

                // validate range
                if (northingValue >= 1000000 && northingValue <= 9999999)
                    return ValidationResult.Success;
            }

            return new ValidationResult("Northing must be a seven-digit number.");
        }

        public static ValidationResult ValidateEcodistrict(object value, ValidationContext context)
        {
            // value is expected to be an int
            if (value is int eco)
            {
                // skip validation if it's 0 (Entry is blank or default)
                if (eco == 0)
                    return ValidationResult.Success;

                // validate range
                if (eco >= 100 && eco <= 999)
                    return ValidationResult.Success;
            }

            return new ValidationResult("Ecodistrict must be a 3 digit number");
        }




        public static ValidationResult ConditionalRequired(object value, ValidationContext context)
        {
            // value is the bool property being validated (IsPlanted)
            if (value is bool isPlanted)
            {
                // get the other property (PlantedType) via reflection
                var instance = context.ObjectInstance;
                var plantedTypeProp = instance.GetType().GetProperty("PlantedType");

                if (plantedTypeProp == null)
                    return new ValidationResult("PlantedType property not found.");

                var plantedTypeValue = plantedTypeProp.GetValue(instance);

                // make sure it's the enum type before comparing
                if (plantedTypeValue is PlantedTypeEnum pt)
                {
                    if (isPlanted && pt != PlantedTypeEnum.None)
                        return ValidationResult.Success;
                    if(!isPlanted)
                        return ValidationResult.Success;
                }
            }

            return new ValidationResult("You must select a PlantedType if IsPlanted is true.");
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


        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Plot Number must be a positive number")]
        public int PlotNumber
        {
            get => _plotNumber;
            set => SetProperty(ref _plotNumber, value, true);

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
            set => SetProperty(ref _transectDirection, value, false);

        }

        public decimal TransectLength
        {
            get => _transectLength;
            set => SetProperty(ref _transectLength, value, false);

        }

        [NotMapped]
        public int TreeCount
        {
            get => _treeCount;
            set
            {
                SetProperty(ref _treeCount, value);
                OnPropertyChanged();
            }
        }

        public UnderstoryDominated UnderstoryDominated
        {
            get => _understoryDominated;
            set => SetProperty(ref _understoryDominated, value, false);

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

        public string? AgeTreeSpeciesCode { get; set; }
        public string? OGTreeSpeciesCode { get; set; }


        private Ecosite? _ecosite = null!;
        public Ecosite? Ecosite
        {
            get => _ecosite;
            set => SetProperty(ref _ecosite, value, false);

        }

        private TreeSpecies? _ageTreeSpecies = null!;
        public TreeSpecies? AgeTreeSpecies
        {
            get => _ageTreeSpecies;
            set
            {
                _ageTreeSpecies = value;
                OnPropertyChanged();
                OnAgeTreeSpeciesChanged(value);

            }
        }

        private void OnAgeTreeSpeciesChanged(TreeSpecies treeSpecies)
        {

            AgeTreeSpeciesCode = treeSpecies?.ShortCode ?? string.Empty;
     
        }   




        [ObservableProperty]
        [CustomValidation(typeof(Plot), nameof(ValidateEcodistrict))]
        private int _ecodistrict;



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
            if (PlantedType == PlantedTypeEnum.Acadian)
            {
                EcositeGroup = EcositeGroup.Acadian;
            }
            else if (PlantedType == PlantedTypeEnum.Coastal ||
                PlantedType == PlantedTypeEnum.MaritimeBoreal)
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

        private void OnHasOldGrowthChanged(bool hasOldGrowth)
        {
            if (!hasOldGrowth)
            { //reset things
                OldGrowthAge = 0;
                OldGrowthDBH = 0;
                OldGrowthSpeciesID = 1;
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