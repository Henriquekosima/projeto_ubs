using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UBS_mvc.Models
{
    public class AtendenteViewModel
    {
        public int AttendantID { get; set; }
        public string AttendantCpf { get; set; }
        public string AttendantEmail { get; set; }
        public string AttendantName { get; set; }
        public string AttendantPass { get; set; }
    }
}
