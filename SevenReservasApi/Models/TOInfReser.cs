using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SevenReservas.Models
{
    public class TOInfReser
    {
        public string Cla_nomb { get; set; }
        public byte[] Cla_foto { get; set; }
        public byte [] Esp_imag { get; set; }
        public string Esp_nomb { get; set; }
        public string Pro_nomb { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int Res_cont { get; set; }
        public int Res_nume { get; set; }
        public string Res_esta { get; set; }
        public string Res_vige { get; set; }
        public string Esp_mdit { get; set; }

        public int Cla_cont { get; set; }
        public int? Cla_tica { get; set; }
        public int Ter_codi { get; set; }
        public string Ter_noco { get; set; }
        public  byte[] Ter_foto { get; set; }


    }
}