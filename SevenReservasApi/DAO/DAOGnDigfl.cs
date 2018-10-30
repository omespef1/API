using Ophelia.DataBase;
using SevenFramework.DataBase;
using SevenReservas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SevenReservas.DAO
{
    public class DAOGnDigfl
    {
        public GnDigfl GetGnDigfl(string dig_codi)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" SELECT * FROM GN_DIGFL  WHERE DIG_CODI=@DIG_CODI ");
            List<SQLParams> parameters = new List<SQLParams>();
            parameters.Add(new SQLParams("@DIG_CODI", dig_codi));

            return new DbConnection().Get<GnDigfl>(sql.ToString(), parameters);           
        }
       
    }
}