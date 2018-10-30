using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SevenFramework.DataBase;

namespace SevenReservas.Models
{
    [TableName("PV_MESSL")]
    public class pv_messl
    {
        public short EMP_CODI { get; set; }
        public long MES_CONT { get; set; }
        public int MES_NUME { get; set; }
        public long ITE_CONT { get; set; }
        public long BOD_CONT { get; set; }
        public string ITE_CODI { get; set; }
    }
}