using PTANonCrown.Data.Models;
using System.Collections.ObjectModel;

namespace PTANonCrown.Services
{
    public static class TreeSummaryHelper
    {
        public static ObservableCollection<SummaryItem> GenerateSummaryResult(IEnumerable<TreeLive> trees)
        {
            ObservableCollection<SummaryItem> result = new ObservableCollection<SummaryItem>();

            result.Add(GetTotalBasalArea(trees));
            result.Add(GetMerchantableBasalArea_m2ha(trees));
            result.Add(GetLITBasalArea_m2ha(trees));
            result.Add(GetLTBasalArea_m2ha(trees));
            result.Add(GetQMDMerchTrees_cm(trees));
            result.Add(GetAGS_m2ha(trees));
            result.Add(GetAGS_LIT_m2ha(trees));
            result.Add(GetBasalArea_gt25cm_m2ha(trees));
            result.Add(GetAverageHeight(trees));
            result.Add(GetPlantedTreesInSitu_perc(trees));
            result.Add(GetPlantedTreesExSitu_perc(trees));
            result.Add(GetMerchConifer_perc(trees));
            result.Add(GetDeciduousLIT_perc(trees));
            result.Add(GetBasalArea_WP_RO(trees));
            result.Add(GetBasalArea_EH_RS_BF(trees));
            result.Add(GetAGS_sM_yB_wP_rO_rS_15to25_cm(trees));

            return result;
        }
        public static SummaryItem GetTotalBasalArea(IEnumerable<TreeLive> trees)
        {
            // Twice the count of all trees
            int result = GetBasalArea(trees);
            var summaryItem = new SummaryItem()
            {
                DisplayName = "Total Basal Area",
                Value = result,
                Units = "m²/ha"
            };
            return summaryItem;
        }
        private static IEnumerable<TreeLive> FilterMerchantableTrees(IEnumerable<TreeLive> trees)
        {
            return trees.Where(t => t.DBH_cm > 9);
        }
        public static SummaryItem GetMerchantableBasalArea_m2ha(IEnumerable<TreeLive> trees)
        {

            var merchantableTrees = FilterMerchantableTrees(trees);
            int result = GetBasalArea(merchantableTrees); 
            var summaryItem = new SummaryItem()
            {
                DisplayName = "Merchantable Basal Area",
                Value = result,
                Units = "m²/ha"
            };
            return summaryItem;
        }

        public static SummaryItem GetLITBasalArea_m2ha(IEnumerable<TreeLive> trees)
        {
            var summaryItem = new SummaryItem()
            {
                DisplayName = "LIT Basal Area",
                Value = -99,
                Units = "m²/ha"
            };
            return summaryItem;
        }

        private static double GetTPH(IEnumerable<TreeLive> trees) {

            double result = trees.Sum(t => Math.Pow(t.DBH_cm, 2) * 0.00007854) ;
            return result;
        }


        public static SummaryItem GetLTBasalArea_m2ha(IEnumerable<TreeLive> trees)
        {
            var legacyTrees = trees.Where(t => t.Legacy == true);
            int result = 2 * legacyTrees.Count();

           
            var summaryItem = new SummaryItem()
            {
                DisplayName = "LT Basal Area",
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
            int BAmerch = (int)GetMerchantableBasalArea_m2ha(merchantableTrees).Value;
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

        public static SummaryItem GetAGS_m2ha(IEnumerable<TreeLive> trees)
        {
            int result = GetBasalArea(trees.Where(t => t.AGS == true));
            var summaryItem = new SummaryItem()
            {
                DisplayName = "AGS",
                Value = result,
                Units = "m²/ha"
            };
            return summaryItem;
        }


        public static SummaryItem GetAGS_LIT_m2ha(IEnumerable<TreeLive> trees)
        {
            var summaryItem = new SummaryItem()
            {
                DisplayName = "AGS LIT",
                Value = 0,
                Units = "m²/ha"
            };
            return summaryItem;
        }



        public static SummaryItem GetAverageHeight(IEnumerable<TreeLive> trees)
        {
            var heights = trees.Select(trees => trees.Height_m);
            decimal result = Math.Round(heights.Average(),2);
            var summaryItem = new SummaryItem()
            {
                DisplayName = "Average Height",
                Value = result,
                Units = "m"
            };
            return summaryItem;
        }

        public static SummaryItem GetBasalArea_EH_RS_BF(IEnumerable<TreeLive> trees)
        {
            var filteredTrees = FilterTreeBySpecies(trees, ["eh", "rs", "bf"]);
            int result = GetBasalArea(filteredTrees);

            var summaryItem = new SummaryItem()
            {
                DisplayName = "Basal Area (EH / RS / BF)",
                Value = result,
                Units = "m²/ha"
            };
            return summaryItem;
        }

        public static SummaryItem GetBasalArea_gt25cm_m2ha(IEnumerable<TreeLive> trees)
        {
            var bigTrees = trees.Where(t => t.DBH_cm > 25);
            int result = GetBasalArea(bigTrees);
            var summaryItem = new SummaryItem()
            {
                DisplayName = "Basal Area > 25cm",
                Value = result,
                Units = "m²/ha"
            };
            return summaryItem;
        }

        public static SummaryItem GetBasalArea_WP_RO(IEnumerable<TreeLive> trees)
        {

            var filteredTrees = FilterTreeBySpecies(trees, ["wp", "ro"]);
            int result = GetBasalArea(filteredTrees);

            var summaryItem = new SummaryItem()
            {
                DisplayName = "Basal Area (WP / RO)",
                Value = result,
                Units = "m²/ha"
            };
            return summaryItem;
        }

        public static SummaryItem GetDeciduousLIT_perc(IEnumerable<TreeLive> trees)
        {
            var summaryItem = new SummaryItem()
            {
                DisplayName = "Deciduous LIT",
                Value = 0,
                Units = "%"
            };
            return summaryItem;
        }

        public static SummaryItem GetAGS_sM_yB_wP_rO_rS_15to25_cm(IEnumerable<TreeLive> trees)
        {
            var filteredTrees = trees.Where(t => t.AGS == true);
            filteredTrees = filteredTrees.Where(t => 15 < t.DBH_cm && t.DBH_cm < 25);
            filteredTrees = FilterTreeBySpecies(filteredTrees, ["sm", "yb", "wp", "ro", "rs"]);

            int result = GetBasalArea(filteredTrees);
            var summaryItem = new SummaryItem()
            {
                DisplayName = "LIT 15 - 25 cm (sM / yB / wP / rO / rS)",
                Value = result,
                Units = "m²/ha"
            };
            return summaryItem;
        }


        public static SummaryItem GetMerchConifer_perc(IEnumerable<TreeLive> trees)
        {
            var summaryItem = new SummaryItem()
            {
                DisplayName = "Merchantable Conifer",
                Value = 0,
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
                    return trees.Where(t => t.PLInSitu).Count();
                case "exsitu":
                    return trees.Where(t => t.PLExSitu).Count();
                case "both":
                    return trees.Where(t => t.PLExSitu || t.PLInSitu).Count();
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
                DisplayName = "Planted Trees Ex Situ",
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
                DisplayName = "Planted Trees In Situ",
                Value = result,
                Units = "%"
            };
            return summaryItem;
        }

       private static IEnumerable<TreeLive> FilterTreeBySpecies(IEnumerable<TreeLive> trees,
           List<string> searchSpecies)
        {
            return trees.Where(t => searchSpecies.Contains(t.TreeLookup.ShortCode, StringComparer.OrdinalIgnoreCase));
        }

     
    }
}