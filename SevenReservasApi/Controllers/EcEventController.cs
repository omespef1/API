using DTOEntities;
using SevenReservas.BO;
using SevenReservas.Models;
using System.Collections.Generic;
using System.Web.Http;

namespace SevenReservas.Controllers
{
    public class EcEventController : ApiController
    {
        BOEcEvent boEcEvent = new BOEcEvent();
        public IEnumerable<TOEcEvent> Get(int soc_cont, int sbe_cont)
        {
            return boEcEvent.GetEcEvent(soc_cont, sbe_cont);
        }
    }
}
