using SevenReservas.BO;
using SevenReservas.DAO;
using SevenReservas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SevenReservas.Controllers
{
    public class GnDigflController : ApiController
    {
        // GET: api/GnDigfl
        public TOTransaction<GnDigfl> Get(string dig_codi)
        {

            BOGnDigfl bo = new BOGnDigfl();
            return bo.GetGnDigfl(dig_codi);
        }

   
    }
}
