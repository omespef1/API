using BackEndReservas;
using DTOEntities;
using SevenReservas.DAO;
using SevenReservas.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace SevenReservas.BO
{
    public class BOPqInpqr
    {
        DAOPqInpqr daoPqInpqr = new DAOPqInpqr();
        DAOPqDinpq daoPqDinpq = new DAOPqDinpq();
        IPqInpqr comPqInpqr = new ComReservas.ComPqInpqr();
        BO.BOWfRflup boWfRflup = new BOWfRflup();
       

        public TOTransaction<Models.TOPqInpqr> SetPqInpqr(DTOEntities.TOPqInpqr toPqInpqr)
        {
            DTOEntities.TSalida salida = new DTOEntities.TSalida();
            try
            {
                Models.TOPqInpqr pqr = new Models.TOPqInpqr();
                var ite_cont = ConfigurationManager.AppSettings["ReciboPQR"].ToString();
                if (ite_cont != null)
                    toPqInpqr.ite_frec = int.Parse(ite_cont);
                
                toPqInpqr.inp_esta = "A";
                toPqInpqr.arb_csuc = "0";
                toPqInpqr.inp_tcli = "S";
                if (string.IsNullOrEmpty(toPqInpqr.arb_ccec))
                    throw new Exception("Debe diligeciar el campo ambiente");
              
               salida = comPqInpqr.CrearPqInpqr(toPqInpqr);
                //crear WorkFlow
                var wf = boWfRflup.CrearCasoWorkFlow(toPqInpqr.emp_codi, salida.retorno);
                pqr.inp_cont = salida.retorno;
                //Se modifica para que retorne un Transaction acorde a la nueva app de reservas
                return new TOTransaction<Models.TOPqInpqr>() { ObjTransaction = pqr, Retorno = 0, TxtError = "" };
                
            }
            catch(Exception ex)
            {
                salida.retorno = 0;
                salida.Txterror = ex.Message;
                var wf = boWfRflup.CrearCasoWorkFlow(toPqInpqr.emp_codi, salida.retorno);
                return new TOTransaction<Models.TOPqInpqr>() { ObjTransaction = null, Retorno = 1, TxtError = ex.Message };

            }
           
        }

        public TOTransaction<List<Models.TOPqInpqr>> GetPqInpqr(int soc_cont, int sbe_cont,short emp_codi)
        {
            try
            {
                List<Models.TOPqInpqr> result = new List<Models.TOPqInpqr>();
                result = daoPqInpqr.GetPqInpqr(emp_codi, soc_cont, sbe_cont);
                if (result == null || !result.Any())
                    throw new Exception("No hay pqr registradas aún.");
                foreach(Models.TOPqInpqr pqr in result)
                {
                    pqr.seguimientos = daoPqDinpq.GetPqDinpq(emp_codi, pqr.inp_cont);
                }
                return new TOTransaction<List<Models.TOPqInpqr>>() { ObjTransaction = result, TxtError = "", Retorno = 0 };
            }
            catch(Exception ex)
            {
                return new TOTransaction<List<Models.TOPqInpqr>>() { ObjTransaction = null, Retorno = 1, TxtError = ex.Message };
            }
          
        }

        public IEnumerable<Models.TOPqDinpq> GetPqDinpq(int inp_cont,short emp_codi)
        {
            return daoPqDinpq.GetPqDinpq(emp_codi,inp_cont);
        }

        public IEnumerable<Models.TOPqDinpq> GetPqDinpqDetail(int inp_cont,int din_cont,short emp_codi)
        {
            return daoPqDinpq.GetPqDinpqDetail(emp_codi, inp_cont,din_cont);
        }

    }
}