using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using DTOEntities;

namespace SevenReservas.BO
{
    public class BOInvitados
    {
        short emp_codi = short.Parse(ConfigurationManager.AppSettings["Emp_codi"]);
        ComInvitados.ComInvitados comInvitado = new ComInvitados.ComInvitados();
        DAO.DAOInvitados dao = new DAO.DAOInvitados();

        public TSalida SetInvitado(TOInvitado invitado)
        {
            try
            {
                //DateTime now = DateTime.Now.Date;
                //invitado.Emp_Codi = emp_codi;
                //StringBuilder validaciones = new StringBuilder();
                //if (invitado.Fecha.Date < now)
                //{
                //    validaciones.AppendLine("La fecha de invitación no debe ser menor a la fecha actual");
                //}
                //if (invitado.Nombre.Trim().Equals(string.Empty))
                //{
                //    validaciones.AppendLine("Los nombres del invitado son obligatorios");
                //}
                //if (invitado.Apellido.Trim().Equals(string.Empty))
                //{
                //    validaciones.AppendLine("Los apellidos del invitado son obligatorios");
                //}
                //if (validaciones.Length > 0)
                //{
                //    return new DTOEntities.TSalida(validaciones.ToString(), 0);
                //}
                // return comInvitado.CrearTSoSocio(invitado);
                var salida = new DTOEntities.TSalida();
                salida.Txterror = "Esta funcionaldad se encuentra deshabilitada. Por favor , actualice la aplicación!";
                salida.retorno = 0;
                return salida;
            }
            catch (Exception ex)
            {
                var salida = new DTOEntities.TSalida();
                salida.Txterror = ex.Message;
                salida.retorno = 0;
                return salida;
            }
        }

        public List<TOInvitado> ListInvitados(TOInvitado invitado)
        {
            invitado.Emp_Codi = emp_codi;
            return dao.ListInvitados(invitado);
        }
    }
}