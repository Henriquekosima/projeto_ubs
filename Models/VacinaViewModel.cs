using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UBS_mvc.Models
{
    public class VacinaViewModel
    {
        public int VacinaId { get; set; }
        public string VacinaName { get; set; }
        public List<DoseViewModel> Doses { get; set; }
    }
}
