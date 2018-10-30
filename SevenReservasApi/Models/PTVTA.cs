using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SevenFramework.DataBase;

namespace SevenReservas.Models
{
    [TableName("sp_ptvta")]
    public class PTVTA
    {
        public short emp_codi { get; set; }
        public decimal ptv_cont { get; set; }
        public string ptv_codi { get; set; }
        public string ptv_nomb { get; set; }
        public string ptv_rdia { get; set; }
        public string ter_coda { get; set; }
        public string ter_nomb { get; set; }
        public string ptv_esta { get; set; }
        public string ptv_dire { get; set; }
        public long ptv_nmei { get; set; }
        public long ptv_nmef { get; set; }

        [NotMapped]
        public long numMesas { get; set; }
        [NotMapped]
        public long totaMesas { get; set; }
    }
}