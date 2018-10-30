using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SevenReservas.Models
{
    public class TODisponible
    {
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int esp_cont { get; set; }
        public short Estado { get; set; }
       
    }

    public class TONoHorDisponible
    {      
        public decimal TER_CODI { get; set; }
    }

}