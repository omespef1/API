using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SevenReservas.Models
{
    public class TOProducto
    {
        public int Pro_cont { get; set; }
        public string Pro_codi { get; set; }
        public string Pro_nomb { get; set; }
        public int Pro_dmin { get; set; }
        public byte[] Bmp_prod { get; set; }
        public int page { get; set; }
        public string esp_mdit { get; set; }
        public int esp_cont { get; set; }
        public string esp_nomb { get; set; }
    }
}