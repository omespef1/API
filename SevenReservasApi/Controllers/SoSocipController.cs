
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
    public class SoSocipController : ApiController
    {
        BOSoSocio boSoSocio = new BOSoSocio();


        public TOSoSocio Get(string id, int sbe_cont,short emp_codi)
        {
            var socio = boSoSocio.GetSoSocio(id, sbe_cont,emp_codi);



            socio.Soc_pass = "";
            return socio;  // "value";
        }

        public int Post(TOSoSocio toSoSocio)
        {
            var socio = boSoSocio.UpdSoSocip(toSoSocio);

            return 0;
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
