using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTANonCrown.Models
{
    public class ItemTally : BaseModel
    {
        public int ID { get; set; }
        public int PlotID { get; set; }
        public int DBH_start { get; set; }
        public int DBH_end { get; set; }

        public string DBH_range { get
            {
                return $"{DBH_start} - {DBH_end}";
            } 
        }

        private int _tally_Softwood;
        public int Tally_Softwood
        {
            get => _tally_Softwood;
            set
            {
                if (_tally_Softwood != value)
                {
                    _tally_Softwood = value;
                    OnPropertyChanged();
                }

            }
        }

        private int _tally_Hardwood;
        public int Tally_Hardwood
        {
            get => _tally_Hardwood;
            set
            {
                if (_tally_Softwood != value)
                {
                    _tally_Hardwood = value;
                    OnPropertyChanged();
                }

            }
        }
        private int _tally_Unknown;
        public int Tally_Unknown
        {
            get => _tally_Unknown;
            set
            {
                if (_tally_Unknown != value)
                {
                    _tally_Unknown = value;
                    OnPropertyChanged();
                }

            }
        }

        private int _tally_Cavity;
        public int Tally_Cavity
        {
            get => _tally_Cavity;
            set
            {
                if (_tally_Cavity != value)
                {
                    _tally_Cavity = value;
                    OnPropertyChanged();
                }

            }
        }
        public Plot Plot { get; set; }

    }
}
