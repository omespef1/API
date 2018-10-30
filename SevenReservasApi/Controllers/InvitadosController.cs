using DTOEntities;
using SevenReservas.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SevenReservas.Controllers
{
    public class InvitadosController : ApiController
    {
        BOInvitados boAutorizados = new BOInvitados();

        public TSalida Post(TOInvitado invitado)
        {
            return boAutorizados.SetInvitado(invitado);
        }

        [HttpPost]
        [Route("api/ListInvitados")]
        public List<TOInvitado> ListInvitados(TOInvitado invitado)
        {
            return boAutorizados.ListInvitados(invitado);
        }
    }
}
