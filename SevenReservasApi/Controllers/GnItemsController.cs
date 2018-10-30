
using SevenReservas.BO;
using SevenReservas.Models;
using System.Collections.Generic;
using System.Web.Http;

namespace SevenReservas.Controllers
{
    public class GnItemsController : ApiController
    {
        BOGnItems boGnItems = new BOGnItems();

        public TOTransaction<List<TOGnItems>> Get(int tit_cont)
        {
            return boGnItems.GetGnItems(tit_cont);
        }
    }
}
