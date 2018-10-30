using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using SevenReservas.Models;
using Ophelia;
using Ophelia.Comun;
using Ophelia.DataBase;

namespace SevenReservas.DAO
{
    public class DAOGnEmpre
    {
        public List<TOGnEmpre> ConsultarEmpresas()
        {

            StringBuilder sql = new StringBuilder();
            sql.Append(" SELECT * FROM GN_EMPRE ");

            OTOContext pTOContext = new OTOContext();
            var conection = DBFactory.GetDB(pTOContext);
            var objeto = conection.ReadList(pTOContext, sql.ToString(), Make, null);
            return objeto;


        }

        public Func<IDataReader, TOGnEmpre> Make = reader => new TOGnEmpre
        {
            Emp_Codi = reader["EMP_CODI"].AsInt16(),
            Emp_Nomb = reader["EMP_NOMB"].AsString()
        };
    }
}