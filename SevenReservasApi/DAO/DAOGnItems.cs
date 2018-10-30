using SevenReservas.Models;
using Ophelia;
using Ophelia.Comun;
using Ophelia.DataBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace SevenReservas.DAO
{
    public class DAOGnItems
    {
        OException BOException = new OException();

        public List<TOGnItems> GetGnItems(int tit_cont)
        {
            List<Parameter> listAux = new List<Parameter>();

            try
            {
                StringBuilder sql = new StringBuilder();

                sql.Append(" SELECT ITE_CONT, TIT_CONT, ITE_CODI, ITE_NOMB ");
                sql.Append(" FROM GN_ITEMS ");
                sql.Append(" WHERE ITE_ACTI = 'S' ");
                sql.Append(" AND TIT_CONT = @P_TIT_CONT ");
                sql.Append(" AND ITE_CODI > '0' ");

                listAux.Add(new Parameter("@P_TIT_CONT", tit_cont));

                Parameter[] oParameter = listAux.ToArray();
                OTOContext pTOContext = new OTOContext();

                var conection = DBFactory.GetDB(pTOContext);
                var objeto = conection.ReadList(pTOContext, sql.ToString(), Make, oParameter);
                return objeto;
            }
            catch (Exception ex)
            {
                BOException.Throw(this.GetType().ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return null;
            }
        }

        public Func<IDataReader, TOGnItems> Make = reader => new TOGnItems
        {
            Ite_cont = reader["ITE_CONT"].AsInt(),
            Tit_cont = reader["TIT_CONT"].AsInt(),
            Ite_codi = reader["ITE_CODI"].AsString(),
            Ite_nomb = reader["ITE_NOMB"].AsString(),
        };
    }
}