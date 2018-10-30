using DTOEntities;
using SevenReservas.BO;
using SevenReservas.Models;
using System.Collections.Generic;
using System.Web.Http;

namespace SevenReservas.Controllers
{
    public class ConsumosController : ApiController
    {
        BOConsumos boConsumos = new BOConsumos();

        public TOTransaction<List<TOConsumos>> Get(int soc_cont, int sbe_cont, int fac_mesp,int fac_anop,short emp_codi)
        {
            return boConsumos.GetConsumos(soc_cont, sbe_cont, fac_mesp, fac_anop,emp_codi);
        }
    }
}
