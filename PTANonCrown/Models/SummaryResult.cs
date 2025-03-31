using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;


namespace PTANonCrown.Models
{
    public class SummaryResult
    {
        public int ID { get; set; }
        public int PlotID { get; set; }
        public int TotalBasalArea_m2ha{ get; set; }
        public int MerchantableBasalArea_m2ha{ get; set; }
        public int LITBasalArea_m2ha{ get; set; }
        public int LTBasalArea_m2ha{ get; set; }
        public int QMDMerchTrees_count{ get; set; }
        public int AGS_m2ha{ get; set; }
        public int AGS_LIT_m2ha{ get; set; }
        public int BasalArea_gt25cm_m2ha{ get; set; }
        public int AverageHeight{ get; set; }
        public int PlantedTreesInSitu_perc{ get; set; }
        public int PlantedTreesExSitu_perc{ get; set; }
        public int MerchConifer_perc{ get; set; }
        public int DeciduousLIT_perc{ get; set; }
        public int BasalArea_WP_RO{ get; set; }
        public int BasalArea_EH_RS_BF{ get; set; }
        public int LIT_sM_yB_wP_rO_rS_10to25_cm{ get; set; }

        public SummaryResult(ObservableCollection<TreeLive> trees)
        {
            TotalBasalArea_m2ha = GetTotalBasalArea(trees);
            MerchantableBasalArea_m2ha = GetMerchantableBasalArea_m2ha(trees);
            LITBasalArea_m2ha = GetLITBasalArea_m2ha(trees);
            LTBasalArea_m2ha = GetLTBasalArea_m2ha(trees);
            QMDMerchTrees_count = GetQMDMerchTrees_count(trees);
            AGS_m2ha = GetAGS_m2ha(trees);
            AGS_LIT_m2ha = GetAGS_LIT_m2ha(trees);
            BasalArea_gt25cm_m2ha = GetBasalArea_gt25cm_m2ha(trees);
            AverageHeight = GetAverageHeight(trees);
            PlantedTreesInSitu_perc = GetPlantedTreesInSitu_perc(trees);
            PlantedTreesExSitu_perc = GetPlantedTreesExSitu_perc(trees);
            MerchConifer_perc  = GetMerchConifer_perc(trees);
            DeciduousLIT_perc = GetDeciduousLIT_perc(trees);
            BasalArea_WP_RO = GetBasalArea_WP_RO(trees);
            BasalArea_EH_RS_BF = GetBasalArea_EH_RS_BF(trees);
            LIT_sM_yB_wP_rO_rS_10to25_cm = GetLIT_sM_yB_wP_rO_rS_10to25_cm(trees);

        }
        public int GetTotalBasalArea(ObservableCollection<TreeLive> trees)
        {
            // Twice the count of all trees
            int result = trees.Count() * 2;
            return result;
        }

        public int GetMerchantableBasalArea_m2ha(ObservableCollection<TreeLive> trees)
        {
            // Twice the count of all trees that are Merchantable (defined as > 9 cm DBH)
            int result = trees.Where(t => t.DBH_cm > 9).Count() * 2;
            return 0;
        }
        public int GetLITBasalArea_m2ha(ObservableCollection<TreeLive> trees)
        {
            // Twice the count of all LIT trees 
            // LIT trees to be defined in database
            int result = trees.Where(t => t.LIT == true).Count() * 2;

            return 0;
        }
        public int GetLTBasalArea_m2ha(ObservableCollection<TreeLive> trees)
        {
            // Twice the count of all Legacy Trees
            int result = trees.Where(t => t.Legacy == true).Count() * 2;
            return 0;
        }
        public int GetQMDMerchTrees_count(ObservableCollection<TreeLive> trees)
        {
            // SQRT ((Average BA/ Tree(Merch Trees)) / 0.00007854)
            return 0;
        }
        public int GetAGS_m2ha(ObservableCollection<TreeLive> trees) { return 0; }
         public int GetAGS_LIT_m2ha(ObservableCollection<TreeLive> trees) { return 0; }
        public int GetBasalArea_gt25cm_m2ha(ObservableCollection<TreeLive> trees) { return 0; }
        public int GetAverageHeight(ObservableCollection<TreeLive> trees) { return 0; }
        public int GetPlantedTreesInSitu_perc(ObservableCollection<TreeLive> trees) { return 0; }
        public int GetPlantedTreesExSitu_perc(ObservableCollection<TreeLive> trees) { return 0; }
        public int GetMerchConifer_perc(ObservableCollection<TreeLive> trees) { return 0; }
        public int GetDeciduousLIT_perc(ObservableCollection<TreeLive> trees) { return 0; }
        public int GetBasalArea_WP_RO(ObservableCollection<TreeLive> trees) { return 0; }
        public int GetBasalArea_EH_RS_BF(ObservableCollection<TreeLive> trees) { return 0; }
        public int GetLIT_sM_yB_wP_rO_rS_10to25_cm(ObservableCollection<TreeLive> trees) { return 0; }
    }




}
