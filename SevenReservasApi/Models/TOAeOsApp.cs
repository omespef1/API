using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SevenReservas.Models
{
    public class TOAeOsApp
    {
        public short Emp_Codi { get; set; }
        public int Osa_Cont { get; set; }
        public string Osa_Nomb { get; set; }
        public byte[] Osa_Bmpr { get; set; }
        public string Osa_Link { get; set; }
    }
}