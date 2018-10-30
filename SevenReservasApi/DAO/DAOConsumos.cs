using SevenReservas.Models;
using Ophelia;
using Ophelia.DataBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Threading.Tasks;
using Ophelia.Comun;
using SevenReservas.Tools;

namespace SevenReservas.DAO
{
    public class DAOConsumos
    {
        OException BOException = new OException();

        public List<TOConsumos> GetConsumos(short emp_codi, int soc_cont, int sbe_cont, int fac_mesp, int fac_anop)
        {
            List<Parameter> listAux = new List<Parameter>();

            try
            {
                StringBuilder sql = new StringBuilder();

                //SE CAMBIA LA VISTA POR INSTRUCCIONES DE AURA 
                //sql.Append(" SELECT AE_RESER.RES_NUME, IN_PRODU.PRO_CODI, IN_PRODU.PRO_NOMB, ");
                //sql.Append(" AE_DVTIP.DVT_VALO, AE_CONSU.CON_FECH ");
                //sql.Append(" FROM AE_CONSU, AE_DCONS, AE_RESER, SO_SBENE, IN_PRODU, AE_DVTIP ");
                //sql.Append(" WHERE AE_CONSU.EMP_CODI = AE_DCONS.EMP_CODI ");
                //sql.Append(" AND AE_CONSU.CON_CONT = AE_DCONS.CON_CONT ");
                //sql.Append(" AND AE_DCONS.EMP_CODI = IN_PRODU.EMP_CODI ");
                //sql.Append(" AND AE_DCONS.PRO_CONT = IN_PRODU.PRO_CONT ");
                //sql.Append(" AND AE_RESER.EMP_CODI = AE_CONSU.EMP_CODI ");
                //sql.Append(" AND AE_RESER.RES_CONT = AE_CONSU.RES_CONT ");
                //sql.Append(" AND AE_CONSU.CON_ESTA = 'A' ");
                //sql.Append(" AND AE_CONSU.CON_CRES = SO_SBENE.SBE_CODI ");
                //sql.Append(" AND AE_DVTIP.EMP_CODI = AE_DCONS.EMP_CODI ");
                //sql.Append(" AND AE_DVTIP.CON_CONT = AE_DCONS.CON_CONT ");
                //sql.Append(" AND AE_DVTIP.DVT_CONT = AE_DCONS.DCO_CONT ");
                //sql.Append(" AND AE_DVTIP.DVT_CODI = 'TOTAL' ");
                //sql.Append(" AND AE_RESER.RES_CONT > 0 ");
                //sql.Append(" AND AE_CONSU.EMP_CODI = @P_EMP_CODI ");
                //sql.Append(" AND SO_SBENE.SOC_CONT = @P_SOC_CONT ");
                //sql.Append(" AND SO_SBENE.SBE_CONT = @P_SBE_CONT ");
                //sql.Append(" ORDER BY AE_RESER.RES_NUME DESC ");


                sql.Append(" SELECT SO_SBENE.SBE_CODI, SO_CONSU.EMP_CODI, SO_CONSU.FAC_CONT, SO_CONSU.TOP_CODI, SO_CONSU.FAC_NUME, ");
                sql.Append(" SO_CONSU.FAC_FECH, SO_CONSU.FAC_NECH, SO_CONSU.FAC_ANOP, SO_CONSU.FAC_MESP, SO_CONSU.FAC_TIDO, ");
                sql.Append(" SO_CONSU.FAC_DIAP, SO_CONSU.LIS_CODI, SO_CONSU.CLI_CODA, SO_CONSU.DCL_CODD, SO_CONSU.VEN_CODI, ");
                sql.Append(" SO_CONSU.VEN_CODS, SO_CONSU.ARB_SUCU, SO_CONSU.BOD_CODI, SO_CONSU.PRO_CONT, IN_PRODU.PRO_CODI, ");
                sql.Append(" IN_PRODU.PRO_NOMB, SO_CONSU.DFA_CANT, SO_CONSU.UNI_CODI, SO_CONSU.DFA_VALO, SO_CONSU.DMI_VALO, ");
                sql.Append(" SO_CONSU.DFA_VADE, SO_CONSU.DFA_VNET        ");
                sql.Append(" FROM SO_CONSU, IN_PRODU, SO_SOCIO, SO_SBENE ");
                sql.Append(" WHERE IN_PRODU.EMP_CODI = SO_CONSU.EMP_CODI ");
                sql.Append(" AND IN_PRODU.PRO_CONT = SO_CONSU.PRO_CONT   ");

                sql.Append(" AND SO_SOCIO.EMP_CODI = SO_SBENE.EMP_CODI   ");
                sql.Append(" AND SO_SOCIO.MAC_NUME = SO_SBENE.MAC_NUME   ");
                sql.Append(" AND SO_SOCIO.SOC_CONT = SO_SBENE.SOC_CONT   ");

                sql.Append(" AND SO_CONSU.EMP_CODI = SO_SBENE.EMP_CODI   ");
                sql.Append(" AND SO_CONSU.CLI_CODA = SO_SBENE.SBE_CODI   ");

                sql.Append(" AND SO_SOCIO.SOC_ESTA = 'A'                 ");
                sql.Append(" AND SO_SBENE.SBE_ESTA = 'A'                 ");

                sql.Append(" AND SO_CONSU.EMP_CODI = @P_EMP_CODI      ");
                sql.Append(" AND SO_SBENE.SOC_CONT = @P_SOC_CONT      ");
                sql.Append(" AND SO_SBENE.SBE_CONT = @P_SBE_CONT      ");

                sql.Append(" AND SO_CONSU.FAC_ANOP = @P_FAC_ANOP ");
                sql.Append(" AND SO_CONSU.FAC_MESP = @P_FAC_MESP ");
                sql.Append(" ORDER BY SO_CONSU.FAC_FECH DESC");


                listAux.Add(new Parameter("@P_EMP_CODI", emp_codi));
                listAux.Add(new Parameter("@P_SOC_CONT", soc_cont));
                listAux.Add(new Parameter("@P_SBE_CONT", sbe_cont));
                listAux.Add(new Parameter("@P_FAC_ANOP", fac_anop));
                listAux.Add(new Parameter("@P_FAC_MESP", fac_mesp));


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

        public Func<IDataReader, TOConsumos> Make = reader => new TOConsumos
        {
            //Res_nume = reader["RES_NUME"].AsString(),
            Pro_codi = reader["PRO_CODI"].AsString(),
            Pro_nomb = reader["PRO_NOMB"].AsString(),
            Dvt_valo = reader["DFA_VNET"].AsDouble(),
            Con_fech = reader["FAC_FECH"].AsDateTime(),
        };
    }
}