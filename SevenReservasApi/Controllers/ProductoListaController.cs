
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
    public class ProductoController : ApiController
    {
        BOProducto boProducto = new BOProducto();
        BOAeClase boAeClase = new BOAeClase();

        public  TOTransaction<List<TOProducto>> Get(int Cla_cont,short emp_codi)
        {
            return boProducto.GetProductos(Cla_cont,emp_codi);
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
