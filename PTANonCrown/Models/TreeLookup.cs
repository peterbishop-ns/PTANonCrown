using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTANonCrown.Models
{
    public class TreeLookup : BaseModel
    {
        public int ID { get; set; }
        public string ShortCode { get; set; }
        public string Name { get; set; }

        public virtual string DisplayName => $"{ShortCode} - {Name}";
       
    }
}
