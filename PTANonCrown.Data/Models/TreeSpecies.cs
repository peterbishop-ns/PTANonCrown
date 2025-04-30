using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTANonCrown.Data.Models
{
    // Special kind of lookup that has more logic tied to it
    public class TreeSpecies : BaseLookup
    {
        public int HardwoodSoftwood { get; set; }
        public bool LIT { get; set; }
        public bool LIT_planted { get; set; }
        public bool LT { get; set; }

        // Navigatioon property to all the trees that use this TreeSpecies
        public List<TreeLive> TreeLives { get; set; }

    }
}
