using SevenReservas.DAO;
using SevenReservas.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace SevenReservas.BO
{
    public class BOAeEspac
    {
        DAOAeEspac daoAeEspac = new DAOAeEspac();
        short emp_codi = short.Parse(ConfigurationManager.AppSettings["Emp_codi"]);

        public IEnumerable<TOAeEspac> GetAeEspac(int cla_cont)
        {
            return daoAeEspac.GetAeEspac(emp_codi, cla_cont);
        }
    }

}