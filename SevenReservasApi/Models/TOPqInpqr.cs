using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SevenReservas.Models
{
    public class TOPqInpqr
    {

        public int emp_codi { get; set; }
        public int inp_cont { get; set; }
        public DateTime inp_feve { get; set; }
        public string inp_esta { get; set; }
        public string arb_csuc { get; set; }
        public string inp_tcli { get; set; }
        public string inp_ncar { get; set; }
        public int ite_frec { get; set; }
        public int ite_tpqr { get; set; }
        public string arb_ccec { get; set; }
        public int ite_spre { get; set; }
        public int ite_ancu { get; set; }
        public string inp_mpqr { get; set; }
        public string sbe_ncar { get; set; }
        public int soc_cont { get; set; }
        public int sbe_cont { get; set; }
        public string mac_nume { get; set; }
        public string desplegar { get; set; }
        public bool open { get; set; }
        public List<TOPqDinpq> seguimientos { get; set; }

    }
}