using DocumentFormat.OpenXml.Office2013.Drawing.ChartStyle;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using PTANonCrown.Data.Models;
using System.Collections.ObjectModel;

namespace PTANonCrown.Services
{
    public static class TreeSummaryHelper
    {
        public static bool CheckTreesValid(IEnumerable<TreeLive> trees)
        {
            bool atLeastOneTree = trees.Count() > 0;
            bool speciesExists = trees.All(t => (t.TreeSpecies is not null));
            bool validMeasurements = trees.All(t => (t.TreeSpecies is not null) & (t.DBH_cm > 0) & (t.Height_m > 0));

            return atLeastOneTree && speciesExists && validMeasurements;
        }


        public static ObservableCollection<SummaryItem> GenerateSummaryResult(IEnumerable<Plot> plots)
        {

            // one plot; 
            var trees = plots.SelectMany(p => p.PlotTreeLive);
            int plotCount = plots.Count();
            ObservableCollection<SummaryItem> result = new ObservableCollection<SummaryItem>();
            


        if ((trees is null || trees.Count() == 0))
        {
            return result;
        }

        if (!CheckTreesValid(trees))
        {
            return result;
        }

                        
        // First Section: These are averages across all plots, if calculating for multiple plots. 
            result.Add(GetBasalAreaTotal(trees, plotCount));
            result.Add(GetBasalAreaMerchantable_m2ha(trees, plotCount));
            result.Add(GetBasalAreaLIT_m2ha(trees, plotCount));
            result.Add(GetBasalAreaLT_m2ha(trees, plotCount));
            result.Add(GetBasalArea_WP_RO(trees, plotCount));
            result.Add(GetBasalArea_EH_RS_BF(trees, plotCount));
            result.Add(GetBasalAreaAGS_sM_yB_wP_rO_rS_15to25_cm(trees, plotCount));
            result.Add(GetAGS_m2ha(trees, plotCount));
            result.Add(GetAGS_LIT_m2ha(trees, plotCount));
            result.Add(GetBasalArea_gt25cm_m2ha(trees, plotCount));

            result.Add(GetPlantedTreesInSitu_perc(trees));
            result.Add(GetPlantedTreesExSitu_perc(trees));
            result.Add(GetMerchConifer_perc(trees));
            result.Add(GetDeciduousLIT_perc(trees));
            result.Add(GetLIT_perc(trees));
            result.Add(GetLT_perc(trees));
            result.Add(GetAGSLIT_NS_WS_RP(trees));
            result.Add(GetQMDMerchTrees_cm(trees));
            result.Add(GetAverageHeight(trees));

            result.Add(GetAge(plots));

            return result;
        }
            
        public static SummaryItem GetAGSLIT_NS_WS_RP(IEnumerable<TreeLive> trees)

        {
            var filteredAGSLIT = trees.Where(t => t.AGS && t.TreeSpecies.LIT);
            var filtered_NS_WS_RP = FilterTreeBySpecies(trees, new List<string> { "ns", "wp", "rp" });

            // Combine the two sets and remove duplicates (by object reference or ID)
            var combined = filteredAGSLIT
                .Union(filtered_NS_WS_RP)
                .Distinct(); // Optional if trees can repeat — ensure it’s by object equality

            int countCombined = combined.Count();
            int countTotal = trees.Count();

            var result = Math.Round((decimal)100 * countCombined / countTotal, 2);

            var summaryItem = new SummaryItem()
            {
                DisplayName = "% LIT or NS/WS/RP",
                Value = result,
                Units = "%"
            };
            return summaryItem;
        }
        public static SummaryItem GetBasalAreaTotal(IEnumerable<TreeLive> trees, int plotCount)
        {
            // Twice the count of all trees
            int result = GetBasalArea(trees) / plotCount;
            var summaryItem = new SummaryItem()
            {
                DisplayName = "Basal Area: Total",
                Value = result,
                Units = "m²/ha"
            };
            return summaryItem;
        }
        private static IEnumerable<TreeLive> FilterMerchantableTrees(IEnumerable<TreeLive> trees)
        {
            return trees.Where(t => t.DBH_cm > 9);
        }
        public static SummaryItem GetBasalAreaMerchantable_m2ha(IEnumerable<TreeLive> trees, int plotCount)
        {

            var merchantableTrees = FilterMerchantableTrees(trees);
            int result = GetBasalArea(merchantableTrees) / plotCount; 
            var summaryItem = new SummaryItem()
            {
                DisplayName = "Basal Area: Merchantable",
                Value = result,
                Units = "m²/ha"
            };
            return summaryItem;
        }

        public static SummaryItem GetBasalAreaLIT_m2ha(IEnumerable<TreeLive> trees, int plotCount)
        {

            var filteredTrees = trees.Where(t => t.TreeSpecies.LIT == true);
            //todo account for LIT difference for planted plots

            var result = GetBasalArea(filteredTrees) / plotCount;
            var summaryItem = new SummaryItem()
            {
                DisplayName = "Basal Area: LIT",
                Value = result,
                Units = "m²/ha"
            };
            return summaryItem;
        }

        private static double GetTPH(IEnumerable<TreeLive> trees) {

            int BA_per_tree = 2;
            double result = trees.Sum(t => BA_per_tree/ (Math.Pow(t.DBH_cm, 2) * 0.00007854 )) ;
            return result;
        }


        public static SummaryItem GetBasalAreaLT_m2ha(IEnumerable<TreeLive> trees, int plotCount)
        {
            var legacyTrees = trees.Where(t => t.Legacy == true);
            int result = 2 * legacyTrees.Count() / plotCount;

           
            var summaryItem = new SummaryItem()
            {
                DisplayName = "Basal Area: LT",
                Value = result,
                Units = "m²/ha"
            };
            return summaryItem;
        }

        private static double BA_conversionFactor = 0.00007854;


        public static SummaryItem GetQMDMerchTrees_cm(IEnumerable<TreeLive> trees)
        {
            //filter for merchantable only
            var merchantableTrees = FilterMerchantableTrees(trees);

            // find BA and TPH
            int BAmerch = GetBasalArea(merchantableTrees);
            var TPHmerch = GetTPH(merchantableTrees);

            //find the QMD
            var avgBA_perTree_merchTrees = BAmerch / TPHmerch;
            var result = Math.Sqrt(avgBA_perTree_merchTrees / BA_conversionFactor);
            result = Math.Round(result, 2);
            var summaryItem = new SummaryItem()
            {
                DisplayName = "QMD Merchantable Trees",
                Value = result,
                Units = "cm"
            };
            return summaryItem;
        }

        private static int GetBasalArea(IEnumerable<TreeLive> trees)
        {
            return 2 * trees.Count();
        }

        public static SummaryItem GetAGS_m2ha(IEnumerable<TreeLive> trees, int plotCount)
        {
            var filteredTrees = trees.Where(t => t.AGS == true);
            int result = GetBasalArea(filteredTrees) / plotCount;
            var summaryItem = new SummaryItem()
            {
                DisplayName = "Basal Area: AGS",
                Value = result,
                Units = "m²/ha"
            };
            return summaryItem;
        }


        public static SummaryItem GetAGS_LIT_m2ha(IEnumerable<TreeLive> trees, int plotCount)
        {
            var filteredTrees = trees.Where(t => (t.AGS == true) & (t.TreeSpecies.LIT == true));
            //todo account for LIT

            int result = GetBasalArea(filteredTrees) / plotCount;
            var summaryItem = new SummaryItem()
            {
                DisplayName = "Basal Area: AGS LIT",
                Value = result,
                Units = "m²/ha"
            };
            return summaryItem;
        }

        public static SummaryItem GetAge(IEnumerable<Plot> plots)
        {
            var ages = plots.Select(p => p.AgeTreeAge);

            decimal result = (decimal)Math.Round(ages.Average(), 2);
            var summaryItem = new SummaryItem()
            {
                DisplayName = "Age",
                Value = result,
                Units = "years"
            };
            return summaryItem;
        }

        public static SummaryItem GetAverageHeight(IEnumerable<TreeLive> trees)
        {
            var heights = trees.Select(trees => trees.Height_m);
            decimal result = (decimal)Math.Round(heights.Average(),2);
            var summaryItem = new SummaryItem()
            {
                DisplayName = "Average Height",
                Value = result,
                Units = "m"
            };
            return summaryItem;
        }

        public static SummaryItem GetBasalArea_EH_RS_BF(IEnumerable<TreeLive> trees, int plotCount)
        {
            var filteredTrees = FilterTreeBySpecies(trees, ["eh", "rs", "bf"]);
            int result = GetBasalArea(filteredTrees) / plotCount;

            var summaryItem = new SummaryItem()
            {
                DisplayName = "Basal Area (EH / RS / BF)",
                Value = result,
                Units = "m²/ha"
            };
            return summaryItem;
        }

        public static SummaryItem GetBasalArea_gt25cm_m2ha(IEnumerable<TreeLive> trees, int plotCount)
        {
            var bigTrees = trees.Where(t => t.DBH_cm > 25);
            int result = GetBasalArea(bigTrees)/plotCount;
            var summaryItem = new SummaryItem()
            {
                DisplayName = "Basal Area > 25cm",
                Value = result,
                Units = "m²/ha"
            };
            return summaryItem;
        }

        public static SummaryItem GetBasalArea_WP_RO(IEnumerable<TreeLive> trees, int plotCount)
        {

            var filteredTrees = FilterTreeBySpecies(trees, ["wp", "ro"]);
            int result = GetBasalArea(filteredTrees) / plotCount;

            var summaryItem = new SummaryItem()
            {
                DisplayName = "Basal Area (WP / RO)",
                Value = result,
                Units = "m²/ha"
            };
            return summaryItem;
        }

        public static SummaryItem GetLIT_perc(IEnumerable<TreeLive> trees)
        {
            var filteredTrees = trees.Where(t => (t.TreeSpecies.LIT == true));
            //todo account for LIT planted vs. LIT not planted; difference in LIT status for at least one tree 


            int countFilteredTrees = filteredTrees.Count();
            int countTotal = trees.Count();

            var result = countTotal > 0 ? Math.Round((decimal)(100 * countFilteredTrees / countTotal), 2) : 0;

            var summaryItem = new SummaryItem()
            {
                DisplayName = "% LIT",
                Value = result,
                Units = "%"
            };
            return summaryItem;
        }
        public static SummaryItem GetLT_perc(IEnumerable<TreeLive> trees)
        {
            var filteredTrees = trees.Where(t => (t.TreeSpecies.LT == true));

            int countFilteredTrees = filteredTrees.Count();
            int countTotal = trees.Count();

            var result = countTotal > 0 ? Math.Round((decimal)(100 * countFilteredTrees / countTotal), 2) : 0;

            var summaryItem = new SummaryItem()
            {
                DisplayName = "% LT",
                Value = result,
                Units = "%"
            };
            return summaryItem;
        }
        public static SummaryItem GetDeciduousLIT_perc(IEnumerable<TreeLive> trees)
        {

            var filteredTrees = trees.Where(t => (t.TreeSpecies.HardwoodSoftwood == 1) & (t.TreeSpecies.LIT == true));
            //todo account for LIT planted vs. LIT not planted; difference in LIT status for at least one tree 


            int countFilteredTrees = filteredTrees.Count();
            int countTotal = trees.Count();

            var result = countTotal > 0 ? Math.Round((decimal)(100 * countFilteredTrees / countTotal),2) : 0;  

            var summaryItem = new SummaryItem()
            {
                DisplayName = "% Deciduous LIT",
                Value = result,
                Units = "%"
            };
            return summaryItem;
        }

        public static SummaryItem GetBasalAreaAGS_sM_yB_wP_rO_rS_15to25_cm(IEnumerable<TreeLive> trees, int plotCount)
        {
            var filteredTrees = trees.Where(t => t.AGS == true);
            filteredTrees = filteredTrees.Where(t => 15 < t.DBH_cm && t.DBH_cm < 25);
            filteredTrees = FilterTreeBySpecies(filteredTrees, ["sm", "yb", "wp", "ro", "rs"]);

            int result = GetBasalArea(filteredTrees) / plotCount;
            var summaryItem = new SummaryItem()
            {
                DisplayName = "Basal Area: LIT 15-25 cm (sM/yB/wP/rO/rS)",
                Value = result,
                Units = "m²/ha"
            };
            return summaryItem;
        }


        public static SummaryItem GetMerchConifer_perc(IEnumerable<TreeLive> trees)
        {
            var filteredTrees = FilterMerchantableTrees(trees).Where(t => t.TreeSpecies.HardwoodSoftwood == 2);

            int countMerchConifer = filteredTrees.Count();
            int totalCount = trees.Count();

            var result = totalCount > 0? Math.Round((decimal)(100 * countMerchConifer/totalCount), 2) : 0;

            var summaryItem = new SummaryItem()
            {
                DisplayName = "% Merchantable Conifer",
                Value = result,
                Units = "%"
            };
            return summaryItem;
        }

        private static int GetCountPlantedTrees(IEnumerable<TreeLive> trees, string type)
        {
            type = type.ToLower();
            switch (type)
            {
                case "insitu":
                    return trees.Where(t => t.PlantedMethod == PlantedMethod.InSitu).Count();
                case "exsitu":
                    return trees.Where(t => t.PlantedMethod == PlantedMethod.ExSitu).Count();
                case "both":
                    return trees.Where(t => t.PlantedMethod == PlantedMethod.ExSitu || t.PlantedMethod == PlantedMethod.InSitu).Count();

                default:
                    throw new Exception("Case not handled. Must be one of: insitu, exsitu, both");
            }
        }    

        public static SummaryItem GetPlantedTreesExSitu_perc(IEnumerable<TreeLive> trees)
        {
            var countTotal = GetCountPlantedTrees(trees, "both");
            var countExSitu = GetCountPlantedTrees(trees, "exsitu");
            decimal result = countTotal > 0 ? 100 * countExSitu / countTotal : 0;
            var summaryItem = new SummaryItem()
            {
                DisplayName = "% Planted Ex Situ (of Total Planted)",
                Value = result,
                Units = "%"
            };
            return summaryItem;
        }

        public static SummaryItem GetPlantedTreesInSitu_perc(IEnumerable<TreeLive> trees)
        {
            
            var countTotal = GetCountPlantedTrees(trees, "both");
            var countInSitu = GetCountPlantedTrees(trees, "insitu");
            decimal result = countTotal > 0? 100 * countInSitu / countTotal : 0;

            var summaryItem = new SummaryItem()
            {
                DisplayName = "% Planted In Situ (of Total Planted)",
                Value = result,
                Units = "%"
            };
            return summaryItem;
        }

       private static IEnumerable<TreeLive> FilterTreeBySpecies(IEnumerable<TreeLive> trees,
           List<string> searchSpecies)
        {
            return trees.Where(t => searchSpecies.Contains(t.TreeSpecies.ShortCode, StringComparer.OrdinalIgnoreCase));
        }

     
    }
}