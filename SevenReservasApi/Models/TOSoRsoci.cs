using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenReservas.Models
{
    public class TOSoRsoci
    {
        public short Emp_codi { get; set; }
        public string Mac_nume { get; set; }
        public string Sbe_ncar { get; set; }

        public string Sbe_mail { get; set; }
        public string Sbe_ncel { get; set; }
        public int Soc_cont { get; set; }
        public int Sbe_cont { get;  set; }
        public int Soc_cing { get; set; }

        public string Sbe_pass { get; set; }

        public DateTime Soc_cfec { get; set; }

    }
}
