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
    public class DAOEcEvent
    {
        OException BOException = new OException();

        public List<TOEcEvent> GetEcEvent(short emp_codi, int soc_cont, int sbe_cont)
        {
            List<Parameter> listAux = new List<Parameter>();

            try
            {
                StringBuilder sql = new StringBuilder();

                sql.Append(" SELECT EVE_ESTA, EVE_FEIN, EVE_FESA, GN_TOPER.TOP_NOMB, ");
                sql.Append(" EVE_VATO, EVE_NOMB, 'N' INVITADO ");
                sql.Append(" FROM EC_EVENT, GN_TOPER                                 ");
                sql.Append(" WHERE EC_EVENT.EMP_CODI = GN_TOPER.EMP_CODI ");
                sql.Append(" AND EC_EVENT.TOP_CODI = GN_TOPER.TOP_CODI               ");
                sql.Append(" AND EC_EVENT.EVE_ESTA IN('C', 'P') ");
                sql.Append(" AND EC_EVENT.EVE_CONT > 0  ");
                sql.Append(" AND EC_EVENT.EMP_CODI = @P_EMP_CODI ");
                sql.Append(" AND EC_EVENT.SOC_CONT = @P_SOC_CONT ");
                sql.Append(" AND EC_EVENT.SBE_CONT = @P_SBE_CONT ");

                sql.Append(" UNION ALL ");
                sql.Append(" SELECT EVE_ESTA, EVE_FEIN, EVE_FESA, GN_TOPER.TOP_NOMB, ");
                sql.Append("        EVE_VATO, EVE_NOMB, 'S' INVITADO  ");
                sql.Append("   FROM EC_EVENT, GN_TOPER, EC_LISEV,SO_SBENE ");
                sql.Append("  WHERE EC_EVENT.EMP_CODI = GN_TOPER.EMP_CODI ");
                sql.Append("    AND EC_EVENT.TOP_CODI = GN_TOPER.TOP_CODI ");
                sql.Append("    AND EC_EVENT.EMP_CODI = EC_LISEV.EMP_CODI ");
                sql.Append("    AND EC_EVENT.EVE_CONT = EC_LISEV.EVE_CONT ");
                sql.Append("    AND EC_LISEV.EMP_CODI = SO_SBENE.EMP_CODI ");
                sql.Append("    AND EC_LISEV.DLI_IDIN = SO_SBENE.SBE_CODI ");
                sql.Append("    AND EC_EVENT.EVE_ESTA IN('C', 'P') ");
                sql.Append("    AND EC_EVENT.EVE_CONT > 0 ");
                sql.Append("    AND EC_EVENT.EMP_CODI = @P_EMP_CODI ");
                sql.Append("    AND SO_SBENE.SOC_CONT = @P_SOC_CONT ");
                sql.Append("    AND SO_SBENE.SBE_CONT = @P_SBE_CONT ");


                listAux.Add(new Parameter("@P_EMP_CODI", emp_codi));
                listAux.Add(new Parameter("@P_SOC_CONT", soc_cont));
                listAux.Add(new Parameter("@P_SBE_CONT", sbe_cont));

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

        public Func<IDataReader, TOEcEvent> Make = reader => new TOEcEvent
        {
            Eve_esta = reader["EVE_ESTA"].AsString(),
            Eve_fein = reader["EVE_FEIN"].AsDateTime(),
            Eve_fesa = reader["EVE_FESA"].AsDateTime(),
            Top_nomb = reader["TOP_NOMB"].AsString(),
            Eve_vato = reader["EVE_VATO"].AsDouble(),
            Eve_nomb = reader["EVE_NOMB"].AsString(),
            Invitado = reader["INVITADO"].AsString(),
        };
    }
}