
using SevenReservas.BO;
using SevenReservas.Models;
using System.Collections.Generic;
using System.Web.Http;

namespace SevenReservas.Controllers
{
    public class GnArbolController : ApiController
    {
        BOGnArbol boGnArbol = new BOGnArbol();

        public TOTransaction<List<TOGnArbol>> Get(short tar_codi)
        {
            return boGnArbol.GetGnArbol(tar_codi);
        }
    }
}
