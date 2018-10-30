using SevenReservas.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Ophelia.Comun;

namespace SevenReservas.BO
{
    public class BOWfRflup
    {
        string urlWorkFlow = ConfigurationManager.AppSettings["UrlWorkFlow"].ToString();
        string WfProcodi = ConfigurationManager.AppSettings["WfProcodi"].ToString();
        string WfForcodi = ConfigurationManager.AppSettings["WfForcodi"].ToString();
        string usu_codi = ConfigurationManager.AppSettings["usuario"].ToString();


        public TSalida CrearCasoWorkFlow(int emp_codi, int num_cont)   
        {
            TSalida tSalida = new TSalida();
            tSalida.retorno = 0;
            tSalida.Txterror = "";

            try
            {
                WWfRflup.SWFRFLUP ws = new WWfRflup.SWFRFLUP();

                if (urlWorkFlow == null)
                    throw new Exception("No esta definida la parametrización de UrlWorkFlow ");

                if (WfProcodi == null)
                    throw new Exception("No esta definida la parametrización de WfProcodi ");

                if (WfForcodi == null)
                    throw new Exception("No esta definida la parametrización de WfForcodi ");

                ws.Url = urlWorkFlow;

                WWfRflup.TOWfRflup toWfRflup = new WWfRflup.TOWfRflup();

                toWfRflup.emp_codi = emp_codi;
                toWfRflup.cas_desc = "Pqr numero " + num_cont + " fue creada";
                toWfRflup.usu_codi = usu_codi;
                toWfRflup.pro_codi = WfProcodi;
                toWfRflup.frm_codi = WfForcodi;
                toWfRflup.tbl_name = "PQ_INPQR";
                toWfRflup.cam_name = "INP_CONT";
                toWfRflup.num_cont = num_cont.ToString();
                //toWfRflup.cas_narc 
                //toWfRflup.cas_arch

                var retorno = ws.EnviarWF(toWfRflup);

                tSalida.retorno = retorno.Retorno.AsInt();
                tSalida.Txterror = retorno.TxtError;
            }
            catch (Exception err)
            {
                tSalida.retorno = 1;
                tSalida.Txterror = err.Message;
            }

            return tSalida;
        }

    }
}