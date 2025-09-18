//using static
//.Renderscripts.Script;
using ClosedXML.Excel;
using CommunityToolkit.Maui.Core.Primitives;
using CommunityToolkit.Maui.Storage;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using PTANonCrown.Data.Models;
using PTANonCrown.Data.Repository;
using PTANonCrown.Services;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Windows.Input;

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
        private ObservableCollection<SummarySoilResult> _soilSummary;
        private ObservableCollection<SummaryResultTreeSpecies> _speciesSummary;
        private ObservableCollection<SummaryItem> _summaryItems;
        private string _summaryPageMessage;
        private Plot _summaryPlot;
        private bool _summarySectionIsVisible;
        private ObservableCollection<SummaryTreatmentResult> _treatmentSummary;
        private string _validationMessage;

        private ObservableCollection<SummaryVegetationResult> _vegetationSummary;

        private bool ContainsError = false;

        public MainViewModel(MainService mainService, StandRepository standRepository, LookupRepository lookupRepository)
        {
            AppLogger.Log("Init", "MainViewModel");

            _mainService = mainService;
            _standRepository = standRepository;
            _lookupRepository = lookupRepository;

            AppLogger.Log("LoadLookupTables", "MainViewModel");

            LoadLookupTables();
            AppLogger.Log("GetOrCreateStand", "MainViewModel");

            GetOrCreateStand();

            AppLogger.Log("GetOrCreatePlot", "MainViewModel");
            GetOrCreatePlot(CurrentStand);
            ValidationMessage = string.Empty;
            RefreshTreeCount();

            // If summaryItems changes, notify the Bool that is used to show/hide the section
            SummaryItems = new ObservableCollection<SummaryItem>();
            SummaryItems.CollectionChanged += OnSummaryItemsChanged;

            TreatmentSummary = new ObservableCollection<SummaryTreatmentResult>();
            TreatmentSummary.CollectionChanged += OnTreatmentSummaryChanged;

        }

        public ObservableCollection<Plot> _allPlots { get; set; }
        public ObservableCollection<Stand> _allStands { get; set; }

        public ObservableCollection<Plot> AllPlots
        {
            get => _allPlots;
            set
            {
                if (_allPlots != value)
                {
                    _allPlots = value;
                    OnPropertyChanged();
                }
            }

        }

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
                if (value is not null)
                {
                    // Unsubscribe from the old collection's property change notifications
                    if (_currentPlot != null)
                    {

                        _currentPlot.PropertyChanged -= CurrentPlot_PropertyChanged;

                    }

                    _currentPlot = value;
                    OnPropertyChanged();
                    OnCurrentPlotChanged();
                    // Subscribe to the new collection's property change notifications
                    if (_currentPlot != null)
                    {
                        _currentPlot.PropertyChanged += CurrentPlot_PropertyChanged;
                    }

                }
            }
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
                }

                _currentStand = value;
                OnPropertyChanged();
                OnCurrentStandChanged();

                if (_currentStand != null)
                {
                    // Subscribe to new CurrentStand's property changes
                    _currentStand.PropertyChanged += Stand_PropertyChanged;
                }

                ValidateStand(); // Run validation in case a new object is assigned
            }

        }

        public ICommand DeleteLiveTreeCommand => new Command<TreeLive>(tree => DeleteLiveTree(tree));
        public ICommand DeletePlotCommand => new Command<Plot>(plot => DeletePlot(plot));

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                if (_errorMessage != value)
                {
                    _errorMessage = value;
                    OnPropertyChanged();
                }
            }
        }

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

        public List<EcodistrictLookup> LookupEcodistricts { get; set; }

        public List<ExposureLookup> LookupExposure { get; set; }

        public List<SoilLookup> LookupSoils { get; set; }

        public List<TreeSpecies> LookupTrees { get; set; }

        public List<VegLookup> LookupVeg { get; set; }

        public ICommand NewPlotCommand =>
            new Command<string>(method => CreateNewPlot(CurrentStand));

        public ICommand PickFolderCommand => new Command(async () => await ExecutePickFolderCommand());

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
        new Command<string>(method => SaveAll());

        // TODO - in progress - trying to get Selected Age Species working
        public TreeSpecies SelectedAgeTreeSpecies
        {
            get
            {
                var spec = LookupTrees.FirstOrDefault(t => t.ID == CurrentPlot.AgeTreeSpecies?.ID);

                return spec;
            }
            set
            {
                if (value != null && CurrentPlot.AgeTreeSpecies?.ID != value.ID)
                {
                    CurrentPlot.AgeTreeSpecies = value;
                    OnPropertyChanged(nameof(SelectedAgeTreeSpecies));
                }
            }
        }
        public TreeSpecies SelectedOldGrowthSpecies
        {
            get
            {
                var spec = LookupTrees.FirstOrDefault(t => t.ID == CurrentPlot.OldGrowthSpecies?.ID);

                return spec;
            }
            set
            {
                if (value != null && CurrentPlot.OldGrowthSpecies?.ID != value.ID)
                {
                    CurrentPlot.OldGrowthSpecies = value;
                    OnPropertyChanged(nameof(SelectedOldGrowthSpecies));
                }
            }
        }

        private SoilLookup _selectedSoil;
        public SoilLookup SelectedSoil
        {
            get => _selectedSoil;
            set
            {
                if (_selectedSoil != value)
                {
                    _selectedSoil = value;
                    OnPropertyChanged();

                    // 1️⃣ update Plot's soil
                    CurrentPlot.Soil = _selectedSoil;

                    // 2️⃣ update dependent things, e.g., phases
                    UpdateSoilPhases();
                }
            }
        }


        public ICommand SetCurrentPlotCommand => new Command<Plot>(plot => SetCurrentPlot(plot));

        public ICommand SetPlotSummaryCommand => new Command<Plot>(plot => SetSummaryPlot(plot));

        public ICommand SetStandOnlyCommand => new Command<Plot>(plot => SetStandOnly());

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
                    if (_summaryItems != null)
                        _summaryItems.CollectionChanged -= OnSummaryItemsChanged;

                    _summaryItems = value;

                    if (_summaryItems != null)
                        _summaryItems.CollectionChanged += OnSummaryItemsChanged;

                    OnPropertyChanged();
                    OnPropertyChanged(nameof(HasSummaryItems));
                    OnPropertyChanged(nameof(HasNoSummaryItems));
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
                if (_treatmentSummary != null)
                    _treatmentSummary.CollectionChanged -= OnTreatmentSummaryChanged;

                _treatmentSummary = value;

                if (_treatmentSummary != null)
                    _treatmentSummary.CollectionChanged += OnTreatmentSummaryChanged;

                OnPropertyChanged(nameof(TreatmentSummary));
                OnPropertyChanged(nameof(HasTreatments));
                OnPropertyChanged(nameof(HasNoTreatments));
            }
        }

        public ObservableCollection<TreeSpecies> TreeLookupFilteredList { get; set; }

        public ICommand UsePredictedHeightsCommand => new Command(_ => UsePredictedHeights());

        public string ValidationMessage
        {
            get => _validationMessage;
            set => SetProperty(ref _validationMessage, value);
        }

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

        private LookupRepository _lookupRepository { get; set; }

        private MainService _mainService { get; set; }

        private StandRepository _standRepository { get; set; }

        public void AddNewTreeToPlot(Plot plot, int treeNumber)
        {

            plot.PlotTreeLive.Add(new TreeLive()
            {
                TreeNumber = treeNumber,
                LookupTrees = LookupTrees
            });

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

            RefreshTreeCount();

        }

        public void OnCurrentStandChanged()
        {
            AllPlots = CurrentStand?.Plots;
            if ((AllPlots != null) & AllPlots?.Count() != 0)
            {
                CurrentPlot = AllPlots.OrderBy(p => p.PlotNumber).FirstOrDefault();

            }
        }

        public void RefreshAllPlots()
        {
            var tempCurrentPlot = CurrentPlot;

            var tempList = AllPlots.ToList();
            AllPlots.Clear();
            foreach (var plot in tempList)
            {
                AllPlots.Add(plot);
            }

            // Now reassign CurrentPlot to the matching instance from AllPlots
            if (tempCurrentPlot != null)
            {
                CurrentPlot = AllPlots.FirstOrDefault(p => p.PlotNumber == tempCurrentPlot.PlotNumber);
            }
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

        public void ValidateStand()
        {

            // StandNumber
            if (CurrentStand?.StandNumber != 2000)
            {
                ValidationMessage = "Wrong stand!";
                IsValidationError = true;
            }
            else
            {
                ValidationMessage = string.Empty;
                IsValidationError = false;
            }

        }

        private void AddTrees(int currentTreeCount)
        {
            int treesToAdd = CurrentPlot.TreeCount - currentTreeCount;

            // Get max tree number
            int currentMaxTreeNumber = GetMaxTreeNumber();

            // Add the trees
            for (int i = 0; i < treesToAdd; i++)
            {
                AddNewTreeToPlot(CurrentPlot, currentMaxTreeNumber + 1);
                currentMaxTreeNumber++;
            }
        }

        private async void CheckTrees(Plot plot)
        {
            if (!plot.PlotTreeLive.Any(t => t.DBH_cm == 0 || t.Height_m == 0))
            {
                await Application.Current.MainPage.DisplayAlert(
               "Missing info.",
               $"Trees in Plot {plot.PlotNumber} missing DBH and/or Heights",
               "Continue",
               "Cancel");
            }
        }

        private int CountDigits(int number)
        {
            int result = number.ToString().TrimStart('-').Length;
            return result;
        }

        private ObservableCollection<CoarseWoody> CreateDefaultCoarseWoody(Plot parentPlot)
        {
            return new ObservableCollection<CoarseWoody>
    {
        new CoarseWoody { Plot = parentPlot, DBH_start = 21, DBH_end = 30 },
        new CoarseWoody { Plot = parentPlot, DBH_start = 31, DBH_end = 40 },
        new CoarseWoody { Plot = parentPlot, DBH_start = 41, DBH_end = 50 },
        new CoarseWoody { Plot = parentPlot, DBH_start = 51, DBH_end = 60 },
        new CoarseWoody { Plot = parentPlot, DBH_start = 60, DBH_end = -1 }
    };
        }

        private ObservableCollection<TreeDead> CreateDefaultTreeDead(Plot parentPlot)
        {
            return new ObservableCollection<TreeDead>
            {
                new TreeDead() { Plot = parentPlot, DBH_start = 21, DBH_end = 30 },
                new TreeDead() { Plot = parentPlot, DBH_start = 31, DBH_end = 40 },
                new TreeDead() { Plot = parentPlot, DBH_start = 41, DBH_end = 50 },
                new TreeDead() { Plot = parentPlot, DBH_start = 51, DBH_end = 60 },
                new TreeDead() { Plot = parentPlot, DBH_start = 60, DBH_end = -1 }
            };

        }

        private Plot CreateNewPlot(Stand stand)
        {

            int newPlotNumber = (stand.Plots != null && stand.Plots.Any())
                ? stand.Plots.Max(p => p.PlotNumber) + 1
                : 1;

            var _newPlot = new Plot
            {
                PlotNumber = newPlotNumber,
                EcodistrictLookup = LookupEcodistricts.Where(x => x.ID == 1).FirstOrDefault(),
                Soil = LookupSoils.Where(x => x.ID == 1).FirstOrDefault(),
                Exposure = LookupExposure.Where(x => x.ID == 1).FirstOrDefault(),
                Vegetation = LookupVeg.Where(x => x.ID == 1).FirstOrDefault(),
                EcositeGroup = EcositeGroup.None,
                AgeTreeSpecies = LookupTrees.Where(x => x.ID == 1).FirstOrDefault(),
                OldGrowthSpecies = LookupTrees.Where(x => x.ID == 1).FirstOrDefault(),

                PlotTreatments = Treatments.Select(t => new PlotTreatment
                {
                    Treatment = t, // Associate the full treatment entity
                    IsActive = false // Default status
                }).ToList()
            };

            _newPlot.PlotCoarseWoody = CreateDefaultCoarseWoody(_newPlot);
            _newPlot.PlotTreeDead = CreateDefaultTreeDead(_newPlot);

            _newPlot.Stand = stand;

            _newPlot.PlotTreeLive = new ObservableCollection<TreeLive>();

            stand.Plots.Add(_newPlot);

            SetCurrentPlot(_newPlot);
            return _newPlot;
        }

        private Stand CreateNewStand()
        {
            // get new stand number
            int newStandNumber = AllStands?.Count() > 0 ? AllStands.Max(s => s.StandNumber) + 1 : 1;

            Stand _stand = new Stand()
            {
                StandNumber = newStandNumber
            };

            CreateNewPlot(_stand);
            AllStands.Add(_stand);
            SetCurrentStand(_stand);
            return _stand;

        }

        private void CurrentPlot_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // Check if the changed property is PlotNumber
            if (e.PropertyName == nameof(Plot.PlotNumber))
            {
                Console.WriteLine($"PlotNumber changed: {(sender as Plot)?.PlotNumber}");
                RefreshAllPlots();

            }
        }

        private void DeletePlot(Plot plot)
        {
            if (plot is not null)
            {
                CurrentStand.Plots.Remove(plot);

                GetOrCreatePlot(CurrentStand);
                SaveAll();
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

        private async Task ExecutePickFolderCommand()
        {
            try
            {

                // Pick a folder from the file system
                var selectedFolder = await FolderPicker.PickAsync(default);

                if (selectedFolder != null)
                {
                    // Extract folder information
                    string folderPath = $"Folder Name: {selectedFolder.Folder}";

                    ExportSummary(folder: selectedFolder.Folder);
                    ExportAllTrees(folder: selectedFolder.Folder);
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

        private void ExportAllTrees(Folder folder)
        {
            IEnumerable<TreeLive> trees = null;
            string tabName = null;

            // Create a new Excel workbook
            var workbook = new XLWorkbook();
            string exportFileName = System.IO.Path.Combine(folder.Path, $"Trees_{CurrentStand.StandNumber}.xlsx");

            // Export Stand
            trees = CurrentStand.Plots.SelectMany(p => p.PlotTreeLive);
            tabName = "Stand " + CurrentStand.StandNumber.ToString();

            ExportToExcel(workbook, tabName, trees, exportFileName);

            //Export all Plots
            foreach (Plot plot in CurrentStand.Plots)
            {
                trees = plot.PlotTreeLive;
                tabName = "Plot " + plot.PlotNumber.ToString();
                ExportToExcel(workbook, tabName, trees, exportFileName);
            }

            ExportSuccessMessage(exportFileName);
        }

        private void ExportSuccessMessage(string destination)
        {
            // Inform the user
            Application.Current.MainPage.DisplayAlert("Export Successful", $"Summary has been exported to {destination}.", "OK");
        }

        private void ExportSummary(Folder folder)
        {

            SpeciesSummary = GenerateTreeSpeciesSummary(CurrentStand.Plots);
            ObservableCollection<SummaryItem> summaryResult = null;
            IEnumerable<TreeLive> trees = null;
            string tabName = null;

            // Create a new Excel workbook
            var workbook = new XLWorkbook();
            //DateTime now = DateTime.Now; _{ now.ToString("HHmmss")}
            string exportFileName = System.IO.Path.Combine(folder.Path, $"Summary_{CurrentStand.StandNumber}.xlsx");

            // Export Stand
            trees = CurrentStand.Plots.SelectMany(p => p.PlotTreeLive);
            summaryResult = TreeSummaryHelper.GenerateSummaryResult(plots: CurrentStand.Plots);
            tabName = "Stand " + CurrentStand.StandNumber.ToString();
            ExportToExcel(workbook, tabName, summaryResult, exportFileName);

            //Export all Plots
            foreach (Plot plot in CurrentStand.Plots)
            {
                trees = plot.PlotTreeLive;
                summaryResult = TreeSummaryHelper.GenerateSummaryResult(new[] { plot });
                tabName = "Plot " + plot.PlotNumber.ToString();
                ExportToExcel(workbook, tabName, summaryResult, exportFileName);
            }

            ExportSuccessMessage(exportFileName);
        }

        private void ExportToExcel<T>(XLWorkbook workBook, string tabName, IEnumerable<T> items, string exportFilePath)
        {

            var itemsList = items.ToList();

            // Add a worksheet
            var worksheet = workBook.Worksheets.Add(tabName);

            // Get the properties of the generic type T (column names)
            var properties = typeof(T).GetProperties();

            // Priority list: put any property containing these keywords first
            string[] priorityKeywords = { "Plot", "Number", "ID", "Name", "Species" };

            // 1. Properties with priority keywords come first
            // 2. All others come after, in alphabetical order (or keep as-is)
            var propertiesSorted = properties
                .OrderByDescending(p => priorityKeywords.Any(keyword => p.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase)))
                .ThenBy(p => p.Name) // or keep order as-is by removing this line
                .ToArray();

            // Define the list of property names to exclude
            string[] excludeProps = { "SearchSpecies", "TreeSpeciesID", "TreeSpeciesFilteredList", "LookupTrees" };

            propertiesSorted = FilterProperties(propertiesSorted, excludeProps);
            // Add headers (column names) to the first row
            for (int col = 0; col < propertiesSorted.Length; col++)
            {
                worksheet.Cell(1, col + 1).Value = propertiesSorted[col].Name; // Column headers are property names
            }

            // Add the data (values from the IEnumerable)
            for (int row = 0; row < itemsList.Count; row++)
            {
                for (int col = 0; col < propertiesSorted.Length; col++)
                {
                    var propertyValue = propertiesSorted[col].GetValue(itemsList[row]);
                    worksheet.Cell(row + 2, col + 1).Value = propertyValue.ToString();
                }
            }

            worksheet.Columns().AdjustToContents();
            // Save the workbook to the specified file path
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
                .GroupBy(p => p.Soil);

            foreach (var group in groupedBySoil)
            {
                var count = group.Count();
                soilSummary.Add(new SummarySoilResult
                {
                    Soil = group.Key,
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
                                        .Where(pt => pt.IsActive == true)
                                        .Select(pt => pt.Treatment.Name);

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
             .SelectMany(plot => plot.PlotTreeLive.Select(tree => new { plot.PlotNumber, tree.TreeSpecies?.DisplayName }));

            ObservableCollection<SummaryResultTreeSpecies> result = new ObservableCollection<SummaryResultTreeSpecies>();

            if ((filtered is null || filtered.Count() == 0))
            {
                return result;
            }

            int total = filtered.Count();

            var summary = filtered.GroupBy(t => new { t.PlotNumber, t.DisplayName })
             .Select(g => new SummaryResultTreeSpecies
             {
                 PlotNumber = g.Key.PlotNumber,
                 Species = g.Key.DisplayName,
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

            // Group plots by Soil object
            var groupedByVeg = plots
                .GroupBy(p => p.Vegetation);

            foreach (var group in groupedByVeg)
            {
                var count = group.Count();
                vegSummary.Add(new SummaryVegetationResult
                {
                    Vegetation = group.Key,
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

        private Plot GetOrCreatePlot(Stand stand)
        {
            Plot plot = null;

            // Get first one
            if (stand.Plots?.Count > 0)
            {
                plot = stand.Plots[0];
            }

            // Otherwise create one
            else
            {
                Application.Current.MainPage.DisplayAlert(
                "Creating new plot",
                "No more plots. Creating a new Plot.",
                "Continue",
                "Cancel");
                plot = CreateNewPlot(stand);
            }

            // needs to reference the same list that used in the loook up, or it will fail
            // to display the correct item in the AgreTree picker list
            var match = LookupTrees.Where(t => t.ID == plot.AgeTreeSpecies.ID).FirstOrDefault();
            SelectedAgeTreeSpecies = match;

            SetCurrentPlot(plot);
            return plot;

        }

        private Stand GetOrCreateStand()
        {
            AppLogger.Log("GetOrCreateStand", "MainViewModel");

            try
            {
                Stand _stand;

                AllStands = new ObservableCollection<Stand>(_standRepository.GetAll());
                // Get first stand if any exist
                if (AllStands.Count > 0)
                {

                    _stand = AllStands[0];

                }
                // Otherwise create a new one (Stand #1)
                else
                {
                    _stand = CreateNewStand();
                }
                // AllPlots = _stand.Plots;
                SetCurrentStand(_stand);
                return _stand;

            }
            catch (Exception ex)
            {

                AppLogger.Log($"GetOrCreateStand Failed: {ex}", "MainViewModel");
                throw;
            }

        }

        private void LoadLookupTables()
        {
            LookupTrees = _standRepository.GetTreeSpecies();
            LookupSoils = _lookupRepository.GetSoilLookups();
            LookupEcodistricts = _lookupRepository.GetEcodistrictLookups();
            LookupVeg = _lookupRepository.GetVegLookups();
            LookupExposure = _lookupRepository.GetExposureLookups();
            Treatments = _standRepository.GetTreatments();

            TreeLookupFilteredList = new ObservableCollection<TreeSpecies>() { };

            ListPercentage = new List<int> { 0, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };

        }

        private void OnCurrentPlotChanged()
        {
            //Ensure correct GUI Checkbox of Plot Was Treated
            if (CurrentPlot.PlotTreatments.Any(pt => pt.IsActive))
            {
                PlotWasTreated = true;
            }
            else
            {
                PlotWasTreated = false;
            }

            // Refresh LIT status of trees
            CurrentPlot.UpdateTreeLIT();

            //Refres
            SetSummaryPlot(CurrentPlot);
            //Refresh the count
            RefreshTreeCount();
        }

        private async void OnIsCheckedBiodiversityChanged()
        {
            // only prompty user if some trees actually have Biodiversity Data associated
            if (!IsCheckedBiodiversity && CurrentPlot.PlotTreeLive.Any(t => t.Cavity || t.Diversity))
            {
                bool confirm = await Application.Current.MainPage.DisplayAlert(
                      "Clear Biodiversity data?",
                      "Unchecking Biodiversity will clear the fields 'Diversity' and 'Cavity' for all trees. Continue?",
                      "OK",
                      "Cancel");

                if (confirm)
                {
                    foreach (TreeLive tree in CurrentPlot.PlotTreeLive)
                    {
                        tree.Cavity = false;
                        tree.Diversity = false;
                    }
                }
            }

        }

        private void OnPlotWasTreatedChanged()
        {
            if (PlotWasTreated == false)
            {
                foreach (PlotTreatment t in CurrentPlot.PlotTreatments)
                {
                    t.IsActive = false;
                }
            }
        }

        private void OnSummaryItemsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(HasSummaryItems));
            OnPropertyChanged(nameof(HasNoSummaryItems));

        }

        private void OnTreatmentSummaryChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(HasTreatments));
            OnPropertyChanged(nameof(HasNoTreatments));

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

                    SaveAll();
                }
                else
                {
                    // Do nothing
                }
                GetOrCreatePlot(CurrentStand);

            }
        }

        private void RefreshTreeCount()
        {
            //Refresh Tree Count
            CurrentPlot.TreeCount = CurrentPlot.PlotTreeLive.Count;
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

        private void SaveAll()
        {
            ContainsError = false; // reset
            ValidateStand(CurrentStand);
            ValidatePlot(CurrentPlot);
            ValidateTrees(CurrentPlot.PlotTreeLive);
            //ValidateDeadTree(CurrentPlot.PlotTreeDead);

            // Nullable to allow field to be empty in GUI, but need to save as a zero bec
            CurrentPlot.Easting = CurrentPlot.Easting ?? 0;
            CurrentPlot.Northing = CurrentPlot.Northing ?? 0;

            if (ContainsError)
            {
                Application.Current.MainPage.DisplayAlert("Error", "Please address errors", "OK");
                return;
            }


                _standRepository.Save(CurrentStand);
           
        }

        private void SetCurrentPlot(Plot plot)
        {
            CurrentPlot = plot;
            

            

            // Need to populate each tree with the full LookupTrees to ensure dropdown bindings work
            foreach (TreeLive tree in CurrentPlot.PlotTreeLive)
            {
                tree.LookupTrees = LookupTrees;
                var match = tree.LookupTrees.Where(t => t.ShortCode == tree.TreeSpecies.ShortCode).FirstOrDefault();
                tree.TreeSpecies = match;

            }

        }

        private void SetCurrentStand(Stand stand)
        {
            CurrentStand = stand;
        }

        private void SetStandOnly()
        {
            StandOnlySummary = true;
            SummaryPlot = null;
            SummarySectionIsVisible = true;

            foreach (Plot plot in CurrentStand.Plots)
            {
                //CheckTrees(plot);

            }
            SummaryItems = TreeSummaryHelper.GenerateSummaryResult(CurrentStand.Plots);
            SpeciesSummary = GenerateTreeSpeciesSummary(CurrentStand.Plots);
            TreatmentSummary = GenerateTreatmentSummary(CurrentStand.Plots);
            SoilSummary = GenerateSoilSummary(CurrentStand.Plots);
            VegetationSummary = GenerateVegetationSummary(CurrentStand.Plots);
            SummaryPageMessage = $"Stand {CurrentStand.StandNumber} Summary";

        }

        private void SetSummaryPlot(Plot plot)
        {
            if (plot is not null)
            {
                SummaryPlot = plot;
                StandOnlySummary = false;
                SummarySectionIsVisible = true;
                //CheckTrees(plot);
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
                AddTrees(currentTreeCount);
            }
            // Remove Trees if necessary
            else if (CurrentPlot.TreeCount < currentTreeCount)
            {
                RemoveTrees(currentTreeCount);
            }
        }

        private void Stand_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ValidateStand(sender as Stand);
            // Check if the changed property is StandNumber
            if (e.PropertyName == nameof(Stand.StandNumber))
            {
                // Handle the update accordingly
                Console.WriteLine($"StandNumber changed: {(sender as Stand)?.StandNumber}");
                RefreshAllStands();
            }
        }

        private void ValidatePlot(Plot plot)
        {

            if ((plot.IsPlanted) && (plot.PlantedType == PlantedType.None))
            {
                ErrorMessage = "Plot was marked as Planted, but no Planted Type was chosen. Please select a Planted Type (e.g. Acadian)";
                ContainsError = true;
            }

            if ((PlotWasTreated) && !(CurrentPlot.PlotTreatments.Where(pt => pt.IsActive).Any()))
            {
                ErrorMessage = "Plot is marked as Treated, but no Treatment Type is selected. Please select a treatment type (e.g. pre-commercial thinning)";
                ContainsError = true;
            }

            if (plot.Easting is int easting && CountDigits(easting) != 6 && easting != 0)
            {
                ErrorMessage = "Easting should be 6 digits long.";
                ContainsError = true;
            }

            if (plot.Northing is int northing && CountDigits(northing) != 7 && northing != 0)
            {
                ErrorMessage = "Northing should be 7 digits long.";
                ContainsError = true;
            }

        }

        private void ValidateStand(Stand stand)
        {

            if ((stand.StandNumber == 0) | (stand.StandNumber.ToString() == string.Empty))
            {
                ErrorMessage = "Stand Number cannot be empty";
                ContainsError = true;
            }
            else if ((AllStands.Where(s => s.StandNumber == stand.StandNumber).Count() > 1))
            {
                ErrorMessage = $"Stand Number {stand.StandNumber} already exists. This must be unique.";
                ContainsError = true;
            }

            else if (stand.CruiseID is null || stand.PlannerID is null)
            {
                var missingFields = new List<string>();
                if (stand.CruiseID is null) missingFields.Add("CruiseID");
                if (stand.PlannerID is null) missingFields.Add("PlannerID");

                ErrorMessage = $"Stand Number {stand.StandNumber} has missing required fields: {string.Join(", ", missingFields)}";
                ContainsError = true;
            }
            else
            {
                ErrorMessage = string.Empty;
            }




        }

        private readonly Dictionary<string, List<string>> _phaseToSoilTypes = new()
        {
            { "Boulder Phase", new List<string> { "st1", "st2", "st3", "st8", "st9", "st14", "st15", "st16" } },
            { "Coarse Phase", new List<string> { "st2", "st3", "st5", "st6", "st8", "st9", "st15", "st16" } },
            { "Loamy Phase", new List<string> { "st2", "st3", "st15", "st16" } },
            { "Stony Phase", Enumerable.Range(1, 18).Select(i => $"st{i}").ToList() },
            { "Upland Phase", new List<string> { "st14"} }

        };
        public ObservableCollection<string> SoilPhases { get; } = new ObservableCollection<string>();


        private void UpdateSoilPhases()
        {
            SoilPhases.Clear();

            if (SelectedSoil is not null)
            {
                var soilCode = SelectedSoil.ShortCode;


                var phases = _phaseToSoilTypes
                .Where(kvp => kvp.Value.Contains(soilCode, StringComparer.OrdinalIgnoreCase))
                    .Select(kvp => kvp.Key);

                foreach (var phase in phases)
                    SoilPhases.Add(phase);
            }

            SelectedSoilPhase = null;
        }


        private string _selectedSoilPhase;
        public string SelectedSoilPhase
        {
            get => _selectedSoilPhase;
            set
            {
                if (_selectedSoilPhase != value)
                {
                    _selectedSoilPhase = value;

                    CurrentPlot.SoilPhase = _selectedSoilPhase;
                    OnPropertyChanged();

                }
            }
        }

        private void OnSoilChanged()
        {
            UpdateSoilPhases();
        }

        private void ValidateTrees(ObservableCollection<TreeLive> trees)
        {
            foreach (TreeLive tree in trees)
            {
                if (tree.TreeSpecies is null)
                {
                    ErrorMessage = ErrorMessage + "\n" + $"Please enter a valid Tree Species for Tree #{tree.TreeNumber}";
                    ContainsError = true;
                }
            }
        }

        /* private void ValidateDeadTree(Plot plot)
         {
             foreach(TreeDead deadTree in plot.PlotTreeDead)
             {
                 if (deadTree.Tally_Cavity)
             }
         }*/
    }
}