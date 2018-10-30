using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using SevenReservas.BO;
using SevenReservas.Models;

namespace SevenReservas.Controllers
{
    public class ptvtaController : ApiController
    {
        BOptvta boptvta = new BOptvta();

        public IEnumerable<PTVTA> Get()
        {
            return boptvta.GetPtvta();
        }

        [HttpGet]
        [Route("api/ptvta/GetMesas")]
        public IEnumerable<Mesa> GetMesas(long ptv_cont, long ptv_nmei, long ptv_nmef)
        {
            return boptvta.GetMesas(ptv_cont, ptv_nmei, ptv_nmef);
        }

    }
}