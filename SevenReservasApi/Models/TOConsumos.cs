using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SevenReservas.Models
{
    public class TOConsumos
    {
        public string Res_nume { get; set; }
        public string Pro_codi { get; set; }
        public string Pro_nomb { get; set; }
        public double Dvt_valo { get; set; }
        public DateTime Con_fech { get; set; }


        public List<TOConsumos> lConsumos = new List<TOConsumos>();
    }
}