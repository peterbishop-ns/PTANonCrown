using System.Collections.ObjectModel;
using PTANonCrown.Models;
using PTANonCrown.Services;
using PTANonCrown.Repository;
using System.Windows.Input;
//using static Android.Renderscripts.Script;
using System.ComponentModel.Design;
using System.Globalization;
using CsvHelper;
using Microsoft.VisualBasic;
using OfficeOpenXml;
using ClosedXML.Excel;
using System.Runtime.CompilerServices;
using DocumentFormat.OpenXml.Drawing;
namespace PTANonCrown.ViewModel
{
    public class MainViewModel : BaseViewModel
    {

        private MainService _mainService { get; set; }
        private StandRepository _standRepository { get; set; }
        public SummaryResult SummaryResult { get; set; }
        public MainViewModel(MainService mainService, StandRepository standRepository)
        {
            _mainService = mainService;
            _standRepository = standRepository;

            GetOrCreateStand();
            CurrentPlot = GetOrCreatePlot(CurrentStand);

            InitializeCollections();
            SummaryResult = new SummaryResult(trees: LiveTrees);
        }


        public ICommand SetCurrentPlotCommand => new Command<Plot>(plot => SetCurrentPlot(plot));


        public ICommand ExportSummaryCommand =>
            new Command<string>(method => ExportSummary(99, SummaryResult, method?.ToString()));
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
            stand.Plots.Add(_newPlot);
            SetCurrentPlot(_newPlot);
            return _newPlot;
        }

        private void SetCurrentPlot(Plot plot)
        {
            CurrentPlot = plot;
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

                // Get the properties of the SummaryResult object
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


        private void ExportToExcel(int plotID, SummaryResult summaryResult)
        {
            // Create a new Excel workbook
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Summary");

            // Get the properties of the SummaryResult object
            var properties = summaryResult.GetType().GetProperties();

            for (int i = 0; i < properties.Length; i++)
            {
                // Write headers (property names)
                worksheet.Cell(i + 1, 1).Value = properties[i].Name;
                worksheet.Cell(i + 1, 2).Value = properties[i].GetValue(summaryResult)?.ToString() ?? string.Empty;


            }
            // Save the workbook to a file
            workbook.SaveAs("C://temp//summary.xlsx");


        }

        private void ExportSummary(int plotID, SummaryResult summaryResult, string method)
        {
            switch (method)
            {

                case "csv":
                    ExportToCSV(plotID, summaryResult);
                    ExportSuccessMessage("CSV");
                    break;
                case "xlsx":
                    ExportToExcel(plotID, summaryResult);
                    ExportSuccessMessage("Excel");
                    break;
                default:
                    throw new NotImplementedException("Only csv and xlsx are supported.");


            }

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
        private ObservableCollection<TreeLive>? LoadLiveTrees(int plotID)
        {
            _LiveTrees = new ObservableCollection<TreeLive>();
            _LiveTrees.Add(new TreeLive() { ID = 1, PlotID = plotID, Species = 33, DBH_cm = 14, Height_m = 7, AGS = false, LIT = true });
            _LiveTrees.Add(new TreeLive() { ID = 2, PlotID = plotID, Species = 44, DBH_cm = 20, Height_m = 10, AGS = false, LIT = true });
            _LiveTrees.Add(new TreeLive() { ID = 3, PlotID = plotID, Species = 44, DBH_cm = 14, Height_m = 10, AGS = false, LIT = true });
            _LiveTrees.Add(new TreeLive() { ID = 4, PlotID = plotID, Species = 44, DBH_cm = 10, Height_m = 13, AGS = false, LIT = true });
            _LiveTrees.Add(new TreeLive() { ID = 5, PlotID = plotID, Species = 44, DBH_cm = 12, Height_m = 14, AGS = false, LIT = true });
            _LiveTrees.Add(new TreeLive() { ID = 6, PlotID = plotID, Species = 44, DBH_cm = 10, Height_m = 5, AGS = false, LIT = true });
            _LiveTrees.Add(new TreeLive() { ID = 7, PlotID = plotID, Species = 44, DBH_cm = 16, Height_m = 5, AGS = false, LIT = true });
            _LiveTrees.Add(new TreeLive() { ID = 8, PlotID = plotID, Species = 44, DBH_cm = 8, Height_m = 5, AGS = false, LIT = true });
            _LiveTrees.Add(new TreeLive() { ID = 9, PlotID = plotID, Species = 44, DBH_cm = 16, Height_m = 5, AGS = false, LIT = true });
            _LiveTrees.Add(new TreeLive() { ID = 10, PlotID = plotID, Species = 44, DBH_cm = 2, Height_m = 8, AGS = false, LIT = true });
            _LiveTrees.Add(new TreeLive() { ID = 11, PlotID = plotID, Species = 44, DBH_cm = 2, Height_m = 9, AGS = false, LIT = true });
            _LiveTrees.Add(new TreeLive() { ID = 12, PlotID = plotID, Species = 44, DBH_cm = 2, Height_m = 8, AGS = false, LIT = true });
            _LiveTrees.Add(new TreeLive() { ID = 13, PlotID = plotID, Species = 44, DBH_cm = 2, Height_m = 7, AGS = false, LIT = true });
            _LiveTrees.Add(new TreeLive() { ID = 14, PlotID = plotID, Species = 44, DBH_cm = 8, Height_m = 9, AGS = false, LIT = true });
            _LiveTrees.Add(new TreeLive() { ID = 15, PlotID = plotID, Species = 44, DBH_cm = 6, Height_m = 9, AGS = false, LIT = true });
            _LiveTrees.Add(new TreeLive() { ID = 16, PlotID = plotID, Species = 44, DBH_cm = 10, Height_m = 8, AGS = false, LIT = true });
            _LiveTrees.Add(new TreeLive() { ID = 17, PlotID = plotID, Species = 44, DBH_cm = 8, Height_m = 13, AGS = false, LIT = true });
            return _LiveTrees;
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
            LiveTrees = LoadLiveTrees(plotID: 2);
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
        

        private Stand CreateNewStand()
        {
            Stand _stand = new Stand()
            {
            };

            _stand.Plots.Add(new Plot() {  });
            return _stand;

        }


        private void GetOrCreateStand()


        {
            AllStands = new ObservableCollection<Stand>(_standRepository.GetAll());

            // Get first stand if any exist
            if (AllStands.Count > 0) {

                CurrentStand = AllStands[0];

                    }
            // Otherwise create a new one (Stand #1)
            else
            {
                CurrentStand = CreateNewStand();
            }

            if (CurrentStand.Plots is null)
            {
                CurrentStand.Plots = new ObservableCollection<Plot>();
            }


        }
    }
}
