using BackEndReservas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOEntities;
using System.Configuration;
using Ophelia.Seven;
using System.Transactions;
using System.Threading;

namespace ComReservas
{
    public class ComReserva : IReserva
    {
        string usuario = ConfigurationManager.AppSettings["usuario"].ToString();
        string password = ConfigurationManager.AppSettings["password"].ToString();
        string alias = ConfigurationManager.AppSettings["alias"].ToString();


        public TOTransaction CancelarReserva(short emp_codi, int id, int motivo)
        {
            string txterror = "";
            SAeReser.SAeReserDMR com = new SAeReser.SAeReserDMR();
            object[] varEntr = { usuario, Encrypta.EncriptarClave(password), alias, "SAeReser", "", "", "", "", "", "N" };
            object varSali;
            try
            {
                var txOptions = new System.Transactions.TransactionOptions();
                txOptions.IsolationLevel = System.Transactions.IsolationLevel.Serializable;
                txOptions.Timeout = TimeSpan.MaxValue;

                if (com.ProgramLogin(varEntr, out varSali, out txterror) != 0)
                {
                    throw new Exception("Error al ingresar a SEVEN-ERP, " + txterror);
                }
                object[] pdataIn = { emp_codi, id, motivo };

                object pDatOut;
                if (com.Cancelar(pdataIn, out pDatOut, out txterror) != 0)
                {
                    throw new Exception(txterror);
                }
                var pDataOut2 = (object[])pDatOut;
                return new TOTransaction() { TxtError = txterror, Retorno = 0 };               
            }
            catch (Exception ex)
            {

                return new TOTransaction() { Retorno = 1, TxtError = ex.Message };
            }
        }

        public TOTransaction CrearReserva(TOAeReser reserva)
        {

            string txterror = "";
         
            try
            {
                SAeReser.SAeReserDMR com = new SAeReser.SAeReserDMR();
                object[] varEntr = { usuario, Encrypta.EncriptarClave(password), alias, "SAeReser", "", "", "", "", "", "N" };
                //object[] varEntr = { "seven12", "182193186192127126174178192192", "SevenDesarrollo", "SAeReser", "cerezo", "", "", "", "", "N" };
                object varSali;
                if (com.ProgramLogin(varEntr, out varSali, out txterror) != 0)
                {
                    throw new Exception("Error al ingresar a SEVEN-ERP, " + txterror);
                }

                object[] arrayReserva = new object[17];

                arrayReserva[0] = reserva.Emp_codi;
                arrayReserva[1] = reserva.Res_fini.AddMinutes(1);
                arrayReserva[2] = reserva.Res_fina.AddMinutes(-1);
                arrayReserva[3] = reserva.Soc_cont;
                arrayReserva[4] = reserva.Mac_nume;
                arrayReserva[5] = reserva.Sbe_cont;
                arrayReserva[6] = reserva.Esp_cont;
                arrayReserva[7] = reserva.Res_numd;
                arrayReserva[8] = reserva.Ite_cont;
                arrayReserva[9] = reserva.Ter_codi;
                arrayReserva[10] = reserva.Res_tdoc;
                arrayReserva[11] = reserva.Res_dinv;
                arrayReserva[12] = reserva.Res_ninv;
                arrayReserva[13] = reserva.Res_inac;
                arrayReserva[15] = reserva.Productos[0].Pro_cont;
                arrayReserva[16] = reserva.Cla_cont;

                int i = reserva.Productos.Count;
                int y = 3;
                object[,] arrayProductos = new object[i, y];


                for (int index = 0; index < i; index++)
                {
                    arrayProductos[index, 0] = reserva.Productos[index].Pro_cont;
                    arrayProductos[index, 1] = reserva.Productos[index].Dpr_valo;
                    arrayProductos[index, 2] = reserva.Productos[index].Dpr_dura;
                }

                arrayReserva[14] = arrayProductos;

                object pDatOut;
                if (com.InsertarReserva(arrayReserva, out pDatOut, out txterror) == 1)
                    throw new Exception(txterror);
                com = null;
                var pDataOut2 = (object[])pDatOut;
                var salida = new TSalida();
                salida.Txterror = txterror;
                salida.retorno = (int)pDataOut2[1];

                return new TOTransaction() { Retorno = 0, TxtError = "" ,InvoiceId = salida.retorno};
            }
            catch (Exception ex)
            {
                return new TOTransaction() { Retorno = 1, TxtError = ex.Message,InvoiceId=0 };
            }
            
        }
    }
}
