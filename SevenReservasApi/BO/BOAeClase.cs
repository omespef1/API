using SevenReservas.DAO;
using SevenReservas.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace SevenReservas.BO
{
    public class BOAeClase
    {
        DAOAeClase daoAeClase = new DAOAeClase();
       
        public TOTransaction<List<TOAeClase>> GetAeClase(short emp_codi)
        {
            try
            {             
                var resullt = daoAeClase.GetAeClase(emp_codi);
                if (resullt == null || !resullt.Any())
                    throw new Exception("No se encontraron tipos de espacio disponibles");

                return new TOTransaction<List<TOAeClase>>() { ObjTransaction = resullt, Retorno = 0, TxtError = "" };
            }
           catch(Exception ex)
            {
                return new TOTransaction<List<TOAeClase>>() { ObjTransaction = null, Retorno = 1, TxtError = ex.Message };
            }
        }

        internal TOAeClase GetAeClaseByClaCont(int cla_cont,short emp_codi)
        {
            return daoAeClase.GetAeClaseByClaCont(emp_codi, cla_cont);
        }
    }

   

}