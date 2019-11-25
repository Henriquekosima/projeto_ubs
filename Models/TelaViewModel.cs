using System;
using System.Collections.Generic;

namespace UBS_mvc.Models
{
    public class TelaViewModel
    {
        public ResponsavelViewModel Responsavel { get; set; }
        public List<VacinaViewModel> Vacinas { get; set; }
    }
}
