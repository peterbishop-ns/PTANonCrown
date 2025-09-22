using System.Reflection;

namespace PTANonCrown.Services
{
    public class HelpService
    {

        private readonly Dictionary<string, string> _helpTexts = new()
    {
        { "AgeGeneral", " A minimum of one tree should be cored with an increment borer and aged for each PTA plot. If the plot is not suspected to be old growth, the age tree is selected as a codominant tree of average diameter (e.g., the quadratic mean diameter of the plot). The most abundant, merchantable species should be used and LIT species are preferred. If old growth is suspected, the tree selected to age should be from the most dominant LIT/LT species in the plot and should be representative of the top 20% of the basal area. Refer to the old-growth scoring procedures." },
        { "AgeTreeAge", "The breast height age of the sampled tree." },
        { "AgeTreeDBH", "Diameter at 1.3 m of the tree selected for aging." },
        { "AgeTreeSpecies", "Species of the tree selected for aging." },
        { "OldGrowthAge", "The breast height age of the sampled Old Growth tree." },
        { "OldGrowthPresent", "Plot contains, or is suspected to contain, Old Growth trees." },
        { "OldGrowthDBH", "Diameter at 1.3 m of the Old Growth tree selected for aging." },
        { "OldGrowthSpecies", "Species of the Old Growth tree selected for aging." },
        { "AGS", "Trees are acceptable or unacceptable growing stock, based on the definitions in Appendix 1 of the SGEM." },
        { "Blowdown", " Percent blowdown by basal area in 10% classes. Blowdown includes wind damaged trees that are uprooted, have broken stems, or are leaning at an angle greater than 15 degrees from vertical." } ,
        { "Cavity", "Live or dead trees that contain hollows (or cavities) in the trunk or limbs, or that show signs of decay that may lead to the formation of cavities that would be used by wildlife for nesting and reproduction. This includes large trees with cavities at the base that will provide den and overwintering sites for mammals." },
        { "CoarseWoodySpecisGroup", "Softwood (SW), hardwood (HW), or unknown species." },
        { "CWMTransectDirection", "Direction of the transect used to measure coarse woody material." },
        { "CWMTransectLength", " Length of the transect used to measure coarse woody material. A minimum of 20 m is recommended." },
        { "CruiseID", "A unique identification number for a given PTA. For example, the two letter county code, followed by the last two digits of the year collected, followed by a four-number unique identifier." },
        { "CWMCavity", "Coarse Woody Material pieces that contain cavities." },
        { "CWMDBH", "Measure CWM pieces that are greater than 20 cm in diameter where the transect crosses the central axis or pith of the piece. Diameter is measured perpendicular to the pith, not along the transect line. CWM pieces are measured in 20-cm diameter classes (e.g., 21-40 cm)." },
        { "CWMGeneral", " Coarse woody material (CWM) that is measured in a straight transect. A dead tree is considered to be Coarse Woody Material. If it is at less than 45 degrees from the horizontal. Dead trees that are standing at greater than 45 degrees are considered snags." },
        { "CWMSpeciesGroup", "Softwood (SW), hardwood (HW), or unknown species." },
        { "DBH", "Diameter at Breast Height. Tree diameter (cm) at 1.3 m. Tally all living trees and dead trees greater than 20 cm DBH within the prism sweep, though only living merchantable trees (i.e., greater than 9 cm DBH) are counted towards the growing stock of the plot. Trees can be tallied in 2-cm diameter classes (e.g., 10, 12, 14). For example, the 20-cm class would include any tree between 19.1-21 cm DBH." },
        { "DeadwoodGeneral", " The deadwood information includes both standing dead trees (i.e., snags) that are tallied in the prism sweep and coarse woody material (CWM) that is measured in a separate transect. The transect should be a straight, 20-m line in a consistent direction (e.g., always north). A dead tree is considered a snag if it is standing at a 45 degrees or greater from the horizontal and is Coarse Woody Material when under 45 degrees." },
        { "Diversity", "Trees is unusual in context, representing a small portion of the stand, or providing for diversity within the stand (e.g. ironwood tree, black cherry, or a large yellow birch in a softwood stand). Column is active if Include Biodiversity is enabled." },
        { "Easting", "Easting coordinate." },
        { "Ecodistrict", "The ecodistrict where the PTA is situated. See Nova Scotia’s Ecological Land Classification (ELC) for details. Ecoregion is needed to determine nutrient-sustainable harvest levels, along with the FEC information described below." },
        { "EcositeGroup", "Forest Ecosystem Classification (FEC) Ecosite Group." },
        { "Exposure", 
                "Exposure refers to the relative openness of a site to weather conditions, particularly wind. Exposure can affect moisture conditions on a site and severely impact the windthrow hazard of stands." },
        { "ExportAll", "Select a folder and export a stand and plot summary in Microsoft Excel format." },
        { "FECSoil", "The Forest Ecosystem Classification (FEC) soil type, including any phase (e.g., loamy, stony). Record whether the soil type is an inclusion or generally representative of the stand. It is also useful to record whether the soil type was identified through direct observation or was inferred because measurement was not possible (e.g.,frozen-ground conditions)." },
        { "FECVeg", " The Forest Ecosystem Classification (FEC) vegetation type. Record whether the vegetation type is an inclusion or generally representative of the stand. If the vegetation type is within the Planted Forest Group, record whether the plot is located on an Acadian, Maritime Boreal, or a Coastal site within the Maritime Boreal." },
        { "Height", "Total tree height (m), in 1-m classes. It is acceptable to use predicted heights from models that predict tree height based on DBH and/or site conditions." },
        { "HorizontalStructure", "The horizontal structure of the stand, recorded as either patchy or uniform. For example, a spruce-fir stand might have patches of balsam\r\nfir surrounded by red spruce or the fir might be uniformly mixed with the spruce\r\nacross the stand." } ,
        { "HWLITRegen", "Hardwood regeneration height is within the suitable window for pre-commercial thinning." },
        { "IncludeBiodiversity", "Choosing to include Biodiversity will activate the followoing columns: Cavity, Diversity, Mast." },
        { "LIT", "Long-Lived Intermediate–Tolerant species (LIT) are shade intermediate-tolerant that predominate in late succession stands under light disturbances." },
        { "LivingCavity", "Tree contains cavities. Column is active if Include Biodiversity is enabled." },
        { "Location", "Description of the location (e.g., nearest community)." },
        { "LT", "Long-Lived Tolerant species (LT) are shade-tolerant species that regenerate readily under heavy shade." },
        { "Mast", "Mast trees are those that contain fruit. The best mast trees include red oak and beech, but may also include beaked hazelnut, wild apple trees and large cone-bearing conifers. Column is active if Include Biodiversity is enabled." },
        { "Northing", "Northing coordinate." },
        { "NumberOfTreesInPlot", "Number of trees in the plot. Update and click 'Refresh' to add/remove trees." },
        { "LivingTreeNumber", "Tree Number. If a tree is deleted, trees will automatically be re-numbered to avoid a gap." },
        { "LivingTreeSpecies", "Tree species." },
        { "LivingTreeDBH", "Diameter at breast height (cm)." },
        { "LivingTreeAGS", "Acceptable Growing Stock." },
        { "LivingTreeLIT", "Long-lived, intermediate-to-tolerant species."},
        { "LivingTreeLT", " Long-lived, tolerant species." },
        { "LivingTreeMast", "Mast Tree." },
        { "LivingTreeCavity", "Wildlife cavity is present." },
        { "LivingTreeDiversity", "Uncommon species such as black cherry and ironwood." },
        { "OGFSampleTreeDBH_cm", "Diameter at 1.3 m of the tree selected for aging." },
        { "OGFSampleTreeSpecies", "The breast height age of the old growth sampled tree." } ,
        { "OneCohortSenescent", " At least one cohort is past the onset age of senescence. Refer to the silvics table in the SGEM for species-specific ages. " } ,
        { "Organization", "Organization name (e.g. company name, etc.)" },
        { "PlannerID", "Name or identification number of PTA practitioner." },
        { "PlantedGeneral", "Plot was planted. Select the type of plantation." },
        { "PlotComments", "Comments relating to this plot." },
        { "PlotEasting", "Approximate Easting of the plot." },
        { "PlotNorthing", "Approximate Northing of the plot." },
        { "PlotNumber", "Unique identifier of the Plot." },
        { "PlotNumberEdit", "Edit the unique identifier of the Plot." },
        { "PredictedHeight", "Height predicted based on the DBH of the tree. Click Use Predicted Heights to use these heights." },
        { "SnagCavity", "Dead trees (snags) that contain cavities." },
        { "SnagDBH", "Snag diameter at 1.3 m. Only tally snags that are greater than 20 cm DBH. Snags are tallied in 10-cm diameter classes (e.g., 21-30 cm)." },
        { "SnagSpecisGroup", "Softwood (SW), hardwood (HW), or unknown species." },
        { "Species", "Note that only commercial species will count towards the growing stock of the plot but all species should be recorded. Being able to identify long-lived, intermediate-to-tolerant (LIT) and long-lived, tolerant (LT) species is an essential component of PTA. If the tree is planted, identify whether it is in situ or ex situ, as described in the SGEM and FEC Field Guide." },
        { "StandArea", "Total area of Stand, in hectares." },
        { "StandComment", "Comments relating to this stand." },
        { "StandNumber", "Unique identifier of the Stand." },
        { "StandNumberEdit", "Edit the unique identifier of the Stand." },
        { "StockingBeechRegeneration", "The stocking of beech regeneration greater than 25%. The highly shade-tolerant beech seedlings and suckers outcompete other hardwoods but are susceptible to beech bark disease and are not preferred growing stock." },
        { "StockingLITSeedTree", " Percent stocking of LIT trees of seed-bearing age at 20-m spacing in 10% stocking classes." } ,
        { "StockingRegenCommercialSpecies", "Percent stocking of all commercial tree species at 2.4-m spacing in 10% stocking classes." } ,
        { "StockingRegenLITSpecies", "Percent stocking of all LIT species at 2.4-m spacing in 10% stocking classes." },
        { "SummaryPlot", "Generate summary at the Plot level. If trees are updated, please re-generate the Plot summary." },
        { "SummaryStand", "Generate summary at the Stand level. Combines all plots within the stand." },
        { "SWLITRegen", "Softwood regeneration height is within the suitable window for pre-commercial thinning." },
        { "TreatedGeneral", "Treatment(s) applied to current plot." },
        { "TreeNumber", "Unique identifier of the tree within the plot." },
        { "UnderStoryDominatedBy", "Understory dominated by trees or woody shrubs." },
        { "UnderstoryStrata", "Percent cover of woody plants in the understory between 1 and 3 m in height, recorded as either trees or shrubs." } ,
        { "UnevenAged", " Includes two-aged, multi-aged, and all-aged stands. Cohorts should be separated by at least 20 years." } ,
        { "UsePredictedHeights", "Set the heights of all trees to the heights in the 'Predicted Heights' column. Predicted heights are based on DBH/height relationships for hardwood and softwood trees." } ,

    };

        public string GetHelpText(string key)
        {
            return _helpTexts.TryGetValue(key, out var text) ? text : "No help available.";
        }

        public static class HelpTextRetriever
        {
            public static string? GetHelpText<T>(string propertyName)
            {
                var property = typeof(T).GetProperty(propertyName);
                var attribute = property?.GetCustomAttribute<HelpTextAttribute>();
                return attribute?.Text;
            }
        }

        [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
        public sealed class HelpTextAttribute : Attribute
        {
            public HelpTextAttribute(string text) => Text = text;

            public string Text { get; }
        }
    }
}