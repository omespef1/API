using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DTOEntities
{
    public class TOInvitado
    {
        public short Emp_Codi { get; set; }
        public int Sbe_Cont { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public DateTime Fecha { get; set; }
        public string Mac_nume { get; set; }
        public int Soc_cont { get; set; }
        public string Observacion { get; set; }
        public string Sbe_codi { get; set; }
    }
}