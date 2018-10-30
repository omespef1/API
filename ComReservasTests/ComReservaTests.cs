using Microsoft.VisualStudio.TestTools.UnitTesting;
using ComReservas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace ComReservas.Tests
{
    [TestClass()]
    public class ComReservaTests
    {
        [TestMethod()]
        public void CrearReservaTest()
        {
            ComReserva reserva = new ComReserva();
            var docreserva = new DTOEntities.TOAeReser();
            docreserva.Emp_codi = 102;
            docreserva.Res_fini = new DateTime(2015, 11, 25, 14, 0, 0);
            docreserva.Res_fina = new DateTime(2015, 11, 25, 17, 0, 0);
            docreserva.Soc_cont = 1;
            docreserva.Mac_nume = "5";
            docreserva.Sbe_cont = 1;
            docreserva.Esp_cont = 1;
            docreserva.Ite_cont = 10207;
            docreserva.Ter_codi = 13760507;
            docreserva.Res_tdoc = 0;
            docreserva.Res_dinv = 0;
            docreserva.Res_ninv = "";
            docreserva.Res_inac = "";
            docreserva.Productos = new List<DTOEntities.TOAeDprod>();
            var producto = new DTOEntities.TOAeDprod();
            producto.Pro_cont = 2031;
            producto.Dpr_dura = 1;
            producto.Dpr_valo = 0;
            docreserva.Productos.Add(producto);

            string json = JsonConvert.SerializeObject(docreserva);

            var respuesta =  reserva.CrearReserva(docreserva);
            
            Assert.Fail();
        }
    }
}