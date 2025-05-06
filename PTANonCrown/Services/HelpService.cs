using System.Reflection;

namespace PTANonCrown.Services
{
    public class HelpService
    {

        private readonly Dictionary<string, string> _helpTexts = new()
    {
        { "PlotNumber", "Unique identifier of the Plot." },
        { "TreeNumber", "Unique identifier of the tree within the plot." },
        { "CWMSpecies", "Softwood (SW), hardwood (HW), or unknown species." },
        { "CWMCavity", "Coarse Woody Material pieces that contain cavities." },
        { "CWMDBH", "Measure CWM pieces that are greater than 20 cm in diameter " +
                "where the transect crosses the central axis or pith of the piece. " +
                "Diameter is measured perpendicular to the pith, not along the " +
                "transect line. CWM pieces are measured in 20-cm diameter classes " +
                "(e.g., 21-40 cm)." },
        { "SnagCavity", "Dead trees (snags) that contain cavities." },
        { "CWMGeneral", "Coarse woody material (CWM) that is measured in " +
                "a straight transect. A dead tree is considered to be Coarse Woody Material " +
                "If it is at less than 45 degrees. Dead trees that are standing at " +
                "greater than 45 degrees are considered snags." },
        { "SnagDBH", "Snag diameter at 1.3 m. Only tally snags that are greater than " +
                "20 cm DBH. Snags are tallied in 10-cm diameter classes (e.g., 21-30 cm)." },
        { "SnagSpecisGroup", "Softwood (SW), hardwood (HW), or unknown species." },
        { "DeadwoodGeneral", "The deadwood information includes both standing dead trees " +
                "(i.e., snags) that are tallied in the prism sweep and coarse woody " +
                "material (CWM) that is measured in a separate transect. " +
                "The transect should be a straight, 20-m line in a consistent direction" +
                "(e.g., always north). A dead tree is considered a snag if it is " +
                "standing at a 45 degrees or greater and is Coarse Woody Material when under 45 degrees." },
        { "Mast", "Mast trees are those that contain fruit. The best mast trees" +
                " include red oak and beech, but may also include beaked hazelnut," +
                " wild apple trees and large cone-bearing conifers." },
        { "Diversity", "Trees is unusual in context, representing a small portion of " +
                "the stand, or providing for diversity within the stand " +
                "(e.g. ironwood tree, black cherry, or a large yellow birch" +
                " in a softwood stand)." },
        { "Cavity", "Live or dead trees that contain hollows (or cavities) " +
                "in the trunk or limbs, or that show signs of decay that may lead " +
                "to the formation of cavities that would be used by wildlife for " +
                "nesting and reproduction. This includes large trees with cavities " +
                "at the base that will provide den and overwintering sites for mammals." },
        { "AgeSpecies", "Species of the tree selected for aging." },
        { "AGS", "Trees are acceptable or unacceptable growing stock, " +
                "based on the definitions in Appendix 1 of the SGEM." },
        { "PredictedHeight", "Height predicted based on the DBH of the tree." },
        { "Height", "Total tree height (m), in 1-m classes. " +
                "It is acceptable to use predicted heights from models that predict tree height" +
                " based on DBH and/or site conditions." },
        { "DBH", "Diameter at Breast Height. Tree diameter (cm) at 1.3 m." +
                " Tally all living trees and dead trees greater than 20 cm DBH within the prism sweep, " +
                "though only living merchantable trees (i.e., greater than 9 cm DBH) are counted towards" +
                "the growing stock of the plot. Trees can be tallied in 2-cm diameter classes " +
                "(e.g., 10, 12, 14). For example, the 20-cm class would include any tree between 19.1-21 cm DBH." },
        { "Species", "Note that only commercial species will count towards the" +
                " growing stock of the plot but all species should be recorded. " +
                " Being able to identify long-lived, intermediate-to-tolerant (LIT) and " +
                " long-lived, tolerant (LT) species is an essential component of PTA. " +
                " See Table 1 of the SGEM below that describes the silvics of common Nova Scotia " +
                " trees. If the tree is planted, identify whether it is in situ or ex situ," +
                " as described in the SGEM and FEC Field Guide." },
        { "AgeGeneral", "A minimum of one tree must be cored with an increment borer and aged for each PTA" +
                " plot. If the plot is not suspected to be old growth, the age tree is selected as a " +
                "codominant tree of average diameter (e.g., the quadratic mean diameter of the plot). " +
                "The most abundant, merchantable species should be used and LIT species are preferred. " +
                "If old growth is suspected, the tree selected to age should be from the most dominant " +
                "LIT/LT species in the plot and should be representative of the top 20% of the basal area." },
        { "Age", "The breast height age of the sampled tree." },
        { "AgeDBH", "Diameter at 1.3 m of the tree selected for aging." },
        { "LT", "Long-Lived Tolerant species (LT) are shade-tolerant species that regenerate" +
                " readily under heavy shade." },
        { "LIT", "Long-Lived Intermediate–Tolerant species (LIT) are shade " +
                "intermediate-tolerant that predominate in late succession stands under " +
                "light disturbances." },
        { "StandNumber", "Unique identifier of the Stand." },
        { "StandNorthing", "Approximate Northing of the stand." },
        { "StandEasting", "Approximate Easting of the stand." },
        { "StandComments", "Comments relating to this stand." },
        { "PlotComments", "Comments relating to this plot." },
        { "CruiseID", "A unique identification number for a given PTA. " +
                "For example, the two letter county code, followed by the last two digits of the year collected, followed by a four-number unique identifier." },
        { "Ecodistrict", "The ecodistrict where the PTA is situated. See Nova Scotia’s Ecological Land Classification (ELC) for details. Ecoregion is needed to determine nutrient-sustainable harvest levels, along with the FEC information described below." },
        { "PlannerID", "Name or identification number of PTA practitioner." },
        { "Easting", "Easting coordinate." },
        { "Northing", "Northing coordinate." },
        { "Organization", "Organization name (e.g. company name, etc.)" },
        { "Location", "Description of the location (e.g., nearest community)." },
        { "StockingBeechRegeneration", "The stocking of beech regeneration greater than 25%. " +
                "The highly shade-tolerant beech seedlings and suckers outcompete " +
                "other hardwoods but are susceptible to beech bark disease and are not preferred growing stock." },
        { "SWLITRegen", "Softwood regeneration height is within the suitable window for pre-commercial thinning." },
        { "HWLITRegen", "Hardwood regeneration height is within the suitable window for pre-commercial thinning." },
        { "StockingRegenCommercialSpecies", "Percent stocking of all commercial tree\r\nspecies in 10% stocking classes." } ,
        { "StockingRegenLITSpecies", "Percent stocking of all LIT species in 10% stocking\r\nclasses." },
        { "UnderStoryDominatedBy", "Understory dominated by trees or woody shrubs." },

            { "Blowdown", "Percent blowdown, by basal area. Blowdown includes wind damaged trees that are uprooted, have broken stems, or are leaning at an angle greater than 15o from vertical." } ,

            { "StockingLITSeedTree", "Percent stocking of LIT trees of seed-bearing age (Table 1) at 20-m spacing." } ,
        { "UnevenAged", "____" } ,
        { "OneCohortSenescent", "At least one cohort is past the onset age of senescence (Table 1). " } ,
        { "HorizontalStructure", "The horizontal structure of the stand, recorded as either\r\npatchy or uniform. For example, a spruce-fir stand might have patches of balsam\r\nfir surrounded by red spruce or the fir might be uniformly mixed with the spruce\r\nacross the stand." } ,
        { "UnderstoryStrata", "Percent cover of woody plants in the understory between 1 and 3 m in height, recorded as either trees or shrubs." } ,
        { "UnderstoryDominated", "____" } ,
        { "AverageSampleTreeSpecies", "Species of the tree selected for aging." },
        { "AverageSampleTreeAge", "The breast height age of the sampled tree." } ,
        { "AverageSampleTreeDBH_cm", "____" } ,
        { "OGFSampleTreeSpecies", "The breast height age of the old growth sampled tree." } ,
        { "OGFSampleTreeDBH_cm", "Diameter at 1.3 m of the tree selected for aging." },
        { "FECSoil", "The FEC soil type, including any phase (e.g., loamy, stony). " +
                "Record whether the soil type is an inclusion or generally representative of the stand." +
                " It is also useful to record whether the soil type was identified through direct" +
                " observation or was inferred because measurement was not possible " +
                " (e.g.,frozen-ground conditions)." },
        { "FECVeg", "The FEC vegetation type. Record whether the vegetation type" +
                " is an inclusion or generally representative of the stand. If the vegetation type is" +
                " within the Planted Forest Group, record whether the plot is located on an" +
                " Acadian, Maritime Boreal, or Coastal site." }

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