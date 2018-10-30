using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SevenReservas.DAO;
using SevenReservas.Models;
namespace SevenReservas.BO
{
    public class BOAeOsApp
    {
        DAOAeOsApp dao = new DAOAeOsApp();      

        internal TOTransaction<List<TOAeOsApp>> GetAeOsApp(short emp_codi)
        {
            try
            {
                List<TOAeOsApp> result = new List<TOAeOsApp>();
                result = dao.GetAeOsApp(emp_codi);
                if (result == null || !result.Any())
                    throw new Exception("No se encontraron convenios");
                return new TOTransaction<List<TOAeOsApp>>() { ObjTransaction = result, Retorno = 0, TxtError = "" };
            }
            catch(Exception ex)
            {
                return new TOTransaction<List<TOAeOsApp>>() { ObjTransaction = null, TxtError = ex.Message, Retorno = 1 };
            }
           
        }
    }
}