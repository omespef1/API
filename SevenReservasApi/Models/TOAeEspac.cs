using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SevenReservas.Models
{
    public class TOAeEspac
    {
        public short Emp_codi { get; set; }
        public int Esp_cont { get; set; }
        public string Esp_codi { get; set; }
        public string Esp_nomb { get; set; }
        public int Cla_cont { get; set; }
        public int Tip_cont { get; set; }
        public string Esp_desc { get; set; }
        public byte[] Esp_imag { get; set; }
        public string Esp_mdit { get; set; }

        public List<TOGnTerce> lTerceros = new List<TOGnTerce>();



    }
}