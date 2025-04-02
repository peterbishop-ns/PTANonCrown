using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PTANonCrown.Services;

using static PTANonCrown.Services.HelpService;

namespace PTANonCrown.Models
{
    public class Plot :BaseModel
    {
        public int ID { get; set; }


        private int _plotNumber;
        public int PlotNumber
        {
            get => _plotNumber;
            set
            {
                if (_plotNumber != value)
                {
                    _plotNumber = value;
                    OnPropertyChanged();
                }

            }
        }


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


        private ObservableCollection<TreeLive> _treeLive;
        private ObservableCollection<TreeDead> _treeDead;
        private ObservableCollection<CoarseWoody> _coarseWoody;


        public ObservableCollection<TreeLive> TreeLive
        {
            get => _treeLive;
            set
            {
                if (_treeLive != value)
                {
                    _treeLive = value;
                    OnPropertyChanged();
                }

            }
        }

        public ObservableCollection<TreeDead> TreeDead
        {
            get => _treeDead;
            set
            {
                if (_treeDead != value)
                {
                    _treeDead = value;
                    OnPropertyChanged();
                }

            }
        }

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

        public Stand Stand { get; set; }

        public Plot()
        {
            TreeLive = new ObservableCollection<TreeLive>();
            TreeDead = new ObservableCollection<TreeDead>();
            CoarseWoody = new ObservableCollection<CoarseWoody>();
        }


        //public int PreviousTreatments { get; set; } 
        //TODO - one-to-many? 

    }
}
