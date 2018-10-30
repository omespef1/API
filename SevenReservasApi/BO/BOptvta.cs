using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace SevenReservas.BO
{
    public class BOptvta
    {
        short emp_codi = short.Parse(ConfigurationManager.AppSettings["Emp_codi"]);
        DAO.DAOptvta dao = new DAO.DAOptvta();

        internal IEnumerable<Models.PTVTA> GetPtvta()
        {
            return dao.GetPtvta(emp_codi);
        }

        internal IEnumerable<Models.Mesa> GetMesas(long ptv_cont, long ptv_nmei, long ptv_nmef)
        {
            return dao.GetMesas(emp_codi, ptv_cont, ptv_nmei, ptv_nmef);
        }
    }
}