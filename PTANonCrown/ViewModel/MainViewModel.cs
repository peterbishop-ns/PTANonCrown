//using static
//.Renderscripts.Script;
using ClosedXML.Excel;
using CommunityToolkit.Maui.Core.Primitives;
using CommunityToolkit.Maui.Storage;
using CsvHelper;
using PTANonCrown.Data.Models;
using PTANonCrown.Data.Repository;
using PTANonCrown.Services;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Input;
using PTANonCrown.Data.Services;
using Microsoft.Maui.Storage;
using System.IO;
using CommunityToolkit.Maui.Storage;
using Microsoft.EntityFrameworkCore;
using DocumentFormat.OpenXml.Wordprocessing;
using Windows.Devices.SerialCommunication;
using PTANonCrown.Helpers;


namespace PTANonCrown.ViewModel
{
    public class MainViewModel : BaseViewModel
    {

        private Plot _currentPlot;
        private Stand _currentStand;
        private string _errorMessage;
        private bool _isCheckedBiodiversity;
        private bool _isSelectedExportAll;
        private bool _isSelectedExportSelectedOnly;
        private bool _isValidationError;
        private bool _plotWasTreated;
        private Soil _selectedSoil;
        private ObservableCollection<SummarySoilResult> _soilSummary;
        private ObservableCollection<SummaryResultTreeSpecies> _speciesSummary;
        private ObservableCollection<SummaryItem> _summaryItems = new ObservableCollection<SummaryItem>();
        private string _summaryPageMessage;
        private Plot _summaryPlot;
        private bool _summarySectionIsVisible;
        private ObservableCollection<SummaryTreatmentResult> _treatmentSummary = new ObservableCollection<SummaryTreatmentResult>();
        private string _validationMessage;

        private ObservableCollection<SummaryVegetationResult> _vegetationSummary;


        public MainViewModel(DatabaseService databaseService, StandRepository standRepository, LookupRepository lookupRepository, LookupRefreshService lookupRefreshService)
        {
            AppLogger.Log("Init");

            _databaseService = databaseService;
            _standRepository = standRepository;
            _lookupRepository = lookupRepository;
            _lookupRefreshService = lookupRefreshService;


            AppLogger.Log("LoadLookupTables", MethodBase.GetCurrentMethod().Name);
            LoadLookupTables();

            ValidationMessage = string.Empty;

        }

        public void PopulateUiTreatments(Plot plot)
        {
            // a UI only list of Plot Treatments
            // This is to prevent conflicts when saving to the database (by binding directly 
            // to the plot's PlotTreatment objects, there were Key insert errors, which 
            // were not resolved even after 'hydrating' the colleciton

            if (plot is null) { return; }

            UiPlotTreatments = new ObservableCollection<PlotTreatment>(
                Treatments.Select(t => new PlotTreatment
                {
                    TreatmentId = t.ID,
                    Treatment = t,
                    IsActive = plot.PlotTreatments?.Any(pt => pt.TreatmentId == t.ID && pt.IsActive) == true
                })
            );

            RefreshPlotWasTreated(UiPlotTreatments);

            OnPropertyChanged(nameof(UiPlotTreatments));

        }



        public async Task OnShellNavigatedAsync(object sender, ShellNavigatedEventArgs e)
        {
            // PROMPT User To Save 
            var previous = e.Previous?.Location.ToString().ToUpper();
            bool promptUserToSave = previous is not null && previous != "//MAINPAGE" && previous != "//SUMMARYPAGE";
            if (promptUserToSave)
            {
                var db = _databaseService.GetContext();
                var changeTracker = db.ChangeTracker;
                var hasChanges = changeTracker.HasChanges();
                var entries = db.ChangeTracker
                    .Entries();
                var Stands = entries.Where(e => e.Entity is Stand);

                var changes = entries
                    .Where(e => e.State != EntityState.Unchanged);
                if (hasChanges)
                {
                    OnPropertyChanged(nameof(SaveFilePath));
                   
                }
            }
        }

        public async Task<bool> HandleUnsavedChangesOnExitAsync()
        {
            var context = _databaseService.GetContext();
            OnPropertyChanged(nameof(SaveFilePath));
            if (!context.ChangeTracker.HasChanges())
                return true;

            // Prompt user
            // Show Yes / No / Cancel
            string choice = await Shell.Current.DisplayActionSheet(
                "Save before exiting?",
                null,
                null,       // destruction button (optional)
                "Save", 
                "Don't Save",// other buttons
                "Cancel"   // cancel button
            );

            switch (choice)
            {
                case "Save":
                    await SaveAllAsync();
                    return true;  // safe to exit after saving

                case "Don't Save":
                    return true;  // discard changes, safe to exit

                case "Cancel":
                default:
                    return false; // don't exit
            }
        }



        public ObservableCollection<Stand> _allStands { get; set; }

        public ICommand AddTreeCommand => new Command(_ => AddTrees(1));



        public ObservableCollection<Stand> AllStands
        {
            get => _allStands;
            set
            {
                if (_allStands != value)
                {
                    _allStands = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand CreateNewStandCommand => new Command(_ => CreateNewStand());

        public Plot CurrentPlot
        {
            get => _currentPlot;

            set
            {

 
                    PopulatePlotFromUi(_currentPlot); // BEFORE Change: populate PREVIOUS plot with the CURRENT UI
                    _currentPlot = value; // CHANGE value
                    PopulateUiFromPlot(value); // AFTER Change: load the NEW plot's values into the UI

                    OnPropertyChanged();
            }
        }


        public string StandErrors => ValidationHelpers.GetStandErrors(AllStands);
        public string PlotErrors => ValidationHelpers.GetPlotErrors(AllStands);
        public string TreeErrors => ValidationHelpers.GetTreeErrors(AllStands);
        public string SummaryErrors => ValidationHelpers.GetSummaryErrors(AllStands);
        public string AllStandAllsErrors => ValidationHelpers.GetAllErrors(AllStands);

        public void RefreshErrors()
        {
            OnPropertyChanged(nameof(StandErrors));
            OnPropertyChanged(nameof(PlotErrors));
            OnPropertyChanged(nameof(TreeErrors));
            OnPropertyChanged(nameof(SummaryErrors));
            OnPropertyChanged(nameof(AllStandAllsErrors));
        }

        public Stand CurrentStand
        {
            get => _currentStand;
            set
            {

                if (_currentStand != null)
                {
                    // Unsubscribe from old CurrentPlot's property changes
                    _currentStand.PropertyChanged -= Stand_PropertyChanged;
                    //_currentStand.ErrorsChanged -= RefreshErrors;


                }

                _currentStand = value;
                OnPropertyChanged();

                if (_currentStand != null)
                {
                    // Subscribe to new CurrentStand's property changes
                    _currentStand.PropertyChanged += Stand_PropertyChanged;
                   // _currentStand.ErrorsChanged += RefreshErrors;

                }

            }

        }

        public ICommand DeleteLiveTreeCommand => new Command<TreeLive>(tree => DeleteLiveTree(tree));
        public ICommand DeletePlotCommand => new Command<Plot>(plot => DeletePlot(plot));

        public ICommand ExportSummaryCommand =>
            new Command<string>(method => ExecutePickFolderCommand());

        public bool HasNoSummaryItems => SummaryItems?.Count == 0;
        public bool HasNoTreatments => TreatmentSummary?.Count == 0;
        public bool HasSummaryItems => SummaryItems?.Count > 0;
        public bool HasTreatments => TreatmentSummary?.Count > 0;

        public bool IsCheckedBiodiversity
        {
            get => _isCheckedBiodiversity;
            set
            {
                if (_isCheckedBiodiversity != value)
                {
                    _isCheckedBiodiversity = value;
                    OnPropertyChanged();
                    OnIsCheckedBiodiversityChanged();

                }
            }
        }

        public bool IsSelectedExportAll
        {
            get => _isSelectedExportAll;
            set => SetProperty(ref _isSelectedExportAll, value);

        }

        public bool IsSelectedExportSelectedOnly
        {
            get => _isSelectedExportSelectedOnly;
            set => SetProperty(ref _isSelectedExportSelectedOnly, value);
        }

        public bool IsValidationError
        {
            get => _isValidationError;
            set => SetProperty(ref _isValidationError, value);
        }

        public List<int> ListPercentage { get; set; }

        private List<string> _listExposure;
        public List<string> ListExposure
        {
            get => _listExposure;
            set
            {
                _listExposure = value;
                OnPropertyChanged();
            }
        }



        public List<Ecosite> LookupEcosites { get; set; }

        public List<EcositeSoilVeg> LookupEcositeSoilVeg { get; set; }


        private List<Soil> _lookupSoils;
        public List<Soil> LookupSoils
        {
            get => _lookupSoils;
            set
            {
                _lookupSoils = value;
                OnPropertyChanged();
            }
        }

        private List<Vegetation> _lookupVeg;
        public List<Vegetation> LookupVeg
        {
            get => _lookupVeg;
            set
            {
                _lookupVeg = value;
                OnPropertyChanged();
            }
        }


        private List<TreeSpecies> _lookupTreeSpecies;
        public List<TreeSpecies> LookupTreeSpecies
        {
            get => _lookupTreeSpecies;
            set
            {
                _lookupTreeSpecies = value;
                OnPropertyChanged();
            }
        }

        public ICommand NewPlotCommand =>
            new Command<string>(method => CreateNewPlot(CurrentStand));

        public ICommand OpenFileCommand => new Command<string>(method => OpenFile());
        public ICommand NewFileCommand => new Command<string>(method => NewFile());

        public ICommand PickFolderCommand => new Command(async () => await ExecutePickFolderCommand());
        public ObservableCollection<PlotTreatment> UiPlotTreatments { get; set; }

        public bool PlotWasTreated
        {
            get => _plotWasTreated;
            set
            {
                if (_plotWasTreated != value)
                {
                    _plotWasTreated = value;
                    OnPropertyChanged();
                    OnPlotWasTreatedChanged();
                }
            }
        }

        public ICommand PromptDeleteStandCommand => new Command<Stand>(stand => PromptDeleteStand(stand));

        public ICommand SaveAllCommand =>
        new Command<string>(method => SaveAllAsync());
        

        public Soil SelectedSoil
        {
            get => _selectedSoil;
            set
            {
                if (_selectedSoil != value)
                {
                    _selectedSoil = value;
                    OnPropertyChanged();

                }
            }
        }

        public ICommand SetCurrentPlotCommand => new Command<Plot>(plot => SetCurrentPlot(plot));

        public ICommand SetPlotSummaryCommand => new Command<Plot>(plot => SetSummaryPlot(plot));

        public ICommand SetStandOnlyCommand => new Command<Plot>(plot => SetSummaryStandOnly());

        public ObservableCollection<string> SoilPhases { get; } = new ObservableCollection<string>();

        public List<string> soils { get; set; }

        public ObservableCollection<SummarySoilResult> SoilSummary
        {
            get => _soilSummary;
            set
            {
                if (_soilSummary != value)
                {
                    _soilSummary = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<SummaryResultTreeSpecies> SpeciesSummary
        {
            get => _speciesSummary;
            set
            {
                if (_speciesSummary != value)
                {
                    _speciesSummary = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand SpecifyTreeCountInPlotCommand => new Command(SpecifyTreeCountInPlot);

        public bool StandOnlySummary { get; set; }

        public ObservableCollection<SummaryItem> SummaryItems
        {
            get => _summaryItems;
            set
            {
                if (_summaryItems != value)
                {
                    _summaryItems = value;
                    OnPropertyChanged();
       
                }
            }
        }

        public string SummaryPageMessage
        {
            get => _summaryPageMessage;
            set
            {
                if (_summaryPageMessage != value)
                {
                    _summaryPageMessage = value;
                    OnPropertyChanged();
                }
            }
        }

        public Plot SummaryPlot
        {
            get => _summaryPlot;
            set => SetProperty(ref _summaryPlot, value);

        }

        public bool SummarySectionIsVisible
        {
            get => _summarySectionIsVisible;
            set
            {
                if (_summarySectionIsVisible != value)
                {
                    _summarySectionIsVisible = value;
                    OnPropertyChanged();
                }
            }
        }

        public IEnumerable<CardinalDirections> TransectDirections { get; } =
    Enum.GetValues(typeof(CardinalDirections)).Cast<CardinalDirections>();

        public List<Treatment> Treatments { get; set; }

        public ObservableCollection<SummaryTreatmentResult> TreatmentSummary
        {
            get => _treatmentSummary;
            set
            {
                
                _treatmentSummary = value;

   
                OnPropertyChanged();

            }
        }

        public ObservableCollection<TreeSpecies> TreeLookupFilteredList { get; set; }

        public ICommand UsePredictedHeightsCommand => new Command(_ => UsePredictedHeights());

        public string ValidationMessage
        {
            get => _validationMessage;
            set => SetProperty(ref _validationMessage, value);
        }

        public List<string> veg { get; set; }

        public ObservableCollection<SummaryVegetationResult> VegetationSummary
        {
            get => _vegetationSummary;
            set
            {
                if (_vegetationSummary != value)
                {
                    _vegetationSummary = value;
                    OnPropertyChanged();
                }
            }
        }

        public string SaveFilePath
        {
            get
            {
                var path = _databaseService.SaveFilePath;
                bool hasChanges = _databaseService.GetContext().ChangeTracker.HasChanges();

                string displayText = string.IsNullOrEmpty(path) ? string.Empty : $"({path})";

                if (hasChanges)
                    displayText += "                        * UNSAVED changes *";

                return $"Pre-Treatment Assessment {displayText}";
            }
        }

        private LookupRepository _lookupRepository { get; set; }
        private LookupRefreshService _lookupRefreshService { get; set; }
        private DatabaseService _databaseService { get; set; }
        private Dictionary<string, List<string>> _phaseToSoilTypes { get; set; }
        private StandRepository _standRepository { get; set; }

        public void AddNewTreeToPlot(Plot plot, int treeNumber)
        {
            var tree = new TreeLive()
            {
                TreeNumber = treeNumber,
                LookupTrees = LookupTreeSpecies,
                Plot = plot
            };

            plot.PlotTreeLive.Add(tree);

            _databaseService.GetContext().Add(tree);

        }

        public void DeleteLiveTree(TreeLive tree)
        {
            //remove it
            CurrentPlot.PlotTreeLive.Remove(tree);

            //Re-adjust the treenumbering (so as to not have a gap)
            int treeCount = 1;
            foreach (TreeLive treeLive in CurrentPlot.PlotTreeLive)
            {
                treeLive.TreeNumber = treeCount;
                treeCount++;
            }


        }

        public string GetEco(string soilType, string soilPhase, string VegType, List<dynamic> csvRecords)
        {
            string match = (string)csvRecords.Where(r => r.Soil == soilType + soilPhase && r.Veg == VegType).Select(r => r.Eco).FirstOrDefault();
            return match;
        }

        public List<string> GetForestGroups(List<dynamic> csvRecords)
        {
            var pattern = new Regex(@"^([A-Z]+)"); // capture letters at the start

            // Get all distinct soil codes
            var veg = csvRecords.Select(r => (string)r.Veg).Distinct();

            // Extract base code without trailing letters and sort by numeric part
            var forestGroups = veg
                .Select(s =>
                {
                    var match = pattern.Match(s);
                    if (!match.Success)
                        return s; // fallback if regex doesn't match

                    string forestGroup = match.Groups[1].Value;  // "MW"
                    return forestGroup;
                })
                .Distinct()
                .OrderBy(s => s) // alphabetical
                .ToList();

            return forestGroups;
        }

        // Get list of unique soil types
        public List<string> GetSoilTypes(List<dynamic> csvRecords)
        {
            var pattern = new Regex(@"^([A-Z]+)(\d+)([A-Z]*)$");
            // Group 1 = prefix letters (ST)
            // Group 2 = numeric part
            // Group 3 = optional trailing letters

            // Get all distinct soil codes
            var soils = csvRecords.Select(r => (string)r.Soil).Distinct();

            // Extract base code without trailing letters and sort by numeric part
            var soilTypes = soils
                .Select(s =>
                {
                    var match = pattern.Match(s);
                    if (!match.Success)
                        return s; // fallback if regex doesn't match

                    string prefix = match.Groups[1].Value;  // "ST"
                    string number = match.Groups[2].Value;  // "1", "2", etc.
                    return prefix + number;
                })
                .Distinct()
                .OrderBy(s =>
                {
                    var match = pattern.Match(s);
                    return int.Parse(match.Groups[2].Value); // sort by numeric part
                })
                .ToList();

            return soilTypes;
        }

        public List<string> GetVegTypes(List<dynamic> csvRecords)
        {
            var vegTypes = csvRecords.Select(r => (string)r.Veg).Distinct().OrderBy(v => v).ToList();
            return vegTypes;

        }


     
        // Method that takes soil type, soil phase, veg type, and returns an eco group
        public List<dynamic> ReadCsvRecords(string csvFileName)
        {
            using var stream = FileSystem.OpenAppPackageFileAsync(csvFileName).Result;
            using var reader = new StreamReader(stream);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            // Force enumeration immediately
            return csv.GetRecords<dynamic>().ToList();
        }


        public void RefreshAllStands()
        {

            // Store the existing collection in a temporary list
            var tempList = AllStands.ToList();
            var tempCurrentStand = CurrentStand; //store CurrentStand; this gets wiped

            // Clear the existing ObservableCollection
            AllStands.Clear();

            // Re-add the same items from the temporary list
            foreach (var stand in tempList)
            {
                AllStands.Add(stand);
            }

            //reset the Current stand
            SetCurrentStand(tempCurrentStand);

        }

        public void UsePredictedHeights()
        {
            foreach (TreeLive tree in CurrentPlot.PlotTreeLive)
            {
                tree.Height_m = tree.HeightPredicted_m;
            }
        }



        private void AddTrees(int treesToAdd)
        {

            // Get max tree number
            int currentMaxTreeNumber = GetMaxTreeNumber();

            // Add the trees
            for (int i = 0; i < treesToAdd; i++)
            {
                AddNewTreeToPlot(CurrentPlot, currentMaxTreeNumber + 1);
                currentMaxTreeNumber++;
            }
        }

        private int CountDigits(int number)
        {
            int result = number.ToString().TrimStart('-').Length;
            return result;
        }

        public void InitializeCoarseWoody(Plot parentPlot)
        {
            if (parentPlot.PlotCoarseWoody is not null && parentPlot.PlotCoarseWoody.Count > 0)
            {
                return;
            }

            
            var coarseWoody = new ObservableCollection<CoarseWoody>
            {
                new CoarseWoody { Plot = parentPlot, DBH_start = 21, DBH_end = 30 },
                new CoarseWoody { Plot = parentPlot, DBH_start = 31, DBH_end = 40 },
                new CoarseWoody { Plot = parentPlot, DBH_start = 41, DBH_end = 50 },
                new CoarseWoody { Plot = parentPlot, DBH_start = 51, DBH_end = 60 },
                new CoarseWoody { Plot = parentPlot, DBH_start = 60, DBH_end = -1 }
            };
            parentPlot.PlotCoarseWoody = coarseWoody;
        }

        public void InitializeFirstTree(Plot parentPlot)
        {
            if ((parentPlot?.PlotTreeLive == null) | (parentPlot?.PlotTreeLive.Count == 0))
            {
                AddNewTreeToPlot(parentPlot, 1);
            }
        }

        public void InitializeTreeDead(Plot parentPlot)
        {
            if (parentPlot.PlotTreeDead is not null && parentPlot.PlotTreeDead.Count > 0) { return; }
            var treeDead = new ObservableCollection<TreeDead>
            {
                new TreeDead() { Plot = parentPlot, DBH_start = 21, DBH_end = 30 },
                new TreeDead() { Plot = parentPlot, DBH_start = 31, DBH_end = 40 },
                new TreeDead() { Plot = parentPlot, DBH_start = 41, DBH_end = 50 },
                new TreeDead() { Plot = parentPlot, DBH_start = 51, DBH_end = 60 },
                new TreeDead() { Plot = parentPlot, DBH_start = 60, DBH_end = -1 }
            };

            parentPlot.PlotTreeDead = treeDead;

        }

        private Plot CreateNewPlot(Stand stand)
        {

            
            RefreshEcosite(CurrentSoil, CurrentVeg, CurrentEcositeGroup.ToString());


            int newPlotNumber = (stand.Plots != null && stand.Plots.Any())
                ? stand.Plots.Max(p => p.PlotNumber) + 1
                : 1;


            var _newPlot = new Plot
            {
                PlotNumber = newPlotNumber,
                Soil = LookupSoils.FirstOrDefault(s => string.IsNullOrEmpty(s.ShortCode)) ?? new Soil { ShortCode = string.Empty },
                Exposure = null,
                Easting = 0,
                Northing = 0,
                Vegetation = LookupVeg.FirstOrDefault(v => string.IsNullOrEmpty(v.ShortCode)) ?? new Vegetation { ShortCode = string.Empty },
                AgeTreeSpecies = null,
                EcositeGroup = EcositeGroup.Acadian,
                AgeTreeSpeciesCode = LookupTreeSpecies.FirstOrDefault(v => string.IsNullOrEmpty(v.ShortCode))?.ShortCode ?? string.Empty,
                OGTreeSpeciesCode = LookupTreeSpecies.FirstOrDefault(v => string.IsNullOrEmpty(v.ShortCode))?.ShortCode ?? string.Empty,
                PlotTreatments = new ObservableCollection<PlotTreatment>(
        (Treatments ?? Enumerable.Empty<Treatment>()).Select(t => new PlotTreatment
        {
            TreatmentId = t.ID,
            IsActive = false
        })
    ),
                AgeTreeAge = 0,
                AgeTreeDBH = 0,
                OldGrowthAge = 0,
                OldGrowthDBH = 0,
                Ecodistrict = 0
            };

            // keep UI Treatments in sync
            _newPlot.Stand = stand;
            _newPlot.PlotTreeLive = new ObservableCollection<TreeLive>();
            stand.Plots.Add(_newPlot);
            _databaseService.GetContext().Add(_newPlot);  // ensures EF knows about them
            SetCurrentPlot(_newPlot);
            return _newPlot;

        }



        private void PopulateUiFromPlot(Plot plot)
        {
            if (plot is null) { return;  }
            PopulateUiTreatments(plot);
            CurrentSoil = LookupSoils.Where(s => s.ShortCode == plot.SoilCode).FirstOrDefault();
            CurrentVeg = LookupVeg.Where(v => v.ShortCode == plot.VegCode).FirstOrDefault();
            CurrentAgeTreeSpecies = LookupTreeSpecies
               .FirstOrDefault(t => string.Equals(t.ShortCode, CurrentPlot.AgeTreeSpeciesCode, StringComparison.OrdinalIgnoreCase));
            CurrentOGTreeSpecies = LookupTreeSpecies
                .FirstOrDefault(t => string.Equals(t.ShortCode, CurrentPlot.OGTreeSpeciesCode, StringComparison.OrdinalIgnoreCase));
            CurrentEcositeGroup = plot.EcositeGroup;
            RefreshEcosite(plot.Soil, plot.Vegetation, plot.EcositeGroup.ToString());
            RefreshTreeLookupsOnPlot(plot);
            // Refresh LIT status of trees
            plot.UpdatePlotTreeLIT();
        }
        
        private void RefreshTreeLookupsOnPlot(Plot plot)
        {
            // Need to populate each tree with the full LookupTreeSpecies to ensure dropdown bindings work
            foreach (TreeLive tree in plot.PlotTreeLive)
            {
                tree.LookupTrees = LookupTreeSpecies.Where(t => t.ID != -1).ToList(); 
                var match = tree.LookupTrees.Where(t => t.ShortCode == tree.TreeSpeciesShortCode).FirstOrDefault();
                if (match is not null)
                {
                    tree.TreeSpecies = match;

                }

            }

        }

        private Stand CreateNewStand()
        {
            // get new stand number
            int newStandNumber = AllStands?.Count() > 0 ? AllStands.Max(s => s.StandNumber) + 1 : 1;

            Stand _stand = new Stand()
            {
                StandNumber = newStandNumber
            };

            _stand.CruiseID = string.Empty;
            _stand.PlannerID = string.Empty;

            AllStands.Add(_stand);


            // Add to context so it is tracked
            var db = _databaseService.GetContext();
            db.Stands.Add(_stand);


            SetCurrentStand(_stand);
            return _stand;

        }



        private void RefreshEcosite(Soil? soil, Vegetation? veg, string ecositeGroup)
        {
            // Step 1 - look it up in the soil.
            EcositeSoilVeg? matchingRecord = LookupEcositeSoilVeg.Where(r => r.SoilCode == soil?.ShortCode &&
            r.VegCode == veg?.ShortCode &&
            r.EcositeGroup == ecositeGroup

            ).FirstOrDefault();

            Ecosite newEcosite = LookupEcosites.Where(e => e.ShortCode == matchingRecord?.EcositeCode).FirstOrDefault();
            CurrentEcosite = newEcosite;

            OnPropertyChanged(nameof(EcositeErrorMessage));


        }


        private void DeletePlot(Plot plot)
        {
            if (plot is not null)
            {
                CurrentStand.Plots.Remove(plot);

                GetOrCreatePlot(CurrentStand);
            }

        }

        private void DeleteStand(Stand stand)
        {

            AllStands.Remove(stand);
            _standRepository.Delete(stand.ID);
        }

        // Async method to show the remove trees confirmation dialog
        private async Task<bool> DisplayRemoveTreesConfirmation(string message)
        {
            return await Application.Current.MainPage.DisplayAlert(
                "Remove Trees?",
                message,
                "Continue",
                "Cancel"
            );
        }

        public string EcositeErrorMessage
        {
            get
            {
                if (CurrentSoil is null || CurrentVeg is null || CurrentEcositeGroup == EcositeGroup.None)
                {
                    return "Please select a Soil Type, Vegetation Type, and Ecosite Group to determine the Ecosite.";
                }
                // If any selection is missing, or combination is invalid
                if (CurrentEcosite == null)
                {
                    return "This combination of Soil Type, Vegetation Type and Ecosite Group has no matching Ecosite.";
                }



                return string.Empty; // valid
            }
        }



        private async void DisplayTreeWarningMessage(Plot plot)
        {

            string message = $"Stand {plot.Stand.StandNumber} | Plot {plot.PlotNumber}: Cruise Summary cannot be generated; tree(s) missing DBH, heights or species. ";


        }

        private async Task ExecutePickFolderCommand()
        {

            if (_databaseService.DbIsNew)
            {
                await Application.Current.MainPage.DisplayAlert("Save First", "Before exporting a summary, please save this file.", "OK");
                return;
            }

            RefreshErrors();
            if (SummaryErrors != string.Empty)
            {
                await Application.Current.MainPage.DisplayAlert("Invalid Trees", "Some tree(s) are invalid - summary cannot be exported.", "OK");
                return;
            }
            try
            {
                // Pick a folder from the file system
                var ptaFolderName = Path.GetDirectoryName(_databaseService.SaveFilePath);
                var selectedFolder = await FolderPicker.PickAsync(ptaFolderName);

                if (selectedFolder != null)
                {
                    string folderPath = $"Folder Name: {selectedFolder.Folder}";
                    ExportSummary(folder: selectedFolder.Folder);
                }
                else
                {
                    // Handle case when no folder is picked
                    await Application.Current.MainPage.DisplayAlert("No Folder", "No folder was selected.", "OK");
                }
            }
            catch (Exception ex)
            {
                // Handle potential errors, like permission issues
                await Application.Current.MainPage.DisplayAlert("Error", "An error occurred: " + ex.Message, "OK");
            }
        }

        private void ExportAllTrees(XLWorkbook workbook, string exportFilePath)
        {
            string tabName = null;
            PropertyInfo[] treeProps = new PropertyInfo[] { };

            foreach (Stand stand in AllStands)
            {
                var trees = stand.Plots
                    .SelectMany(p => p.PlotTreeLive);
                tabName = $"Stand {stand.StandNumber}-Trees";
                treeProps = GetOrderedProperties<TreeLive>(priorityKeywords: new[] { "Plot","TreeNumber","TreeSpecies","DBH_cm","Height_m","HeightPredicted_m"}, 
                                                            excludeProps: new[] { "TreeSpeciesShortCode","SearchSpecies", "LookupTrees","TreeSpeciesFilteredList","HasErrors", "ID"});
                ExportToExcel(workbook, tabName, trees, exportFilePath, treeProps);
            }
        }

        private void ExportSuccessMessage(string destination)
        {
            // Inform the user
            Application.Current.MainPage.DisplayAlert("Export Successful", $"Summary has been exported to {destination}.", "OK");
        }

        private void ExportSummary(Folder folder)
        {
            // Generate species summary for current stand
            SpeciesSummary = GenerateTreeSpeciesSummary(CurrentStand.Plots);

            
            // Create a new Excel workbook
            var workbook = new XLWorkbook();
            var ptaFileName = Path.GetFileNameWithoutExtension(_databaseService.SaveFilePath);
            string exportFilePath = Path.Combine(folder.Path, $"{ptaFileName}_summary.xlsx");

            // -----------------------------
            // Execute exports
            // -----------------------------
            ExportStandsOverview(workbook, exportFilePath);
            ExportPlotsOverview(workbook, exportFilePath);
            ExportTreatments(workbook, exportFilePath);
            ExportAllTrees(workbook, exportFilePath);

            ExportStandSummaries(workbook, exportFilePath);
            ExportPlotSummaries(workbook, exportFilePath);

            // -----------------------------
            // Done
            // -----------------------------
            ExportSuccessMessage(exportFilePath);
        }
        private void ExportStandsOverview(XLWorkbook workbook, string exportFilePath)
        {
            var props = GetOrderedProperties<Stand>(
                priorityKeywords: new[] { "StandNumber" },
                excludeProps: new[] { "ID", "Plots" });

            ExportToExcel(workbook, "Stands", AllStands, exportFilePath, props);
        }




        private void ExportPlotsOverview(XLWorkbook workbook, string exportFilePath)
        {
            var props = GetOrderedProperties<Plot>(
                priorityKeywords: new[] { "PlotNumber" },
                excludeProps: new[]
                {
            "ID", "PlotCoarseWoody", "Stand", "PlotTreatmentsDisplayString",
            "PlotTreeDead", "PlotTreeLive"
                });

            foreach(Stand stand in AllStands)
            {
                var plotsFlattened = stand.Plots.ToList();

                ExportToExcel(workbook, $"Stand {stand.StandNumber}-Plots", plotsFlattened, exportFilePath, props, transpose: true);
            }

        }
        private void ExportTreatments(XLWorkbook workbook, string exportFilePath)
        {
            var props = GetOrderedProperties<PlotTreatment>(
                priorityKeywords: new[] { "Plot" },
                excludeProps: new[] { "PlotId", "HasErrors", "ID", "IsActive", "Treatment", "TreatmentId" });

            var treatments = AllStands
                .SelectMany(s => s.Plots)
                .SelectMany(p => p.PlotTreatments)
                .ToList();

            ExportToExcel(workbook, "Treatments", treatments, exportFilePath, props);
        }
        private void ExportStandSummaries(XLWorkbook workbook, string exportFilePath)
        {
            var props = typeof(SummaryItem).GetProperties();

            foreach (var stand in AllStands)
            {
                var summaryResult = TreeSummaryHelper.GenerateSummaryResult(plots: stand.Plots);
                string tabName = $"Cruise (S{stand.StandNumber})";

                ExportToExcel(workbook, tabName, summaryResult, exportFilePath, props);
            }
        }
        private void ExportPlotSummaries(XLWorkbook workbook, string exportFilePath)
        {
            var props = typeof(SummaryItem).GetProperties();

            foreach (var plot in AllStands.SelectMany(s => s.Plots))
            {
                var summaryResult = TreeSummaryHelper.GenerateSummaryResult(new[] { plot });
                string tabName = $"Cruise (S{plot.Stand.StandNumber}-P{plot.PlotNumber})";

                ExportToExcel(workbook, tabName, summaryResult, exportFilePath, props);
            }
        }

        private PropertyInfo[] GetOrderedProperties<T>(
    string[]? priorityKeywords = null,
    string[]? excludeProps = null,
    string[]? includeProps = null)
        {
            var properties = typeof(T).GetProperties();

            // Sort
            var sorted = priorityKeywords != null
                ? priorityKeywords
                    .SelectMany(k => properties.Where(p => p.Name.Contains(k, StringComparison.OrdinalIgnoreCase)))
                    .Concat(properties.Where(p => priorityKeywords.All(k => !p.Name.Contains(k, StringComparison.OrdinalIgnoreCase))))
                    .ToArray()
                : properties;
            // Filter
            if (excludeProps != null)
            {
                sorted = sorted
                   .Where(p =>!excludeProps.Contains(p.Name))
                   .ToArray();
            }

            return sorted;
        }

        private void ExportToExcel<T>(
                                XLWorkbook workBook,
                                string tabName,
                                IEnumerable<T> items, string exportFilePath,
                                PropertyInfo[] properties,
                                bool transpose = false)
        {
            try
            {
                AppLogger.Log($"Exporting {workBook}-{tabName}");
                DoExportToExcel(workBook, tabName, items, exportFilePath, properties, transpose);

            }
            catch(Exception e)
            {
                AppLogger.Log($"Failed to Export {workBook}-{tabName}: {e}");

            }

        }

        private void DoExportToExcel<T>(
    XLWorkbook workBook,
    string tabName,
    IEnumerable<T> items, string exportFilePath,

        PropertyInfo[] properties,
    bool transpose = false)
        {
            var itemsList = items.ToList();

            var worksheet = workBook.Worksheets.Add(tabName);


            // ==========================================================
            // TRANSPOSED (columns = items, rows = columns)
            // ==========================================================
            if (transpose)
            {

                // First column = property names
                for (int p = 0; p < properties.Length; p++)
                {
                    worksheet.Cell(p + 2, 1).Value = properties[p].Name;

                    for (int itemIndex = 0; itemIndex < itemsList.Count; itemIndex++)
                    {
                        var val = properties[p].GetValue(itemsList[itemIndex]);
                        worksheet.Cell(p + 2, itemIndex + 2).Value = val?.ToString() ?? string.Empty;
                    }
                }

                worksheet.Columns().AdjustToContents();
                workBook.SaveAs(exportFilePath);
                return;
            }

            // ==========================================================
            // NORMAL (rows = items, columns = properties)
            // ==========================================================
            for (int col = 0; col < properties.Length; col++)
            {
                worksheet.Cell(1, col + 1).Value = properties[col].Name;
            }

            for (int row = 0; row < itemsList.Count; row++)
            {
                for (int col = 0; col < properties.Length; col++)
                {
                    var propertyValue = properties[col].GetValue(itemsList[row]);
                    worksheet.Cell(row + 2, col + 1).Value = propertyValue?.ToString() ?? string.Empty;
                }
            }

            worksheet.Columns().AdjustToContents();
            workBook.SaveAs(exportFilePath);
        }


        private PropertyInfo[] FilterProperties(PropertyInfo[] properties, params string[] excludeNames)
        {
            var excludeList = excludeNames.ToList();

            return properties
                .Where(p => !excludeList.Contains(p.Name, StringComparer.OrdinalIgnoreCase))
                .ToArray();
        }

        private ObservableCollection<SummarySoilResult> GenerateSoilSummary(IEnumerable<Plot> plots)
        {
            ObservableCollection<SummarySoilResult> soilSummary = new ObservableCollection<SummarySoilResult>();

            int totalCount = plots.Count();

            // Group plots by Soil object
            var groupedBySoil = plots
                .GroupBy(p => new { Code = p.SoilCode});
            foreach (var group in groupedBySoil)
            {
                var count = group.Count();
                soilSummary.Add(new SummarySoilResult
                {
                    SoilCode = group.Key.Code,
                    Count = count,
                    Percentage = Math.Round(100.0 * count / totalCount, 1)
                });
            }

            return soilSummary; 
        }

        private ObservableCollection<SummaryTreatmentResult> GenerateTreatmentSummary(IEnumerable<Plot> plots)
        {


            ObservableCollection<SummaryTreatmentResult> treatmentSummary = new ObservableCollection<SummaryTreatmentResult>();
            foreach (Plot plot in plots)
            {
                var treatments = plot.PlotTreatments
                     .Where(pt => pt.IsActive)
                     .Select(pt =>
                         Treatments.FirstOrDefault(t => t.ID == pt.TreatmentId)?.Name 
                     );
                if (treatments.Count() != 0)
                {
                    SummaryTreatmentResult summary = new SummaryTreatmentResult()
                    {
                        PlotNumber = plot.PlotNumber,
                        Treatments = string.Join("\n", treatments)
                    };
                    treatmentSummary.Add(summary);

                }

            }

            return treatmentSummary;
        }

        private ObservableCollection<SummaryResultTreeSpecies> GenerateTreeSpeciesSummary(IEnumerable<Plot> plots)
        {


            var filtered = plots
             .SelectMany(plot => plot.PlotTreeLive.Select(tree => new { plot.PlotNumber, tree.TreeSpecies?.Name }));

            ObservableCollection<SummaryResultTreeSpecies> result = new ObservableCollection<SummaryResultTreeSpecies>();

            if ((filtered is null || filtered.Count() == 0))
            {
                return result;
            }

            int total = filtered.Count();

            var summary = filtered.GroupBy(t => new { t.PlotNumber, t.Name})
             .Select(g => new SummaryResultTreeSpecies
             {
                 PlotNumber = g.Key.PlotNumber,
                 Species = g.Key.Name,
                 Count = g.Count(),
                 Percentage = Math.Round(100 * (double)g.Count() / total, 1)

             })
             .OrderBy(x => x.PlotNumber)
             .ThenBy(x => x.Species)
             .ToList();

            return new ObservableCollection<SummaryResultTreeSpecies>(summary);

        }

        private ObservableCollection<SummaryVegetationResult> GenerateVegetationSummary(IEnumerable<Plot> plots)
        {
            ObservableCollection<SummaryVegetationResult> vegSummary = new ObservableCollection<SummaryVegetationResult>();
            int totalCount = plots.Count();

            // Group plots by Veg object
            var groupedByVeg = plots
                .GroupBy(p => p.Vegetation?.ShortCode);

            foreach (var group in groupedByVeg)
            {
                var count = group.Count();
                vegSummary.Add(new SummaryVegetationResult
                {
                    VegCode = group.Key,
                    Count = count,
                    Percentage = Math.Round(100.0 * count / totalCount, 1)
                });
            }

            return vegSummary;
        }

        // Method to get the max tree number
        private int GetMaxTreeNumber()
        {
            return CurrentPlot.PlotTreeLive.Count > 0
                ? CurrentPlot.PlotTreeLive.Max(t => t.TreeNumber)
                : 0;
        }

        public Plot GetOrCreatePlot(Stand stand)
        {

            if (CurrentPlot is not null)
            {
                return CurrentPlot;
            }
            Plot plot = null;

            // Get first one
            if (stand.Plots?.Count > 0)
            {
                plot = stand.Plots?[0];
            }

            // Otherwise create one
            else
            {
                plot = CreateNewPlot(stand);
            }




            SetCurrentPlot(plot);
            return plot;

        }

        public async Task<Stand> GetOrCreateStandAsync()
        {
            AppLogger.Log("GetOrCreateStandAsync", "MainViewModel");
            if (CurrentStand is not null) { return CurrentStand; }

            try
            {
                Stand _stand;

                // Await the async GetAll
                var stands = await _standRepository.GetAllAsync();
                AllStands = new ObservableCollection<Stand>(stands);

                if (AllStands.Count > 0)
                {
                    _stand = AllStands[0];
                }
                else
                {
                    _stand = CreateNewStand();
                }

                SetCurrentStand(_stand);
                return _stand;
            }
            catch (Exception ex)
            {
                AppLogger.Log($"GetOrCreateStandAsync Failed: {ex}", "MainViewModel");
                throw;
            }
        }


        public async Task LoadLookupTables()
        {
            LookupTreeSpecies = await _standRepository.GetTreeSpeciesAsync();
            Treatments = await _standRepository.GetTreatmentsAsync();

            LookupSoils = await _lookupRepository.GetSoilLookupsAsync();
            LookupEcosites = await _lookupRepository.GetEcositeLookups();
            LookupEcositeSoilVeg = await _lookupRepository.GetEcositeSoilVegLookups();
            LookupVeg = await _lookupRepository.GetVegLookups();

            TreeLookupFilteredList = new ObservableCollection<TreeSpecies>() { };

            ListPercentage = new List<int> { 0, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };
            ListExposure = new List<string?> { null, "N", "NE", "E", "SE", "S", "SW", "W", "NW" };
        }


            
        private void OnCurrentPlotChanged(Plot plot)
        {

            if (plot is null) return;




            // Populate UI Dropdowns whenever the current plot changes

        }


        private void RefreshPlotWasTreated(ObservableCollection<PlotTreatment> plotTreatments)
        {
            //Ensure correct GUI Checkbox of Plot Was Treated
            if (plotTreatments.Any(pt => pt.IsActive))
            {
                PlotWasTreated = true;
            }
            else
            {
                PlotWasTreated = false;
            }
        }




private async void OnIsCheckedBiodiversityChanged()
        {
            // only prompty user if some trees actually have Biodiversity Data associated
            if (!IsCheckedBiodiversity && CurrentPlot.PlotTreeLive.Any(t => t.Cavity || t.Diversity))
            {
                bool confirm = await Application.Current.MainPage.DisplayAlert(
                      "Clear Biodiversity data?",
                      "Unchecking Biodiversity will clear the fields 'Diversity', 'Cavity' and 'Mast' for all trees. Continue?",
                      "OK",
                      "Cancel");

                if (confirm)
                {
                    foreach (TreeLive tree in CurrentPlot.PlotTreeLive)
                    {
                        tree.Cavity = false;
          tree.Diversity = false;
                        tree.Mast = false;
                    }
                }
            }

        }

        private void OnPlotWasTreatedChanged()
        {
            if (PlotWasTreated == false)
            {
                foreach (PlotTreatment t in UiPlotTreatments)
                {
                    t.IsActive = false;
                }
            }
        }




        public async void NewFile()
        {

            bool isYes = await Application.Current.MainPage.DisplayAlert(
                 "New File",
                 "Create new file? Any unsaved changes will be lost.",
                 "New File",
                 "Cancel");

            if (!isYes)
            {
                return;
            }

            CreateNewWorkingFile(isNewFile: true);

        }



        private void SetSaveFilePath(string? saveFilePath)
        {
            AppLogger.Log($"Setting SAVE path {saveFilePath}", "SetSaveFilePath");

            _databaseService.SetSaveFilePath(saveFilePath);
            OnPropertyChanged(nameof(SaveFilePath)); // GUIW

        }

        public async Task ResetBindingsForNewDatabase()
        {
            AllStands?.Clear();
            
            CurrentPlot = null;
            CurrentStand = null;

            // Need to reload lookup tables, because switching databases breaks references to the lookups
            // Selected values, and their corresponding lookups, must come from the same database
            await LoadLookupTables();

           
        }

        private async void OpenFile()
        {
            bool changesExist = _databaseService.GetContext().ChangeTracker.HasChanges();

            var currentPage = AppShell.Current.CurrentItem?.CurrentItem.Title;

            if (changesExist)
            {
                bool isYes = await Application.Current.MainPage.DisplayAlert(
                "Open File",
                "Open file? Any unsaved changes will be lost.",
                "Open",
                "Cancel");
                if (!isYes)
                {
                    return; // leave early
                }
            }
            var result = await FilePicker.Default.PickAsync();
            if (result is null)
            {
                return;
            }

            CreateNewWorkingFile(isNewFile:false, result.FullPath);

            // have to get stand here, and not just in the "OnAppearing" of Stand.xaml.cs, since 
            // it is posisble to Open a PTA file from elsewhere in the application
            GetOrCreateStandAsync();


        }


        public void CleanupOldWorkingFiles()
        {
            string directoryPath = FileSystem.AppDataDirectory;
            try
            {
                if (!Directory.Exists(directoryPath))
                    return;

                AppLogger.Log($"Clearing old working files", "Cleanup");

                var today = DateTime.Today;

                var files = Directory.GetFiles(directoryPath, "working_*.pta*", SearchOption.TopDirectoryOnly);

                foreach (var file in files)
                {
                    try
                    {
                        var creationTime = File.GetCreationTime(file);

                        // Delete only if created before today (clean old files only)
                       if (creationTime.Date < today)
                        {
                            File.Delete(file);
                        }
                    }
                    catch (Exception ex)
                    {
                        AppLogger.Log($"Failed to delete {file}: {ex.Message}", "Cleanup");
                    }
                }
            }
            catch (Exception ex)
            {
                AppLogger.Log($"Cleanup error: {ex.Message}", "Cleanup");
            }
        }


        private void CreateNewWorkingFile(bool isNewFile, string? sourceFile = null)
        {
            //the app always saves to a temp file in LocalCache, not directly to the save location 
            // for a NEW file, we need to create this database from the template (so that the lookups are pre-populated)
            // for EXISTING file (OPEN file), we need to create this from the original Saved file

            CleanupOldWorkingFiles();

            SetSaveFilePath(sourceFile);

            // NEW FILE
            if (isNewFile) {
                AppLogger.Log($"Creating new file from template", "CreateNewWorkingFile");

                var templatePath = Path.Combine(FileSystem.AppDataDirectory, "template.pta");
                _databaseService.CreateNewDatabase(templatePath);

            }
            else
            {
                AppLogger.Log($"Creating new working file from existing: {sourceFile}", "CreateNewWorkingFile");

                // EXISTING FILE
                var newWorkingFile = Path.Combine(FileSystem.AppDataDirectory, $"working_{Guid.NewGuid()}.pta");

                File.Copy(sourceFile, newWorkingFile);
                _databaseService.SetDatabasePath(newWorkingFile);
                _databaseService.DbIsNew = false;
                _databaseService.ResetContext();
            }

            ResetBindingsForNewDatabase();
            
        }

            private async void PromptDeleteStand(Stand stand)
        {
            if (stand is not null)
            {
                bool isYes = await Application.Current.MainPage.DisplayAlert(
                     "Confirm",
                     "Are you sure you want to delete this stand? All related data (all plots, plot history, tree measurements, etc.), will be deleted. " +
                     "This cannot be undone. Are you sure you want to continue?",
                     "Delete Stand",
                     "Cancel"
                 );

                if (isYes)
                {
                    // User clicked Yes
                    DeleteStand(stand);

                    // if last stand, create a new stand
                    if (AllStands.Count == 0)
                    {
                        Application.Current.MainPage.DisplayAlert(
                                "Notice",
                                "No more stands. Creating a new one.",
                                "OK"
                            );
                        CreateNewStand();
                    }

                }
                else
                {
                    // Do nothing
                }
                GetOrCreatePlot(CurrentStand);

            }
        }


        private async void RemoveTrees(int currentTreeCount)
        {
            int treesToSubtract = currentTreeCount - CurrentPlot.TreeCount;
            string message = string.Empty;
            int firstRemoved = CurrentPlot.PlotTreeLive[CurrentPlot.PlotTreeLive.Count - treesToSubtract].TreeNumber; // First element to be removed
            int lastRemoved = CurrentPlot.PlotTreeLive[CurrentPlot.PlotTreeLive.Count - 1].TreeNumber; // Last element to be removed

            if (firstRemoved == lastRemoved)
            {
                message = $"Do you want to set the tree count to {CurrentPlot.TreeCount}?" +
                    $" Tree {firstRemoved} will be removed. This cannot be undone.";
            }
            else
            {
                message = $"Do you want to set the tree count to {CurrentPlot.TreeCount}?" +
                   $" Trees {firstRemoved} - {lastRemoved} will be removed. This cannot be undone.";
            }

            // Call the async DisplayAlert method
            bool answer = await DisplayRemoveTreesConfirmation(message);

            if (answer)
            {
                // Remove trees
                for (int i = 0; i < treesToSubtract; i++)
                {
                    CurrentPlot.PlotTreeLive.RemoveAt(CurrentPlot.PlotTreeLive.Count - 1);
                }
            }
            else
            {
                // Reset the count if user clicked "Cancel"
                CurrentPlot.TreeCount = CurrentPlot.PlotTreeLive.Count;
            }
        }

        private void ClearSingleEmptyTree(Stand stand)
        {// for convenience, we automatically add an empty tree.
            // but, if it is not filled in, we don't want to save it.
            if (stand.Plots.Count == 0 || stand.Plots is null) { return; }

            foreach(Plot plot in stand.Plots)
            {
                if (plot.PlotTreeLive.Count() == 1)
                {
                    var tree = plot.PlotTreeLive.FirstOrDefault();
                    if (tree.TreeSpecies is null)
                    {
                        plot.PlotTreeLive.Remove(tree);
                        _databaseService.GetContext().Set<TreeLive>().Remove(tree);

                    }
                }
            }

        }


        public void PopulatePlotTreatmentsFromUI(Plot plot)
        {
            if ((plot is null) || (UiPlotTreatments is null))
            {
                return;
            }

            // Populate the Plot's PlotTreatments with the checkboxes in the UI
            plot.PlotTreatments = new ObservableCollection<PlotTreatment>(
                    UiPlotTreatments
                        .Where(t => t.IsActive)
                        .Select(t => new PlotTreatment
                        {
                            TreatmentId = t.TreatmentId,
                            IsActive = true
                        })
                );
        }

        public void PopulatePlotFromUi(Plot plot)
        {
            if (plot is null) { return; }
            // Nullable to allow field to be empty in GUI, but need to save as a zero because of database requirements
            plot.Easting = CurrentPlot.Easting;
            plot.Northing = CurrentPlot.Northing;

            plot.SoilCode = CurrentSoil?.ShortCode;
            plot.VegCode = CurrentVeg?.ShortCode;
            plot.AgeTreeSpeciesCode = CurrentAgeTreeSpecies?.ShortCode;
            plot.OGTreeSpeciesCode = CurrentOGTreeSpecies?.ShortCode;

            plot.EcositeGroup = CurrentEcositeGroup;
            plot.EcositeCode = CurrentEcosite?.ShortCode;

            PopulatePlotTreatmentsFromUI(plot);
        }






        private async Task SaveAllAsync()
        {
            AppLogger.Log("SaveAllAsync", "MainViewModel");
            // Cleanup some properties before save
            // todo refactor
            if (CurrentStand != null)
            {
                ClearSingleEmptyTree(CurrentStand);
            }

            RefreshErrors();
                

            if (!(AllStandAllsErrors == string.Empty))
            {
                Application.Current.MainPage.DisplayAlert("Error", "Please address errors before saving.", "OK");   

                return; // cancel save
            }

            var plots = AllStands.SelectMany(s => s.Plots);
            var trees = AllStands.SelectMany(s => s.Plots).SelectMany(p => p.PlotTreeLive);

            _standRepository.Save(CurrentStand); // always save to the current Database Context (working database file)
            await SaveFileAsAsync();  // copy the working file to the save location 
            OnPropertyChanged(nameof(SaveFilePath));


        }

        public async Task SaveFileAsAsync()
        {

            // to maintain cleaner database connections, we don't actually maintain a connection 
            // to the save location... we always save in the cache location, and then copy that file to the Save Directory
            // to the user, it looks like we just save to their directory

            if (_databaseService.DbIsNew)
            {

                await SaveAsAsync();

            }

            else
            {
                Save();
            }


        }



        private async Task SaveAsAsync()
        {
            FileSaverResult result;


            // Copy working DB to a temp location so it's safe to read
            var tempPath = Path.Combine(FileSystem.AppDataDirectory, "temp.pta");
            File.Copy(_databaseService.WorkingDBPath, tempPath, overwrite: true);

                using var stream = File.OpenRead(tempPath);
            try
            {
                // Save dialog – user may cancel
                result = await FileSaver.Default.SaveAsync(
                    "PTAFile.pta",   // fileName
                    stream,          // stream
                    default          // cancellation token
                );
            }
            catch (CommunityToolkit.Maui.Storage.FileSaveException ex)
            {
                // Normal user cancel → no UI error needed
                AppLogger.Log("User cancelled save");
                return;
            }
            catch (Exception ex)
            {
                // Actual unexpected error
                AppLogger.Log($"Unexpected save error: {ex}");
                return;
            }

            // If user cancelled, result will be null or unsuccessful
            if (result == null || !result.IsSuccessful)
            {
                AppLogger.Log("Save cancelled (no exception thrown)");
                return;
            }

            // ✔ Success — copy the working DB to the chosen location
            File.Copy(
                _databaseService.WorkingDBPath,
                result.FilePath,
                overwrite: true
            );

            AppLogger.Log($"Saved to: {result.FilePath}");

            // Update your application state
            _databaseService.SetSaveFilePath(result.FilePath);
            OnPropertyChanged(nameof(SaveFilePath));

            _databaseService.DbIsNew = false;
        }

        private void Save()
        {
            try
            {
                File.Copy(_databaseService.WorkingDBPath, _databaseService.SaveFilePath, overwrite: true);
                AppLogger.Log($"Saved to: {_databaseService.SaveFilePath}");
                _databaseService.DbIsNew = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving file: {ex.Message}");
            }
        }

        private Soil? _currentSoil;
        public Soil? CurrentSoil
        {
            get => _currentSoil;
            set
            {
                if (_currentSoil != value)
                {
                    _currentSoil = value;
                    OnPropertyChanged();
                    OnSoilChanged(value);
                    RefreshEcosite(CurrentSoil, CurrentVeg, CurrentEcositeGroup.ToString());
                }
            }
        }

        private void OnSoilChanged(Soil soil)
        {
            if (soil is null || CurrentPlot is null)
            {
                return;
            }
            CurrentPlot.Soil= soil;
            CurrentPlot.SoilCode = soil.ShortCode;
        }




       

        private Vegetation? _currentVeg;
        public Vegetation? CurrentVeg
        {
            get => _currentVeg;
            set
            {
                if (_currentVeg != value)
                {
                    _currentVeg = value;
                    OnPropertyChanged();
                    OnVegChanged(value);
                    RefreshEcosite(CurrentSoil, CurrentVeg, CurrentEcositeGroup.ToString());

                }
            }
        }

        private void OnVegChanged(Vegetation veg)
        {
            if (veg is null || CurrentPlot is null)
            {
                return;
            }
            CurrentPlot.Vegetation = veg;
            CurrentPlot.VegCode = veg.ShortCode;
        }

        private EcositeGroup _currentEcositeGroup;
        public EcositeGroup CurrentEcositeGroup
        {
            get => _currentEcositeGroup;
            set
            {
                if (_currentEcositeGroup != value)
                {
                    _currentEcositeGroup = value;
                    OnPropertyChanged();
                    OnCurrentEcositeGroupChanged(value);
                    RefreshEcosite(CurrentSoil, CurrentVeg, CurrentEcositeGroup.ToString());

                }
            }

        }

        private void OnCurrentEcositeGroupChanged(EcositeGroup ecositeGroup)
        {
            if (CurrentPlot is null) { return; }
            CurrentPlot.EcositeGroup = ecositeGroup;
        }

        private Ecosite? _currentEcosite;
        public Ecosite? CurrentEcosite
        {
            get => _currentEcosite;
            set
            {
                if (_currentEcosite != value)
                {
                    _currentEcosite = value;
                    OnCurrentEcositeChanged(value);
                    OnPropertyChanged();
                }
            }
        }
        private void OnCurrentEcositeChanged(Ecosite ecosite)
        {
            if (CurrentPlot is null || ecosite is null) { return; }
            CurrentPlot.Ecosite = ecosite;
            CurrentPlot.EcositeCode = ecosite.ShortCode;
        }
        private TreeSpecies? _currentAgeTreeSpecies;
        public TreeSpecies? CurrentAgeTreeSpecies
        {
            get => _currentAgeTreeSpecies;
            set
            {
                if (_currentAgeTreeSpecies != value)
                {
                    _currentAgeTreeSpecies = value;
                    OnAgeTreeSpeciesChanged(value);
                    OnPropertyChanged();
                }
            }
        }

        private void OnAgeTreeSpeciesChanged(TreeSpecies species)
        {
            if (species is null || CurrentPlot is null)
            {
                return;
            }

            CurrentPlot.AgeTreeSpeciesCode = species.ShortCode;
        }

                private TreeSpecies? _currentOGTreeSpecies;
        public TreeSpecies? CurrentOGTreeSpecies
        {
            get => _currentOGTreeSpecies;
            set
            {
                if (_currentOGTreeSpecies != value)
                {
                    _currentOGTreeSpecies = value;
                    OnCurrentOGTreeSpeciesChanged(value);
                    OnPropertyChanged();
                }
            }
        }

        private void OnCurrentOGTreeSpeciesChanged(TreeSpecies species)
        {
            if (species is null ||  CurrentPlot is null)
            {
                return;
            }

            CurrentPlot.OGTreeSpeciesCode = species.ShortCode;

        }
        private void SetCurrentPlot(Plot plot)
        {
            CurrentPlot = plot;


        }

        private void SetCurrentStand(Stand stand)
        {
            CurrentStand = stand;

        }

        private void SetSummaryStandOnly()
        {
            StandOnlySummary = true;
            SummaryPlot = null;
            SummarySectionIsVisible = true;



            foreach (Plot plot in CurrentStand.Plots)
            {
                if (!TreeSummaryHelper.CheckTreesValid(plot.PlotTreeLive))
                {
                    DisplayTreeWarningMessage(plot);
                }

            }
            SummaryItems = TreeSummaryHelper.GenerateSummaryResult(CurrentStand.Plots);
            SpeciesSummary = GenerateTreeSpeciesSummary(CurrentStand.Plots);
            TreatmentSummary = GenerateTreatmentSummary(CurrentStand.Plots);
            SoilSummary = GenerateSoilSummary(CurrentStand.Plots);
            VegetationSummary = GenerateVegetationSummary(CurrentStand.Plots);
            SummaryPageMessage = $"Stand {CurrentStand.StandNumber} Summary";

        }

        public void SetSummaryPlot(Plot plot)
        {
            if (plot is not null)
            {
                SummaryPlot = plot;
                StandOnlySummary = false;
                SummarySectionIsVisible = true;


                if (!TreeSummaryHelper.CheckTreesValid(plot.PlotTreeLive))
                {
                    DisplayTreeWarningMessage(plot);
                }

                SummaryItems = TreeSummaryHelper.GenerateSummaryResult(new[] { plot });
                SpeciesSummary = GenerateTreeSpeciesSummary(new[] { plot });
                SummaryPageMessage = $"Plot {CurrentPlot.PlotNumber} Summary";
                TreatmentSummary = GenerateTreatmentSummary(new[] { plot });
                VegetationSummary = GenerateVegetationSummary(new[] { plot });
                SoilSummary = GenerateSoilSummary(new[] { plot });

            }

        }

        private void SpecifyTreeCountInPlot()
        {
            int currentTreeCount = CurrentPlot.PlotTreeLive.Count;

            // Add Trees if necessary
            if (CurrentPlot.TreeCount > currentTreeCount)
            {
                int treesToAdd = CurrentPlot.TreeCount - currentTreeCount;

                AddTrees(treesToAdd);
            }
            // Remove Trees if necessary
            else if (CurrentPlot.TreeCount < currentTreeCount)
            {
                RemoveTrees(currentTreeCount);
            }
        }

        private void Stand_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // Check if the changed property is StandNumber
            if (e.PropertyName == nameof(Stand.StandNumber))
            {
                // Handle the update accordingly
                Console.WriteLine($"StandNumber changed: {(sender as Stand)?.StandNumber}");
                RefreshAllStands();
            }
        }

    }
}