
using SevenReservas.BO;
using SevenReservas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Script.Serialization;

namespace SevenReservas.Controllers
{
    public class SoRsociController : ApiController
    {

        BOSoSocio boSoSocio = new BOSoSocio();
        
        // GET: api/Usuarios
        // public IEnumerable<string> Get()
        // {
        //     return new string[] { "value1", "value2" };
        // }
       
            //metdo permite determinar si el codigo es valido y vigente
        public int Get(int soc_cont, int sbe_cont, int soc_cing,short emp_codi)
        {
            var valido = boSoSocio.GetSoRsoci(soc_cont, sbe_cont, soc_cing,emp_codi);
            return valido;
        }

        public TOTransaction< TOSoRsoci> Post(TOSoRsoci toSoRsoci)
        {
            var socio = boSoSocio.GetSoRsoci(toSoRsoci);

            return socio;
        }



        // // PUT: api/Usuarios/5
        // public void Put(int id, [FromBody]string value)
        // {
        // }
        //
        // // DELETE: api/Usuarios/5
        // public void Delete(int id)
        // {
        // }
    }
}
