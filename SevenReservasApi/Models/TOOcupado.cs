using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SevenReservas.Models
{
    public class TOOcupado
    {
        public int Esp_cont { get; set; }
        public string Esp_codi { get; set; }
        public string Esp_nomb { get; set; }
        public string Esp_esta { get; set; }
        public int Res_cont { get; set; }
        public DateTime Res_fini { get; set; }
        public DateTime Res_fina { get; set; }
        public string Res_esta { get; set; }
        public int Res_colo { get; set; }
        public int Res_even { get; set; }
        public int Res_opci { get; set; }
        public string Res_tipo { get; set; }
        public string Res_desc { get; set; }

    }
}