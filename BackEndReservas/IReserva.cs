using DTOEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEndReservas
{
    public interface IReserva
    {
        TOTransaction CrearReserva(TOAeReser reserva);
        TOTransaction CancelarReserva(short emp_codi, int id, int motivo);
    }
}
