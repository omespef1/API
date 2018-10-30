using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenReservas.Models
{
    public class TOSoSocio
    {
        public string Soc_nomb { get; set; }
        public string Soc_apel { get; set; }
        public string Soc_tele { get; set; }
        public string Mac_nume { get; set; }
        public string Soc_pass { get; set; }
        public int Soc_cont { get; set; }
        public int Sbe_cont { get; set; }
        public string Sbe_codi { get; set; }
        public short Emp_codi { get; set; }
        public byte[] Soc_foto { get; set; }
        public DateTime Sbe_fexp { get; set; }
        public string Sbe_ncar { get; set; }
        public string Mac_nume1 { get; set; }
        public string Soc_ncar { get; set; }
        public string Sbe_mail { get; set; }
        public string Sbe_ncel { get; set; }

        public string Sbe_dire { get; set; }

        public string Emp_tele { get; set; }
        public string Emp_nite { get; set; }

        public string soc_ncar { get; set; }    

        public List<TOSoSocio> beneficiarios = new List<TOSoSocio>();

    }
}
