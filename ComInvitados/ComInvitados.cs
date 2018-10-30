using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOEntities;
using Ophelia.Seven;

namespace ComInvitados
{
    public class ComInvitados
    {
        string usuario = ConfigurationManager.AppSettings["usuario"].ToString();
        string password = ConfigurationManager.AppSettings["password"].ToString();
        string alias = ConfigurationManager.AppSettings["alias"].ToString();

        public TSalida CrearTSoSocio(TOInvitado toSoIngre)
        {
            string txterror = "";
            SSoIngre.SSoIngreDMR com = new SSoIngre.SSoIngreDMR();
            object[] varEntr = { usuario, Encrypta.EncriptarClave(password), alias, "SSoIngre", "", "", "", "", "", "N" };

            try
            {
                object varSali;
                if (com.ProgramLogin(varEntr, out varSali, out txterror) != 0)
                {
                    throw new Exception("Error al ingresar a SEVEN-ERP, " + txterror);
                }

                List<object> lentrada = new List<object>();
                lentrada.Add((int)toSoIngre.Emp_Codi);
                lentrada.Add(toSoIngre.Soc_cont);
                lentrada.Add(toSoIngre.Sbe_Cont);
                lentrada.Add(toSoIngre.Mac_nume);
                lentrada.Add(toSoIngre.Nombre);
                lentrada.Add(toSoIngre.Apellido);
                lentrada.Add(toSoIngre.Fecha);
                lentrada.Add(toSoIngre.Observacion);

                var res = com.InsertarPreingreso(lentrada.ToArray(), out txterror);
                if (res == 1)
                {
                    throw new Exception(txterror);
                }
                var salida = new TSalida();
                salida.Txterror = "";
                salida.retorno = 1;
                return salida;
            }
            catch (Exception ex)
            {
                var salida = new TSalida();
                salida.Txterror = ex.Message;
                salida.retorno = 0;
                return salida;
            }
        }
    }
}
