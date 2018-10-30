using DTOEntities;
using SevenReservas.BO;
using SevenReservas.Models;
using System.Collections.Generic;
using System.Web.Http;

namespace SevenReservas.Controllers
{
    public class ReservaController : ApiController
    {
        BOReserva boReserva = new BOReserva();
        [HttpGet]
        [Route("api/reserva")]
        public TOTransaction<List<TOInfReser>> Get(int soc_cont, int sbe_cont,short emp_codi)
        {
            return boReserva.GetInfoReserva(soc_cont, sbe_cont,emp_codi);
        }
        
        [HttpPost]
        [Route("api/reserva")]
        public TOTransaction Post(TOAeReser reserva)
        {
            return boReserva.CrearReserva(reserva);
        }

        //     // PUT: api/Usuarios/5
        //     [HttpPut]
        //     public void Put(int id, TOSoSocio value)
        //     {
        //     }
        //
        // DELETE: api/Usuarios/5
        [HttpPost]
        [Route("api/reserva/Cancelar")]
        public DTOEntities.TOTransaction Cancelar(TOInReject cancel)
        {          
            return boReserva.CancelarReserva(cancel.id, cancel.justification,cancel.emp_codi);
        }
        //public TSalida Delete(int id)
        //{
        //    return boReserva.CancelarReserva(id);
        //}
    }
}
