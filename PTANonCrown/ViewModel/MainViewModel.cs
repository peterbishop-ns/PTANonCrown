using System.Collections.ObjectModel;
using PTANonCrown.Models;
using PTANonCrown.Services;
using PTANonCrown.Repository;
using System.Windows.Input;
//using static Android.Renderscripts.Script;
using System.ComponentModel.Design;
using System.Globalization;
using CsvHelper;
using PTANonCrown.Models;
using ClosedXML.Excel;

namespace PTANonCrown.ViewModel
{
    public class MainViewModel : BaseViewModel
    {

        private MainService _mainService { get; set; }
        private StandRepository _standRepository { get; set; }
        private LookupRepository _lookupRepository { get; set; }

        private ObservableCollection<SummaryItem> _summaryItems;
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

        private ObservableCollection<TreeLookup> _treeLookupFilteredList;
        public ObservableCollection<TreeLookup> TreeLookupFilteredList
        {
            get => _treeLookupFilteredList;

            set
            {
                if (_treeLookupFilteredList != value)
                {
                    _treeLookupFilteredList = value;
                    OnPropertyChanged();
                }
            }
        }

        public MainViewModel(MainService mainService, StandRepository standRepository, LookupRepository lookupRepository)
        {
            _mainService = mainService;
            _standRepository = standRepository;
            _lookupRepository = lookupRepository;

           GetOrCreateStand();
           GetOrCreatePlot(CurrentStand);
            LoadLookupTables();

           InitializeCollections();
        }

        private string _searchString;
        public string SearchString
        {
            get => _searchString;

            set
            {
                if (_searchString != value)
                {
                    _searchString = value;
                    OnPropertyChanged();
                    ApplyFilter();
                }
            }
        }

        public void ApplyFilter()
        {
            TreeLookupFilteredList.Clear();
            TreeLookupFilteredList = new ObservableCollection<TreeLookup>(LookupTrees.
                Where(t => t.ShortCode.Contains(SearchString, StringComparison.OrdinalIgnoreCase))) { };



        }
        public List<TreeLookup> LookupTrees {  get; set; }

        private void LoadLookupTables()
        {
            LookupTrees = _lookupRepository.GetTreeLookups();
            TreeLookupFilteredList = new ObservableCollection<TreeLookup>(LookupTrees) { };
        }

        public ICommand SetCurrentPlotCommand => new Command<Plot>(plot => SetCurrentPlot(plot));
        public ICommand SetPlotSummaryCommand => new Command<Plot>(plot => SetSummaryPlot(plot));
        public ICommand DeletePlotCommand => new Command<Plot>(plot => DeletePlot(plot));
        public ICommand SetStandOnlyCommand => new Command<Plot>(plot => SetStandOnly());


        public ICommand ExportSummaryCommand =>
            new Command<string>(method => ExportSummary());
        public ICommand NewPlotCommand =>
            new Command<string>(method => CreateNewPlot(CurrentStand));

        public ICommand SaveAllCommand =>
        new Command<string>(method => SaveAll());

        private void SaveAll()
        {
            _standRepository.Save(CurrentStand);
        }

        private Plot CreateNewPlot(Stand stand)
        {

            int newPlotNumber = (stand.Plots != null && stand.Plots.Any())
                ? stand.Plots.Max(p => p.PlotNumber) + 1
                : 1; Plot _newPlot = new Plot() { StandID = stand.ID, PlotNumber = newPlotNumber};

            _newPlot.TreeLive = new ObservableCollection<TreeLive>();
            _newPlot.TreeLive.Add(new TreeLive() {TreeNumber = 1,  PlotID = _newPlot.ID});


            stand.Plots.Add(_newPlot);
            SetCurrentPlot(_newPlot);
            return _newPlot;
        }

        private void SetCurrentPlot(Plot plot)
        {
            CurrentPlot = plot;
        }    

        public bool StandOnlySummary {  get; set; }
        
        private void SetSummaryPlot(Plot plot)
        {
            SummaryPlot = plot;
            StandOnlySummary = false;
            SummaryItems = TreeSummaryHelper.GenerateSummaryResult(plot.TreeLive);
            SpeciesSummary =  GenerateTreeSpeciesSummary(new List<Plot>{ plot });
        }
        private void DeletePlot(Plot plot)
        {
            CurrentStand.Plots.Remove(plot);
            GetOrCreatePlot(CurrentStand);
            SaveAll();
        }

 
        private void SetStandOnly()
        {
            StandOnlySummary = true;
            SummaryPlot = null;

            var trees = CurrentStand.Plots.SelectMany(p => p.TreeLive);
            SummaryItems = TreeSummaryHelper.GenerateSummaryResult(trees);
            SpeciesSummary = GenerateTreeSpeciesSummary(CurrentStand.Plots);
        }

        private void ExportToCSV(int plotID, SummaryResult summaryResult)
        {
            using (var writer = new StreamWriter("C:\\temp\\temp.csv"))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                // Plot/Stand summary comes first
                csv.WriteField("ID");
                csv.WriteField(plotID.ToString());
                csv.NextRecord();

                // Write headers (Property names)
                csv.WriteField("Field Name");
                csv.WriteField("Field Value");
                csv.NextRecord();

                // Get the properties of the SummaryItems object
                var properties = summaryResult.GetType().GetProperties();

                foreach (var property in properties)
                {
                    var fieldName = property.Name;
                    var fieldValue = property.GetValue(summaryResult)?.ToString() ?? string.Empty;

                    csv.WriteField(fieldName);
                    csv.WriteField(fieldValue);
                    csv.NextRecord();
                }
            } }


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

        private bool _isSelectedExportSelectedOnly;
        public bool IsSelectedExportSelectedOnly
        {
            get => _isSelectedExportSelectedOnly;

            set
            {
                if (_isSelectedExportSelectedOnly != value)
                {
                    _isSelectedExportSelectedOnly = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isSelectedExportAll;
        public bool IsSelectedExportAll
        {
            get => _isSelectedExportAll;

            set
            {
                if (_isSelectedExportAll != value)
                {
                    _isSelectedExportAll = value;
                    OnPropertyChanged();
                }
            }
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
                    trees = SummaryPlot.TreeLive;
                    summaryResult = TreeSummaryHelper.GenerateSummaryResult(trees);
                    tabName = "Plot " + SummaryPlot.PlotNumber.ToString();

                    // Create a new Excel workbook
                    var workbook = new XLWorkbook();
                    ExportToExcel(workbook, tabName, summaryResult);
                } 
                else if (SummaryPlot is null && StandOnlySummary)
                {
                    trees = CurrentStand.Plots.SelectMany(p => p.TreeLive);
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
                trees = CurrentStand.Plots.SelectMany(p => p.TreeLive);
                summaryResult = TreeSummaryHelper.GenerateSummaryResult(trees);
                tabName = "Stand " + CurrentStand.StandNumber.ToString();
                ExportToExcel(workbook, tabName, summaryResult);

                //Export all Plots
                foreach (Plot plot in CurrentStand.Plots)
                {
                    trees = plot.TreeLive;
                    summaryResult = TreeSummaryHelper.GenerateSummaryResult(trees);
                    tabName = "Plot " + plot.PlotNumber.ToString();
                    ExportToExcel(workbook, tabName, summaryResult);
                }

            }
            ExportSuccessMessage("Excel");
        }

        private void ExportSuccessMessage(string method) {
            // Inform the user
            Application.Current.MainPage.DisplayAlert("Export Successful", $"Summary has been exported to {method}.", "OK");
        }

        private ObservableCollection<TreeLive> _LiveTrees;

        public ObservableCollection<TreeLive> LiveTrees
        {
            get => _LiveTrees;

            set
            {
                if (_LiveTrees != value)
                {
                    _LiveTrees = value;
                    OnPropertyChanged();
                }
            }
        }
        private ObservableCollection<TreeDead> _deadTrees;

        public ObservableCollection<TreeDead> DeadTrees
        {
            get => _deadTrees;

            set
            {
                if (_deadTrees != value)
                {
                    _deadTrees = value;
                    OnPropertyChanged();
                }
            }
        }
        private ObservableCollection<Stand> _allStands;

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


        private ObservableCollection<CoarseWoody> _coarseWoody;

        public ObservableCollection<CoarseWoody> CoarseWoody
        {
            get => _coarseWoody;

            set
            {
                if (_coarseWoody != value)
                {
                    _coarseWoody = value;
                    OnPropertyChanged();
                }
            }
        }
        private ObservableCollection<TreeDead>? LoadDeadTrees(int plotID)
        {
            // todo this should first check DB; if none exist, THEN initialize

            _deadTrees = new ObservableCollection<TreeDead>();
            _deadTrees.Add(new TreeDead() { PlotID = plotID, DBH_start = 21, DBH_end = 30 });
            _deadTrees.Add(new TreeDead() { PlotID = plotID, DBH_start = 31, DBH_end = 40 });
            _deadTrees.Add(new TreeDead() { PlotID = plotID, DBH_start = 41, DBH_end = 50 });
            _deadTrees.Add(new TreeDead() { PlotID = plotID, DBH_start = 51, DBH_end = 60 });
            _deadTrees.Add(new TreeDead() { PlotID = plotID, DBH_start = 60, DBH_end = 1000 });
            return _deadTrees;
        }
        private ObservableCollection<CoarseWoody>? LoadCoarseWoody(int plotID)
        {            // todo this should first check DB; if none exist, THEN initialize

            _coarseWoody = new ObservableCollection<CoarseWoody>();
            _coarseWoody.Add(new CoarseWoody() { PlotID = plotID, DBH_start = 21, DBH_end = 30 });
            _coarseWoody.Add(new CoarseWoody() { PlotID = plotID, DBH_start = 31, DBH_end = 40 });
            _coarseWoody.Add(new CoarseWoody() { PlotID = plotID, DBH_start = 41, DBH_end = 50 });
            _coarseWoody.Add(new CoarseWoody() { PlotID = plotID, DBH_start = 51, DBH_end = 60 });
            _coarseWoody.Add(new CoarseWoody() { PlotID = plotID, DBH_start = 60, DBH_end = 1000 });
            return _coarseWoody;
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
        private void InitializeCollections()
        {
            //LiveTrees = LoadLiveTrees(plotID: 2);
            DeadTrees = LoadDeadTrees(plotID: 2);
            CoarseWoody = LoadCoarseWoody(plotID: 2);
        }
        private Stand _currentStand;
        public Stand CurrentStand
        {
            get => _currentStand;

            set
            {
                if (_currentStand != value)
                {
                    _currentStand = value;
                    OnPropertyChanged();
                }
            }
        }
        private Plot _summaryPlot;
        public Plot SummaryPlot
        {
            get => _summaryPlot;

            set
            {
                if (_summaryPlot != value)
                {
                    _summaryPlot = value;
                    OnPropertyChanged();
                }
            }
        }



        private ObservableCollection<SummaryResultTreeSpecies> _speciesSummary;
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


        private Plot _currentPlot;
        public Plot CurrentPlot
        {
            get => _currentPlot;

            set
            {
                if (_currentPlot != value)
                {
                    _currentPlot = value;
                    OnPropertyChanged();
                }
            }
        }
        
        private ObservableCollection<SummaryResultTreeSpecies> GenerateTreeSpeciesSummary(IEnumerable<Plot> plots)
        {
            var summary = plots
             .SelectMany(plot => plot.TreeLive.Select(tree => new { plot.PlotNumber, tree.Species }))
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


        private Stand CreateNewStand()
        {
            Stand _stand = new Stand()
            {
            };

            Plot _newPlot = CreateNewPlot(_stand);

            return _stand;

        }

        private void SetCurrentStand(Stand stand)
        {
            CurrentStand = stand;
        }
        private Stand GetOrCreateStand()
        {
            Stand _stand;
            AllStands = new ObservableCollection<Stand>(_standRepository.GetAll());

            // Get first stand if any exist
            if (AllStands.Count > 0) {

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
    }
}
