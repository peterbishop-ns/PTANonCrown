using System.Collections.ObjectModel;
using PTANonCrown.Models;
using PTANonCrown.Services;
using System.Windows.Input;
//using static Android.Renderscripts.Script;
using System.ComponentModel.Design;
using System.Globalization;
using CsvHelper;
using Microsoft.VisualBasic;
using OfficeOpenXml;
using ClosedXML.Excel;
namespace PTANonCrown.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        
        private MainService _mainService { get; set; }
        public SummaryResult SummaryResult { get; set; }
        public MainViewModel(MainService mainService)
        {
            _mainService = mainService;

            InitializeStand();
            InitializePlot();
            SummaryResult = new SummaryResult(trees: LiveTrees);
            ExportSummary(plotID: 101, summaryResult: SummaryResult, method: "xlsx");
        }



        public ICommand ExportSummaryCommand => new Command<string>(method => ExportSummary(99, SummaryResult, method?.ToString()));

        private void ExportToCSV(int plotID, SummaryResult summaryResult)
        {
            using (var writer = new StreamWriter("C:\\temp\\temp.csv"))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                // Plot/Stand summary comes first
                csv.WriteField("PlotID");
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
            _LiveTrees.Add(new TreeLive() { TreeID = 1, PlotID = plotID, Species = 33, DBH_cm = 14, Height_m = 7, AGS = false, LIT = true });
            _LiveTrees.Add(new TreeLive() { TreeID = 2, PlotID = plotID, Species = 44, DBH_cm = 20, Height_m = 10, AGS = false, LIT = true });
            _LiveTrees.Add(new TreeLive() { TreeID = 3, PlotID = plotID, Species = 44, DBH_cm = 14, Height_m = 10, AGS = false, LIT = true });
            _LiveTrees.Add(new TreeLive() { TreeID = 4, PlotID = plotID, Species = 44, DBH_cm = 10, Height_m = 13, AGS = false, LIT = true });
            _LiveTrees.Add(new TreeLive() { TreeID = 5, PlotID = plotID, Species = 44, DBH_cm = 12, Height_m = 14, AGS = false, LIT = true });
            _LiveTrees.Add(new TreeLive() { TreeID = 6, PlotID = plotID, Species = 44, DBH_cm = 10, Height_m = 5, AGS = false, LIT = true });
            _LiveTrees.Add(new TreeLive() { TreeID = 7, PlotID = plotID, Species = 44, DBH_cm = 16, Height_m = 5, AGS = false, LIT = true });
            _LiveTrees.Add(new TreeLive() { TreeID = 8, PlotID = plotID, Species = 44, DBH_cm = 8, Height_m = 5, AGS = false, LIT = true });
            _LiveTrees.Add(new TreeLive() { TreeID = 9, PlotID = plotID, Species = 44, DBH_cm = 16, Height_m = 5, AGS = false, LIT = true });
            _LiveTrees.Add(new TreeLive() { TreeID = 10, PlotID = plotID, Species = 44, DBH_cm = 2, Height_m = 8, AGS = false, LIT = true });
            _LiveTrees.Add(new TreeLive() { TreeID = 11, PlotID = plotID, Species = 44, DBH_cm = 2, Height_m = 9, AGS = false, LIT = true });
            _LiveTrees.Add(new TreeLive() { TreeID = 12, PlotID = plotID, Species = 44, DBH_cm = 2, Height_m = 8, AGS = false, LIT = true });
            _LiveTrees.Add(new TreeLive() { TreeID = 13, PlotID = plotID, Species = 44, DBH_cm = 2, Height_m = 7, AGS = false, LIT = true });
            _LiveTrees.Add(new TreeLive() { TreeID = 14, PlotID = plotID, Species = 44, DBH_cm = 8, Height_m = 9, AGS = false, LIT = true });
            _LiveTrees.Add(new TreeLive() { TreeID = 15, PlotID = plotID, Species = 44, DBH_cm = 6, Height_m = 9, AGS = false, LIT = true });
            _LiveTrees.Add(new TreeLive() { TreeID = 16, PlotID = plotID, Species = 44, DBH_cm = 10, Height_m =8, AGS = false, LIT = true });
            _LiveTrees.Add(new TreeLive() { TreeID = 17, PlotID = plotID, Species = 44, DBH_cm = 8, Height_m = 13, AGS = false, LIT = true });
            return _LiveTrees;
        }


        private void InitializePlot()
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
        private void InitializeStand()
        {
            CurrentStand = new Stand()
            {
                CruiseID = 99,
                Easting = 10,
                Northing = 11,
                Ecodistrict = 234,
                PlannerID = 999,
                StandID = 888,
                Location = "Truro",
                Organization = "PNS"
            };
        }
    }
}
