using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PTANonCrown.Services;

using static PTANonCrown.Services.HelpService;

namespace PTANonCrown.Models
{
    class Plot
    {
        [HelpText("WOOHOO.")] 
        public int PlotID { get; set; }


        public int StandID { get; set; }

        public int StockingBeechRegeneration { get; set; }

        public bool RegenHeightSWLIT { get; set; }


        public bool RegenHeightHWLIT { get; set; }
        public int StockingRegenCommercialSpecies { get; set; }
        public int StockingRegenLITSpecies { get; set; }
        public int Blowdown { get; set; }
        public int StockingLITSeedTree { get; set; }
        public bool UnevenAged { get; set; }
        public bool OneCohortSenescent { get; set; }
        public int HorizontalStructure { get; set; }
        public int UnderstoryStrata { get; set; }
        public int UnderstoryDominated { get; set; }
        public int AverageSampleTreeSpecies { get; set; }
        public int AverageSampleTreeAge { get; set; }
        public int AverageSampleTreeDBH_cm { get; set; }        
        public int OGFSampleTreeSpecies { get; set; }
        public int OGFSampleTreeAge { get; set; }
        public int OGFSampleTreeDBH_cm { get; set; }
        //public int PreviousTreatments { get; set; } 
        //TODO - one-to-many? 

    }
}
