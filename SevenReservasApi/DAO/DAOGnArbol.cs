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
    public class DAOGnArbol
    {
        OException BOException = new OException();

        public List<TOGnArbol> GetGnArbol(short emp_codi, short tar_codi)
        {
            List<Parameter> listAux = new List<Parameter>();

            try
            {
                StringBuilder sql = new StringBuilder();

                sql.Append(" SELECT EMP_CODI, ARB_CONT, ARB_CODI, ARB_NOMB ");
                sql.Append(" FROM GN_ARBOL ");
                sql.Append(" WHERE TAR_CODI = @P_TAR_CODI ");
                sql.Append(" AND ARB_MOVI = 'S' ");
                sql.Append(" AND ARB_ACTI = 'A' ");
                sql.Append(" AND EMP_CODI = @P_EMP_CODI ");

                listAux.Add(new Parameter("@P_TAR_CODI", tar_codi));
                listAux.Add(new Parameter("@P_EMP_CODI", emp_codi));

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

        public Func<IDataReader, TOGnArbol> Make = reader => new TOGnArbol
        {
            Arb_cont = reader["ARB_CONT"].AsInt(),
            Arb_codi = reader["ARB_CODI"].AsString(),
            Arb_nomb = reader["ARB_NOMB"].AsString(),
        };
    }
}