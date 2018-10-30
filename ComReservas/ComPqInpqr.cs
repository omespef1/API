using BackEndReservas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOEntities;
using System.Configuration;
using Ophelia.Seven;

namespace ComReservas
{
    public class ComPqInpqr : IPqInpqr
    {
        string usuario = ConfigurationManager.AppSettings["usuario"].ToString();
        string password = ConfigurationManager.AppSettings["password"].ToString();
        string alias = ConfigurationManager.AppSettings["alias"].ToString();

        public TSalida CrearPqInpqr(TOPqInpqr toPqInpqr)
        {
            string txterror = "";
            int retorno = 0;
            SPqInpqr.SPqInpqrDMR com = new SPqInpqr.SPqInpqrDMR();
            object[] varEntr = { usuario, Encrypta.EncriptarClave(password), alias, "SPqInpqr", "", "", "", "", "", "N" };
            object varSali;

            try
            {
                if (com.ProgramLogin(varEntr, out varSali, out txterror) != 0)
                {
                    throw new Exception("Error al ingresar a SEVEN-ERP, " + txterror);
                }

                List<object> lentrada = new List<object>();

                lentrada.Add(toPqInpqr.emp_codi);
                lentrada.Add(toPqInpqr.inp_feve);
                lentrada.Add(toPqInpqr.inp_esta);
                lentrada.Add(toPqInpqr.arb_csuc);
                lentrada.Add(toPqInpqr.inp_tcli);
                lentrada.Add(toPqInpqr.inp_ncar);
                lentrada.Add(toPqInpqr.ite_frec);
                lentrada.Add(toPqInpqr.ite_tpqr);
                lentrada.Add(toPqInpqr.arb_ccec);
                lentrada.Add(toPqInpqr.ite_spre);
                lentrada.Add(toPqInpqr.ite_ancu);
                lentrada.Add(toPqInpqr.inp_mpqr);
                lentrada.Add(toPqInpqr.soc_cont);
                lentrada.Add(toPqInpqr.sbe_cont);
                lentrada.Add(toPqInpqr.mac_nume);

                object pDatOut;
                if (com.InsertarPqInpqr(lentrada.ToArray(), out pDatOut, out txterror) == 1)
                    throw new Exception(txterror);
                var pDataOut2 = (object[])pDatOut;
                var salida = new TSalida();
                salida.Txterror = txterror;
                salida.retorno = (int)pDataOut2[0];

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
