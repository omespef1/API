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
    public class AeOsAppController : ApiController
    {
        BOAeOsApp bo = new BOAeOsApp();
        [HttpGet]
        [Route("api/AeOsApp/GetAeOsApp")]
        public TOTransaction< List<TOAeOsApp>> GetAeOsApp(short emp_codi)
        {
            return bo.GetAeOsApp(emp_codi);
        }
    }
}