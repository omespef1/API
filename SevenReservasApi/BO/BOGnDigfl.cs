using SevenReservas.DAO;
using SevenReservas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SevenReservas.BO
{
    public class BOGnDigfl
    {
        public TOTransaction<GnDigfl> GetGnDigfl(string dig_codi)
        {
            DAOGnDigfl daoDigl = new DAOGnDigfl();
            try
            {
                var result = daoDigl.GetGnDigfl(dig_codi);
                return new TOTransaction<GnDigfl>() { ObjTransaction = result, Retorno = 0, TxtError = "" };
            }
            catch (Exception ex)
            {
                return new TOTransaction<GnDigfl>() { ObjTransaction = null, TxtError = ex.Message, Retorno = 1 };
            }
        }
    }
}