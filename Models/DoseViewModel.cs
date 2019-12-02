using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UBS_mvc.Models
{
    public class DoseViewModel
    {
        public int DoseID { get; set; }
        public string DoseType { get; set; }
        public int VacinaID { get; set; }
        public int DependenteID { get; set; }        
    }
}
