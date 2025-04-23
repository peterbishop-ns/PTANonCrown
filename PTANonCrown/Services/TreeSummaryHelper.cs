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
            result.Add(GetQMDMerchTrees_count(trees));
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
            result.Add(GetLIT_sM_yB_wP_rO_rS_10to25_cm(trees));

            return result;
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

        public static SummaryItem GetAGS_m2ha(IEnumerable<TreeLive> trees)
        {
            var summaryItem = new SummaryItem()
            {
                DisplayName = "AGS",
                Value = 0,
                Units = "m²/ha"
            };
            return summaryItem;
        }

        public static SummaryItem GetAverageHeight(IEnumerable<TreeLive> trees)
        {
            var summaryItem = new SummaryItem()
            {
                DisplayName = "Average Height",
                Value = 0,
                Units = "m"
            };
            return summaryItem;
        }

        public static SummaryItem GetBasalArea_EH_RS_BF(IEnumerable<TreeLive> trees)
        {
            var summaryItem = new SummaryItem()
            {
                DisplayName = "Basal Area (EH / RS / BF)",
                Value = 0,
                Units = "m²/ha"
            };
            return summaryItem;
        }

        public static SummaryItem GetBasalArea_gt25cm_m2ha(IEnumerable<TreeLive> trees)
        {
            var summaryItem = new SummaryItem()
            {
                DisplayName = "Basal Area > 25cm",
                Value = 0,
                Units = "m²/ha"
            };
            return summaryItem;
        }

        public static SummaryItem GetBasalArea_WP_RO(IEnumerable<TreeLive> trees)
        {
            var summaryItem = new SummaryItem()
            {
                DisplayName = "Basal Area (WP / RO)",
                Value = 0,
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

        public static SummaryItem GetLIT_sM_yB_wP_rO_rS_10to25_cm(IEnumerable<TreeLive> trees)
        {
            var summaryItem = new SummaryItem()
            {
                DisplayName = "LIT 10 - 25 cm (sM / yB / wP / rO / rS)",
                Value = 0,
                Units = "m²/ha"
            };
            return summaryItem;
        }

        public static SummaryItem GetLITBasalArea_m2ha(IEnumerable<TreeLive> trees)
        {
            var summaryItem = new SummaryItem()
            {
                DisplayName = "LIT Basal Area",
                Value = 0,
                Units = "m²/ha"
            };
            return summaryItem;
        }

        public static SummaryItem GetLTBasalArea_m2ha(IEnumerable<TreeLive> trees)
        {
            var summaryItem = new SummaryItem()
            {
                DisplayName = "LT Basal Area",
                Value = 0,
                Units = "m²/ha"
            };
            return summaryItem;
        }

        public static SummaryItem GetMerchantableBasalArea_m2ha(IEnumerable<TreeLive> trees)
        {

            var summaryItem = new SummaryItem()
            {
                DisplayName = "Merchantable Basal Area",
                Value = 0,
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

        public static SummaryItem GetPlantedTreesExSitu_perc(IEnumerable<TreeLive> trees)
        {
            var summaryItem = new SummaryItem()
            {
                DisplayName = "Planted Trees Ex Situ",
                Value = 0,
                Units = "%"
            };
            return summaryItem;
        }

        public static SummaryItem GetPlantedTreesInSitu_perc(IEnumerable<TreeLive> trees)
        {
            var summaryItem = new SummaryItem()
            {
                DisplayName = "Planted Trees In Situ",
                Value = 0,
                Units = "%"
            };
            return summaryItem;
        }

        public static SummaryItem GetQMDMerchTrees_count(IEnumerable<TreeLive> trees)
        {
            var summaryItem = new SummaryItem()
            {
                DisplayName = "QMD Merchantable Trees",
                Value = 0,
                Units = "m²/ha"
            };
            return summaryItem;
        }

        public static SummaryItem GetTotalBasalArea(IEnumerable<TreeLive> trees)
        {
            // Twice the count of all trees
            int result = trees.Count() * 2;
            var summaryItem = new SummaryItem()
            {
                DisplayName = "Total Basal Area",
                Value = result,
                Units = "m²/ha"
            };
            return summaryItem;
        }
    }
}