using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using Ophelia;
using Ophelia.DataBase;
using Ophelia.Comun;
using SevenReservas.Tools;

namespace SevenReservas.DAO
{
    public class DAOAeOsApp
    {
        OException BOException = new OException();

        internal List<Models.TOAeOsApp> GetAeOsApp(short emp_codi)
        {
            try
            {
                StringBuilder sql = new StringBuilder();

                sql.AppendLine(" SELECT CONVERT(VARBINARY (MAX), AE_OSAPP.OSA_BMPR) AS OSA_IMAGE, * FROM AE_OSAPP");
                sql.AppendLine(" WHERE EMP_CODI = @P_EMP_CODI  ");
                //sql.AppendLine(" AND AUD_ESTA = 'A'    ");
                sql.AppendLine(" AND OSA_MAPP = 'S'    ");

                Parameter[] oParameter = new Parameter[] {
                    new Parameter("P_EMP_CODI", emp_codi)
                };
                OTOContext pTOContext = new OTOContext();

                var conection = DBFactory.GetDB(pTOContext);
                return conection.ReadList(pTOContext, sql.ToString(), Make, oParameter);
            }
            catch (Exception ex)
            {
                BOException.Throw(this.GetType().ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return null;
            }
        }

        public Func<IDataReader, Models.TOAeOsApp> Make = reader => new Models.TOAeOsApp
        {
            Emp_Codi = reader["EMP_CODI"].AsInt16(),
            Osa_Bmpr = reader["OSA_IMAGE"].AsFoto(),
            Osa_Cont = reader["OSA_CONT"].AsInt(),
            Osa_Link = reader["OSA_LINK"].AsString(),
            Osa_Nomb = reader["OSA_NOMB"].AsString()
        };
    }
}