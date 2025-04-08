//using static Android.Renderscripts.Script;
using ClosedXML.Excel;
using PTANonCrown.Models;
using PTANonCrown.Repository;
using PTANonCrown.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace PTANonCrown.ViewModel
{
    public class MainViewModel : BaseViewModel
    {

        private Plot _currentPlot;
        private Stand _currentStand;
        private bool _isSelectedExportAll;
        private bool _isSelectedExportSelectedOnly;
        private bool _isValidationError;
        private ObservableCollection<SummaryResultTreeSpecies> _speciesSummary;
        private ObservableCollection<SummaryItem> _summaryItems;
        private Plot _summaryPlot;
        private string _summaryPageMessage;

        private string _validationMessage;

        public MainViewModel(MainService mainService, StandRepository standRepository, LookupRepository lookupRepository)
        {
            _mainService = mainService;
            _standRepository = standRepository;
            _lookupRepository = lookupRepository;

            GetOrCreateStand();
            GetOrCreatePlot(CurrentStand);
            LoadLookupTables();
            ValidationMessage = string.Empty;

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
        public void RefreshAllPlots()
        {

            // Store the existing collection in a temporary list
            var tempList = AllPlots.ToList();
            var tempCurrentPlot = CurrentPlot; //store ; this gets wiped

            // Clear the existing ObservableCollection
            AllPlots.Clear();

            // Re-add the same items from the temporary list
            foreach (var plot in tempList)
            {
                AllPlots.Add(plot);
            }

            //reset the Current stand
            SetCurrentPlot(tempCurrentPlot);

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
        public ObservableCollection<Stand> _allStands { get; set; }
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

        public ObservableCollection<Plot> _allPlots { get; set; }
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
        public Plot CurrentPlot
        {
            get => _currentPlot;
            set
            {
                if (_currentPlot != value)
                {
                    // Unsubscribe from the old collection's property change notifications
                    if (_currentPlot != null)
                    {

                        _currentPlot.PropertyChanged -= CurrentPlot_PropertyChanged;

                    }

                    _currentPlot = value;
                    OnPropertyChanged();

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
                if (_currentStand != value)
                {
                    if (_currentStand != null)
                    {
                        // Unsubscribe from old CurrentPlot's property changes
                        _currentStand.PropertyChanged -= Stand_PropertyChanged;
                    }

                    _currentStand = value;
                    OnPropertyChanged();

                    if (_currentStand != null)
                    {
                        // Subscribe to new CurrentPlot's property changes
                        _currentStand.PropertyChanged += Stand_PropertyChanged;
                    }

                    ValidateStand(); // Run validation in case a new object is assigned
                }
            }

        }

        public ICommand DeletePlotCommand => new Command<Plot>(plot => DeletePlot(plot));
        public ICommand CreateNewStandCommand => new Command(_ => CreateNewStand());

        public ICommand ExportSummaryCommand =>
            new Command<string>(method => ExportSummary());

        public bool IsSelectedExportAll
        {
            get => _isSelectedExportAll;
            set => SetProperty(ref _isSelectedExportAll, value);

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

        public List<TreeLookup> LookupTrees { get; set; }

        public ICommand NewPlotCommand =>
            new Command<string>(method => CreateNewPlot(CurrentStand));

        public ICommand SaveAllCommand =>
        new Command<string>(method => SaveAll());

        public ICommand SetCurrentPlotCommand => new Command<Plot>(plot => SetCurrentPlot(plot));

        public ICommand SetPlotSummaryCommand => new Command<Plot>(plot => SetSummaryPlot(plot));

        public ICommand SetStandOnlyCommand => new Command<Plot>(plot => SetStandOnly());

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
        private bool _summarySectionIsVisible;
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

        public Plot SummaryPlot
        {
            get => _summaryPlot;
            set => SetProperty(ref _summaryPlot, value);

        }

        public ObservableCollection<TreeLookup> TreeLookupFilteredList { get; set; }

        public string ValidationMessage
        {
            get => _validationMessage;
            set => SetProperty(ref _validationMessage, value);
        }

        private LookupRepository _lookupRepository { get; set; }

        private MainService _mainService { get; set; }

        private StandRepository _standRepository { get; set; }

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
                TreeLive _treeLive = new TreeLive() { TreeNumber = currentMaxTreeNumber + 1 };
                CurrentPlot.PlotTreeLive.Add(_treeLive);
                currentMaxTreeNumber++;
            }
        }

        private Plot CreateNewPlot(Stand stand)
        {

            int newPlotNumber = (stand.Plots != null && stand.Plots.Any())
                ? stand.Plots.Max(p => p.PlotNumber) + 1
                : 1; Plot _newPlot = new Plot() { StandID = stand.ID, PlotNumber = newPlotNumber };

            _newPlot.PlotTreeLive = new ObservableCollection<TreeLive>();
            _newPlot.PlotTreeLive.Add(new TreeLive() { TreeNumber = 1, PlotID = _newPlot.ID});

            stand.Plots.Add(_newPlot);

            SetCurrentPlot(_newPlot);
            return _newPlot;
        }

        private Stand CreateNewStand()
        {
            Stand _stand = new Stand()
            {
            };

            Plot _newPlot = CreateNewPlot(_stand);
            AllStands.Add(_stand);
            SetCurrentStand(_stand);
            return _stand;

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

        private void ExportSuccessMessage(string method)
        {
            // Inform the user
            Application.Current.MainPage.DisplayAlert("Export Successful", $"Summary has been exported to {method}.", "OK");
        }

        private void ExportSummary()
        {

            SpeciesSummary = GenerateTreeSpeciesSummary(CurrentStand.Plots);
            ObservableCollection<SummaryItem> summaryResult = null;
            IEnumerable<TreeLive> trees = null;
            string tabName = null;
            if (IsSelectedExportSelectedOnly)
            {
                if (SummaryPlot is not null && !StandOnlySummary)
                {
                    trees = SummaryPlot.PlotTreeLive;
                    summaryResult = TreeSummaryHelper.GenerateSummaryResult(trees);
                    tabName = "Plot " + SummaryPlot.PlotNumber.ToString();

                    // Create a new Excel workbook
                    var workbook = new XLWorkbook();
                    ExportToExcel(workbook, tabName, summaryResult);
                }
                else if (SummaryPlot is null && StandOnlySummary)
                {
                    trees = CurrentStand.Plots.SelectMany(p => p.PlotTreeLive);
                    summaryResult = TreeSummaryHelper.GenerateSummaryResult(trees);
                    tabName = "Stand " + CurrentStand.StandNumber.ToString();

                    // Create a new Excel workbook
                    var workbook = new XLWorkbook();
                    ExportToExcel(workbook, tabName, summaryResult);
                }
            }
            else
            {
                // Create a new Excel workbook
                var workbook = new XLWorkbook();

                // Export Stand
                trees = CurrentStand.Plots.SelectMany(p => p.PlotTreeLive);
                summaryResult = TreeSummaryHelper.GenerateSummaryResult(trees);
                tabName = "Stand " + CurrentStand.StandNumber.ToString();
                ExportToExcel(workbook, tabName, summaryResult);

                //Export all Plots
                foreach (Plot plot in CurrentStand.Plots)
                {
                    trees = plot.PlotTreeLive;
                    summaryResult = TreeSummaryHelper.GenerateSummaryResult(trees);
                    tabName = "Plot " + plot.PlotNumber.ToString();
                    ExportToExcel(workbook, tabName, summaryResult);
                }

            }
            ExportSuccessMessage("Excel");
        }

        private void ExportToExcel<T>(XLWorkbook workBook, string tabName, ObservableCollection<T> items)
        {

            // Add a worksheet
            var worksheet = workBook.Worksheets.Add(tabName);

            // Get the properties of the generic type T (column names)
            var properties = typeof(T).GetProperties();

            // Add headers (column names) to the first row
            for (int col = 0; col < properties.Length; col++)
            {
                worksheet.Cell(1, col + 1).Value = properties[col].Name; // Column headers are property names
            }

            // Add the data (values from the ObservableCollection)
            for (int row = 0; row < items.Count; row++)
            {
                for (int col = 0; col < properties.Length; col++)
                {
                    var propertyValue = properties[col].GetValue(items[row]);
                    worksheet.Cell(row + 2, col + 1).Value = propertyValue.ToString();
                }
            }

            // Save the workbook to the specified file path
            workBook.SaveAs("C://temp//Summary.xlsx");

        }

        private ObservableCollection<SummaryResultTreeSpecies> GenerateTreeSpeciesSummary(IEnumerable<Plot> plots)
        {
            var summary = plots
             .SelectMany(plot => plot.PlotTreeLive.Select(tree => new { plot.PlotNumber, tree.TreeLookup.DisplayName }))
             .GroupBy(t => new { t.PlotNumber, t.DisplayName })
             .Select(g => new SummaryResultTreeSpecies
             {
                 PlotNumber = g.Key.PlotNumber,
                 Species = g.Key.DisplayName,
                 Count = g.Count()
             })
             .OrderBy(x => x.PlotNumber)
             .ThenBy(x => x.Species)
             .ToList();

            return new ObservableCollection<SummaryResultTreeSpecies>(summary);

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
                plot = CreateNewPlot(stand);
            }

            SetCurrentPlot(plot);

            return plot;

        }

        private Stand GetOrCreateStand()
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

            AllPlots = _stand.Plots;

            SetCurrentStand(_stand);

            return _stand;

        }

        private void LoadLookupTables()
        {
            LookupTrees = _lookupRepository.GetTreeLookups();
            TreeLookupFilteredList = new ObservableCollection<TreeLookup>() { };
        }

      
        private async void RemoveTrees(int currentTreeCount)
        {
            int treesToSubtract = currentTreeCount - CurrentPlot.TreeCount;
            string message =string.Empty;
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
            _standRepository.Save(CurrentStand);
        }

        private void SetCurrentPlot(Plot plot)
        {
            CurrentPlot = plot;
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
            var trees = CurrentStand.Plots.SelectMany(p => p.PlotTreeLive);
            SummaryItems = TreeSummaryHelper.GenerateSummaryResult(trees);
            SpeciesSummary = GenerateTreeSpeciesSummary(CurrentStand.Plots);

            SummaryPageMessage = $"Stand {CurrentStand.StandNumber} Summary";
        }

        private void SetSummaryPlot(Plot plot)
        {
            if (plot is not null)
            {
                SummaryPlot = plot;
                StandOnlySummary = false;
                SummarySectionIsVisible = true;

                SummaryItems = TreeSummaryHelper.GenerateSummaryResult(plot.PlotTreeLive);
                SpeciesSummary = GenerateTreeSpeciesSummary(new List<Plot> { plot });
                SummaryPageMessage = $"Plot {CurrentPlot.PlotNumber} Summary";
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
    }
}