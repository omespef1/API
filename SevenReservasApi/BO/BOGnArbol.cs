using SevenReservas.DAO;
using SevenReservas.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace SevenReservas.BO
{
    public class BOGnArbol
    {
        DAOGnArbol daoGnArbol = new DAOGnArbol();
        short emp_codi = short.Parse(ConfigurationManager.AppSettings["Emp_codi"]);
        public TOTransaction<List<TOGnArbol>> GetGnArbol (short tar_codi)
        {
            List<TOGnArbol> litems = new List<TOGnArbol>();
            try
            {
                litems = daoGnArbol.GetGnArbol(emp_codi, tar_codi);
                if (litems == null || !litems.Any())
                    throw new Exception("Se deben configurar tipos de PQR: Contacte con su adminsitrador");
                return new TOTransaction<List<TOGnArbol>>() { ObjTransaction = litems, Retorno = 0, TxtError = "" };
            }
            catch(Exception ex)
            {
                return new TOTransaction<List<TOGnArbol>>() { ObjTransaction = null, Retorno = 1, TxtError = ex.Message };
            }                                         
        }
    }
}