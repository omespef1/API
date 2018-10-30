using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOEntities
{
    public class TOAeReser
    {
        public short Emp_codi { get; set; }
        public DateTime Res_fini { get; set; }
        public DateTime Res_fina { get; set; }
        public int Soc_cont { get; set; }
        public string Mac_nume { get; set; }
        public int Sbe_cont { get; set; }
        public int Esp_cont { get; set; }
        public int Res_numd { get; set; }
        public int Ite_cont { get; set; }
        public double Ter_codi { get; set; }
        public int Res_tdoc { get; set; }
        public int Res_dinv { get; set; }
        public string Res_ninv { get; set; }
        public string Res_inac { get; set; }
        public List<TOAeDprod> Productos {get;set;}

        public int Cla_cont { get; set; }
        public string Esp_mdit { get; set; }
        
    }

    public class TOAeDprod
    {
        public int Pro_cont { get; set; }
        public double Dpr_valo { get; set; }
        public double Dpr_dura { get; set; }

    }
}
