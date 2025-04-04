using System.Reflection;

namespace PTANonCrown.Services
{
    public class HelpService
    {

        private readonly Dictionary<string, string> _helpTexts = new()
    {
        { "PlotID", "Unique identifier of the Plot." },
        { "StandID", "Unique identified of the Stand." },
        { "CruiseID", "A unique identification number for a given PTA. For example, the two letter county code, followed by the last two digits of the year collected, followed by a four-number unique identifier." },
        { "Ecodistrict", "The ecodistrict where the PTA is situated. See Nova Scotia’s Ecological Land Classification (ELC) for details. Ecoregion is needed to determine nutrient-sustainable harvest levels, along with the FEC information described below." },
        { "PlannerID", "Name or identification number of PTA practitioner." },
        { "Easting", "Easting coordinate." },
        { "Northing", "Northing coordinate." },
        { "Organization", "Organization name" },
        { "Location", "____" },
        { "StockingBeechRegeneration", "The stocking of beech regeneration greater\r\nthan 25%. The highly shade-tolerant beech seedlings and suckers outcompete\r\nother hardwoods but are susceptible to beech bark disease and are not preferred\r\ngrowing stock." },
        { "RegenHeightSWLIT", "Softwood regeneration height is\r\nwithin the suitable window for pre-commercial thinning." },
        { "RegenHeightHWLIT", "Hardwood regeneration height is\r\nwithin the suitable window for pre-commercial thinning." },
        { "StockingRegenCommercialSpecies", "Percent stocking of all commercial tree\r\nspecies in 10% stocking classes." } ,
        { "StockingRegenLITSpecies", "Percent stocking of all LIT species in 10% stocking\r\nclasses." },

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
        { "OGFSampleTreeDBH_cm", "Diameter at 1.3 m of the tree selected for aging." }

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