using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SevenFramework.TO;
using SevenReservas.DAO;
using SevenReservas.Models;

namespace SevenReservas.BO
{
    public class BOGnEmpre
    {
        DAOGnEmpre dao = new DAOGnEmpre();

        public SevenFramework.TO.TOTransaction<List<TOGnEmpre>> ConsultarEmpresas()
        {
            try
            {
                var empresas = dao.ConsultarEmpresas();
                if (empresas == null)
                    throw new Exception("No hay empresas");
                return new SevenFramework.TO.TOTransaction<List<TOGnEmpre>>() { ObjTransaction = empresas, TxtError = "", Retorno = 0 };
            }
            catch (Exception ex)
            {
                return new SevenFramework.TO.TOTransaction<List<TOGnEmpre>>() { ObjTransaction = null, Retorno = 1, TxtError = ex.Message };
            }

        }
    }
}