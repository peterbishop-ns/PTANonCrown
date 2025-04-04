//using static Android.Renderscripts.Script;
using ClosedXML.Excel;
using PTANonCrown.Models;
using PTANonCrown.Repository;
using PTANonCrown.Services;
using System.Collections.ObjectModel;
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
        private Plot _summaryPlot;

        public MainViewModel(MainService mainService, StandRepository standRepository, LookupRepository lookupRepository)
        {
            _mainService = mainService;
            _standRepository = standRepository;
            _lookupRepository = lookupRepository;

            GetOrCreateStand();
            GetOrCreatePlot(CurrentStand);
            LoadLookupTables();

        }

        public ObservableCollection<Stand> AllStands { get; set; }

        public Plot CurrentPlot
        {
            get => _currentPlot;
            set => SetProperty(ref _currentPlot, value);
        }

        public Stand CurrentStand
        {
            get => _currentStand;
            set => SetProperty(ref _currentStand, value);

        }

        public ICommand DeletePlotCommand => new Command<Plot>(plot => DeletePlot(plot));
        public ICommand SpecifyTreeCountInPlotCommand => new Command(SpecifyTreeCountInPlot);

        public ICommand ExportSummaryCommand =>
            new Command<string>(method => ExportSummary());

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

        public ObservableCollection<TreeLive> LiveTrees { get; set; }

        public List<TreeLookup> LookupTrees { get; set; }

        public ICommand NewPlotCommand =>
            new Command<string>(method => CreateNewPlot(CurrentStand));

        public ICommand SaveAllCommand =>
        new Command<string>(method => SaveAll());

        public ICommand SetCurrentPlotCommand => new Command<Plot>(plot => SetCurrentPlot(plot));

        public ICommand SetPlotSummaryCommand => new Command<Plot>(plot => SetSummaryPlot(plot));

        public ICommand SetStandOnlyCommand => new Command<Plot>(plot => SetStandOnly());

        public ObservableCollection<SummaryResultTreeSpecies> SpeciesSummary { get; set; }

        public bool StandOnlySummary { get; set; }

        public ObservableCollection<SummaryItem> SummaryItems { get; set; }

        public Plot SummaryPlot
        {
            get => _summaryPlot;
            set => SetProperty(ref _summaryPlot, value);

        }

        public ObservableCollection<TreeLookup> TreeLookupFilteredList { get; set; }

        private LookupRepository _lookupRepository { get; set; }

        private MainService _mainService { get; set; }

        private StandRepository _standRepository { get; set; }

      

        private Plot CreateNewPlot(Stand stand)
        {

            int newPlotNumber = (stand.Plots != null && stand.Plots.Any())
                ? stand.Plots.Max(p => p.PlotNumber) + 1
                : 1; Plot _newPlot = new Plot() { StandID = stand.ID, PlotNumber = newPlotNumber };

            _newPlot.PlotTreeLive = new ObservableCollection<TreeLive>();
            _newPlot.PlotTreeLive.Add(new TreeLive() { TreeNumber = 1, PlotID = _newPlot.ID });

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

            return _stand;

        }



        private async void SpecifyTreeCountInPlot() //async because we hae await in the display alert
        {
            int currentTreeCount = CurrentPlot.PlotTreeLive.Count;


            // Adding Trees
            if (CurrentPlot.TreeCount > currentTreeCount)
            {

                int treesToAdd = CurrentPlot.TreeCount - currentTreeCount;

                // Get max tree number
                int currentMaxTreeNumber = CurrentPlot.PlotTreeLive.Count > 0
                    ? CurrentPlot.PlotTreeLive.Max(t => t.TreeNumber)
                    : 0;
                
                // Add the trees
                for (int i = 0; i < treesToAdd; i++)
                {
                    TreeLive _treeLive = new TreeLive() { TreeNumber = currentMaxTreeNumber + 1 };
                    CurrentPlot.PlotTreeLive.Add(_treeLive);
                    currentMaxTreeNumber++;
                }
            }


            // Remove trees
            else if (CurrentPlot.TreeCount < currentTreeCount)
            {
                int treesToSubtract = currentTreeCount - CurrentPlot.TreeCount;

                int firstRemoved = CurrentPlot.PlotTreeLive[CurrentPlot.PlotTreeLive.Count - treesToSubtract].TreeNumber; // First element to be removed
                int lastRemoved = CurrentPlot.PlotTreeLive[CurrentPlot.PlotTreeLive.Count - 1].TreeNumber; // Last element to be removed

                string message = $"Do you want to set the tree count to {CurrentPlot.TreeCount}? Trees {firstRemoved} - {lastRemoved} will be removed. This cannot be undone.";

                bool answer = await  Application.Current.MainPage.DisplayAlert(
                        "Remove Trees?",   
                        message, 
                        "Continue",  
                        "Cancel"    
                    );

                if (answer) {

                    for (int i = 0; i < treesToSubtract; i++)
                    {
                        CurrentPlot.PlotTreeLive.RemoveAt(CurrentPlot.PlotTreeLive.Count - 1);
                    } }
                else
                {
                    //Reset the count
                    CurrentPlot.TreeCount = CurrentPlot.PlotTreeLive.Count;
                }

            }

        }
        private void DeletePlot(Plot plot)
        {
            CurrentStand.Plots.Remove(plot);
            GetOrCreatePlot(CurrentStand);
            SaveAll();
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
             .SelectMany(plot => plot.PlotTreeLive.Select(tree => new { plot.PlotNumber, tree.Species }))
             .GroupBy(t => new { t.PlotNumber, t.Species })
             .Select(g => new SummaryResultTreeSpecies
             {
                 PlotNumber = g.Key.PlotNumber,
                 Species = g.Key.Species,
                 Count = g.Count()
             })
             .OrderBy(x => x.PlotNumber)
             .ThenBy(x => x.Species)
             .ToList();

            return new ObservableCollection<SummaryResultTreeSpecies>(summary);

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

            SetCurrentStand(_stand);

            return _stand;

        }

        private void LoadLookupTables()
        {
            LookupTrees = _lookupRepository.GetTreeLookups();
            TreeLookupFilteredList = new ObservableCollection<TreeLookup>() { };
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

            var trees = CurrentStand.Plots.SelectMany(p => p.PlotTreeLive);
            SummaryItems = TreeSummaryHelper.GenerateSummaryResult(trees);
            SpeciesSummary = GenerateTreeSpeciesSummary(CurrentStand.Plots);
        }

        private void SetSummaryPlot(Plot plot)
        {
            SummaryPlot = plot;
            StandOnlySummary = false;
            SummaryItems = TreeSummaryHelper.GenerateSummaryResult(plot.PlotTreeLive);
            SpeciesSummary = GenerateTreeSpeciesSummary(new List<Plot> { plot });
        }
    }
}