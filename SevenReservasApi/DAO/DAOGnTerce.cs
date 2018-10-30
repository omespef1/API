using SevenReservas.Models;
using Ophelia;
using Ophelia.DataBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ophelia.Comun;
using SevenReservas.Tools;

namespace SevenReservas.DAO
{
    public class DAOGnTerce
    {

        OException BOException = new OException();

        public List<TOGnTerce> GetImageTerce(List<string> ter_codi)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" SELECT CONVERT(VARBINARY (MAX), GN_TERCE.TER_FOTO) AS TER_FOTO, TER_CODI FROM GN_TERCE WHERE TER_CODI IN (" + string.Join(",", ter_codi.ToArray()) + ") ");
                List<Parameter> parameters = new List<Parameter>();
                Parameter[] oParameter = parameters.ToArray();
                OTOContext pTOContext = new OTOContext();
                var conection = DBFactory.GetDB(pTOContext);
                var objeto = conection.ReadList(pTOContext, sql.ToString(), ImageProfile, oParameter);
                return objeto;
            }
            catch (Exception ex)
            {
                BOException.Throw(this.GetType().ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return null;
            }
        }

        public Func<IDataReader, TOGnTerce> ImageProfile = reader => new TOGnTerce ()
        {
            Ter_foto = reader["TER_FOTO"].AsFoto(),
            Ter_codi = reader["TER_CODI"].AsInt(),
        };


        public List<TOGnTerce> GetGnTerce(short emp_codi, int cla_cont, int pro_cont)
        {
            List<Parameter> listAux = new List<Parameter>();

            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" SELECT *, (SELECT CONVERT(VARBINARY (MAX), GN_TERCE.TER_FOTO) FROM GN_TERCE WHERE X.EMP_CODI = GN_TERCE.EMP_CODI AND x.TER_CODI = GN_TERCE.TER_CODI ) AS TER_FOTO FROM ( ");
                sql.Append(" SELECT AE_ESPTE.TER_CODI, GN_TERCE.TER_CODA, GN_TERCE.TER_NOCO, AE_ESPTE.EMP_CODI ");
                sql.Append(" FROM AE_ESPTE, GN_TERCE ");
                sql.Append(" WHERE AE_ESPTE.EMP_CODI = GN_TERCE.EMP_CODI ");
                sql.Append(" AND AE_ESPTE.TER_CODI = GN_TERCE.TER_CODI ");
                sql.Append(" AND AE_ESPTE.EMP_CODI = @P_EMP_CODI ");
                sql.Append(" AND AE_ESPTE.ESP_CONT IN (SELECT  AE_ESPAC.ESP_CONT ");
                sql.Append(" FROM AE_ESPAC, AE_DESPA, AE_CLASE, IN_PRODU ");
                sql.Append(" LEFT OUTER JOIN IN_BMPRO ON(IN_PRODU.EMP_CODI = IN_BMPRO.EMP_CODI ");
                sql.Append(" AND IN_PRODU.PRO_CONT = IN_BMPRO.PRO_CONT) ");
                sql.Append(" WHERE AE_ESPAC.EMP_CODI = AE_CLASE.EMP_CODI ");
                sql.Append(" AND AE_ESPAC.CLA_CONT = AE_CLASE.CLA_CONT ");
                sql.Append(" AND AE_ESPAC.EMP_CODI = AE_DESPA.EMP_CODI ");
                sql.Append(" AND AE_ESPAC.ESP_ESTA = 'A' ");
                sql.Append(" AND AE_ESPAC.ESP_CONT = AE_DESPA.ESP_CONT ");
                sql.Append(" AND AE_DESPA.EMP_CODI = IN_PRODU.EMP_CODI ");
                sql.Append(" AND AE_DESPA.PRO_CONT = IN_PRODU.PRO_CONT ");
                sql.Append(" AND AE_CLASE.CLA_CONT = @P_CLA_CONT ");
                sql.Append(" AND IN_PRODU.PRO_DMIN > 0 ");
                sql.Append(" AND AE_DESPA.EMP_CODI = @P_EMP_CODI ");
                sql.Append(" AND AE_DESPA.PRO_CONT = @P_PRO_CONT ");
                sql.Append(" GROUP BY AE_ESPAC.ESP_CONT) ");
                sql.Append(" GROUP BY AE_ESPTE.TER_CODI, GN_TERCE.TER_CODA, ");
                sql.Append(" GN_TERCE.TER_NOCO, AE_ESPTE.EMP_CODI ");
                sql.Append(" ) AS X ORDER BY X.TER_NOCO ");

                /*sql.Append(" SELECT AE_ESPTE.TER_CODI, GN_TERCE.TER_CODA, GN_TERCE.TER_NOCO, CAST(GN_TERCE.TER_FOTO AS VARBINARY (MAX)) TER_FOTO ");
                sql.Append(" FROM AE_ESPTE, GN_TERCE                                                                             ");
                sql.Append(" WHERE AE_ESPTE.EMP_CODI = GN_TERCE.EMP_CODI                                                         ");
                sql.Append(" AND AE_ESPTE.TER_CODI = GN_TERCE.TER_CODI                                                           ");
                sql.Append(" AND AE_ESPTE.EMP_CODI = @P_EMP_CODI                                                                 ");
                sql.Append(" AND AE_ESPTE.ESP_CONT IN (SELECT  AE_ESPAC.ESP_CONT                                                 ");
                sql.Append(" FROM AE_ESPAC, AE_DESPA, AE_CLASE, IN_PRODU                                                         ");
                sql.Append(" LEFT OUTER JOIN IN_BMPRO ON(IN_PRODU.EMP_CODI = IN_BMPRO.EMP_CODI                                   ");
                sql.Append(" AND IN_PRODU.PRO_CONT = IN_BMPRO.PRO_CONT)                                                          ");
                sql.Append(" WHERE AE_ESPAC.EMP_CODI = AE_CLASE.EMP_CODI                                                         ");
                sql.Append(" AND AE_ESPAC.CLA_CONT = AE_CLASE.CLA_CONT                                                           ");
                sql.Append(" AND AE_ESPAC.EMP_CODI = AE_DESPA.EMP_CODI                                                           ");
                sql.Append(" AND AE_ESPAC.ESP_ESTA = 'A'                                                                         ");
                sql.Append(" AND AE_ESPAC.ESP_CONT = AE_DESPA.ESP_CONT                                                           ");
                sql.Append(" AND AE_DESPA.EMP_CODI = IN_PRODU.EMP_CODI                                                           ");
                sql.Append(" AND AE_DESPA.PRO_CONT = IN_PRODU.PRO_CONT                                                           ");
                sql.Append(" AND AE_CLASE.CLA_CONT = @P_CLA_CONT                                                                 ");
                sql.Append(" AND IN_PRODU.PRO_DMIN > 0                                                                           ");
                sql.Append(" AND AE_DESPA.EMP_CODI = @P_EMP_CODI                                                                 ");
                sql.Append(" AND AE_DESPA.PRO_CONT = @P_PRO_CONT                                                                 ");
                sql.Append(" GROUP BY AE_ESPAC.ESP_CONT)                                                                         ");
                sql.Append(" GROUP BY AE_ESPTE.TER_CODI, GN_TERCE.TER_CODA,                                                      ");
                sql.Append(" GN_TERCE.TER_NOCO, CAST(GN_TERCE.TER_FOTO AS VARBINARY (MAX))                                       ");*/



                //sql.Append(" SELECT AE_ESPTE.ESP_CONT, AE_ESPTE.TER_CODI, GN_TERCE.TER_CODA, ");
                //sql.Append(" GN_TERCE.TER_NOCO, GN_TERCE.TER_FOTO ");
                //sql.Append(" FROM AE_ESPTE, GN_TERCE ");
                //sql.Append(" WHERE AE_ESPTE.EMP_CODI = GN_TERCE.EMP_CODI ");
                //sql.Append(" AND AE_ESPTE.TER_CODI = GN_TERCE.TER_CODI ");
                //sql.Append(" AND AE_ESPTE.EMP_CODI = @P_EMP_CODI ");
                //sql.Append(" AND AE_ESPTE.ESP_CONT = @P_ESP_CONT ");

                listAux.Add(new Parameter("@P_EMP_CODI", emp_codi));
                listAux.Add(new Parameter("@P_CLA_CONT", cla_cont));
                listAux.Add(new Parameter("@P_PRO_CONT", pro_cont));

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

        public  TOGnTerce GetGnTerce(short emp_codi, int ter_codi)
        {
            List<Parameter> listAux = new List<Parameter>();

            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" SELECT CONVERT(VARBINARY(MAX), GN_TERCE.TER_FOTO) TER_FOTO,TER_NOCO,TER_CODI ");
                sql.Append(" FROM   GN_TERCE WHERE  TER_CODI = @TER_CODI AND EMP_CODI = @EMP_CODI ");            
                listAux.Add(new Parameter("@EMP_CODI", emp_codi));
                listAux.Add(new Parameter("@TER_CODI", ter_codi));              

                Parameter[] oParameter = listAux.ToArray();
                OTOContext pTOContext = new OTOContext();

                var conection = DBFactory.GetDB(pTOContext);
                var objeto = conection.Read(pTOContext, sql.ToString(), Make, oParameter);
                return objeto;
            }
            catch (Exception ex)
            {
                BOException.Throw(this.GetType().ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return null;
            }
        }

        public  List<TOGnTerce> GetGnTerceNewVersion(short emp_codi, int cla_cont, int pro_cont, DateTime? Fini, DateTime? Ffin, string Op_Disp)
        {
            List<Parameter> listAux = new List<Parameter>();
            try
            {
                StringBuilder sql = new StringBuilder();
                if (Op_Disp == "" || Op_Disp == "P")
                {
                    sql.Append(" SELECT AE_ESPTE.TER_CODI, GN_TERCE.TER_CODA, GN_TERCE.TER_NOCO ");
                    sql.Append(" FROM AE_ESPTE, GN_TERCE ");
                    sql.Append(" WHERE AE_ESPTE.EMP_CODI = GN_TERCE.EMP_CODI ");
                    sql.Append(" AND AE_ESPTE.TER_CODI = GN_TERCE.TER_CODI ");
                    sql.Append(" AND AE_ESPTE.EMP_CODI = @P_EMP_CODI ");
                    sql.Append(" AND AE_ESPTE.ESP_CONT IN (SELECT  AE_ESPAC.ESP_CONT ");
                    sql.Append(" FROM AE_ESPAC, AE_DESPA, AE_CLASE, IN_PRODU ");
                    sql.Append(" LEFT OUTER JOIN IN_BMPRO ON(IN_PRODU.EMP_CODI = IN_BMPRO.EMP_CODI ");
                    sql.Append(" AND IN_PRODU.PRO_CONT = IN_BMPRO.PRO_CONT) ");
                    sql.Append(" WHERE AE_ESPAC.EMP_CODI = AE_CLASE.EMP_CODI ");
                    sql.Append(" AND AE_ESPAC.CLA_CONT = AE_CLASE.CLA_CONT ");
                    sql.Append(" AND AE_ESPAC.EMP_CODI = AE_DESPA.EMP_CODI ");
                    sql.Append(" AND AE_ESPAC.ESP_ESTA = 'A' ");
                    sql.Append(" AND AE_ESPAC.ESP_CONT = AE_DESPA.ESP_CONT ");
                    sql.Append(" AND AE_DESPA.EMP_CODI = IN_PRODU.EMP_CODI ");
                    sql.Append(" AND AE_DESPA.PRO_CONT = IN_PRODU.PRO_CONT ");
                    sql.Append(" AND AE_CLASE.CLA_CONT = @P_CLA_CONT ");
                    sql.Append(" AND IN_PRODU.PRO_DMIN > 0 ");
                    sql.Append(" AND AE_DESPA.EMP_CODI = @P_EMP_CODI ");
                    sql.Append(" AND AE_DESPA.PRO_CONT = @P_PRO_CONT ");
                    sql.Append(" GROUP BY AE_ESPAC.ESP_CONT) ");
                    sql.Append(" GROUP BY AE_ESPTE.TER_CODI, GN_TERCE.TER_CODA, ");
                    sql.Append(" GN_TERCE.TER_NOCO ORDER BY GN_TERCE.TER_NOCO ");
                }
                else if (Op_Disp == "F")
                {
                    sql.AppendLine(" SELECT DISTINCT GN_TERCE.TER_CODI, GN_TERCE.TER_CODA, GN_TERCE.TER_NOCO                           ");
                    sql.AppendLine(" FROM AE_HORAR, AE_DHORA, AE_CLASE, AE_ESPAC,AE_DESPA,AE_ESPTE,GN_TERCE                            ");
                    sql.AppendLine(" WHERE AE_HORAR.EMP_CODI = AE_DHORA.EMP_CODI                                                       ");
                    sql.AppendLine(" AND AE_HORAR.HOR_CONT = AE_DHORA.HOR_CONT AND AE_CLASE.EMP_CODI = AE_ESPAC.EMP_CODI               ");
                    sql.AppendLine(" AND AE_CLASE.CLA_CONT = AE_ESPAC.CLA_CONT AND AE_ESPAC.ESP_CONT = AE_DESPA.ESP_CONT               ");
                    sql.AppendLine(" AND AE_ESPAC.EMP_CODI = AE_DESPA.EMP_CODI AND AE_HORAR.EMP_CODI = AE_ESPTE.EMP_CODI               ");
                    sql.AppendLine(" AND AE_HORAR.TER_CODI = AE_ESPTE.TER_CODI AND AE_ESPAC.ESP_CONT = AE_ESPTE.ESP_CONT               ");
                    sql.AppendLine(" AND GN_TERCE.EMP_CODI = AE_ESPTE.EMP_CODI AND GN_TERCE.TER_CODI = AE_ESPTE.TER_CODI               ");
                    sql.AppendLine(" AND AE_ESPAC.EMP_CODI = AE_ESPTE.EMP_CODI AND AE_HORAR.HOR_ESTA = 'A'                             ");
                    sql.AppendLine(" AND YEAR(DHO_FECH) = @P_YEAR AND MONTH(DHO_FECH) = @P_MONTH                                       ");
                    sql.AppendLine(" AND DAY(DHO_FECH) = @P_DAY AND AE_CLASE.CLA_CONT = @P_CLA_CONT                                    ");
                    sql.AppendLine(" AND AE_DESPA.PRO_CONT = @P_PRO_CONT                                                               ");
                    sql.AppendLine(" AND AE_HORAR.HOR_DISP = 'D'                                                                       ");
                    //sql.AppendLine(" AND((@P_FINI < DHO_HORF AND @P_FFIN >= DHO_HORF) OR (@P_FINI <= DHO_HORI AND @P_FFIN > DHO_HORI)) ");
                    sql.AppendLine(" AND (DHO_HORI<=@P_FINI AND DHO_HORF>=@P_FFIN)                                                   ");
                    sql.AppendLine(" AND GN_TERCE.TER_CODI NOT IN (SELECT ar.TER_CODI  FROM   AE_RESER AS ar ");
                    sql.AppendLine(" WHERE  ar.EMP_CODI = @P_EMP_CODI  AND ar.TER_CODI = GN_TERCE.TER_CODI ");                
                    sql.AppendLine(" AND AR.RES_FINA > @P_FINI AND AR.RES_FINI < @P_FFIN  AND AR.RES_ESTA IN ('C','E')) ");
                    listAux.Add(new Parameter("@P_FINI", Fini));
                    listAux.Add(new Parameter("@P_FFIN", Ffin));
                    listAux.Add(new Parameter("@P_YEAR", Ffin.Value.Year));
                    listAux.Add(new Parameter("@P_MONTH", Ffin.Value.Month));
                    listAux.Add(new Parameter("@P_DAY", Ffin.Value.Day));
                }

                /*sql.Append(" SELECT AE_ESPTE.TER_CODI, GN_TERCE.TER_CODA, GN_TERCE.TER_NOCO, CAST(GN_TERCE.TER_FOTO AS VARBINARY (MAX)) TER_FOTO ");
                sql.Append(" FROM AE_ESPTE, GN_TERCE                                                                             ");
                sql.Append(" WHERE AE_ESPTE.EMP_CODI = GN_TERCE.EMP_CODI                                                         ");
                sql.Append(" AND AE_ESPTE.TER_CODI = GN_TERCE.TER_CODI                                                           ");
                sql.Append(" AND AE_ESPTE.EMP_CODI = @P_EMP_CODI                                                                 ");
                sql.Append(" AND AE_ESPTE.ESP_CONT IN (SELECT  AE_ESPAC.ESP_CONT                                                 ");
                sql.Append(" FROM AE_ESPAC, AE_DESPA, AE_CLASE, IN_PRODU                                                         ");
                sql.Append(" LEFT OUTER JOIN IN_BMPRO ON(IN_PRODU.EMP_CODI = IN_BMPRO.EMP_CODI                                   ");
                sql.Append(" AND IN_PRODU.PRO_CONT = IN_BMPRO.PRO_CONT)                                                          ");
                sql.Append(" WHERE AE_ESPAC.EMP_CODI = AE_CLASE.EMP_CODI                                                         ");
                sql.Append(" AND AE_ESPAC.CLA_CONT = AE_CLASE.CLA_CONT                                                           ");
                sql.Append(" AND AE_ESPAC.EMP_CODI = AE_DESPA.EMP_CODI                                                           ");
                sql.Append(" AND AE_ESPAC.ESP_ESTA = 'A'                                                                         ");
                sql.Append(" AND AE_ESPAC.ESP_CONT = AE_DESPA.ESP_CONT                                                           ");
                sql.Append(" AND AE_DESPA.EMP_CODI = IN_PRODU.EMP_CODI                                                           ");
                sql.Append(" AND AE_DESPA.PRO_CONT = IN_PRODU.PRO_CONT                                                           ");
                sql.Append(" AND AE_CLASE.CLA_CONT = @P_CLA_CONT                                                                 ");
                sql.Append(" AND IN_PRODU.PRO_DMIN > 0                                                                           ");
                sql.Append(" AND AE_DESPA.EMP_CODI = @P_EMP_CODI                                                                 ");
                sql.Append(" AND AE_DESPA.PRO_CONT = @P_PRO_CONT                                                                 ");
                sql.Append(" GROUP BY AE_ESPAC.ESP_CONT)                                                                         ");
                sql.Append(" GROUP BY AE_ESPTE.TER_CODI, GN_TERCE.TER_CODA,                                                      ");
                sql.Append(" GN_TERCE.TER_NOCO, CAST(GN_TERCE.TER_FOTO AS VARBINARY (MAX))                                       ");*/

                //sql.Append(" SELECT AE_ESPTE.ESP_CONT, AE_ESPTE.TER_CODI, GN_TERCE.TER_CODA, ");
                //sql.Append(" GN_TERCE.TER_NOCO, GN_TERCE.TER_FOTO ");
                //sql.Append(" FROM AE_ESPTE, GN_TERCE ");
                //sql.Append(" WHERE AE_ESPTE.EMP_CODI = GN_TERCE.EMP_CODI ");
                //sql.Append(" AND AE_ESPTE.TER_CODI = GN_TERCE.TER_CODI ");
                //sql.Append(" AND AE_ESPTE.EMP_CODI = @P_EMP_CODI ");
                //sql.Append(" AND AE_ESPTE.ESP_CONT = @P_ESP_CONT ");

                listAux.Add(new Parameter("@P_EMP_CODI", emp_codi));
                listAux.Add(new Parameter("@P_CLA_CONT", cla_cont));
                listAux.Add(new Parameter("@P_PRO_CONT", pro_cont));

                Parameter[] oParameter = listAux.ToArray();
                OTOContext pTOContext = new OTOContext();

                var conection = DBFactory.GetDB(pTOContext);
                var objeto = conection.ReadList(pTOContext, sql.ToString(), MakeNew, oParameter);
                return objeto;
            }
            catch (Exception ex)
            {
                BOException.Throw(this.GetType().ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return null;
            }
        }

        public Func<IDataReader, TOGnTerce> Make = reader => new TOGnTerce
        {
            Ter_codi = reader["TER_CODI"].AsInt(),
            Ter_noco = reader["TER_NOCO"].AsString(),
            Ter_foto = reader["TER_FOTO"].AsFoto(),
        };

        public Func<IDataReader, TOGnTerce> MakeNew = reader => new TOGnTerce
        {
            Ter_codi = reader["TER_CODI"].AsInt(),
            Ter_noco = reader["TER_NOCO"].AsString()
        };


    }
}
