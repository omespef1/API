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
using DTOEntities;
using SevenReservas.Tools;

namespace SevenReservas.DAO
{
    public class DAOReserva
    {
        OException BOException = new OException();
        
        public List<TOInfReser> GetInfoReservas(short emp_codi,int soc_cont,int sbe_cont)
        {
            List<Parameter> listAux = new List<Parameter>();
            DateTime ahora = DateTime.Now.AddDays(-2);

            try
            {
                StringBuilder sql = new StringBuilder();

                sql.Append(" SELECT AE_CLASE.CLA_NOMB, AE_ESPAC.ESP_NOMB, AE_CLASE.CLA_TICA, ");
                sql.Append(" IN_PRODU.PRO_NOMB, AE_CLASE.CLA_CONT, AE_CLASE.CLA_FOTO, ");
                sql.Append(" AE_RESER.RES_FINI, AE_RESER.RES_FINA,");
                sql.Append(" AE_RESER.RES_CONT, AE_RESER.RES_NUME, AE_RESER.RES_ESTA,AE_ESPAC.ESP_MDIT, AE_ESPAC.ESP_IMAG,AE_RESER.TER_CODI ");
                sql.Append(" FROM AE_ESPAC, AE_RESER, AE_CLASE, AE_DPROD, IN_PRODU");
                sql.Append(" WHERE AE_ESPAC.EMP_CODI = AE_RESER.EMP_CODI");
                sql.Append(" AND AE_ESPAC.ESP_CONT = AE_RESER.ESP_CONT");
                sql.Append(" AND AE_ESPAC.EMP_CODI = @P_EMP_CODI");
                sql.Append(" AND AE_RESER.SOC_CONT = @P_SOC_CONT");
                sql.Append(" AND AE_RESER.SBE_CONT = @P_SBE_CONT");
                sql.Append(" AND AE_RESER.RES_FINI > @P_RES_FINI");
                //sql.Append(" AND AE_RESER.RES_ESTA IN('R', 'C', 'U', 'E')");
                sql.Append(" AND AE_RESER.RES_ESTA = 'C'");
                sql.Append(" AND AE_RESER.RES_TIPO = 'U'");
                sql.Append(" AND AE_RESER.EMP_CODI = AE_DPROD.EMP_CODI");
                sql.Append(" AND AE_RESER.RES_CONT = AE_DPROD.RES_CONT");
                sql.Append(" AND AE_ESPAC.EMP_CODI = AE_CLASE.EMP_CODI");
                sql.Append(" AND AE_ESPAC.CLA_CONT = AE_CLASE.CLA_CONT");
                sql.Append(" AND AE_DPROD.EMP_CODI = IN_PRODU.EMP_CODI");
                sql.Append(" AND AE_DPROD.PRO_CONT = IN_PRODU.PRO_CONT");
                //sql.Append(" ORDER BY 7 DESC ");
                sql.Append(" ORDER BY  AE_RESER.RES_NUME DESC ");


                listAux.Add(new Parameter("@P_EMP_CODI", emp_codi));
                listAux.Add(new Parameter("@P_SOC_CONT", soc_cont));
                listAux.Add(new Parameter("@P_SBE_CONT", sbe_cont));
                listAux.Add(new Parameter("@P_RES_FINI", ahora));
                



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

        

        public Func<IDataReader, TOInfReser> Make = reader => new TOInfReser
        {
            Cla_nomb = reader["CLA_NOMB"].AsString(),
            Esp_nomb = reader["ESP_NOMB"].AsString(),
            FechaFin = reader["RES_FINA"].AsDateTime(),
            FechaInicio = reader["RES_FINI"].AsDateTime(),
            Pro_nomb = reader["PRO_NOMB"].AsString(),
            Res_cont = reader["RES_CONT"].AsInt(),
            Res_nume = reader["RES_NUME"].AsInt(),
            Res_esta = reader["RES_ESTA"].AsString(),
            Esp_mdit = reader["ESP_MDIT"].AsString(),
            Cla_cont = reader["CLA_CONT"].AsInt(),
            Cla_foto = reader["CLA_FOTO"].AsFoto(),
            Esp_imag = reader["ESP_IMAG"].AsFoto(),
            Cla_tica = reader["CLA_TICA"] == DBNull.Value ? (int?)null : reader["CLA_TICA"].AsInt(),
            Ter_codi = reader["TER_CODI"].AsInt()
        };
    }
}