using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace PTANonCrown.Data.Models
{
    // Special kind of lookup that has more logic tied to it
    public class TreeSpecies : BaseLookup
    {
        public HardwoodSoftwood HardwoodSoftwood { get; set; }
        public bool LIT { get; set; }
        public bool LIT_planted { get; set; }
        public bool LT { get; set; }

        [NotMapped]
        public string DisplayName { get => $"{Name} - {ShortCode}"; }
        // Navigatioon property to all the trees that use this TreeSpecies
        public List<TreeLive> TreeLives { get; set; }

    }
}
