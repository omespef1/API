
using SevenReservas.BO;
using SevenReservas.Models;
using System.Collections.Generic;
using System.Web.Http;

namespace SevenReservas.Controllers
{
    public class AeEspacController : ApiController
    {
        BOAeEspac boAeEspac = new BOAeEspac();

        public IEnumerable<TOAeEspac> Get(int cla_cont)
        {
            return boAeEspac.GetAeEspac(cla_cont);
        }

   //     // PUT: api/Usuarios/5
   //     [HttpPut]
   //     public void Put(int id, TOSoSocio value)
   //     {
   //     }
   //
   //     // DELETE: api/Usuarios/5
   //     public void Delete(int id)
   //     {
   //     }
    }
}
