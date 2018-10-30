using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SevenReservas.Models
{
    public class TOAeClase
    {
        public short Emp_codi { get; set; }
        public int Cla_cont { get; set; }
        public string Cla_codi { get; set; }
        public string Cla_nomb { get; set; }
        public string Cla_clti { get; set; }
        public int Lip_cont { get; set; }
        public byte[] Cla_foto { get; set; }

        public DateTime? Cla_Fchr { get; set; }
        public int? Cla_Tica { get; set; }
        public string Cla_desc { get; set; }
    }
}