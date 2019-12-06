using System;
using System.Collections.Generic;

namespace UBS_mvc.Models
{
    public class ResponsavelViewModel
    {
        public int ResponsavelID { get; set; }
        public string ResponsavelName { get; set; }
        public string ResponsavelEmail { get; set; }
        public string ResponsavelCpf { get; set; }
        public List<DependenteViewModel> Dependentes { get; set; }
        public List<VacinaViewModel> Vacinas { get; set; }
    }
}
