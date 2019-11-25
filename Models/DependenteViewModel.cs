using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UBS_mvc.Models
{
    public class DependenteViewModel
    {
        public int DependentID { get; set; }
        public string DependentName { get; set; }
        public DateTime DependentBirth { get; set; }
        public string DependentBlood { get; set; }
        public string DependentAllergy { get; set; }
        public string DependentSus { get; set; }
        public int ResponsavelID { get; set; }
        public List<VacinaViewModel> Vacinas { get; set; }
    }
}
