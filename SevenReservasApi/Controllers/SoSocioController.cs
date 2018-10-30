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
      public class SoSocioController : ApiController
    {
        BOSoSocio boSoSocio = new BOSoSocio();

        // GET: api/Usuarios
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Usuarios/5
        public string Get(string id, int sbe_cont,short emp_codi)
        {
            var socio = boSoSocio.GetSoSocio(id,sbe_cont,emp_codi);
            socio.Soc_pass = "";
            return new JavaScriptSerializer().Serialize(socio);  // "value";
        }

        public TOTransaction<TOSoSocio> Post(TOSoSocio toSoSocio)
        //    public int Post(TOSoSocio toSoSocio)
        {
            try
            {
               var KeyDecript = Ophelia.Seven.Encrypta.EncriptarClave(toSoSocio.Soc_pass);
                var socio = boSoSocio.GetSoSocio(toSoSocio.Sbe_ncar, KeyDecript, toSoSocio.Emp_codi);
                if (socio == null)
                    throw new Exception("Número de acción o contraseña incorrectos. Verifique por favor.");
                socio = boSoSocio.GetSoSocio(toSoSocio.Sbe_ncar, socio.Sbe_cont,toSoSocio.Emp_codi);               
                return new TOTransaction<TOSoSocio>() { ObjTransaction = socio, Retorno = 0, TxtError = "" };
            }
            catch(Exception ex) {
                return new TOTransaction<TOSoSocio>() { ObjTransaction = null, Retorno = 1, TxtError = ex.Message };
            }
                            
        }
        //public TOSoSocio Post(TOSoSocio toSoSocio)
        ////    public int Post(TOSoSocio toSoSocio)
        //{
        //    var socio = boSoSocio.GetSoSocio(toSoSocio.Sbe_ncar, toSoSocio.Soc_pass);
        //    //System.Threading.Thread.Sleep(5000);
        //    var encriptada = Ophelia.Seven.Encrypta.DesencriptaClave(socio.Soc_pass);


        //    if (socio == null || socio.Soc_pass != Ophelia.Seven.Encrypta.EncriptarClave(toSoSocio.Soc_pass))
        //        return null;

        //    //socio.Soc_pass = "";
        //    socio.Soc_pass = toSoSocio.Soc_pass;

        //    return socio;
        //}


        [HttpPost]
        [Route("api/sosocio/update")]
        public int PostUpdate(TOSoSocio toSoSocio)
        {
            var socio = boSoSocio.UpdSoSocio(toSoSocio);
            return socio;
        }

        // PUT: api/Usuarios/5
        [HttpPut]
        public void Put(int id, TOSoSocio value)
        {
        }

        // DELETE: api/Usuarios/5
        public void Delete(int id)
        {
        }
    }
}
