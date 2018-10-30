
using SevenReservas.BO;
using SevenReservas.Models;
using System.Collections.Generic;
using System.Web.Http;

namespace SevenReservas.Controllers
{
    public class AeClaseController : ApiController
    {
        BOAeClase boAeClase = new BOAeClase();

        [HttpGet]
        public TOTransaction<List<TOAeClase>> Get(short emp_codi)
        {
            return boAeClase.GetAeClase(emp_codi);
        }

        [HttpGet]
        [Route("api/AeClase/GetAeClase")]
        public TOAeClase Get(int cla_cont,short emp_codi)
        {
            return boAeClase.GetAeClaseByClaCont(cla_cont,emp_codi);
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
