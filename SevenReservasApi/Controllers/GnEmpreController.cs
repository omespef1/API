using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SevenReservas.BO;
using SevenReservas.Models;

namespace SevenReservas.Controllers
{
    public class GnEmpreController : ApiController
    {
        BOGnEmpre bo = new BOGnEmpre();

        [HttpGet]
        [Route("api/GnEmpre/GetEmpresas")]
        public SevenFramework.TO.TOTransaction <List<TOGnEmpre>> GetEmpresas()
        {
            return bo.ConsultarEmpresas();
        }
    }
}
