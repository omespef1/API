using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SevenReservas.Models
{
    public class sp_turno
    {
        public short emp_codi { get; set; }
        public long tur_cont { get; set; }
        public long tep_cont { get; set; }
        public long caj_cont { get; set; }
    }
}